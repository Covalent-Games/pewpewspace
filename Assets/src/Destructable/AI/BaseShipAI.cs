using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BaseShipAI : MonoBehaviour {

	public ShipObject actions;

	[SerializeField]
    public ShipObject target;
    public Vector3 Destination;
	
	public void AcquireTarget(){

		if (SceneHandler.PlayerShips.Count <= 0) {
			return;
		}
		int index = Random.Range(0, SceneHandler.PlayerShips.Count);
		Debug.Log(this.name + " is targeting " + index.ToString());
		target = SceneHandler.PlayerShips[index];
	}
}
