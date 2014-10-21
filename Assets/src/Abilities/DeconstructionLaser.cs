using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DeconstructionLaser : BaseAbility, IAbility{	
	
	public void Begin(ShipAction ship){
		
		this.Ship = ship;
		this.ShipMove = ship.GetComponent<ShipMovement>();
		this.ShipClass = ship.ShipClass;
		Cost = 0; // DEFINE
		Duration = 1f/60f; //DEFINE (Currently set to run for one frame)
		StartCoroutine("Execute");
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
	}
	
	public void TearDown(){
		
		Executing = false;
		DurationTimer = 0f;
	}
}