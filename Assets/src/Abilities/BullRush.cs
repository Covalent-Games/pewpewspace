using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BullRush : BaseAbility, IAbility{
	
	private Vector3 moveTowards;
	
	public void Begin(ShipAction ship){
		
		this.Ship = ship;
		this.ShipMove = ship.GetComponent<ShipMovement>();
		this.ShipClass = ship.ShipClass;
		Cost = 50;
		Duration = 0.3f;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();
		while (DurationTimer < Duration){
			DurationTimer += Time.deltaTime;
			ShipMove.MoveShip(Vector3.MoveTowards(Ship.transform.position, moveTowards, Time.deltaTime * ShipMove.moveSpeed * 4));
			yield return new WaitForFixedUpdate();
		}
		TearDown();
	}
	
	public void Setup(){
		
		Executing = true;
		moveTowards = new Vector3(0, 0, 20f) + Ship.transform.position;
		ShipMove.moveEnabled = false;
		Ship.Invulnerable = true;
		Ship.Shields -= Cost;
	}
	
	public void TearDown(){
		
		Executing = false;
		ShipMove.moveEnabled = true;
		Ship.Invulnerable = false;
		DurationTimer = 0f;
	}
}