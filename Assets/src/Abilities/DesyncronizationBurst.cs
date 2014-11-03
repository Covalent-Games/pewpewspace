using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DesyncronizationBurst : BaseAbility, IAbility{

    [SerializeField]
    AreaOfEffectSphere Sphere;

	public void Start() {
		
		this.Cost = 35;
		this.Duration = 4f;

        // Effect = reduction by 35%
        this.PrimaryEffect = 35;    
        this.Condition = Condition.Speed;
        this.Resource = Resources.Load("AbilityObjects/AreaOfEffectSphere", typeof(GameObject));
    }
	
	public void Begin(ShipAction ship){
		
		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();

		while (DurationTimer < Duration){
			DurationTimer += Time.deltaTime;
			yield return new WaitForFixedUpdate();
		}

		TearDown();
	}
	
	public void Setup(){

        this.Ship.Shields -= this.Cost;
		Executing = true;

        var sphere = (GameObject)Instantiate(Resource, Ship.transform.position, Quaternion.identity);
        this.Sphere = sphere.GetComponent<AreaOfEffectSphere>();
        this.Sphere.transform.position = Ship.transform.position;
        this.Sphere.ModifySphereRadius(9);
        this.Sphere.Ability = this;
	}
	
	public void TearDown(){
		
		Executing = false;
		DurationTimer = 0f;
        Destroy(this.Sphere.gameObject);
        this.Sphere = null;
	}
	
	public override void TriggerEnter(Collider collider){

        ShipAction target = collider.GetComponent<ShipAction>();
        if (target == null) { return; }
        target.GetComponent<ConditionHandler>().ApplyCondition(this.Condition, this.PrimaryEffect, this.Duration);
    }
	
	public override void TriggerStay(Collider collider){}
	
	public override void TriggerExit(Collider collider){}
}