using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mission_02 : BaseMission {

	List<GameObject> RewardUI = new List<GameObject>();

	void Start() {

		RewardUI = GameObject.FindObjectOfType<SceneHandler>().RewardUI;
		StartMission();
	}

	void Update() {

		if (Ended) {
			AwardScrap();
			DisplayScrapReward(RewardUI);
		}
	}
}
