using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BaseMission : MonoBehaviour {

	[HideInInspector]
	public bool InProgress = false;
	[HideInInspector]
	public bool Ended = false;
	[HideInInspector]
	public Canvas DialogueCanvas;
	[HideInInspector]
	public Text DialogueText;

	[Tooltip("The reward amount on completing the missions")]
	public int ScrapReward;
	public int ScrapBonusReward;
	public ScrapObject.QualityRating ScrapQuality;

	[Tooltip("Time Delay after sequence ends. (Leave  size zero if default values are desired)")]
	public List<float> SequencePadding = new List<float>();
	[Tooltip("List of sequence prefabs in the order to be executed")]
	public List<GameObject> Sequences = new List<GameObject>();

	/// <summary>
	/// Starts the mission. This is the clean way of starting a mission.
	/// </summary>
	public void StartMission() {

		InProgress = true;
		StartCoroutine(IterateSequences());
	}

	IEnumerator IterateSequences() {

		// Loop through each attached sequence
		for (int index = 0; index < Sequences.Count; index++) {

			GameObject sequenceGO;

			// Grab the current sequence gameobject from the list
			if (Sequences[index]) {
				sequenceGO = Instantiate(Sequences[index]) as GameObject;
			} else {
				continue;
			}

			if (!sequenceGO) {
				Debug.LogWarning(name + " has null sequence at index " + index);
				continue;
			}

			// Get the interface and initialize the sequence
			IMissionSequence sequence = sequenceGO.GetComponent(typeof(IMissionSequence)) as IMissionSequence;
			sequence.Init(this);

			// Wait one frame for everything to properly set up.
			yield return null;

			// If sequence is concurrent, continue on to next sequence.
			if (sequence.IsConcurrent()) {
				Debug.Log(sequenceGO.name + " is concurrent. Moving to next sequence.");
				continue;
			}

			// Wait for the sequence to finish running.
			Debug.Log("Waiting for " + sequenceGO.name + " sequence to finish");
			while (sequence.Running) {
				yield return new WaitForEndOfFrame();
			}

			// If time padding has been added, wait for the corresponding amount of time.
			if (SequencePadding.Count > 0) {
				yield return new WaitForSeconds(SequencePadding[index]);
			}
		}

		// Conclude the mission. 
		InProgress = false;
		Ended = true;
	}

	protected void AwardScrap() {

		foreach (Player player in GameValues.Players.Values) {
			player.Scrap.AddScrap(ScrapQuality, ScrapReward);
			// TODO: Add generic mission bonus check and bonus reward.
		}
	}

	public void DisplayScrapReward(List<GameObject> rewardUI) {

		foreach (ShipObject ship in SceneHandler.PlayerShips) {
			ship.enabled = false;
			ship.Movement.enabled = false;
		}

		// TODO: Instantiate PostMissionScore object instead.
		GameObject.Find("PostMissionScore").GetComponent<Canvas>().enabled = true;

		foreach (var playerDict in GameValues.Players) {
			int playerNum = playerDict.Key;
			Player player = playerDict.Value;
			GameObject ui = rewardUI[playerNum - 1];

			Transform[] uiElements = ui.GetComponentsInChildren<Transform>();

			Transform uiParent = ui.transform.parent;
			Canvas parentCanvas = uiParent.GetComponent<Canvas>();
			parentCanvas.enabled = true;

			Text text;

			foreach (var element in uiElements) {
				switch (element.name) {
					case "PlayerName":
						// Temp until we have actual usernames
						text = element.GetComponent<Text>();
						text.text = string.Format("{0}: {1}", playerNum, player.SelectedPrefab.name);
						break;
					case "ScrapEarned":
						text = element.GetComponent<Text>();
						text.text = player.Scrap.PreviousScrapGained.ToString();
						break;
					case "BonusScrapEarned":
						text = element.GetComponent<Text>();
						// No bonus scrap system yet, but here this is when we need it. :)
						text.text = "0";
						break;
				}
			}
		}
	}
}
