using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RepairDrone : BaseAbility, IAbility {

	public void Start() {

		Cost = 75f;
		PrimaryEffect = 75;
	}

	public void Begin(ShipObject ship) {

		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}

	public IEnumerator Execute() {

		Setup();
		foreach (ShipObject ship in SceneHandler.PlayerShips) {
			ship.RestoreArmor(PrimaryEffect);
		}
		yield return new WaitForEndOfFrame();
		TearDown();
	}

	public void Setup() {

		Ship.Heat += Cost;
		Executing = true;
	}

	public void TearDown() {

		Executing = false;
		DurationTimer = 0f;
	}

	public override void TriggerEnter(Collider collider) { }

	public override void TriggerStay(Collider collider) { }

	public override void TriggerExit(Collider collider) { }
}