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

		// First check if modifier is in Boons
		oldMod = newMod.Ship.ActiveBoons.Find(b => b.ID == newMod.ID);

		// If not, check Conditions
		if (oldMod == null)
			oldMod = newMod.Ship.ActiveConditions.Find(c => c.ID == newMod.ID);

		// If not there either, it's new!
		if(oldMod ==  null)
			oldMod = newMod;
		return exists;
	}
}