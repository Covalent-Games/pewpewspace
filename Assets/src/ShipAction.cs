using UnityEngine;
using System.Collections;

public class ShipAction : MonoBehaviour {

	public GameObject projectilePrefab;
	float shotTimer;
	public float shotDelay;
	public int damage;

	void Start(){
		
		this.shotDelay = 1f/this.shotDelay;
	}

	void UpdateShotTimer(){
	
		this.shotTimer += Time.deltaTime;
	}

	void HandleInput(){
	
		if (Input.GetButton("Fire1") && this.shotTimer >= this.shotDelay){
			this.shotTimer = 0f;
			Fire();
		}
	}
	
	void Fire(){
	
		// TODO: Projectile is rotated incorrectly... just rotating the projectileOrigin or the projectile prefab doesn't fix it.
		Vector3 projectileOrigin = transform.position + transform.TransformDirection(Vector3.forward * 2);
		GameObject projectileGO = (GameObject)Instantiate(this.projectilePrefab, projectileOrigin, transform.localRotation);
		
		Projectile projectile = projectileGO.GetComponent<Projectile>();
		projectile.damage = damage;
		projectile.direction = Vector3.forward;
		// This may seem odd, but it's so each projectile can destroy itself once it goes off-screen.
		projectile.camera = GetComponent<ShipMovement>().mainCamera;
	}

	void Update () {
		
		UpdateShotTimer();
		HandleInput();
	}
}
