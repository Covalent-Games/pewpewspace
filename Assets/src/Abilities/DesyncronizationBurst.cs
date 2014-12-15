using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DesyncronizationBurst : BaseAbility, IAbility{

    [SerializeField]
    ColliderHelper Sphere;

	public void Start() {
		
		Cost = 35f;
		Duration = 4f;

        // Effect = reduction by 35%
        this.PrimaryEffect = 35;    
        this.Condition = Condition.Speed;
        this.Resource = Resources.Load("AbilityObjects/ColliderHelper", typeof(GameObject));
    }
	
	public void Begin(ShipObject ship){
		
		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();

        //NOTE: Quick burst or lingering field?
		yield return null;

		TearDown();
	}
	
	public void Setup(){

        this.Ship.Heat += this.Cost;
		Executing = true;

        var sphere = (GameObject)Instantiate(Resource, Ship.transform.position, Quaternion.identity);
        this.Sphere = sphere.GetComponent<ColliderHelper>();
        this.Sphere.transform.position = Ship.transform.position;
        this.Sphere.ModifySphereRadius(9);
        this.Sphere.Ability = this;
	}
	
	public void TearDown(){
		
		Executing = false;
		DurationTimer = 0f;
        Destroy(this.Sphere.gameObject);
        Sphere = null;
	}
	
	public override void TriggerEnter(Collider collider){

        ShipObject target = collider.GetComponent<ShipObject>();
        if (target == null) { return; }
        target.GetComponent<ConditionHandler>().ApplyCondition(
				Condition, 
				AbilityID.DesyncBurst, 
				PrimaryEffect, 
				Duration, 
				true);
    }
	
	public override void TriggerStay(Collider collider){}
	
	public override void TriggerExit(Collider collider){}
}