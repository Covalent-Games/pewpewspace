using UnityEngine;
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
				StartCoroutine(DisableTargeting(condition, id, modifier, duration, stacking));
				break;
		}
	}
	
	IEnumerator ReduceDamage(Condition condition, AbilityID id, float mod, float duration, bool stacking){

		ShipObject ship = GetComponent<ShipObject>();
		Modifier modifier = new Modifier(ship, id, duration, mod, condition);

		if (Exists(modifier, out modifier)) {
			modifier.DurationTimer = 0f;
			yield break;
		} else {
			ship.ActiveConditions.Add(modifier);
		}

		float change = ship.DamageMod * mod;
		ship.DamageMod -= change;

		while (modifier.DurationTimer < duration){
			modifier.DurationTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		ship.DamageMod += change;
		ship.ActiveConditions.Remove(modifier);
	}

    /// <summary>
    /// Slows down enemies within the blast radius by "mod" as a percentage.
    /// </summary>
	IEnumerator ReduceSpeed(Condition condition, AbilityID id, float mod, float duration, bool stacking) {

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
		var cond = ship.ActiveConditions.Find(m => m.ID == modifier.ID);
		cond.DurationTimer = 0f;
		while (cond.DurationTimer < cond.Duration) {
			cond.DurationTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        ship.Speed += speedChange;
		ship.ActiveConditions.Remove(modifier);
    }

	IEnumerator DisableTargeting(Condition condition, AbilityID id, float mod, float duration, bool stacking) {

		BaseShipAI ship = GetComponent<BaseShipAI>();
		Modifier modifier = new Modifier(ship.BaseShip, id, duration, mod, condition, stacking);
		Modifier modifierOut;
		if (Exists(modifier, out modifierOut)) {
			modifierOut.DurationTimer = 0f;
			yield break;
		} else {
			ship.BaseShip.ActiveConditions.Add(modifier);
		}
		ship.BaseShip.CanTarget = false;

		var cond = ship.BaseShip.ActiveConditions.Find(m => m.ID == modifier.ID);
		cond.DurationTimer = 0f;
		while (cond.DurationTimer < cond.Duration) {
			cond.DurationTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		ship.BaseShip.CanTarget = true;
		ship.BaseShip.ActiveConditions.Remove(modifier);

	}
}
