﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionSelecter : MonoBehaviour {

	void OnTriggerStay(Collider collider) {

		if (Input.GetButton(GameValues.Players[1].Controller.ButtonA)) {
			collider.GetComponent<MissionLoader>().LoadMission();
		}
	}
}
