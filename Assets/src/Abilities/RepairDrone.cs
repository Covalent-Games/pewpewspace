using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RepairDrone : BaseAbility, IAbility {

	public void Start() {

		Resource = Resources.Load("Effects/RepairDroneEffect");
		if (Resource == null)
			Debug.LogError("Resource RepairDroneEffect did not load properly");
		Cost = 75f;
		PrimaryEffect = 75;
	}

	public void Begin(ShipObject ship) {

		Name = "Repair Drone";
		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
		DisplayName(Name);
	}

	public IEnumerator Execute() {

		Setup();
		foreach (ShipObject ship in Ship.InRange) {
			if (ship) {
				StartCoroutine(CreateEffect(ship));
				ship.RestoreArmor(PrimaryEffect);
			}
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

	public IEnumerator CreateEffect(ShipObject ship) {

		var effect = (GameObject)Instantiate(Resource, ship.transform.position, Quaternion.identity);
		effect.transform.parent = ship.transform;
		effect.transform.Rotate(-90, -180, -180);
		yield return new WaitForSeconds(2);
		Destroy(effect);
	}

	public override void TriggerEnter(Collider collider) { }

	public override void TriggerStay(Collider collider) { }

	public override void TriggerExit(Collider collider) { }
}