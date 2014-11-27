using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

public class SceneHandler : MonoBehaviour {

	public Mission CurrentMission;
	public static List<ShipAction> PlayerShips = new List<ShipAction>();
	public static List<BaseShipAI> Enemies = new List<BaseShipAI>();
	public LayerMask TargetingLayerMask;

	[SerializeField]
	GameObject dronePrefab;
	public int ThisWave;

	// Use this for initialization
	void Start () {

		// TODO: This will need to take some kind of mission indentifier parameter.
		StartCoroutine(ExecuteMission());

		SpawnPlayer();

		Screen.lockCursor = true;
	}

	private void LoadMission() {

		XmlSerializer deserializer = new XmlSerializer(typeof(Mission));
		TextReader reader = new StreamReader(Application.dataPath + "/MissionTemplates/TestMission.xml");
		object data = deserializer.Deserialize(reader);
		CurrentMission = (Mission)data;
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

		ConcludeMission();
	}

	private void ConcludeMission() {

		Debug.Log("Mission Ended!");
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

			BaseShipAI newEnemy = newEnemyGO.GetComponent<BaseShipAI>();
			SceneHandler.Enemies.Add(newEnemy);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
		//TODO (Jesse): GameObject.Find is pretty expensive, and I think you could make MnuHandler.OpenEscapeMenu static in this case.
		// This should probably not be here. A menu object exists in the scene so it will have an Update() to use for this.
		if(Input.GetKeyDown(KeyCode.Escape)) {
			MenuHandler menuHandler = GameObject.Find("MenuObject").GetComponent<MenuHandler>();
			menuHandler.OpenEscapeMenu();
		}
	}
}
