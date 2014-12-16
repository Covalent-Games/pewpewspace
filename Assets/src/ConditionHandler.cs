using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Unit testing is needed for all boons and conditions.

public class ConditionHandler : BaseModifierHandler {

	public void ApplyCondition(Condition condition, AbilityID id, int modifier, float duration, bool stacking=false){
	
		switch(condition){
			case Condition.Damage:
			    StartCoroutine(ReduceDamage(condition, id, modifier, duration, stacking));
			    break;
            case Condition.Speed:
                StartCoroutine(ReduceSpeed(condition, id, modifier, duration, stacking));
                break;
			case Condition.Targeting:
				StartCoroutine(DisableTargeting(duration));
				break;
		}
	}
	
	IEnumerator ReduceDamage(Condition condition, AbilityID id, int mod, float duration, bool stacking){

		ShipObject ship = GetComponent<ShipObject>();
		Modifier modifier = new Modifier(ship, id, duration, mod, condition);

		if (Exists(modifier, out modifier)) {
			modifier.DurationTimer = 0f;
			yield break;
		}

		Debug.Log(ship + " has reduced damage");
		ship.DamageMod -= mod;
		float timer = 0f;
		while (timer < duration){
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		ship.DamageMod += mod;
		ship.ActiveConditions.Remove(modifier);
		Debug.Log(ship + " is free from reduced damaged");
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

        Debug.Log(ship + " has reduced speed");
        float speedChange = ship.Speed * (mod/100f);
        ship.Speed -= speedChange;
        while (modifier.DurationTimer < duration) {
            modifier.DurationTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        ship.Speed += speedChange;
		ship.ActiveConditions.Remove(modifier);
        Debug.Log(ship + " is no longer slowed");
    }

	IEnumerator DisableTargeting(float duration) {

		BaseShipAI ship = GetComponent<BaseShipAI>();
		ship.CanTarget = false;

		float timer = 0f;
		while (timer < duration) {
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		ship.CanTarget = true;
	}
}
