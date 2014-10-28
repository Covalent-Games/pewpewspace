using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SalvageConversionRounds : BaseAbility, IAbility{
	
	GameObject CustomProjectile;
	GameObject OriginalProjectile;
	
	public void Start() {
		
		Cost = 10; // DEFINE
		Duration = 1f/60f; //DEFINE (Currently set to run for one frame)
		string path = "PlayerShips/ShipObjects/SalvageConversionProjectile";
		CustomProjectile = (GameObject)Resources.Load(path, typeof(GameObject));
	}
	
	public void Begin(ShipAction ship){
		
		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		Toggle = !Toggle;
		Debug.Log("Salvage Conversion Rounds is on: " + Toggle);
		StartCoroutine(Execute());
	}

	
	public IEnumerator Execute(){
		
		if (Toggle){
			Setup();
		}

		while (Toggle){
			// TODO: This might not be responsive enough at 1 second.
			yield return new WaitForSeconds(1f);
			if (Ship.Shields < Cost){
				Toggle = false;
				TearDown();
				yield break;
			}
			Ship.Shields -= Cost;
		}

		TearDown();
	}
	
	public void Setup(){
		
		OriginalProjectile = Ship.projectilePrefab;
		Ship.projectilePrefab = CustomProjectile;
	}
	
	public void TearDown(){
	
		Ship.projectilePrefab = OriginalProjectile;
	}
	
	public void TriggerEnter(Collider collider){}
	
	public void TriggerStay(Collider collider){}
	
	public void TriggerExit(Collider collider){}
}