using UnityEngine;
using System.Collections;


/// <summary>
/// Ship modifier object representing a positive or negative effect on a Ship.
/// The Modifier will handle it's own timer and remove itself automatically.
/// </summary>
public class Modifier {

	public ShipObject Ship;
	public AbilityID ID;
	public float Duration;
	public float DurationTimer = 0f;
	public float Mod;
	public bool Stacking;
	public Boon Boon;
	public Condition Condition;


	public Modifier(
			ShipObject ship,
			AbilityID id,
			float duration,
			float mod,
			Boon boon,
			bool stacking = false) {

		Ship = ship;
		ID = id;
		Duration = duration;
		Mod = mod;
		Boon = boon;
		Stacking = stacking;
	}

	public Modifier(
			ShipObject ship,
			AbilityID id,
			float duration,
			float mod,
			Condition condition,
			bool stacking = false) {

		Ship = ship;
		ID = id;
		Duration = duration;
		Mod = mod;
		Condition = condition;
		Stacking = stacking;

	}
}

