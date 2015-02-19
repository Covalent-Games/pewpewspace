using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionSelecter : MonoBehaviour {

	void OnTriggerStay(Collider collider) {

		if (Input.GetButton(GameValues.Players[1].Controller.ButtonA)) {
			GameValues.NextScene = collider.GetComponent<MissionLoader>().SceneToLoad;
			SceneHandler.LoadScene("ShipSelection");
		}
	}
}
