using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SelectionStatUpdater : MonoBehaviour {

	public List<Text> Player1Stats;
	public List<Text> Player2Stats;
	public List<Text> Player3Stats;
	public List<Text> Player4Stats;

	public void UpdateStats(int playerNumber, ShipAction ship) {

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
			}
		}
	}
}
