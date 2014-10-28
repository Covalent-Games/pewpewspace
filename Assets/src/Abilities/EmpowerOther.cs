using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EmpowerOther : BaseAbility, IAbility{	
	
	public void Start() {
		
		Cost = 50;
		Duration = 1f/60f;
		string path = "AbilityObjects/EmpowerOtherProjectile";
		Resource = Resources.Load(path, typeof(GameObject));
		Duration = 5f;
		Percentage = 0.35f;
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
		Debug.Log(Resource);
		GameObject projectileGO = (GameObject)Instantiate(
				Resource,
				Ship.transform.position, 
				Quaternion.identity);
		EmpowerOtherProjectile projectile = projectileGO.GetComponent<EmpowerOtherProjectile>();

		SetTarget(projectile);
		
		StartCoroutine(projectile.TrackToTarget());
	}
	
	void SetTarget(EmpowerOtherProjectile projectile){
		
		// The player has a target
		if (Ship.Target != null){
			// The target is a player
			ShipAction target = Ship.Target.GetComponent<ShipAction>();
			if (AbilityUtils.IsPlayer(target)){
				projectile.Target = target;	
			// The target is not a player, so we target the player that triggered the ability
			} else {
				projectile.Target = Ship;
			}
		// There is no target, so we randomly find an ally to target
		} else {
			//TODO: Check if the targeted player has this boon already
			int index = Random.Range(0, SceneHandler.PlayerShips.Count);
			ShipAction friendlyTarget = SceneHandler.PlayerShips[index];
			projectile.Target = friendlyTarget;
		}
		
		if (projectile.Target == null) {
			Debug.LogError("EmpowerOther target did not succesfully set!");
		}
		
		projectile.DamageModifier = Percentage;
		projectile.Duration = Duration;
		
	}
	
	public void TearDown(){
		
		Executing = false;
		DurationTimer = 0f;
	}
	
	public void TriggerEnter(Collider collider){}
	
	public void TriggerStay(Collider collider){}
	
	public void TriggerExit(Collider collider){}
}