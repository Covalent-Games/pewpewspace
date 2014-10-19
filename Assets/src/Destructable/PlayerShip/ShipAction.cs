using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

public class ShipAction : Destructable {

	public static Dictionary<string, System.Type> AbilityDict = new Dictionary<string, System.Type>();

	public GameObject projectilePrefab;
	float shotTimer;
	public float shotPerSecond;
	public int damage;
	public float triggerValue;
	public int PlayerNumber;
	
	IAbility AbilityOne;
	IAbility AbilityTwo;
	IAbility AbilityThree;
	IAbility AbilityFour;
	public ShipType Type;
	public Player player;

	// HUD elements
	public GameObject healthBar;
	public GameObject shieldBar;
	public GameObject AbilityOneIcon;
	public GameObject AbilityTwoIcon;
	public GameObject AbilityThreeIcon;
	public GameObject AbilityFourIcon;

	void Start(){
	}
	
	public void SetupPlayer(int playerNumber){
	
		this.PlayerNumber = playerNumber;
		
		GameValues.Players.TryGetValue(playerNumber, out this.player);
		if (this.player == null){
			Debug.LogWarning("Had to manually create a new Player! Something may be wrong. Ignore this if testing.");
			this.player = new Player(playerNumber);
		}
		GetComponent<ShipMovement>().player = this.player;
		
		/*These 4 lines have been moved from Start() because a lot of 
		this logic will depend on which player is controlling the ship.*/
		SetUpBaseAttributes();
		this.shotPerSecond = 1f/this.shotPerSecond;
		AcquireHud();
		AssignAbilities();
		
		this.enabled = true;
		GetComponent<ShipMovement>().enabled = true;
	}
	
	void AssignAbilities(){
	
		this.AbilityOne = (IAbility)System.Activator.CreateInstance(ShipAction.AbilityDict["BullRush"]);
	}

	/// <summary>
	/// Assigns the player HUD UI elements
	/// </summary>
	void AcquireHud() {
		
		// TODO: (Jesse) Put this on a "HUD" object in the scene, or even just on the 
		// sceneHandler, and have it just display based on ships available instead of 
		// being attached to the ship itself.
		healthBar = GameObject.Find("HealthBar");
		shieldBar = GameObject.Find("ShieldBar");
	}

	void UpdateShotTimer(){
	
		this.shotTimer += Time.deltaTime;
	}

	void HandleInput(){
	
		triggerValue = Input.GetAxis(player.Controller.LeftRightTrigger);
		if (triggerValue < InputCode.AxisThresholdNegative && this.shotTimer >= this.shotPerSecond){
			this.shotTimer = 0f;
			Fire();
		}
		if (Input.GetButtonDown(player.Controller.ButtonA)){
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
		base.Update();
		//TODO: Could we change this to UpdateHUD or something similar? UpdateData() seems kind of ambiguous nested in Update(). 
		UpdateData();
	}
	
	public void AIUpdate(){
	
		UpdateShotTimer();
		// FIXME: this.shotPerSecond is actually seconds per shot, but shots per second for the players. *shrug*.
		if (this.shotTimer >= this.shotPerSecond){
			this.shotTimer = 0f;
			Fire();
		}
	}
	
	void OnTriggerEnter(Collider collider){
		
		Destructable destructable = collider.GetComponent<Destructable>();
		
		if (destructable == null) { return; }
		
		// On collision with another destructable object, deal 10% of max health as damage
		destructable.DamageArmor(Mathf.RoundToInt(this.maxHealth/10f));
	}
}
