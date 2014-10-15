using UnityEngine;
using System.Collections;


public class BaseShipAI : Destructable {

	[SerializeField]
    public GameObject target;
    public GameObject[] players;
    public Vector3 Destination;
	
	public void AcquireTarget(){
	
		target = players[Random.Range(0, players.Length -1)];
	}
}
