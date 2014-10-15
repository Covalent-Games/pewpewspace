using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

public class ShipAction : Destructable {

	public GameObject projectilePrefab;
	float shotTimer;
	public float shotPerSecond;
	public int damage;
	public float triggerValue;
	
	delegate void Ability(ShipAction shipAction);
	Ability AbilityOne;
	Ability AbilityTwo;
	Ability AbilityThree;
	Ability AbilityFour;

	// HUD elements
	public GameObject healthBar;
	public GameObject shieldBar;
	public GameObject AbilityOneIcon;
	public GameObject AbilityTwoIcon;
	public GameObject AbilityThreeIcon;
	public GameObject AbilityFourIcon;

	void Start(){
		
		SetUpBaseAttributes();
		this.shotPerSecond = 1f/this.shotPerSecond;
		PopulateAbilityDict();
		AcquireHud();
	}
	
	void PopulateAbilityDict(){
		
		var methods = typeof(GuardianAbilities).GetMethods(BindingFlags.Static | BindingFlags.Public);
	}

	/// <summary>
	/// Assigns the player HUD UI elements
	/// </summary>
	void AcquireHud() {
		
		healthBar = GameObject.Find("HealthBar");
		shieldBar = GameObject.Find("ShieldBar");
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

	/// <summary>
	/// Updates the player's health and shield bars
	/// </summary>
	void UpdateData() {
	
		float healthRatio = (float)this.Health/(float)this.maxHealth;
		float shieldRatio = (float)this.Shields/(float)this.maxShields;

		this.healthBar.GetComponent<Slider>().value = healthRatio;
		this.shieldBar.GetComponent<Slider>().value = shieldRatio;
		
	}

	void Update () {

		UpdateShotTimer();
		HandleInput();
		UpdateData();
	}
	
	public void AIUpdate(){
	
		UpdateShotTimer();
		if (this.shotTimer >= this.shotPerSecond){
			this.shotTimer = 0f;
			Fire();
		}
	}
}
