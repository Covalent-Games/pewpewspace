using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RapidFire : BaseAbility, IAbility{

	public void Start() {
		
		Cost = 53f;
		Duration = 5f;
		Percentage = 0.2f;
	}
	
	public void Begin(ShipObject ship){
		
		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();

		foreach (var ship in SceneHandler.PlayerShips) {
			ship.GetComponent<BoonHandler>().ApplyBoon(Boon.FireRate, this.Percentage, this.Duration);
		}

		// Prevent player from using this ability again until boon wears off
		while (DurationTimer < Duration){
			DurationTimer += Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}

		TearDown();
	}
	
	public void Setup(){

        Ship.Dissipation += this.Cost;
		Executing = true;
	}
	
	public void TearDown(){
		
		Executing = false;
		DurationTimer = 0f;
	}
	
	public override void TriggerEnter(Collider collider){}
	
	public override void TriggerStay(Collider collider){}
	
	public override void TriggerExit(Collider collider){}
}