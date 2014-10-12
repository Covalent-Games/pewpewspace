using UnityEngine;
using System.Collections;

public class MainMenuHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void ButtonPressed(string buttonName) {

		switch(buttonName) {
			case "Engage":
				StartGame();
				break;
			case "Resign":
				Application.Quit();
				break;
			case default:
				break;
		}
	}

	void StartGame() {

		Application.LoadLevel("main");
	}
}
