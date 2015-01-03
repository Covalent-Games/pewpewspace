using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectionHandler : MonoBehaviour {

	// Array of ship prefabs
	public GameObject[] prefabs;
	SelectionStatUpdater StatUpdater;
	int maxSelection;

	enum selectionStatus {
		ship,
		ability,
		join
	}

	//HACK: make selection indexes better
	// PLAYER ARRAYS
	//	Which ship is the player currently viewing?
	GameObject[][] availableShips = new GameObject[4][];
	//	Index of currently viewed ship
	[SerializeField]
	int[] currentSelection = new int[] { 0, 0, 0, 0 };
	//	What screen is the player on?
	int[] playerStatus = new int[4];
	GameObject[] readyScreens = new GameObject[4];
	//	Which ship did the player select?
	GameObject[] selectedShip = new GameObject[4];
	// 	Did the player select "ready"?
	bool[] isReady = new bool[] { true, true, true, true };


	void Start() {

		StatUpdater = GetComponent<SelectionStatUpdater>();
		this.maxSelection = prefabs.Length;

		PopulateSelectableShips();

		// Displays ship selectors depending on number of players
		// Sets the ready status for real players to false and their current screen to ship selection
		for (int i = 1; i <= GameValues.numberOfPlayers; i++) {
			string selectPrefabScreenName = string.Format("Player{0}ShipScreen", i);
			GameObject selectPrefabScreen = GameObject.Find(selectPrefabScreenName);
			if (selectPrefabScreen == null) {
				Debug.LogError("Did not find GameObject: " + selectPrefabScreenName);
			}
			selectPrefabScreen.GetComponent<Canvas>().enabled = true;
			this.isReady[i - 1] = false;
			this.playerStatus[i - 1] = (int)selectionStatus.ship;
			this.readyScreens[i - 1] = GameObject.Find(string.Format("P{0}NextLabel", i));
		}
	}

	/// <summary>
	/// Loads available ship prefabs from the Resources folder.
	/// Sets the first to be visible for all players.
	///
	/// </summary>
	void PopulateSelectableShips() {

		for (int playerNumber = 0; playerNumber < GameValues.numberOfPlayers; playerNumber++) {
			// TODO: Convert this to a List (will make adding ship types in the future easier.
			availableShips[playerNumber] = new GameObject[4];

			GameObject prefabPlaceholder = GameObject.Find(string.Format("P{0}Placeholder", playerNumber + 1));
			if (prefabPlaceholder == null) {
				Debug.LogError(string.Format("P{0}Placeholder is null", playerNumber + 1));
			}
			Vector3 shipDisplayCoords = prefabPlaceholder.transform.position;
			Quaternion shipDisplayRotation = prefabPlaceholder.transform.rotation;

			int count = 0;
			GameObject ship;

			foreach (var prefab in prefabs) {
				if (prefab == null) {
					Debug.LogError("Missing resource. Check that all objects are assigned in the inspector.");
				}

				// Disable ship functionality to make it a "floor model"
				ship = (GameObject)Instantiate(prefab, shipDisplayCoords, shipDisplayRotation);
				ship.GetComponent<MeshRenderer>().enabled = false;
				ship.GetComponent<ShipMovement>().enabled = false;
				ship.GetComponent<ShipObject>().enabled = false;
				ship.name = prefab.name;

				availableShips[playerNumber][count] = ship;

				count++;
			}

			// Make the first ship in the list visible and display that ship's stats on the screen.
			ship = availableShips[playerNumber][0];
			ship.GetComponent<MeshRenderer>().enabled = true;
			StatUpdater.UpdateStats(playerNumber, ship.GetComponent<ShipObject>());

		}
	}

	/// <summary>
	/// Updates the screen state of a given player.
	/// </summary>
	/// <param name="playerIndex"></param> Player number - 1
	void updatePlayerSelectionScreen(int playerIndex) {

		switch (this.playerStatus[playerIndex]) {
			case (int)selectionStatus.ship:
				readyScreens[playerIndex].GetComponent<Text>().text = "Press 'A' to select";
				break;
			case (int)selectionStatus.ability:
				break;
			case (int)selectionStatus.join:
				readyScreens[playerIndex].GetComponent<Text>().text = "Ready!";
				break;
		}
	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.Return)) {
			StartGame();
		}

		//TODO: Include ability menu in here
		foreach (var player in GameValues.Players) {
			// Since player.Key ranges from 1 to 4, need index from 0 to 3;
			int playerIndex = player.Key - 1;

			// Press A
			if (Input.GetButtonDown(player.Value.Controller.ButtonA)) {
				// If this player is on ship selection, procede
				if (this.playerStatus[playerIndex] == (int)selectionStatus.ship) {
					this.isReady[playerIndex] = true;
					this.playerStatus[playerIndex] = (int)selectionStatus.join;
					updatePlayerSelectionScreen(playerIndex);
					// Check if everyone is ready
					if (AllReady()) {
						StartGame();
					}
				}
			}

			// Press B
			if (Input.GetButtonDown(player.Value.Controller.ButtonB)) {
				// If this player is readied, go back to selection
				if (this.playerStatus[playerIndex] == (int)selectionStatus.join) {
					this.isReady[playerIndex] = false;
					this.playerStatus[playerIndex] = (int)selectionStatus.ship;
					updatePlayerSelectionScreen(playerIndex);
				}
			}

			// Force delay for selection change
			if (GetComponent<InputDelay>().SignalAllowed(playerIndex) && playerStatus[playerIndex] != (int)selectionStatus.join) {

				float input = Input.GetAxis(player.Value.Controller.LeftStickX);
				// -1 if left, 1 if right.
				int direction = Mathf.RoundToInt(input);

				// true if stick is angled more than 95%
				// NOTE The control feels more responsive if the action happens the same time the stick "clicks".
				if (Mathf.Abs(input) > 0.65f) {
					RotateSelection(playerIndex, currentSelection[playerIndex], direction);
					currentSelection[playerIndex] += direction;
				}
			}
		}
	}


	/// <summary>
	/// Returns true if all players are in a ready state (all have pressed the 'ready button').
	/// </summary>
	bool AllReady() {

		for (int i = 0; i < GameValues.numberOfPlayers; i++) {
			if (!this.isReady[i]) {
				return false;
			}
		}
		return true;
	}

	void RotateSelection(int playerNumber, int previousIndex, int currentIndex) {

		int selectionIndex = Mathf.Abs(previousIndex + currentIndex) % availableShips.Length;
		int previousSlection = Mathf.Abs(previousIndex) % availableShips.Length;

		GameObject previousGO = availableShips[playerNumber][previousSlection];
		previousGO.GetComponent<MeshRenderer>().enabled = false;
		GameObject newSelection = availableShips[playerNumber][selectionIndex];
		newSelection.GetComponent<MeshRenderer>().enabled = true;
		StatUpdater.UpdateStats(playerNumber, newSelection.GetComponent<ShipObject>());
	}

	void StartGame() {

		// Loop through participating players
		for (int playerNumber = 0; playerNumber < GameValues.numberOfPlayers; playerNumber++) {
			// Get a positive integar from 0 to the number of available ships that represents the player's selection.
			int index = Mathf.Abs(currentSelection[playerNumber] % availableShips.Length);
			// Set the player's selected ship.
			GameValues.Players[playerNumber + 1].SelectedPrefab = this.prefabs[index];
		}
		// TODO: This prevents us from having more than one mission. Fix asap.
		Application.LoadLevel("main");
	}

	void ReturnToMainMenu() {

		Application.LoadLevel("MainMenu");
	}

}
