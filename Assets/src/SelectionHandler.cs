using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectionHandler : MonoBehaviour {
 
	// Array of ship prefabs
	public GameObject[] prefabs;

	int maxSelection;

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

	//HACK: make selection indexes better
	// PLAYER ARRAYS
	//	Which ship is the player currently viewing?
	GameObject[][] availableShips = new GameObject[4][];
	//	Index of currently viewed ship
	[SerializeField]
	int[] currentSelection = new int[] {0, 0, 0, 0};
	//	What screen is the player on?
	int[] playerStatus = new int[4];
	//	Which ship did the player select?
	GameObject[] selectedShip = new GameObject[4];
	// 	Did the player select "ready"?
	bool[] isReady = new bool[] {true, true, true, true};
	// 	Enforces a selection scroll delay
	float[] selectionTimer = new float[] {0, 0, 0, 0};
	float selectionDelay = 0.20f;
	

	void Start () {

		this.maxSelection = prefabs.Length;

		PopulateSelectableShips();

		// Displays ship selectors depending on number of players
		// Sets the ready status for real players to false and their current screen to ship selection
		for(int i = 1; i <= GameValues.numberOfPlayers; i++) {
			string selectPrefabScreenName = string.Format("Player{0}ShipScreen", i);
			GameObject selectPrefabScreen = GameObject.Find(selectPrefabScreenName);
			if (selectPrefabScreen == null) {
				Debug.LogError("Did not find GameObject: " + selectPrefabScreenName);
			}
			selectPrefabScreen.GetComponent<Canvas>().enabled = true;
			this.isReady[i-1] = false;
			this.playerStatus[i-1] = (int)selectionStatus.ship;
		}

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
			foreach(var prefab in prefabs) {
				if(prefab == null) {
					Debug.LogError("Resource was not loaded worth crap");
				}
				availableShips[playerNumber][count] = (GameObject)Instantiate(prefab, 
				                                                              shipDisplayCoords, 
				                                                              shipDisplayRotation);
				availableShips[playerNumber][count].GetComponent<MeshRenderer>().enabled = false;
				availableShips[playerNumber][count].GetComponent<ShipMovement>().enabled = false;
				availableShips[playerNumber][count].GetComponent<ShipAction>().enabled = false;
				count++;
			}
			availableShips[playerNumber][0].GetComponent<MeshRenderer>().enabled = true;

		}
	}

	void UpdateSelectionTimers() {
		
		foreach(var player in GameValues.Players) {
			selectionTimer[player.Key-1] += Time.deltaTime;
		}
	}

	void Update() {
		
		UpdateSelectionTimers();
		//TODO: Include ability menu in here
		foreach (var player in GameValues.Players){
			// Since player.Key ranges from 1 to 4, need index from 0 to 3;
			int playerIndex = player.Key - 1;
			if(Input.GetButtonDown(player.Value.Controller.ButtonA)) {
				// If this player is on ship selection, procede
				if(this.playerStatus[playerIndex] == (int)selectionStatus.ship) {
					this.isReady[playerIndex] = true;
					// Check if everyone is ready
					if(AllReady()) {
						StartGame();
					}
				}
			}		

			// Force delay for selection change
			if(selectionTimer[playerIndex] >= selectionDelay) {
				// Player goes left with joystick
				if(Input.GetAxis(player.Value.Controller.LeftStickX) <= -0.5 && currentSelection[playerIndex] > 0) {
					currentSelection[playerIndex] -= 1;
					RotateSelectionLeft(playerIndex);
					selectionTimer[playerIndex] = 0f;
				}
				// Player goes right with joystick
				if(Input.GetAxis(player.Value.Controller.LeftStickX) >= 0.5 && currentSelection[playerIndex] < availableShips[playerIndex].Length - 1) {
					currentSelection[playerIndex] += 1;
					RotateSelectionRight(playerIndex);
					selectionTimer[playerIndex] = 0f;
				}
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

	void RotateSelectionRight(int playerNumber) {

		GameObject previousSelection = availableShips[playerNumber][currentSelection[playerNumber] - 1];
		previousSelection.GetComponent<MeshRenderer>().enabled = false;
		GameObject newSelection = availableShips[playerNumber][currentSelection[playerNumber]];
		newSelection.GetComponent<MeshRenderer>().enabled = true;
	}

	void RotateSelectionLeft(int playerNumber) {

		GameObject previousSelection = availableShips[playerNumber][currentSelection[playerNumber] + 1];
		previousSelection.GetComponent<MeshRenderer>().enabled = false;
		GameObject newSelection = availableShips[playerNumber][currentSelection[playerNumber]];
		newSelection.GetComponent<MeshRenderer>().enabled = true;
	}

	void StartGame() {
		
		for(int playerNumber = 0; playerNumber < GameValues.numberOfPlayers; playerNumber++) {
			GameValues.Players[playerNumber+1].SelectedPrefab = this.prefabs[currentSelection[playerNumber]];   // availableShips[playerNumber][currentSelection[playerNumber]];
			//selectedShip[playerNumber] = availableShips[playerNumber][currentSelection[playerNumber]];			
		}
		Application.LoadLevel("main");
	}

	void ReturnToMainMenu() {

		Application.LoadLevel("MainMenu");
	}
	
}
