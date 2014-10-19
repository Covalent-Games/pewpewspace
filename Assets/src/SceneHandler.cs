using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour {

	public int enemiesToSpawn;
	public float spawnDelay;
	public float spawnTimer;
	public static List<ShipAction> playerShips = new List<ShipAction>();
	public static List<BaseShipAI> enemies = new List<BaseShipAI>();

	[SerializeField]
	GameObject dronePrefab;

	// Use this for initialization
	void Start () {
		
		SpawnPlayer();

		Screen.lockCursor = true;
		this.spawnTimer = this.spawnDelay/2f;
	}
	
	void SpawnPlayer(){
		
		float xPos = 0f;
		float yPos = 0.15f;
		for (int playerNum = 0; playerNum < GameValues.numberOfPlayers; playerNum++){
			//FIXME: Here I'm assuming 1 player is playing, but this will need to come from numberOfPlayers.
			xPos += 1f / (GameValues.numberOfPlayers + 1);
			Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(xPos, yPos, 40f));
			print(spawnPosition);
			//TODO: Here we prepare to spawn the player, so the ship the player has chosen needs to be accessible here.
			GameObject prefabToLoad = GameValues.Players[playerNum + 1].SelectedPrefab;
			if (prefabToLoad == null){
				Debug.LogError("Prefab is null. Wa wa waaaaa");
			}
			GameObject newShipGO = (GameObject)Instantiate(prefabToLoad, spawnPosition, Quaternion.identity);
			ShipAction newShip = newShipGO.GetComponent<ShipAction>();
			newShip.SetupPlayer(playerNum + 1);
			
			playerShips.Add(newShip);
		}
	}
	
	void SpawnEnemies(){
	
		if (playerShips.Count == 0) { return; }
		
		if (spawnTimer >= spawnDelay){
			spawnTimer = 0f;
			for (int i = 0; i < this.enemiesToSpawn; i++){
				float xpos = Random.Range(0f, 1f);
				float ypos = Random.Range(1.05f, 1.2f);
				Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(xpos, ypos, 40f));
				GameObject newEnemyGO = (GameObject)Instantiate(dronePrefab, spawnPosition, Quaternion.LookRotation(Vector3.back));
				BaseShipAI newEnemy = newEnemyGO.GetComponent<BaseShipAI>();
				//TODO: AI can access this directly and don't need players as a member.
				newEnemy.players = SceneHandler.playerShips;
				SceneHandler.enemies.Add(newEnemy);
			}
		} else {
			spawnTimer += Time.deltaTime;
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
		
		SpawnEnemies();
	}
}
