using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {

	int playerNumber;

	// Use this for initialization
	void Start () {
	
	}

	public void ButtonPress(string buttonName) {

		switch(buttonName) {
			case "Enlist":
				SelectShip();
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

	void SelectShip() {

		GameValues.numberOfPlayers = playerNumber;
		Application.LoadLevel("ShipSelection");
	}
}
