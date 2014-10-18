using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BaseShipAI : MonoBehaviour {

	public ShipAction actions;

	[SerializeField]
    public ShipAction target;
    public List<ShipAction> players = new List<ShipAction>();
    public Vector3 Destination;
	
	public void AcquireTarget(){
	
		target = players[Random.Range(0, players.Count -1)];
	}
}
