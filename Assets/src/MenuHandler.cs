using UnityEngine;
using System.Collections;

public class MenuHandler : MonoBehaviour {

	bool isPaused = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
		GameObject player = GameObject.Find("Player");
		player.GetComponent<ShipMovement>().enabled = false;
		player.GetComponent<ShipAction>().enabled = false;
	}

	void ResumeEverything() {

		GameObject player = GameObject.Find("Player");
		player.GetComponent<ShipMovement>().enabled = true;
		player.GetComponent<ShipAction>().enabled = true;
	}
}
