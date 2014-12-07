using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class SceneHandler : MonoBehaviour {

	public Mission CurrentMission;
	public static List<ShipAction> PlayerShips = new List<ShipAction>();
	public static List<ShipAction> Enemies = new List<ShipAction>();
	public LayerMask TargetingLayerMask;

	[SerializeField]
	GameObject dronePrefab;
	public int ThisWave;
	public delegate bool WinCheck();
	bool MissionOver = false;

	// Use this for initialization
	void Start () {

		SpawnPlayer();

		// TODO: This will need to take some kind of mission indentifier parameter.
		StartCoroutine(ExecuteMission());

		Screen.lockCursor = true;
	}

	private void LoadMission() {

		XmlSerializer deserializer = new XmlSerializer(typeof(Mission));
		TextReader reader = new StreamReader(Application.dataPath + "/MissionTemplates/TestMission.xml");
        CurrentMission = (Mission)deserializer.Deserialize(reader);
	}

	IEnumerator ExecuteMission() {

		// TODO: This needs to be set based on user input/selection.
		LoadMission();

		Debug.Log("Mission Started!");
		ThisWave = 1;

		while (ThisWave <= CurrentMission.NumberOfWaves) {
			SpawnEnemies();
			Debug.Log(string.Format("Wave: {0}, spawning {1} enemies", ThisWave, CurrentMission.EnemiesPerWave));
			ThisWave += 1;
			yield return new WaitForSeconds(CurrentMission.TimeBetweenWaves);
		}

		Debug.Log("Queueing end of mission logic");
		StartCoroutine(WaitForMissionEnd());
	}

	private IEnumerator WaitForMissionEnd() {

		// This is just a temporary default to satisfy the compiler.
		WinCheck winCheck = CheckForAllEnemiesKilled;

		Debug.Log("Mission is waiting for win condition to be met...");
		switch (CurrentMission.WinCondition) {
			case WinCondition.AllEnemiesKilled:
				winCheck = CheckForAllEnemiesKilled;
				break;
		}

		while (winCheck() == false) {
			yield return new WaitForSeconds(1f);
		}

		Debug.Log("Win condition met, ending mission");
		GameObject.FindObjectOfType<FadeCanvas>().FadeToBlack();
		// TODO Instantiate this from a prefab.
		GameObject.Find("PostMissionScore").GetComponent<Canvas>().enabled = true;
		ConcludeMission();

	}

	void ConcludeMission() {

		MissionOver = true;
	}

	public bool CheckForAllEnemiesKilled() {

		bool result = false;

		Debug.Log(SceneHandler.Enemies.Count + " left to kill");
		if (SceneHandler.Enemies.Count <= 0) {
			result = true;
		}

		return result;
	}
	
	void SpawnPlayer(){
		
		// A value of 0 to 1 representing the left and right side of the screen respectively.
		float xPos = 0f;
		// Same as xPos but the bottom and top, respectively.
		float yPos = 0.15f;
		for (int playerNum = 0; playerNum < GameValues.numberOfPlayers; playerNum++){

            GameObject hudGO = GameObject.Find(string.Format("Player{0}HUD", playerNum + 1));
            hudGO.GetComponent<Canvas>().enabled = true;

			// Offset the player's starting position.
			xPos += 1f / (GameValues.numberOfPlayers + 1);
			Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(xPos, yPos, 40f));

			GameObject prefabToLoad = GameValues.Players[playerNum + 1].SelectedPrefab;
			if (prefabToLoad == null){
				Debug.LogError("Prefab is null. Wa wa waaaaa");
			}
			GameObject newShipGO = (GameObject)Instantiate(prefabToLoad, spawnPosition, Quaternion.identity);

			ShipAction newShip = newShipGO.GetComponent<ShipAction>();

			newShip.SetupPlayer(playerNum + 1);
			PlayerShips.Add(newShip);
			newShip.Container = PlayerShips;
            
		}
	}
	
	void SpawnEnemies(){

		if (PlayerShips.Count == 0) { return; }
		
		for (int i = 0; i < CurrentMission.EnemiesPerWave; i++){
			float xpos = Random.Range(0f, 1f);
			float ypos = Random.Range(1.02f, 1.05f);
			Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(xpos, ypos, 40f));

			GameObject newEnemyGO = (GameObject)Instantiate(
					dronePrefab, 
					spawnPosition, 
					Quaternion.LookRotation(Vector3.back));

			ShipAction newEnemy = newEnemyGO.GetComponent<ShipAction>();
			SceneHandler.Enemies.Add(newEnemy);
			newEnemy.Container = SceneHandler.Enemies;
		}
	}

	void Update() {

		if (MissionOver) {
			foreach (ShipAction ship in PlayerShips) {
				if (Input.GetButtonDown(ship.player.Controller.ButtonA)) {
					Application.LoadLevel("ShipSelection");
				}
			}
		}
	}
}
