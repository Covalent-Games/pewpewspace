using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Unit testing is needed for all boons and conditions.

public class ConditionHandler : BaseModifierHandler {

	public void ApplyCondition(Condition condition, AbilityID id, float modifier, float duration, bool stacking=false){
	
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
		}

		Debug.Log(ship + " has reduced damage");
		ship.DamageMod -= Mathf.RoundToInt(mod);
		float timer = 0f;
		while (timer < duration){
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		ship.DamageMod += Mathf.RoundToInt(mod);
		ship.ActiveConditions.Remove(modifier);
		Debug.Log(ship + " is free from reduced damaged");
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
		Modifier modifier = new Modifier(ship.actions, id, duration, mod, condition, stacking);
		Modifier modifierOut;
		if (Exists(modifier, out modifierOut)) {
			modifierOut.DurationTimer = 0f;
			yield break;
		} else {
			ship.actions.ActiveConditions.Add(modifier);
		}
		ship.CanTarget = false;

		var cond = ship.actions.ActiveConditions.Find(m => m.ID == modifier.ID);
		cond.DurationTimer = 0f;
		while (cond.DurationTimer < cond.Duration) {
			cond.DurationTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		ship.CanTarget = true;
		ship.actions.ActiveConditions.Remove(modifier);

	}
}
