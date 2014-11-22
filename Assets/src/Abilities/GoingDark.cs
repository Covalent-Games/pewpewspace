using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GoingDark : BaseAbility, IAbility{	
	
	public void Start() {
		
		Cost = 0f;
		Duration = 1f/60f;
	}
	
	public void Begin(ShipAction ship){
		
		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();

		while (DurationTimer < Duration){
			DurationTimer += Time.deltaTime;
			// ABILITY DURATION LOGIC
			yield return new WaitForFixedUpdate();
		}

		TearDown();
	}
	
	public void Setup(){
		
		Executing = true;
		Ship.Dissipation += Cost;
	}
	
	public void TearDown(){
		
		Executing = false;
		DurationTimer = 0f;
	}
	
	public override void TriggerEnter(Collider collider){}
	
	public override void TriggerStay(Collider collider){}
	
	public override void TriggerExit(Collider collider){}
}