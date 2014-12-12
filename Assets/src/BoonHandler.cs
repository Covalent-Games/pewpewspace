﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoonHandler : MonoBehaviour {
	
	public void ApplyBoon(Boon boon, int modifier, float duration){
		
		switch(boon){
		case Boon.Damage:
			StartCoroutine(IncreaseDamage(boon, modifier, duration));
			break;
		}
	}

	public void ApplyBoon(Boon boon, float modifier, float duration) {

		switch (boon) {
			case Boon.FireRate:
				StartCoroutine(IncreaseFireRate(boon, modifier, duration));
				break;
		}
	}
	
	IEnumerator IncreaseDamage(Boon boon, int mod, float duration){
		
		ShipObject ship = GetComponent<ShipObject>();
		
		// If the ship is destroyed before this effect ends it will raise an exception.
		if (ship == null) {
			yield break;
		}
		
		/*FIXME: This is actually incorrect. This will prevent all damage increases after the first.
		We'll want to create a way of maintaining which specific boons are present and whether
		they're stacking or not.*/
		if (ship.ActiveBoons.Contains(boon)) {
			yield break;
		}
		ship.DamageMod += mod;
		print (ship.gameObject.name + "'s Damage = " + ship.GetDamage());
		float timer = 0f;
		while (timer < duration){
			timer += Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}
		ship.DamageMod -= mod;
		print (ship.gameObject.name + "'s Damage = " + ship.GetDamage());
	}

	IEnumerator IncreaseFireRate(Boon boon, float mod, float duration) {

		ShipObject ship = GetComponent<ShipObject>();

		if (ship == null)
			yield break;

		float fireRateDelta = ship.shotPerSecond *= mod;
		print(ship.gameObject.name + "'s original rate of fire: " + ship.shotPerSecond);
		print(ship.gameObject.name + "'s RoF buff value: " + fireRateDelta);
		ship.shotPerSecond -= fireRateDelta;
		print(ship.gameObject.name + "'s new rate of fire: " + ship.shotPerSecond);

		
		float timer = 0f;
		while (timer < duration) {
			timer += Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}

		ship.shotPerSecond += fireRateDelta;
		print(ship.gameObject.name + "'s rate of fire went back to normal: " + ship.shotPerSecond);
	}
}
