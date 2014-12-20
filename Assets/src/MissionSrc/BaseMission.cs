using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BaseMission : MonoBehaviour {

	public bool InProgress = false;
	public bool Ended = false;
	public int ScrapReward;
	public int ScrapBonusReward;
	public ScrapObject.QualityRating ScrapQuality;

	[Tooltip("Time Delay after sequence ends. (Leave  size zero if default values are desired)")]
	public List<float> SequencePadding = new List<float>();
	[Tooltip("List of sequence prefabs in the order to be executed")]
	public List<GameObject> Sequences = new List<GameObject>();

	public void StartMission() {

		InProgress = true;
		StartCoroutine(IterateSequences());
	}

	IEnumerator IterateSequences() {

		for (int index = 0; index < Sequences.Count; index++) {
			GameObject sequenceGO = Instantiate(Sequences[index]) as GameObject;
			IMissionSequence sequence = sequenceGO.GetComponent(typeof(IMissionSequence)) as IMissionSequence;

			yield return null;

			Debug.Log("Waiting for " + sequenceGO.name + " sequence to finish");
			while (sequence.Running) {
				yield return new WaitForEndOfFrame();
			}
			Debug.Log(sequenceGO.name + " has finished");
			if (SequencePadding.Count > 0) {
				yield return new WaitForSeconds(SequencePadding[index]);
			}
			Destroy(sequenceGO);
		}

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

		foreach (var playerDict in GameValues.Players) {
			int playerNum = playerDict.Key;
			Player player = playerDict.Value;
			Transform[] uiElements = rewardUI[playerNum - 1].GetComponentsInChildren<Transform>();
			// This line might be horrible... I haven't decided. 
			rewardUI[playerNum - 1].transform.parent.GetComponent<Canvas>().enabled = true;
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
