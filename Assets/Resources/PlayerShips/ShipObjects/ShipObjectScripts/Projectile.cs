using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour, IProjectile {

	public float velocity;
	public Vector3 Direction { get; set; }
	public ShipObject Target { get; set; }
	public ShipObject Owner { get; set; }
	public int Damage { get; set; }

	void Update () {

		if (Target != null) {
			Direction = Vector3.Normalize(Target.transform.position - transform.position);
			transform.position += this.velocity * Direction * Time.deltaTime;
		} else {
			// TODO: Needs to work with all angles.
			transform.position += transform.TransformDirection(this.velocity * Time.deltaTime * Direction);
		}
		
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
			destructable.DamageArmor(this.Damage, Owner);
			//TODO: Trigger destructable.projectileJustHitMe particle effect
			Destroy(gameObject);
		}
	}
}
