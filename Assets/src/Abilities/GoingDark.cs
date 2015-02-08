using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GoingDark : BaseAbility, IAbility {

	public void Start() {

		Name = "Goind Dark";
		Cost = 35f;
		Duration = 3f;
		Resource = Resources.Load("AbilityObjects/GoingDarkVeil");
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
		GameObject veil = (GameObject)Instantiate(Resource, transform.position, Quaternion.identity);
		veil.transform.Rotate(Vector3.right, 90f);

		while (DurationTimer < Duration) {
			DurationTimer += Time.deltaTime;
			veil.transform.position = transform.position + new Vector3(0, .5f, 0);
			yield return new WaitForEndOfFrame();
		}

		Destroy(veil);
		TearDown();
	}

	public void Setup() {

		Executing = true;
		Ship.Heat += Cost;
		Ship.CanBeTargetted = false;

		// Reduce all current enemy's threat table entries for this player to 0.
		foreach (ShipObject enemy in SceneHandler.Enemies.ToArray()) {
			BaseShipAI ai = enemy.GetComponent<BaseShipAI>();
			var table = ai.ThreatTable;
			foreach (ShipObject ship in new List<ShipObject>(table.Keys)) {
				if (ship == Ship) {
					table[ship] = 0;
				}
			}

			if (ai.BaseShip.Target == Ship.transform) {
				ai.BaseShip.Target = null;
			}
		}
	}

	public void TearDown() {

		Ship.CanBeTargetted = true;
		Executing = false;
		DurationTimer = 0f;
	}

	public override void TriggerEnter(Collider collider) { }

	public override void TriggerStay(Collider collider) { }

	public override void TriggerExit(Collider collider) { }
}