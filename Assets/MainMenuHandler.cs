using UnityEngine;
using System.Collections;

public class MainMenuHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void ButtonPressed(string buttonName) {

		if(buttonName == "Engage") {
			StartGame();
		} else if(buttonName == "Resign") {
			Application.Quit();
		}
	}

	void StartGame() {

		Debug.Log("Starting game");
		Application.LoadLevel("main");
	}
}
