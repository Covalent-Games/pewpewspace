using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ExplosiveShot : BaseAbility, IAbility{	
	
	public void Start() {

		Resource = Resources.Load("AbilityObjects/ExplosiveShotProjectile", typeof(GameObject));
		if (Resource == null) {
			Debug.LogError("Resource ExplosiveShotProjectile did not load properly");
		}
		Cost = 18;
		Duration = 1f/60f;
		Damage = 15;
	}
	
	public void Begin(ShipAction ship){
		
		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();

		yield return new WaitForFixedUpdate();

		TearDown();
	}
	
	public void Setup(){

		Ship.Dissipation += Cost;
		Executing = true;

		GameObject explosiveShotProjectile = (GameObject)Instantiate(Resource, Ship.transform.position, Ship.Turret.rotation);
		ExplosiveShotProjectile projectile = explosiveShotProjectile.GetComponent<ExplosiveShotProjectile>();
		projectile.Damage = this.Damage;
		projectile.Direction = Ship.Turret.rotation.eulerAngles;
		projectile.damageRadius = 15f;

	}
	
	public void TearDown(){
		
		Executing = false;
		DurationTimer = 0f;
	}
	
	public override void TriggerEnter(Collider collider){}
	
	public override void TriggerStay(Collider collider){}
	
	public override void TriggerExit(Collider collider){}
}