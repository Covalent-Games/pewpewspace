using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class SceneHandler : MonoBehaviour {

	public Mission CurrentMission;
	public static List<ShipObject> PlayerShips = new List<ShipObject>();
	public static List<ShipObject> Enemies = new List<ShipObject>();
	public LayerMask TargetingLayerMask;

	[SerializeField]
	GameObject dronePrefab;
	public int ThisWave;
	public delegate bool WinCheck();
	public bool MissionOver = false;
	public List<GameObject> Explosions;
	public List<GameObject> RewardUI;

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

		// A delegate for the win condition method
		WinCheck winCheck;

		Debug.Log("Mission is waiting for win condition to be met...");
		switch (CurrentMission.WinCondition) {
			default:
				winCheck = CurrentMission.CheckForAllEnemiesKilled;
				break;
			case WinCondition.AllEnemiesKilled:
				winCheck = CurrentMission.CheckForAllEnemiesKilled;
				break;
		}

		while (winCheck() == false) {
			yield return new WaitForSeconds(1f);
		}

		Debug.Log("Win condition met, ending mission");
		GameObject.FindObjectOfType<FadeCanvas>().FadeToBlack();
		// TODO Instantiate this from a prefab.
		GameObject.Find("PostMissionScore").GetComponent<Canvas>().enabled = true;
		CurrentMission.ConcludeMission(this);

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

			ShipObject newShip = newShipGO.GetComponent<ShipObject>();

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

			ShipObject newEnemy = newEnemyGO.GetComponent<ShipObject>();
			SceneHandler.Enemies.Add(newEnemy);
			newEnemy.Container = SceneHandler.Enemies;
		}
	}

	void Update() {

		if (MissionOver) {
			foreach (ShipObject ship in PlayerShips) {
				if (Input.GetButtonDown(ship.PlayerObject.Controller.ButtonA)) {
					Application.LoadLevel("ShipSelection");
				}
			}
		}
	}
}
