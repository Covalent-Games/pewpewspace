using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionLoader : MonoBehaviour {

	public void LoadMissionOne() {

		Application.LoadLevel("ShipSelection");
	}

	internal void LoadMission() {
		
		Application.LoadLevel("ShipSelection");
	}
}
