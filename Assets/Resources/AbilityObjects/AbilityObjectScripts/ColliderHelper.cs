using UnityEngine;
using System.Collections;

public class ColliderHelper : MonoBehaviour {

	public BaseAbility Ability;

	void OnTriggerEnter(Collider collider){
		if (Ability == null){
			Debug.LogWarning("AreaOfEffectShere.Ability: " + Ability.ToString());
			return;
		}
		Ability.TriggerEnter(collider);
	}
	
	void OnTriggerStay(Collider collider){
		
		if (Ability == null){
			Debug.LogWarning("AreaOfEffectShere.Ability: " + Ability.ToString());
			return;
		}
		Ability.TriggerStay(collider);
	}
	
	void OnTriggerExit(Collider collider){
		
		if (Ability == null){
			Debug.LogWarning("AreaOfEffectShere.Ability: " + Ability.ToString());
			return;
		}
		Ability.TriggerExit(collider);
	}
	
	public float ModifySphereRadius(float newRadias){
	
		SphereCollider sphere = transform.GetComponent<SphereCollider>();
		sphere.radius = newRadias;
		
		return sphere.radius;
	}
}
