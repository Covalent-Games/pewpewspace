using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BaseShipAI : MonoBehaviour {

	public ShipObject actions;
	public int ThreatDissipationSpeed = 2;
	public Dictionary<ShipObject, int> ThreatTable = new Dictionary<ShipObject, int>();

	[SerializeField]
    public ShipObject target;
    public Vector3 Destination;
	public bool CanTarget = true;
	
	public void AcquireTarget(){

		if (!CanTarget) {
			target = null;
			GoNuts();
			return;
		}

		if (SceneHandler.PlayerShips.Count <= 0) {
			return;
		}

		bool noThreatFound = true;
		foreach (int threat in ThreatTable.Values) {
			if (threat > 0) {
				noThreatFound = false;
			}
		}

		// If no player has generated threat, pick one randomly
		// Otherwise, pick the player with the highest generated threat.
		if (noThreatFound) {
			
			// If we already have a target we don't need to randomly pick a new one.
			if (target != null) { return; }

			int index;
			index = Random.Range(0, SceneHandler.PlayerShips.Count);
			target = SceneHandler.PlayerShips[index];

		} else {
			// Start threat below in case no one has generated threat and all are at 0.
			int highestThreat = -1;
			foreach (var threatObject in ThreatTable){
				if (threatObject.Value > highestThreat){
					Debug.Log(threatObject.Key.gameObject.name + " -- " + threatObject.Value);
					target = threatObject.Key;
					highestThreat = threatObject.Value;
				}
			}
		}
	}

	private void GoNuts() {

		int fireAngle = Random.Range(0, 360);
		actions.Turret.Rotate(0, fireAngle, 0);
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
}
