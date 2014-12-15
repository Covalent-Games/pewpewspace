using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO: Unit testing is needed for all boons and conditions.

public class BoonHandler : BaseModifierHandler {
	
	public void ApplyBoon(Boon boon, AbilityID id, int modifier, float duration, bool stacking=false){
		
		switch(boon){
		case Boon.Damage:
			StartCoroutine(IncreaseDamage(boon, id, modifier, duration));
			break;
		}
	}
	
	IEnumerator IncreaseDamage(Boon boon, AbilityID id, int mod, float duration){
		
		ShipObject ship = GetComponent<ShipObject>();
		Modifier modifier = new Modifier(ship, id, duration, mod, boon);

		if (Exists(modifier, out modifier)) {
			modifier.DurationTimer = 0f;
			yield break;
		}

		ship.DamageMod += mod;
		print (ship.gameObject.name + "'s Damage = " + ship.GetDamage());
		float timer = 0f;
		while (timer < duration){
			timer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		ship.DamageMod -= mod;
		ship.ActiveBoons.Remove(modifier);
		print (ship.gameObject.name + "'s Damage = " + ship.GetDamage());
	}
}
