using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SelectionStatUpdater : MonoBehaviour {

	public List<Text> Player1Stats;
	public List<Text> Player2Stats;
	public List<Text> Player3Stats;
	public List<Text> Player4Stats;

	public void UpdateStats(int playerNumber, ShipObject ship) {

		List<Text> list;

		switch (playerNumber) {
			default:
				list = Player1Stats;
				break;
			case 1:
				list = Player2Stats;
				break;
			case 2:
				list = Player3Stats;
				break;
			case 3:
				list = Player4Stats;
				break;
		}

		Player player;
		foreach (Text label in list) {
			switch (label.name) {
				case "HealthText":
					label.text = ship.maxHealth.ToString();
					break;
				case "DamageText":
					label.text = ship.GetDamage().ToString();
					break;
				case "SpeedText":
					label.text = ship.Speed.ToString();
					break;
				case "ShipName":
					label.text = ship.gameObject.name;
					break;
				// Placeholder until we have icons for each scrap type.
				case "ScrapStock":
					player = GameValues.Players[playerNumber+1];
					int low = player.Scrap.QualityLow;
					int med = player.Scrap.QualityMedium;
					int high = player.Scrap.QualityHigh;
					label.text = string.Format("Scrap Storage: L: {0} -- M: {1} -- H: {2}", low, med, high);
					break;
			}
		}
	}
}
