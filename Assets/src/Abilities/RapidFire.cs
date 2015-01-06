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
		Debug.Log("Rapid Fire");
	}
	
	public IEnumerator Execute(){
		
		Setup();

		// Prevent player from using this ability again until boon wears off
		while (DurationTimer < Duration){
			DurationTimer += Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}

		TearDown();
		
	}
	
	public void Setup(){

        Ship.Heat += this.Cost;
		foreach (ShipObject ship in SceneHandler.PlayerShips) {

			if (ship) {
				ship.GetComponent<BoonHandler>().ApplyBoon(Boon.FireRate, AbilityID.RapidFire, this.Percentage, this.Duration);
			}
		}
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