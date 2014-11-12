using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BatteryDrone : BaseAbility, IAbility{	
	
	public void Start() {
		
		Cost = 75;
		PrimaryEffect = 100;
	}
	
	public void Begin(ShipAction ship){
		
		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();
		foreach(ShipAction ship in SceneHandler.PlayerShips){
		//	ship.RestoreShields((int)PrimaryEffect);
		}
		yield return new WaitForEndOfFrame();
		TearDown();
	}
	
	public void Setup(){
		
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