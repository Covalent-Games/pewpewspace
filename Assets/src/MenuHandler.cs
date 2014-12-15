using UnityEngine;
using System.Collections;

public class MenuHandler : MonoBehaviour {

	bool isPaused = false;


	public void OpenEscapeMenu() {

		this.isPaused = !this.isPaused;

		if(this.isPaused) {
			Screen.showCursor = true;
			Screen.lockCursor = false;
			StopEverything();
		} else {
			Screen.showCursor = false;
			Screen.lockCursor = true;
			ResumeEverything();
		}

	}

	// Pause
	void StopEverything() {

		Time.timeScale = 0;
		foreach (ShipObject player in SceneHandler.PlayerShips) {
			if (player == null) {
				continue;
			}
			player.GetComponent<ShipObject>().enabled = false;
		}
	}

	// Resume
	void ResumeEverything() {

		Time.timeScale = 1;
		foreach (ShipObject player in SceneHandler.PlayerShips) {
			if (player == null) {
				continue;
			}
			player.GetComponent<ShipObject>().enabled = true;
		}
	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.Escape)){
			OpenEscapeMenu();
		}

		foreach (var player in GameValues.Players) {
			if (Input.GetButtonDown(player.Value.Controller.ButtonStart)) {
				OpenEscapeMenu();
			}
		}
	}

}
