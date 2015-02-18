using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionTester : BaseMission {

	void Start() {

		StartMission();
	}

	void Update() {

		if (Ended) {
			Debug.Log("Completed");
			enabled = false;
		}
	}
}
