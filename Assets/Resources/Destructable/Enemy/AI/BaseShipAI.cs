using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BaseShipAI : MonoBehaviour {

	public ShipObject BaseShip;
	public int ThreatDissipationSpeed = 2;
	public int DistancePerception = -1;
	public AIState State = AIState.Balanced;

	public Dictionary<ShipObject, int> ThreatTable = new Dictionary<ShipObject, int>();

	public Vector3 Destination;
	public Vector3 Velocity;
	public Vector3 DirectionToTarget {
		get {
			if (BaseShip.Target) {
				return Vector3.Normalize(BaseShip.Target.position - transform.position) * BaseShip.baseSpeed;
			} else {
				return Vector3.zero;
			}
		}
	}

	protected void Setup() {

		BaseShip = GetComponent<ShipObject>();
		BaseShip.SetUpBaseAttributes();
		BaseShip.Start();
		StartCoroutine(AIUpdate());
		StartCoroutine(DeterimineState());
	}

	/// <summary>
	/// Chooses a target based on a threat algorithm.
	/// </summary>
	public void AcquireTarget() {

		if (!BaseShip.CanTarget) {
			BaseShip.Target = null;
			GoNuts();
			return;
		}

		if (SceneHandler.PlayerShips.Count <= 0) {
			return;
		}

		// TODO: Cut this down to 1 loop instead of 2
		bool noThreatFound = true;
		foreach (int threat in ThreatTable.Values) {
			if (threat > 0) {
				noThreatFound = false;
			}
		}

		// If no player has generated threat, pick one randomly
		// Otherwise, pick the player with the highest generated threat.
		if (noThreatFound) {

			// TODO: Determine if this should be here. It might be skipping needed logic
			// If we already have a target we don't need to randomly pick a new one.
			if (BaseShip.Target != null) { return; }

			int index;

			// There's definitely a faster way to do this.
			List<ShipObject> possibleTargets = new List<ShipObject>();
			foreach (ShipObject ship in SceneHandler.PlayerShips) {
				if (ship.CanBeTargetted) {
					possibleTargets.Add(ship);
				}
			}

			if (possibleTargets.Count == 0) {
				//TODO: In this case the AI won't actually fire -- just rotate the turret. 
				GoNuts();
				return;
			}

			index = Random.Range(0, possibleTargets.Count);
			BaseShip.Target = possibleTargets[index].transform;

		} else {
			// Start threat below in case no one has generated threat and all are at 0.
			int highestThreat = DistancePerception;
			foreach (var threatObject in ThreatTable) {

				// Skip the players who are in the table but are posing no threat at all
				// If all players are posing no threat one will be randomly chosen
				if (threatObject.Value == 0) {
					continue;
				}

				// Distance is inverted to negative so that the mod decreases and distance increases
				float distanceModifier = Vector3.Distance(transform.position, threatObject.Key.transform.position) / 4;
				int threat = threatObject.Value - Mathf.RoundToInt(distanceModifier);

				if (threat > highestThreat) {
					BaseShip.Target = threatObject.Key.transform;
					highestThreat = threat;
				}
			}
		}
	}

	private void GoNuts() {

		int fireAngle = Random.Range(0, 360);
		BaseShip.Turret.Rotate(0, fireAngle, 0);
	}

	public void Fire() {

		if (BaseShip.Target)
			BaseShip.Turret.LookAt(BaseShip.Target.position);
		else if (!BaseShip.Target && SceneHandler.PlayerShips.Count > 0)
			GoNuts();
		else
			return;

		Vector3 projectileOrigin = transform.position;
		GameObject projectileGO = (GameObject)Instantiate(
				BaseShip.projectilePrefab,
				projectileOrigin,
				BaseShip.Turret.rotation);

		IProjectile projectile = projectileGO.GetComponent(typeof(IProjectile)) as IProjectile;
		// TODO: It would be nice to not have to do this. 
		projectileGO.transform.Rotate(new Vector3(90, 0, 0));

		projectile.Direction = Vector3.up;
		projectile.Damage = BaseShip.GetDamage();
		projectile.Owner = BaseShip;
	}

	public IEnumerator DissipateThreat() {

		while (true) {
			foreach (ShipObject ship in new List<ShipObject>(ThreatTable.Keys)) {
				int threat = ThreatTable[ship];
				threat -= ThreatDissipationSpeed;

				if (threat < 0) {
					threat = 0;
				}

				ThreatTable[ship] = threat;
			}

			yield return new WaitForSeconds(1f);
		}
	}


	IEnumerator DeterimineState() {

		while (enabled) {
			if (State != AIState.Balanced && BaseShip.Armor <= BaseShip.MaxArmor * 0.3f) {
				State = AIState.Balanced;
				renderer.material.color = Color.red;
			}
			yield return new WaitForSeconds(.25f);
		}
	}

	public virtual IEnumerator AIUpdate() {

		// Yield one frame to ensure everything is set up
		yield return null;

		yield return new WaitForSeconds(BaseShip.GetShotTime());

		while (true) {
			AcquireTarget();
			Fire();
			yield return new WaitForSeconds(BaseShip.GetShotTime());
		}
	}
}
