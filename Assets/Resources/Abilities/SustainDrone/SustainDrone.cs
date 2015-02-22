using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SustainDrone : BaseAbility, IAbility {

	float UpdateFrequency = 0.25f;

	public void Start() {

		Name = "Sustain Drone";
		Cost = 45f;
		Duration = 8f;
		PrimaryEffect = 6;
		Resource = Resources.Load("Abilities/SustainDrone/SustainDroneObject");
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

		GameObject SustainDroneGO = (GameObject)Instantiate(Resource, Ship.transform.position, Ship.transform.rotation);
		SustainDroneGO.transform.parent = Ship.transform;

		StartCoroutine(RotateDrone(SustainDroneGO, Random.rotation));

		float time = Time.time;
		while (DurationTimer < Duration) {
			DurationTimer += Time.time - time;
			time = Time.time;

			Ship.RestoreArmor(PrimaryEffect * UpdateFrequency);
			yield return new WaitForSeconds(UpdateFrequency);
		}

		Destroy(SustainDroneGO);

		TearDown();
	}

	public IEnumerator DroneSpark(GameObject drone) {

		LineRenderer spark = drone.GetComponentInChildren<LineRenderer>();
		spark.SetPosition(0, Ship.transform.position);
		float waitTime;
		while (spark) {
			waitTime = Random.Range(0.2f, 1f);
			yield return new WaitForSeconds(waitTime);
			spark.enabled = !spark.enabled;
		}
	}

	public IEnumerator RotateDrone(GameObject drone, Quaternion nextRotation) {

		bool hasZapped = false;
		float startTime = Time.time;
		float journeyLength = 5f;
		while (drone) {
			float distCovered = (Time.time - startTime) * 3;
			float fracJourney = distCovered / journeyLength;
			drone.transform.rotation = Quaternion.Lerp(drone.transform.rotation, nextRotation, fracJourney);
			if (fracJourney >= 0.9 && !hasZapped) {
				Debug.Log("**ZAP!**");
				hasZapped = true;
			}
			if (fracJourney >= 1) {
				StartCoroutine(RotateDrone(drone, Random.rotation));
				break;
			}
			yield return null;
		}
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