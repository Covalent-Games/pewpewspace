using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SustainDrone : BaseAbility, IAbility{	
	
	public void Start() {
		
		Cost = 50f;
		Duration = 7f;
		PrimaryEffect = 10;
		SecondaryEffect = 6;
	}
	
	public void Begin(ShipObject ship){
		
		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();
		float lastFrameTime = Time.time;
		while (DurationTimer < Duration){
		
			// Custom delta time since Time.deltaTime only returns the time since the last frame.
			DurationTimer += Time.time - lastFrameTime;
			lastFrameTime = Time.time;

			//if (Ship.Shields < Ship.maxShields/2f){
			//	Ship.RestoreShields(PrimaryEffect);
			//} else {
				Ship.RestoreArmor(SecondaryEffect);
			//}
			yield return new WaitForSeconds(.25f);
		}

		TearDown();
	}
	
	public void Setup(){
		
		//TODO: Art -- Create a drone which circles the player while this ability is active.
		Executing = true;
	}
	
	public void TearDown(){
		
		Executing = false;
		DurationTimer = 0f;
	}
	
	public void TriggerEnter(Collider collider){}
	
	public void TriggerStay(Collider collider){}
	
	public void TriggerExit(Collider collider){}
}