using UnityEngine;
using System.Collections;

public class MainMenuHandler : MonoBehaviour {

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

	void SelectShip() {

		Application.LoadLevel("ShipSelection");
	}
}
