using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BaseShipAI : MonoBehaviour {

	public ShipObject BaseShip;
	public int ThreatDissipationSpeed = 2;
	public int DistancePerception = -1;
	public Dictionary<ShipObject, int> ThreatTable = new Dictionary<ShipObject, int>();

	[SerializeField]
    public ShipObject target;
    public Vector3 Destination;
	

	/// <summary>
	/// Chooses a target based on a threat algorithm.
	/// </summary>
	public void AcquireTarget(){

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
			if (target != null) { return; }

			int index;

			// There's definitely a faster way to do this.
			List<ShipObject> possibleTargets = new List<ShipObject>();
			foreach (ShipObject ship in SceneHandler.PlayerShips) {
				if (ship.CanBeTargetted) {
					possibleTargets.Add(ship);
				}
			}

			if (possibleTargets.Count == 0) {
				// TODO: In this case, the AI should just fire a random shot, which I believe has
				// already been added via another branch. 
				Debug.Log("FIRE EVERYWHERE!");
				return;
			}

			index = Random.Range(0, possibleTargets.Count);
			target = possibleTargets[index];

		} else {
			// Start threat below in case no one has generated threat and all are at 0.
			int highestThreat = DistancePerception ;
			foreach (var threatObject in ThreatTable){

				// Skip the players who are in the table but are posing no threat at all
				// If all players are posing no threat one will be randomly chosen
				if (threatObject.Value == 0) {
					continue;
				}

				// Distance is inverted to negative so that the mod decreases and distance increases
				float distanceModifier = Vector3.Distance(transform.position, threatObject.Key.transform.position) / 4;
				int threat = threatObject.Value - Mathf.RoundToInt(distanceModifier);

				if (threat > highestThreat){
					target = threatObject.Key;
					highestThreat = threat;
				}
			}
		}
	}

	/// <summary>
	/// Slowly lowers threat of all ShipObjects within the threat table.
	/// </summary>
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
}
