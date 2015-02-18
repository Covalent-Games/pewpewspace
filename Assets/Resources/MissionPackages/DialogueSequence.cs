using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueSequence : BaseSequence {

	[Tooltip("Pause the players during this dialogue?")]
	public bool PausePlayers;
	public List<string> Captions = new List<string>();
	// TODO: Add image for avatar of speaker
	public List<float> CaptionPauses = new List<float>();
	public float DefaultPauseLength;


	void TogglePlayerPause(bool toggle) {

		foreach (ShipObject ship in SceneHandler.PlayerShips) {
			ship.enabled = toggle;
			ship.Movement.enabled = toggle;
		}
	}

	public override IEnumerator ExecuteSequence() {

		if (PausePlayers) {
			TogglePlayerPause(false);
		}

		yield return new WaitForSeconds(1f);
		DialogueCanvas.enabled = true;

		foreach (string caption in Captions) {
			CanvasGroup canvasGroup = DialogueText.GetComponent<CanvasGroup>();
			DialogueText.text = caption;

			while (canvasGroup.alpha < 1) {
				canvasGroup.alpha += Time.deltaTime * 2;
				if (Input.GetButtonDown(GameValues.Players[1].Controller.ButtonA)) {
					break;
				}
				yield return null;
			}

			int index = Captions.IndexOf(caption);
			float pause = DefaultPauseLength;
			float pauseTimer = 0f;
			if (CaptionPauses.Count > 0) {
				pause = CaptionPauses[index];
				if (pause == 0) {
					pause = DefaultPauseLength;
				}
			}

			while (pauseTimer < pause) {
				pauseTimer += Time.deltaTime;
				// Check if playe is skipping dialogue.
				if (Input.GetButtonDown(GameValues.Players[1].Controller.ButtonA)) {
					break;
				}
				yield return null;
			}

			while (canvasGroup.alpha > 0) {
				canvasGroup.alpha -= Time.deltaTime * 2;
				yield return null;
			}
		}

		if (PausePlayers) {
			TogglePlayerPause(true);
		}
		DialogueCanvas.enabled = false;
		Finish();
	}
}
