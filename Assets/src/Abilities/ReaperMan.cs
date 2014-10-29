using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ReaperMan : BaseAbility, IAbility{	
	
	public void Start() {
        
        Resource = Resources.Load("AbilityObjects/ReaperManProjectile");
		Cost = 30;
		Duration = 1f/60f;
        Damage = 100;
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
		
		Executing = true;
        Ship.Shields -= Cost;

        GameObject projectileGO = (GameObject)Instantiate(
                Resource, 
                Ship.transform.position, 
                Ship.transform.rotation);
        ReaperManProjectile projectile = projectileGO.GetComponent<ReaperManProjectile>();
        SetTarget(projectile);
        StartCoroutine(projectile.TrackToTarget());

	}

    void SetTarget(ReaperManProjectile projectile) {

        // Player has no target
        if (Ship.Target == null) {
            int index = Random.Range(0, SceneHandler.Enemies.Count);
            BaseShipAI hostileTarget = SceneHandler.Enemies[index];
            projectile.Target = hostileTarget.actions;
        } else {

            ShipAction target = Ship.Target.GetComponent<ShipAction>();

            // The target is a player
            if (AbilityUtils.IsPlayer(target))
            {
                int index = Random.Range(0, SceneHandler.Enemies.Count);
                BaseShipAI hostileTarget = SceneHandler.Enemies[index];
                projectile.Target = hostileTarget.actions;
            }
            else
            {
                projectile.Target = target;
            }
        }

        if (projectile.Target == null)
        {
            Debug.LogError("ReaperMan target did not succesfully set!");
        }

        projectile.damage = Damage;

    }
	
	public void TearDown(){
		
		Executing = false;
		DurationTimer = 0f;
	}
	
	public override void TriggerEnter(Collider collider){}
	
	public override void TriggerStay(Collider collider){}
	
	public override void TriggerExit(Collider collider){}
}