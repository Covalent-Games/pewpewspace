using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BaseShipAI : MonoBehaviour {

	public ShipAction actions;

	[SerializeField]
    public ShipAction target;
    public Vector3 Destination;
	
	public void AcquireTarget(){
	
		target = SceneHandler.PlayerShips[Random.Range(0, SceneHandler.PlayerShips.Count)];
	}
}
