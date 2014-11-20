using UnityEngine;
using System.Collections;

public class SalvageConversionRoundsProjectile : MonoBehaviour, IProjectile {

	public ShipAction Target { get; set; }
	public float velocity;
	public Vector3 Direction {get; set;}
	public int Damage {get; set;}
	
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
			int oldHealth = destructable.Health;
			destructable.DamageShip(this.Damage);
			int damageDealt = oldHealth - oldHealth;
			int restoreAmount = Mathf.RoundToInt(damageDealt/(float)SceneHandler.PlayerShips.Count);
			
			foreach(ShipAction ship in SceneHandler.PlayerShips){
				ship.RestoreArmor(restoreAmount);
			}
			
			//TODO: Trigger destructable.projectileJustHitMe particle effect
			Destroy(gameObject);
		}
	}
}
