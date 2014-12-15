using UnityEngine;
using System.Collections;

public class SalvageConversionRoundsProjectile : Projectile, IProjectile {

	
	void Update () {
		
		// TODO: Needs to work with all angles.
		transform.position += transform.TransformDirection(this.velocity * Time.deltaTime * Direction);
			
		Vector3 positionToCamera = Camera.main.WorldToViewportPoint(transform.position);
		
		if (positionToCamera.x > 1.5f | positionToCamera.x < -0.5f){
			Destroy(gameObject);
		} else if (positionToCamera.y > 1.5f | positionToCamera.y < -0.5){
			Destroy(gameObject);
		}
	}
	
	void OnTriggerEnter(Collider collider){
		
		Destructable destructable = collider.GetComponent<Destructable>();
		
		if (destructable != null){
			int oldHealth = destructable.Armor;
			destructable.DamageArmor(this.Damage, Owner);
			int damageDealt = oldHealth - oldHealth;
			int restoreAmount = Mathf.RoundToInt(damageDealt/(float)SceneHandler.PlayerShips.Count);
			
			foreach(ShipObject ship in SceneHandler.PlayerShips){
				ship.RestoreArmor(restoreAmount);
			}
			
			//TODO: Trigger destructable.projectileJustHitMe particle effect
			Destroy(gameObject);
		}
	}
}
