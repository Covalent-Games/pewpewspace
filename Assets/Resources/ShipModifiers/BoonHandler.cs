using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Unit testing is needed for all boons and conditions.

public class BoonHandler : BaseModifierHandler {
	
	public void ApplyBoon(Boon boon, AbilityID id, float modifier, float duration, bool stacking=false){
		
		switch(boon){
			case Boon.Damage:
				StartCoroutine(IncreaseDamage(boon, id, modifier, duration));
				break;
			case Boon.FireRate:
				StartCoroutine(IncreaseFireRate(boon, id, modifier, duration));
				break;
			}
	}
	
	IEnumerator IncreaseDamage(Boon boon, AbilityID id, float mod, float duration){
		
		ShipObject ship = GetComponent<ShipObject>();
		Modifier modifier = new Modifier(ship, id, duration, mod, boon);

		if (Exists(modifier, out modifier)) {
			modifier.DurationTimer = 0f;
			yield break;
		} else {
			ship.ActiveBoons.Add(modifier);
		}

		ship.DamageMod += mod;

		while (modifier.DurationTimer < duration) {
			modifier.DurationTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		ship.DamageMod -= mod;
		ship.ActiveBoons.Remove(modifier);
	}

	IEnumerator IncreaseFireRate(Boon boon, AbilityID id, float mod, float duration) {

		ShipObject ship = GetComponent<ShipObject>();
		Modifier modifier = new Modifier(ship, id, duration, mod, boon);

		if (Exists(modifier, out modifier)) {
			modifier.DurationTimer = 0f;
			yield break;
		} else {
			ship.ActiveBoons.Add(modifier);
		}

		// TODO: Determine if this check is needed.
		if (ship == null)
			yield break;

		float fireRateDelta = ship.shotPerSecond * mod;
		ship.shotPerSecond += fireRateDelta;

		while (modifier.DurationTimer < duration) {
			modifier.DurationTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}

		ship.shotPerSecond -= fireRateDelta;
		ship.ActiveBoons.Remove(modifier);
	}
}
