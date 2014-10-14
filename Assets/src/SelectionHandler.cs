using UnityEngine;
using System.Collections;

public class SelectionHandler : MonoBehaviour {

	//HACK: make selection indexes better
	int currentSelection = 0;
	int maxSelection;
	GameObject[] ships;
	public GameObject selectedShip;

	// Use this for initialization
	void Start () {
	
		ships = GameObject.FindGameObjectsWithTag("Ship");
		maxSelection = ships.Length - 1;
		ships[0].GetComponent<MeshRenderer>().enabled = true;
		for(int i = 0; i < ships.Length; i++) {
			ships[i].GetComponent<ShipMovement>().enabled = false;
			ships[i].GetComponent<ShipAction>().enabled = false;
		}
	}

	void Update() {

		if(Input.GetKeyDown(InputCode.Select) || Input.GetKeyDown(KeyCode.Return)) {
			StartGame();
		}
		if(Input.GetKeyDown(InputCode.Cancel) || Input.GetKeyDown(KeyCode.Escape)) {
			ReturnToMainMenu();
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
	
	public void ButtonPress(string buttonName) {

		Debug.Log("Button was pressed");
		switch(buttonName) {
			case "Engage":
				StartGame();
				break;
			case "Back":
				ReturnToMainMenu();
				break;
		}

	}

	void StartGame() {

		selectedShip = ships[currentSelection];
		Application.LoadLevel("main");
	}

	void ReturnToMainMenu() {

		Application.LoadLevel("MainMenu");
	}
	
}
