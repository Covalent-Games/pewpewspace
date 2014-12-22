using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour {

	public GameObject Mission;
	public Canvas DialogueCanvas;
	BaseMission CurrentMission;
	public static List<ShipObject> PlayerShips = new List<ShipObject>();
	public static List<ShipObject> Enemies = new List<ShipObject>();
	public LayerMask TargetingLayerMask;

	public List<GameObject> Explosions;
	public List<GameObject> RewardUI;

	// Use this for initialization
	void Start() {

		SpawnPlayer();
		CreateMission();
	}

	void SpawnPlayer() {

		// A value of 0 to 1 representing the left and right side of the screen respectively.
		float xPos = 0f;
		// Same as xPos but the bottom and top, respectively.
		float yPos = 0.15f;
		for (int playerNum = 0; playerNum < GameValues.numberOfPlayers; playerNum++) {

			GameObject hudGO = GameObject.Find(string.Format("Player{0}HUD", playerNum + 1));
			hudGO.GetComponent<Canvas>().enabled = true;

			// Offset the player's starting position.
			xPos += 1f / (GameValues.numberOfPlayers + 1);
			var viewpoint = new Vector3(xPos, yPos, Camera.main.transform.position.y);
			Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(viewpoint);

			GameObject prefabToLoad = GameValues.Players[playerNum + 1].SelectedPrefab;
			if (prefabToLoad == null) {
				Debug.LogError("Prefab is null. Wa wa waaaaa");
			}
			GameObject newShipGO = (GameObject)Instantiate(prefabToLoad, spawnPosition, Quaternion.identity);

			ShipObject newShip = newShipGO.GetComponent<ShipObject>();

			newShip.SetupPlayer(playerNum + 1);
			newShip.AddContainers(SceneHandler.PlayerShips);

		}
	}

	void CreateMission() {

		Debug.Log("Starting " + Mission.name + " from scenehandler");
		GameObject m = (GameObject)Instantiate(Mission);
		CurrentMission = m.GetComponent<BaseMission>();
		CurrentMission.DialogueCanvas = DialogueCanvas;
		CurrentMission.DialogueText = DialogueCanvas.transform.GetComponentInChildren<Text>();
	}


	void Update() {

		if (CurrentMission.Ended) {
			foreach (ShipObject ship in PlayerShips) {
				if (Input.GetButtonDown(ship.PlayerObject.Controller.ButtonA)) {
					Application.LoadLevel("ShipSelection");
				}
			}
		}
	}
}
