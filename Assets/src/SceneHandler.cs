using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneHandler : MonoBehaviour {

	public int enemiesToSpawn;
	public float spawnDelay;
	public float spawnTimer;
	[SerializeField]
	GameObject[] players;

	[SerializeField]
	GameObject dronePrefab;

	// Use this for initialization
	void Start () {
		
		players = GameObject.FindGameObjectsWithTag("Player");
		for(int i = 0; i < players.Length; i++) {
			players[i].GetComponent<ShipMovement>().enabled = true;
			players[i].GetComponent<ShipAction>().enabled = true;
		}
		Screen.lockCursor = true;
		this.spawnTimer = this.spawnDelay/2f;
	}
	
	void SpawnEnemies(){
	
		if (players.Length == 0) { return; }
		
		if (spawnTimer >= spawnDelay){
			spawnTimer = 0f;
			for (int i = 0; i < this.enemiesToSpawn; i++){
				float xpos = Random.Range(0f, 1f);
				float ypos = Random.Range(1.1f, 1.5f);
				Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(xpos, ypos, 40f));
				GameObject newEnemy = (GameObject)Instantiate(dronePrefab, spawnPosition, Quaternion.LookRotation(Vector3.back));
				newEnemy.GetComponent<BaseShipAI>().players = players;
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
