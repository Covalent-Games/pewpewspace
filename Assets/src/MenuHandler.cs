using UnityEngine;
using System.Collections;

public class MenuHandler : MonoBehaviour {

	bool isPaused = false;
	Canvas PauseMenu;

	void Awake() {

		DontDestroyOnLoad(transform.gameObject);
		PauseMenu = GetComponent<Canvas>();
	}

	/// <summary>
	/// Toggles ship functions and the PauseMenu.
	/// </summary>
	/// <param name="toggle"></param>
	void ToggleEverything(bool toggle) {

		isPaused = !isPaused;

		// If we're turning everything off (toggle = false) we want to turn the menu on (true).
		PauseMenu.enabled = !toggle;

		if (toggle == true)
			Time.timeScale = 1;
		else
			Time.timeScale = 0;

		foreach (ShipObject player in SceneHandler.PlayerShips) {
			if (player) {
				player.GetComponent<ShipObject>().enabled = toggle;
			}
		}
	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.Escape)) {
			ToggleEverything(isPaused);
		}

		foreach (var player in GameValues.Players) {
			if (player.Value != null) {
				if (Input.GetButtonDown(player.Value.Controller.ButtonStart)) {
					ToggleEverything(isPaused);
				}
			}
		}
	}
}
