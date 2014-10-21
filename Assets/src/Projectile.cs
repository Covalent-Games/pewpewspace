using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public float velocity;
	public Vector3 direction;
	public int Damage;

	void Update () {
	
		// TODO: Needs to work with all angles.
		transform.position += transform.TransformDirection(this.velocity * Time.deltaTime * direction);
		
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
			Debug.Log(this.Damage);
			destructable.DamageShip(this.Damage);
			Debug.Log("Your health: " + destructable.Shields);
			//TODO: Trigger destructable.projectileJustHitMe particle effect
			Destroy(gameObject);
		}
	}
}
