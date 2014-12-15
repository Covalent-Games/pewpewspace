using UnityEngine;
using System.Collections;

public class DroneAI : BaseShipAI {

	void Start () {
		
		actions = GetComponent<ShipObject>();
		actions.SetUpBaseAttributes();
		AcquireTarget();
		AcquireDestination();
		actions.Start();
	}
	
	void Move(){
	
		
		transform.position = Vector3.MoveTowards(transform.position, this.Destination, Time.deltaTime * 5 * actions.Speed);
		if (transform.position == this.Destination){
			AcquireDestination();
		}
	}
	
	void AimAndShoot(){

		if (target != null) {
		Transform turret = transform.FindChild("Turret");
		turret.LookAt(this.target.transform.position);
		}
		actions.AIUpdate();
	}
	
	void AcquireDestination(){
	
		float xpos = Random.Range(0f, 1f);
		float ypos = Random.Range(0f, 1f);
		this.Destination = Camera.main.ViewportToWorldPoint(new Vector3(xpos, ypos, 40f));
	}
	
	void Update () {
	
		Move();
		AimAndShoot();
	}
}
