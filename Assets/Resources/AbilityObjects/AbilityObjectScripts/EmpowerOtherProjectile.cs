﻿using UnityEngine;
using System.Collections;

public class EmpowerOtherProjectile : MonoBehaviour {

	public ShipObject Target;
	public float DamageModifier;
	public float Duration;
	bool tracking;
	
	public IEnumerator TrackToTarget(){
		
		tracking = true;
		float speed = 15f;
		
		while (tracking){
		
			if (Target == null) {
				tracking = false;
				Destroy(gameObject);
				yield break;
			}
			
			speed += 0.4f;
			
			transform.position = Vector3.MoveTowards(
					transform.position, 
					Target.transform.position, 
					Time.deltaTime * speed);

			yield return new WaitForEndOfFrame();
		}
	}
	
	void OnTriggerEnter(Collider collider){
	
		ShipObject shipObject = collider.GetComponent<ShipObject>();
		
		if (shipObject == null){
			return;
		}
		
		if (shipObject != Target){
			return;
		}
		
		int dmg = shipObject.GetDamage();
		int dmgMod = Mathf.RoundToInt(dmg * DamageModifier);
		shipObject.GetComponent<BoonHandler>().ApplyBoon(Boon.Damage, AbilityID.EmpowerOther, dmgMod, Duration);
		
		tracking = false;
		Destroy(gameObject);
	}
}
