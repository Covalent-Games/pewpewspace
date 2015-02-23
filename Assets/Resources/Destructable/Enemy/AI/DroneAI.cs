using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneAI : BaseShipAI {

	Dictionary<AIState, float> Distances = new Dictionary<AIState, float>();

	void Start() {

		Setup();
		AcquireTarget();
		AcquireDestination();
		BuildDistanceTable();
	}

	void Move() {

		// If you have no target and are not at your last destination, go somewhere else.
		if (!BaseShip.Target && transform.position == Destination) {
			AcquireDestination();
		} else if (BaseShip.Target) {
			float distancePercent = Distances[State] /
					Vector3.Distance(transform.position, BaseShip.Target.position);
			float x_Change = transform.position.x - BaseShip.Target.position.x;
			float z_Change = transform.position.z - BaseShip.Target.position.z;
			float xCoord = BaseShip.Target.position.x + (x_Change * distancePercent);
			float zCoord = BaseShip.Target.position.z + (z_Change * distancePercent);

			Destination = new Vector3(xCoord, 0, zCoord);
		}

		transform.position = Vector3.MoveTowards(
				transform.position,
				Destination,
				Time.deltaTime * BaseShip.Speed);
		transform.LookAt(Destination);

	}

	void AcquireDestination() {

		float xpos = Random.Range(0f, 1f);
		float ypos = Random.Range(0f, 1f);
		this.Destination = Camera.main.ViewportToWorldPoint(
				new Vector3(xpos, ypos, Camera.main.transform.position.y));
	}

	void BuildDistanceTable() {

		Distances.Add(AIState.Defensive, 60f);
		Distances.Add(AIState.Balanced, 30f);
		Distances.Add(AIState.Aggressive, 15f);
	}

	public override IEnumerator AIUpdate() {

		// Yield one frame to ensure everything is set up
		yield return null;

		yield return new WaitForSeconds(BaseShip.GetShotTime());

		while (true) {
			AcquireTarget();
			Fire();
			BaseShip.shotPerSecond = Random.Range(.75f, 2f);
			yield return new WaitForSeconds(BaseShip.GetShotTime());
		}
	}

	void Update() {

		Move();
	}
}
