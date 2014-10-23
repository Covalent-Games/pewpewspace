using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConditionHandler : MonoBehaviour {

	public void ApplyCondition(Condition condition, int modifier, float duration){
	
		switch(condition){
			case Condition.Damage:
			StartCoroutine(ReduceDamage(condition, modifier, duration));
			break;
		}
	}
	
	IEnumerator ReduceDamage(Condition condition, int mod, float duration){

		ShipAction ship = GetComponent<ShipAction>();
		if (ship.ActiveConditions.Contains(condition)) {
			yield break;
		}
		Debug.Log(ship + " has reduced damage");
		ship.DamageMod -= mod;
		float timer = 0f;
		while (timer < duration){
			timer += Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}
		ship.DamageMod += mod;
		Debug.Log(ship + " is free from reduced damaged");
	}
}
