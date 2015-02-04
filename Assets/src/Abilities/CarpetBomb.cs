using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CarpetBomb : BaseAbility, IAbility {

	int NumberOfMissiles = 3;
	static float rangeDegrees = 15f;
	float Range = rangeDegrees * Mathf.PI / 180;

	public void Start() {

		Resource = Resources.Load("PlayerShips/ShipObjects/CarpetBombProjectile", typeof(GameObject));
		if (Resource == null)
			Debug.LogError("Resource ExplosiveShotProjectile did not load properly");

		Cost = 40f;
		Duration = 1f / 60f;
		Damage = 15f;
	}

	public void Begin(ShipObject ship) {

		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}

	public IEnumerator Execute() {

		Setup();

		yield return null;

		TearDown();
	}

	public void Setup() {

		Ship.Heat += Cost;
		Executing = true;

		float startingAngle = Range / -2;
		float interval = Range / (this.NumberOfMissiles - 1);
		for (int i = 0; i < NumberOfMissiles; i++) {
			float missileAngle = startingAngle + (interval * i);
			Quaternion direction = new Quaternion(Ship.Turret.rotation.x, Ship.Turret.rotation.y + missileAngle, Ship.Turret.rotation.z, Ship.Turret.rotation.w);
			GameObject carpetBombProjectile = (GameObject)Instantiate(Resource, Ship.transform.position, direction);
			CarpetBombProjectile projectile = carpetBombProjectile.GetComponent<CarpetBombProjectile>();
			projectile.Damage = this.Damage;
			projectile.Owner = Ship;
		}

	}

	public void TearDown() {

		Executing = false;
		DurationTimer = 0f;
	}

	public override void TriggerEnter(Collider collider) { }

	public override void TriggerStay(Collider collider) { }

	public override void TriggerExit(Collider collider) { }
}