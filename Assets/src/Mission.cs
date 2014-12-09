using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Mission {

	public int NumberOfWaves { get; set; }
	public int EnemiesPerWave { get; set; }
	public float TimeBetweenWaves { get; set; }
	public int ScrapReward { get; set; }
	public int ScrapBonusReward { get; set; }
	public WinCondition WinCondition { get; set; }
	public ScrapObject.QualityRating ScrapQuality { get; set; }

	/// <summary>
	/// Win Condition: Kill all enemies.
	/// </summary>
	/// <returns>True if there are zero enemies in the scene.</returns>
	public bool CheckForAllEnemiesKilled() {

		bool result = false;

		Debug.Log(SceneHandler.Enemies.Count + " left to kill");
		if (SceneHandler.Enemies.Count <= 0) {
			result = true;
		}

		return result;
	}

	public void ConcludeMission(SceneHandler scene) {

		scene.MissionOver = true;
		ScrapObject scrap = new ScrapObject(ScrapQuality, ScrapReward);

		foreach (var d in GameValues.Players) {
			Player player = d.Value;
			player.Scrap.AddScrap(scrap);
		}

		DisplayScrapReward(scene.RewardUI);
	}

	public void DisplayScrapReward(List<GameObject> rewardUI){

		foreach (var playerDict in GameValues.Players) {
			int playerNum = playerDict.Key;
			Player player = playerDict.Value;
			Transform[] uiElements = rewardUI[playerNum-1].GetComponentsInChildren<Transform>();
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
