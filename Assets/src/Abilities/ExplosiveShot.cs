﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ExplosiveShot : BaseAbility, IAbility {

	public void Start() {

		Name = "Explosive Shot";
		Resource = Resources.Load("AbilityObjects/ExplosiveShotProjectile", typeof(GameObject));
		if (Resource == null) {
			Debug.LogError("Resource ExplosiveShotProjectile did not load properly");
		}
		Cost = 18;
		Duration = 1f / 60f;
		Damage = 15f;
	}

	public void Begin(ShipObject ship) {

		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
		DisplayName(Name);
	}

	public IEnumerator Execute() {

		Setup();

		yield return null;

		TearDown();
	}

	public void Setup() {

		Ship.Heat += Cost;
		Executing = true;

		GameObject explosiveShotProjectile = (GameObject)Instantiate(Resource, Ship.transform.position, Ship.Turret.rotation);
		ExplosiveShotProjectile projectile = explosiveShotProjectile.GetComponent<ExplosiveShotProjectile>();
		projectile.Damage = this.Damage;
		projectile.Direction = Ship.Turret.rotation.eulerAngles;
		projectile.damageRadius = 15f;
		projectile.Owner = Ship;

	}

	public void TearDown() {

		Executing = false;
		DurationTimer = 0f;
	}

	public override void TriggerEnter(Collider collider) { }

	public override void TriggerStay(Collider collider) { }

	public override void TriggerExit(Collider collider) { }
}