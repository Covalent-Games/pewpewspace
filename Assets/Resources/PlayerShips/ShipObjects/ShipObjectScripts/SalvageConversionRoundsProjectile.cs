using UnityEngine;
using System.Collections;

public class SalvageConversionRoundsProjectile : Projectile, IProjectile {

	void Update() {

		if (Target != null) {
			Direction = Vector3.Normalize(Target.transform.position - transform.position);
			transform.position += this.velocity * Direction * Time.deltaTime;
		} else {
			// TODO: Needs to work with all angles.
			transform.position += transform.TransformDirection(this.velocity * Time.deltaTime * Direction);
		}

		Vector3 positionToCamera = Camera.main.WorldToViewportPoint(transform.position);

		if (positionToCamera.x > 1.5f | positionToCamera.x < -0.5f) {
			Destroy(gameObject);
		} else if (positionToCamera.y > 1.5f | positionToCamera.y < -0.5) {
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider collider){
		
		Destructible destructable = collider.GetComponent<Destructible>();
		
		if (destructable){
			float oldHealth = destructable.Armor;
			// TODO: Damage should be calculated elsewhere so it can be upgraded (2)
			float damageDealt = oldHealth - destructable.DamageArmor(this.Damage / 2, Owner);
			
			foreach(ShipObject ship in SceneHandler.PlayerShips){
				ship.RestoreArmor(damageDealt);
			}
			
			//TODO: Trigger destructable.projectileJustHitMe particle effect
			Destroy(gameObject);
		}
	}
}
