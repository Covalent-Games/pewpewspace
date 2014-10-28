using UnityEngine;
using System.Collections;

public class EmpowerOtherProjectile : MonoBehaviour {

	public ShipAction Target;
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

			yield return new WaitForFixedUpdate();
		}
	}
	
	void OnTriggerEnter(Collider collider){
	
		ShipAction shipAction = collider.GetComponent<ShipAction>();
		
		if (shipAction == null){
			return;
		}
		
		if (shipAction != Target){
			return;
		}
		
		int dmg = shipAction.GetDamage();
		int dmgMod = Mathf.RoundToInt(dmg * DamageModifier);
		shipAction.GetComponent<BoonHandler>().ApplyBoon(Boon.Damage, dmgMod, Duration);
		
		tracking = false;
		Destroy(gameObject);
	}
}
