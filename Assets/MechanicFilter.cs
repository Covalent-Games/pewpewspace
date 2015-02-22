using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MechanicFilter : MonoBehaviour {

	void Update() {

		foreach (ShipObject ship in SceneHandler.PlayerShips) {
			if (GameValues.TestingValues.DisableHeat) {
				if (ship.Heat > 0)
					ship.Heat = 0;
			}
			if (GameValues.TestingValues.DisablePlayerArmor) {
				if (ship.Armor < ship.MaxArmor)
					ship.Armor = ship.MaxArmor;
			}
		}

		if (GameValues.TestingValues.DisableEnemyArmor) {
			foreach (ShipObject ship in SceneHandler.Enemies) {
				if (ship.Armor < ship.MaxArmor)
					ship.Armor = ship.MaxArmor;
			}
		}
	}
}
