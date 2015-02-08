using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SustainDrone : BaseAbility, IAbility {

	float UpdateFrequency = 0.25f;

	public void Start() {

		Name = "Sustain Drone";
		Cost = 45f;
		Duration = 8f;
		PrimaryEffect = 8;
	}

	public void Begin(ShipObject ship) {

		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
		DisplayName(Name);
	}

	public IEnumerator Execute() {

		if (Ship.Armor == Ship.MaxArmor) {
			yield break;
		}

		Setup();
		float time = Time.time;
		while (DurationTimer < Duration) {
			DurationTimer += Time.time - time;
			time = Time.time;

			Ship.RestoreArmor(PrimaryEffect * UpdateFrequency);
			yield return new WaitForSeconds(UpdateFrequency);
		}

		TearDown();
	}

	public void Setup() {

		//TODO: Art -- Create a drone which circles the player while this ability is active.
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