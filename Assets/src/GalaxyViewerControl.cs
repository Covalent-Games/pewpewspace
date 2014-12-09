using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GalaxyViewerControl : MonoBehaviour {

	Player Player;

	void Start() {

		Player = GameValues.Players[1];
	}

	void FixedUpdate() {

		float xMove = Input.GetAxis(Player.Controller.LeftStickX);
		float yMove = Input.GetAxis(Player.Controller.LeftStickY);

		Vector3 direction = Vector3.ClampMagnitude(new Vector3(xMove, 0f, yMove), 1f);
		transform.Translate(direction * Time.deltaTime * 4, Space.World);

	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.Return)) {
			Application.LoadLevel("ShipSelection");
		}
	}
}
