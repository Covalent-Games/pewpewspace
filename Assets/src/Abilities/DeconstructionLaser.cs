using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DeconstructionLaser : BaseAbility, IAbility{	
	
	public void Start() {
		
		Cost = 15;
		Duration = 0.5f;
        Damage = 50;

        Resource = Resources.Load("AbilityObjects/DeconstructionLaserObject");
	}
	
	public void Begin(ShipAction ship){
		
		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();

        Vector3 originShift = new Vector3(0, 0, 50) + Ship.transform.position;
        GameObject laser = (GameObject)Instantiate(Resource, originShift, Quaternion.identity);
        laser.GetComponent<AreaOfEffectSphere>().Ability = this;

        while (DurationTimer < Duration) {
            DurationTimer += Time.deltaTime;
            laser.transform.position = Ship.transform.position;
            yield return new WaitForFixedUpdate();
        }

        Destroy(laser);
        
        TearDown();
	}
	
	public void Setup(){
		
		Executing = true;
        Ship.Dissipation += Cost;
	}
	
	public void TearDown(){

        Executing = false;
		DurationTimer = 0f;
	}
	
	public override void TriggerEnter(Collider collider){

        ShipAction target = collider.GetComponent<ShipAction>();
        if (target == null) {  return; };
        target.DamageShip(this.Damage);
    }
	
	public void TriggerStay(Collider collider){}
	
	public void TriggerExit(Collider collider){}
}