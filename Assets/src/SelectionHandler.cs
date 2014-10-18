using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectionHandler : MonoBehaviour {
 
	// Array of ship prefabs
	public GameObject[] test;
	//HACK: make selection indexes better
	int[] currentSelection = new int[] {0, 0, 0, 0};
	int maxSelection = 4;
	GameObject[][] availableShips = new GameObject[4][];
	enum selectionStatus {
		ship,
		ability,
		join
	}
	enum shipClasses {
		Guardian,
		Outrunner,
		Valkyrie,
		Raider
	}
	int[] playerStatus = new int[4];
	GameObject[] selectedShip = new GameObject[4];
	bool[] isReady = new bool[4];
	

	// Use this for initialization
	void Start () {

		PopulateSelectableShips();

		// Assumes all players are ready at first, but changes depending on how many players are playing
		for(int i = 0; i < 4; i++) {
			this.isReady[i] = true;
		}

		// Displays ship selectors depending on number of players
		// Sets the ready status for real players to false and their current screen to ship selection
		for(int i = 1; i <= GameValues.numberOfPlayers; i++) {
			GameObject.Find(string.Format("Player{0}ShipScreen", i)).GetComponent<Canvas>().enabled = true;
			this.isReady[i-1] = false;
			this.playerStatus[i-1] = (int)selectionStatus.ship;
		}

		Debug.Log("Finished start()");
	}

	/// <summary>
	/// Loads available ship prefabs from the Resources folder.
	/// Sets the first to be visible for all players.
	///
	/// </summary>
	void PopulateSelectableShips() {


		for(int playerNumber = 0; playerNumber < GameValues.numberOfPlayers; playerNumber++) {
			availableShips[playerNumber] = new GameObject[4];
			
			GameObject prefabPlaceholder = GameObject.Find(string.Format("P{0}Placeholder", playerNumber+1));
			if(prefabPlaceholder == null) {
				Debug.LogError(string.Format("P{0}Placeholder is null", playerNumber+1));
			}
			Vector3 shipDisplayCoords = prefabPlaceholder.transform.position;
			Quaternion shipDisplayRotation = prefabPlaceholder.transform.rotation;
			int count = 0;
			foreach(string prefab in new string[] {"PlayerShips/Guardian", 
											 	   "PlayerShips/Outrunner", 
											 	   "PlayerShips/Raider", 
											 	   "PlayerShips/Valkyrie"}) {
				Debug.Log(prefab);
				Object tempPrefab = Resources.Load(prefab, typeof(GameObject));
				if(tempPrefab == null) {
					Debug.LogError("Resource was not loaded worth crap");
				}
				if(shipDisplayCoords == null) {
					Debug.LogError("Why u no haf coords?");
				}
				if(shipDisplayRotation == null) {
					Debug.LogError("You're not facing anywhere?!?!?!");
				}
				availableShips[playerNumber][count] = (GameObject)Instantiate(tempPrefab, 
										                               		  shipDisplayCoords, 
										                                      shipDisplayRotation);
				availableShips[playerNumber][count].GetComponent<MeshRenderer>().enabled = false;
				availableShips[playerNumber][count].GetComponent<ShipMovement>().enabled = false;
				availableShips[playerNumber][count].GetComponent<ShipAction>().enabled = false;

				//availableShips[playerNumber][count].transform.FindChild("dumbArrow").GetComponent<MeshRenderer>().enabled = false;
				count++;
			}
			availableShips[playerNumber][0].GetComponent<MeshRenderer>().enabled = true;
		}
	}

	void Update() {

		//TODO: Include ability menu in here
		foreach (var player in GameValues.Players){
			if(Input.GetButtonDown(player.Value.Controller.ButtonA) || Input.GetKeyDown(KeyCode.Return)) {
				// If this player is on ship selection, procede
				if(this.playerStatus[player.Key] == (int)selectionStatus.ship) {
					this.isReady[player.Key] = true;
					// Check if everyone is ready
					if(AllReady()) {
						StartGame();
					}
				}
			}		
			// Player goes left with joystick
			if(Input.GetAxis(player.Value.Controller.LeftStickX) <= 0.5 && currentSelection[player.Key] > 0) {
				currentSelection[player.Key] += 1;
				RotateSelectionLeft(player.Key);
			}
			// Player goes right with joystick
			if(Input.GetAxis(player.Value.Controller.LeftStickX) >= 0.5 && currentSelection[player.Key] < 0) {
				currentSelection[player.Key] -= 1;
				RotateSelectionRight(player.Key);
			}
		}
	}

	bool AllReady() {

		for(int i = 0; i < GameValues.numberOfPlayers; i++) {
			if(!this.isReady[i]) {
				return false;
			}
		}
		return true;
	}

	void RotateSelectionLeft(int playerNumber) {

		GameObject previousSelection = availableShips[playerNumber][currentSelection[playerNumber] - 1];
		previousSelection.GetComponent<MeshRenderer>().enabled = false;
		GameObject newSelection = availableShips[playerNumber][currentSelection[playerNumber]];
		newSelection.GetComponent<MeshRenderer>().enabled = true;
	}

	void RotateSelectionRight(int playerNumber) {

		GameObject previousSelection = availableShips[playerNumber][currentSelection[playerNumber + 1]];
		previousSelection.GetComponent<MeshRenderer>().enabled = false;
		GameObject newSelection = availableShips[playerNumber][currentSelection[playerNumber]];
		newSelection.GetComponent<MeshRenderer>().enabled = true;
	}

	void StartGame() {
		
		for(int playerNumber = 0; playerNumber < GameValues.numberOfPlayers; playerNumber++) {
			selectedShip[playerNumber] = availableShips[playerNumber][currentSelection[playerNumber]];			
		}
		Application.LoadLevel("main");
	}

	void ReturnToMainMenu() {

		Application.LoadLevel("MainMenu");
	}
	
}
