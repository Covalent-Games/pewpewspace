using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour {

	int playerNumber;

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
		//TODO: This will likely be where the user login/data loading starts
		GameValues.Players.Add(1, new Player(1));
		Application.LoadLevel("ShipSelection");
	}
}
