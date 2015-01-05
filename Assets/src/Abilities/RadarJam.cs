using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RadarJam : BaseAbility, IAbility{

	ColliderHelper Field;
	List<Collider> targets = new List<Collider>();

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
		print("*********executing radarjam");
		GameObject radarCone = (GameObject)Instantiate(Resource, Ship.Turret.transform.position, Ship.Turret.transform.rotation);
		radarCone.transform.parent = Ship.Turret;
		radarCone.transform.localScale = new Vector3(500f, 500f, 1f);
		radarCone.transform.Rotate(-90f, 0f, 0f);
		this.Field = radarCone.GetComponent<ColliderHelper>();
		this.Field.Ability = this;
		
		while (!Input.GetButtonDown(Ship.player.Controller.RightStickPress)){
			yield return null;
		}

		// TODO: does not work every time. Find something more reliable.
		//radarCone.GetComponent<MeshCollider>().enabled = true;
		//yield return null;
		TearDown();
		Destroy(radarCone);
	}
	
	public void Setup(){

		Executing = true;
	}
	
	public void TearDown(){

		Ship.Heat += Cost;
		foreach (var target in targets) {
			target.GetComponent<ConditionHandler>().ApplyCondition(Condition.Targeting, AbilityID.RadarJam, 0, Duration);
		}
		targets.Clear();
		Executing = false;
		DurationTimer = 0f;
		Destroy(this.Field.gameObject);
		this.Field = null;
	}
	
	public override void TriggerEnter(Collider collider){

		if(collider.tag != "Enemy") {
			return;
		}
		if (!targets.Contains(collider)) {
			targets.Add(collider);
		}
	}
	
	public override void TriggerStay(Collider collider){}
	
	public override void TriggerExit(Collider collider){

		if (collider.tag != "Enemy") {
			return;
		}
		if (targets.Contains(collider)) {
			targets.Remove(collider);
		}
	}
}