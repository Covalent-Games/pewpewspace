using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnergyMissilePods : BaseAbility, IAbility {

	GameObject CustomProjectile;
	GameObject OriginalProjectile;
	float OriginalCost;

	public void Start() {

		Cost = 20f;
		Duration = 1f / 60f;
		string path = "PlayerShips/ShipObjects/EnergyMissilePodsProjectile";
		CustomProjectile = (GameObject)Resources.Load(path, typeof(GameObject));
	}

	public void Begin(ShipObject ship) {
		Debug.Log("Energy Missile engaged!");
		Ship = ship;
		CustomProjectile.GetComponent<EnergyMissilePodsProjectile>().Owner = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		Toggle = !Toggle;
		Debug.Log("Energy Missile Rounds is on: " + Toggle);
		StartCoroutine(Execute());
	}


	public IEnumerator Execute() {

		if (Toggle) {
			Setup();
		}

		while (Toggle) {
			// TODO: This might not be responsive enough at 1 second.
			yield return new WaitForSeconds(1f);
			if (Ship.MaxHeat - Ship.Heat < Cost) {
				Toggle = false;
				TearDown();
				yield break;
			}
		}

		TearDown();
	}

	public void Setup() {

		OriginalCost = Ship.FireCost;
		Ship.FireCost = Cost;
		OriginalProjectile = Ship.projectilePrefab;
		Ship.projectilePrefab = CustomProjectile;
	}

	public void TearDown() {

		Ship.FireCost = OriginalCost;
		Ship.projectilePrefab = OriginalProjectile;
	}

	public override void TriggerEnter(Collider collider) { }

	public override void TriggerStay(Collider collider) { }

	public override void TriggerExit(Collider collider) { }
}