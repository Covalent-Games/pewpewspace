﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Unit testing is needed for all boons and conditions.

public class ConditionHandler : BaseModifierHandler {

	/// <summary>
	/// Applies a condition to the owner.
	/// </summary>
	/// <param name="condition">Condition enum identifier.</param>
	/// <param name="id">Ability enum identifier.</param>
	/// <param name="modifier">Float value by which to modify the owner.</param>
	/// <param name="duration">How long the condition will last.</param>
	/// <param name="stacking">True if multiple instances can exist simultaneously.</param>
	public void ApplyCondition(Condition condition, AbilityID id, int modifier, float duration, bool stacking=false){
	
		switch(condition){
			case Condition.Damage:
				StartCoroutine(ReduceDamage(condition, id, modifier, duration, stacking));
				break;
			case Condition.Speed:
				StartCoroutine(ReduceSpeed(condition, id, modifier, duration, stacking));
				break;
			case Condition.Targeting:
				StartCoroutine(DisableTargeting(condition, id, duration));
				break;
		}
	}
	
	IEnumerator ReduceDamage(Condition condition, AbilityID id, int mod, float duration, bool stacking){

		ShipObject ship = GetComponent<ShipObject>();
		Modifier modifier = new Modifier(ship, id, duration, mod, condition);

		if (Exists(modifier, out modifier)) {
			modifier.DurationTimer = 0f;
			yield break;
		} else {
			ship.ActiveConditions.Add(modifier);
		}

		ship.DamageMod -= mod;

		while (modifier.DurationTimer < duration){
			modifier.DurationTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		ship.DamageMod += mod;
		ship.ActiveConditions.Remove(modifier);
	}

	/// <summary>
	/// Slows down enemies within the blast radius by "mod" as a percentage.
	/// </summary>
	IEnumerator ReduceSpeed(Condition condition, AbilityID id, int mod, float duration, bool stacking) {

		ShipObject ship = GetComponent<ShipObject>();
		Modifier modifier = new Modifier(ship, id, duration, mod, condition);

		if (Exists(modifier, out modifier)) {
			modifier.DurationTimer = 0f;
			yield break;
		} else {
			ship.ActiveConditions.Add(modifier);
		}

		float speedChange = ship.Speed * (mod/100f);
		ship.Speed -= speedChange;

		while (modifier.DurationTimer < duration) {
			modifier.DurationTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		ship.Speed += speedChange;
		ship.ActiveConditions.Remove(modifier);
	}

	IEnumerator DisableTargeting(Condition condition, AbilityID id, float duration) {

		ShipObject ship = GetComponent<ShipObject>();
		Modifier modifier = new Modifier(ship, id, duration, 0f, condition);

		if (Exists(modifier, out modifier)) {
			modifier.DurationTimer = 0f;
			yield break;
		} else {
			ship.ActiveConditions.Add(modifier);
		}

		ship.CanTarget = false;

		while (modifier.DurationTimer < duration) {
			modifier.DurationTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		ship.ActiveConditions.Remove(modifier);
		ship.CanTarget = true;
	}
}
