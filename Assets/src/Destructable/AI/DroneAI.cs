using UnityEngine;
using System.Collections;

public class DroneAI : BaseShipAI {

	void Start () {
		
		BaseShip = GetComponent<ShipObject>();
		BaseShip.SetUpBaseAttributes();
		AcquireTarget();
		AcquireDestination();
		BaseShip.Start();
	}
	
	void Move(){
	
		
		transform.position = Vector3.MoveTowards(transform.position, this.Destination, Time.deltaTime * 5 * BaseShip.Speed);
		if (transform.position == this.Destination){
			AcquireDestination();
		}
	}
	
	void AcquireDestination(){
	
		float xpos = Random.Range(0f, 1f);
		float ypos = Random.Range(0f, 1f);
		this.Destination = Camera.main.ViewportToWorldPoint(new Vector3(xpos, ypos, Camera.main.transform.position.y));
	}
	
	void Update () {
	
		BaseShip.AIUpdate();
		Move();
	}
}
