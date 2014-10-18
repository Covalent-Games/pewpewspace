using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelectionHandler : MonoBehaviour {
 
	//HACK: make selection indexes better
	int currentSelection = 0;
	int maxSelection;
	GameObject[] ships;
	public GameObject selectedShip;
	int[] selectionStatus = new int[4];	// 0 = ship selection, 1 = ability selection
	bool[] isReady = new bool[4];

	// Use this for initialization
	void Start () {

		ships = GameObject.FindGameObjectsWithTag("Player");
		maxSelection = ships.Length - 1;
		ships[0].GetComponent<MeshRenderer>().enabled = true;
		for(int i = 0; i < ships.Length; i++) {
			ships[i].GetComponent<ShipMovement>().enabled = false;
			ships[i].GetComponent<ShipAction>().enabled = false;
		} 

		for(int i = 0; i < 4; i++) {
			this.isReady[i] = true;
		}

		// Displays ship selectors depending on number of players
		for(int i = 1; i <= GameValues.numberOfPlayers; i++) {
			GameObject.Find(string.Format("Player{0}ShipScreen", i)).GetComponent<Canvas>().enabled = true;
			this.isReady[i-1] = false;
			this.selectionStatus[i-1] = 0;
		}

		Debug.Log("Finished start()");
	}

	void Update() {

		//TODO: Differentiate between player input
		if(Input.GetButtonDown(InputCode.Select)|| Input.GetKeyDown(KeyCode.Return)) {
			if(this.selectionStatus[0] == 0) {	// Move player to ability selection screen
				GameObject.Find(string.Format("Player{0}ShipScreen", 1)).GetComponent<Canvas>().enabled = false;
				GameObject.Find(string.Format("Player{0}Abilities", 1)).GetComponent<Canvas>().enabled = true;
				this.selectionStatus[0] = 1;
			}else {		// player is ready. If all ready, start game.
				GameObject.Find("P1NextLabel").GetComponent<Text>().text = "Ready";
				this.isReady[0] = true;
				if(AllReady()) {
					StartGame();
				}
			}
		}
		if(Input.GetButtonDown(InputCode.Cancel) || Input.GetKeyDown(KeyCode.Escape)) {
			if(this.selectionStatus[0] == 0) {	// return to main menu
				ReturnToMainMenu();
			} else {	// Move player back to ship selection
				GameObject.Find(string.Format("Player{0}ShipScreen", 1)).GetComponent<Canvas>().enabled = true;
				GameObject.Find(string.Format("Player{0}Abilities", 1)).GetComponent<Canvas>().enabled = false;
				this.selectionStatus[0] = 0;
			}
		}
		float selectionDirection = Input.GetAxis(InputCode.Vertical);
		if(selectionDirection < 0 && currentSelection > maxSelection) {
			currentSelection += 1;
			RotateSelectionDown();
		}
		if(selectionDirection > 0 && currentSelection < 0) {
			currentSelection -= 1;
			RotateSelectionUp();
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

	void RotateSelectionDown() {

		GameObject previousSelection = ships[currentSelection - 1];
		previousSelection.GetComponent<MeshRenderer>().enabled = false;
		GameObject newSelection = ships[currentSelection];
		newSelection.GetComponent<MeshRenderer>().enabled = true;
	}

	void RotateSelectionUp() {

		GameObject previousSelection = ships[currentSelection + 1];
		previousSelection.GetComponent<MeshRenderer>().enabled = false;
		GameObject newSelection = ships[currentSelection];
		newSelection.GetComponent<MeshRenderer>().enabled = true;
	}

	void StartGame() {
		
		
		selectedShip = ships[currentSelection];
		Application.LoadLevel("main");
	}

	void ReturnToMainMenu() {

		Application.LoadLevel("MainMenu");
	}
	
}
