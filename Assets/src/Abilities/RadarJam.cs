using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RadarJam : BaseAbility, IAbility{

	ColliderHelper Field;

	public void Start() {

		print("Attached Radar Jam");
		Cost = 35f;
		Duration = 5f;
		Resource = Resources.Load("AbilityObjects/RadarJamCone");
	}
	
	public void Begin(ShipObject ship){
		
		Ship = ship;
		print("Attached ship: " + Ship.gameObject.name);
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();

		GameObject radarCone = (GameObject)Instantiate(Resource, Ship.Turret.transform.position, Ship.Turret.transform.rotation);
		radarCone.transform.parent = Ship.Turret;
		radarCone.transform.localScale = new Vector3(12f, 12f, 12f);
		radarCone.transform.Rotate(-90f, 0f, 0f);
		this.Field = radarCone.GetComponent<ColliderHelper>();
		this.Field.Ability = this;
		
		while (!Input.GetButtonDown(Ship.player.Controller.RightStickPress)){
			yield return null;
		}

		radarCone.GetComponent<MeshCollider>().enabled = true;
		yield return null;
		Destroy(radarCone);
		TearDown();
	}
	
	public void Setup(){

		Executing = true;
	}
	
	public void TearDown(){

		Ship.Heat += Cost;
		Executing = false;
		DurationTimer = 0f;
		Destroy(this.Field.gameObject);
		this.Field = null;
	}
	
	public override void TriggerEnter(Collider collider){

		print("Collided with an enemy");
		if(collider.tag != "Enemy") {
			return;
		}
		collider.GetComponent<ConditionHandler>().ApplyCondition(Condition.Targeting, 0, Duration);
	}
	
	public void TriggerStay(Collider collider){}
	
	public void TriggerExit(Collider collider){}
}