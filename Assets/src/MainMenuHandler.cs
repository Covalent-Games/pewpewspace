using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {

	public GameObject playerNumberSlider;
	int playerNumber = 1;
	float selectionDelay = 0.2f;
	float selectionTimer = 0f;

	void Start() {

		// Initiate player 1 right away
		//TODO: Have this call a SignPlayerOn() method of some kind
		GameValues.Players.Add(1, new Player(1));
	}

	//NOTE: Currently not used, but could be kept for keyboard input
	public void ButtonPress(string buttonName) {
		Debug.Log("Is this running?");
		switch (buttonName) {
			case "Enlist":
				ShipSelection();
				break;
			case "Resign":
				Application.Quit();
				break;
			default:
				break;
		}
	}

	public void ChangeSlider(float players) {

		this.playerNumber = Mathf.RoundToInt(players);
		GameObject.Find("NumberOfPlayers").GetComponent<Text>().text = "Players: " + this.playerNumber.ToString();
	}

	void UpdateSelectionDelay() {

		selectionTimer += Time.deltaTime;
	}

	void Update() {

		UpdateSelectionDelay();

		if (Input.GetButtonDown(GameValues.Players[1].Controller.ButtonA)) {
			LoadGalaxyMenu();
		}

		if (selectionTimer >= selectionDelay) {
			// Player goes left with joystick
			if (Input.GetAxis(GameValues.Players[1].Controller.LeftStickX) <= -0.5 && this.playerNumber > 1) {
				playerNumberSlider.GetComponent<Slider>().value -= 1;
				selectionTimer = 0f;
			}
			// Player goes right with joystick
			if (Input.GetAxis(GameValues.Players[1].Controller.LeftStickX) >= 0.5 && this.playerNumber < 4) {
				playerNumberSlider.GetComponent<Slider>().value += 1;
				selectionTimer = 0f;
			}

		}
	}

	void ShipSelection() {

		GameValues.NumberOfPlayers = playerNumber;
		for (int i = 2; i <= playerNumber; i++) {
			GameValues.Players.Add(i, new Player(i));
		}
		Application.LoadLevel("ShipSelection");
	}

	public void LoadGalaxyMenu() {

		GameValues.NumberOfPlayers = playerNumber;
		for (int i = 2; i <= playerNumber; i++) {
			GameValues.Players.Add(i, new Player(i));
		}
		Application.LoadLevel("GalaxyMenu");
	}
}
