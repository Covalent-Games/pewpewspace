using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class ShipAction : Destructable {

	public static Dictionary<string, IAbility> AbilityDict = new Dictionary<string, IAbility>();

	public GameObject projectilePrefab;
	float shotTimer;
	public float shotPerSecond;
	public int damage;
	public float triggerValue;
	
	IAbility AbilityOne;
	IAbility AbilityTwo;
	IAbility AbilityThree;
	IAbility AbilityFour;
	public ShipType Type;

	void Start(){
		
		SetUpBaseAttributes();
		this.shotPerSecond = 1f/this.shotPerSecond;
		AbilityOne = ShipAction.AbilityDict["BullRush"];
	}

	void UpdateShotTimer(){
	
		this.shotTimer += Time.deltaTime;
	}

	void HandleInput(){
	
		triggerValue = Input.GetAxis(InputCode.LeftRightTrigger);
		if (triggerValue < InputCode.AxisThresholdNegative && this.shotTimer >= this.shotPerSecond){
			this.shotTimer = 0f;
			Fire();
		}
		if (Input.GetButtonDown(InputCode.PlayerOneAbilityOne)){
			AbilityOne.Start(this);
		}
	}
	
	void Fire(){
	
		// TODO: Projectile is rotated incorrectly... just rotating the projectileOrigin or the projectile prefab doesn't fix it.
		// NOTE: The "* 2" at the end moves the bullet ahead of the ship enough not to collide with the ship
		Vector3 projectileOrigin = transform.position;
		Transform turret = transform.FindChild("Turret");
		GameObject projectileGO = (GameObject)Instantiate(this.projectilePrefab, projectileOrigin, turret.rotation);
		
		Projectile projectile = projectileGO.GetComponent<Projectile>();
		// TODO: It would be nice to not have to do this. 
		projectile.transform.Rotate(new Vector3(90, 0, 0));
		projectile.direction = Vector3.up;
		projectile.damage = damage;
	}

	void Update () {

		UpdateShotTimer();
		HandleInput();
	}
	
	public void AIUpdate(){
	
		UpdateShotTimer();
		if (this.shotTimer >= this.shotPerSecond){
			this.shotTimer = 0f;
			Fire();
		}
	}
}
