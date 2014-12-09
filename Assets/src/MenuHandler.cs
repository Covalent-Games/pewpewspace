using UnityEngine;
using System.Collections;

public class MenuHandler : MonoBehaviour {

	bool isPaused = false;


	public void OpenEscapeMenu() {
		
		if(this.isPaused) {
			Screen.showCursor = true;
			Screen.lockCursor = false;
			StopEverything();
		} else {
			Screen.showCursor = false;
			Screen.lockCursor = true;
			ResumeEverything();
		}

		this.isPaused = !this.isPaused;
	}

	void StopEverything() {
		
		// TODO: Figure out a better way to do this. Timescale = 0?
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in players) {
            if (player == null) {
                continue;
            }
			player.GetComponent<ShipMovement>().enabled = false;
			player.GetComponent<ShipObject>().enabled = false;
		}
	}

	void ResumeEverything() {

		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject player in players) {
            if (player == null) {
                continue;
            }
			player.GetComponent<ShipMovement>().enabled = true;
			player.GetComponent<ShipObject>().enabled = true;
		}
	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.Escape)) {
			OpenEscapeMenu();
		}
	}

}
