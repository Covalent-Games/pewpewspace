using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputDelay: MonoBehaviour {

	float MaxDelay = 0.25f;
	public float[] DelayTimer = new float[] { 0f, 0f, 0f, 0f };
	Coroutine UpdateRoutine;

	void Awake() {

		UpdateRoutine = StartCoroutine(UpdateDelayTimers());
	}

	public void Stop() {

		StopCoroutine(UpdateRoutine);
	}

	private IEnumerator UpdateDelayTimers() {

		while (true) {
			foreach (var player in GameValues.Players) {
				DelayTimer[player.Key - 1] += Time.deltaTime;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	public bool SignalAllowed(int playerNum) {

		DelayTimer[playerNum] += Time.deltaTime;

		if (DelayTimer[playerNum] > MaxDelay) {
			DelayTimer[playerNum] = 0f;
			return true;
		} else {
			return false;
		}
	}
}
