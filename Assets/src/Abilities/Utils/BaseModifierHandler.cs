﻿using UnityEngine;
using System.Collections;

public class BaseModifierHandler : MonoBehaviour {

	ShipObject Ship;

	/// <summary>
	/// Checks if a specific Modifier exists on the ship, and if it does replaces the new modifier with that object.
	/// </summary>
	/// <param name="newMod">Check if this object exists</param>
	/// <param name="oldMod">Object to replace if newMod exists already</param>
	/// <returns></returns>
	public bool Exists(Modifier newMod, out Modifier oldMod) {

		bool exists = false;

		foreach (Modifier mod in newMod.Ship.ActiveBoons) {
			if (mod.ID == newMod.ID) {
				exists = true;
				oldMod = mod;
			}
		}

		if (!exists) {
			foreach (Modifier mod in newMod.Ship.ActiveConditions) {
				if (mod.ID == newMod.ID) {
					exists = true;
					oldMod = mod;
				}
			}
		}

		oldMod = newMod;
		return exists;
	}
}