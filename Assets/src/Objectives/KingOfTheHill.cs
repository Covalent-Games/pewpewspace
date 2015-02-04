using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KingOfTheHill : MonoBehaviour {

	public Image ProgressBar;
	float Timer;
	public float TimeGoal;
	bool DecreaseTime = true;
	public delegate void OnWinDelegate();
	public delegate void OnLoseDelegate();

	// Even handlers (kinda?)
	public OnWinDelegate OnWin;
	public OnLoseDelegate OnLose;

	void OnTriggerEnter(Collider collider) {

		if (collider.tag == "Player") {
			DecreaseTime = false;
		}
	}

	void OnTriggerExit(Collider collider) {

		if (collider.tag == "Player") {
			DecreaseTime = true;
		}
	}

	void OnTriggerStay(Collider collider) {

		if (collider.tag == "Player") {
			Timer += Time.deltaTime;
		}
	}

	void Update() {

		if (DecreaseTime && Timer > 0f) {
			Timer -= Time.deltaTime;
		}

		ProgressBar.fillAmount = Timer / TimeGoal;

		if (Timer >= TimeGoal) {
			ProgressBar.enabled = false;
			if (OnWin != null) {
				OnWin();
				enabled = false;
			}
		}
	}
}
