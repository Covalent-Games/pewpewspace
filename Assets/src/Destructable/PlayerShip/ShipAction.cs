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
	/// <summary>
	/// Unmodified damage. DO NOT call this directly. Use GetDamage() instead.
	/// </summary>
	public int Damage;
	/// <summary>
	/// The damage modifier. Change this to modify damage. Do not access Damage directly.
	/// </summary>
	public int DamageMod = 0;
	public float triggerValue;
	public int PlayerNumber;
	
	IAbility Ability1;
	IAbility Ability2;
	IAbility Ability3;
	IAbility Ability4;
	public ShipType ShipClass;
	public Player player;
	public Transform Target;
	Transform Turret;
	public List<Condition> ActiveConditions = new List<Condition>();
	public List<Boon> ActiveBoons = new List<Boon>();
	
	// HUD elements
	public GameObject healthBar;
	public GameObject shieldBar;
	public GameObject Ability1Icon;
	public GameObject Ability2Icon;
	public GameObject Ability3Icon;
	public GameObject Ability4Icon;
	public TargetCursor EnemyCursor;
	public TargetCursor PlayerCursor;
	
	public void Start(){
		
		gameObject.AddComponent("ConditionHandler");
		gameObject.AddComponent("BoonHandler");
	
		Turret = transform.FindChild("Turret");
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
		
		// Initialize targeting cursors. TODO: This might need to be somewhere else. More graphics may eventually be needed.
		if (AbilityUtils.IsPlayer(this)){
			GameObject enemyCursor = (GameObject)Instantiate(
				Resources.Load("GUIPrefabs/TargetEnemyCursorObject"),
				Vector3.zero,
				Quaternion.Euler(new Vector3(90, 0, 0)));
			GameObject playerCursor = (GameObject)Instantiate(
				Resources.Load("GUIPrefabs/TargetPlayerCursorObject"),
				Vector3.zero,
				Quaternion.Euler(new Vector3(90, 0, 0)));
			
			EnemyCursor = enemyCursor.GetComponent<TargetCursor>();
			PlayerCursor = playerCursor.GetComponent<TargetCursor>();
		}
	}
	
	void AssignAbilities(){
		
		switch (ShipClass){
			case ShipType.Guardian:
				Ability1 = (IAbility)gameObject.AddComponent(ShipAction.AbilityDict["SonicDisruption"]);
				Ability3 = (IAbility)gameObject.AddComponent(ShipAction.AbilityDict["BullRush"]);
				Ability4 = (IAbility)gameObject.AddComponent(ShipAction.AbilityDict["SustainDrone"]);
			break;
			case ShipType.Outrunner:
				Ability1 = AddAbility("SalvageConversionRounds");
				Ability2 = AddAbility("EmpowerOther");
				Ability3 = AddAbility("BatteryDrone");
				break;
			case ShipType.Raider:
				break;
			case ShipType.Valkyrie:
				break;
		}	
	}

	IAbility AddAbility(string name){
		
		return (IAbility)gameObject.AddComponent(ShipAction.AbilityDict[name]);
	}
	
	/// <summary>
	/// Assigns the player HUD UI elements
	/// </summary>
	void AcquireHud() {
		
		// TODO: (Jesse) Put this on a "HUD" object in the scene, or even just on the 
		// sceneHandler, and have it just display based on ships available instead of 
		// being attached to the ship itself.
		healthBar = GameObject.Find(string.Format("Player{0}ArmorBar", PlayerNumber));
		shieldBar = GameObject.Find(string.Format("Player{0}ShieldBar", PlayerNumber));
	}
	
	void UpdateShotTimer(){
		
		this.shotTimer += Time.deltaTime;
	}
	
	void HandleInput(){
		
		triggerValue = Input.GetAxis(player.Controller.LeftRightTrigger);
		if (triggerValue < InputCode.AxisThresholdNegative && this.shotTimer >= this.shotPerSecond){
			this.shotTimer = 0f;
			Fire();
		} else if (triggerValue > InputCode.AxisThresholdPositive){
			FindNewTarget();
		}
		if (Input.GetButtonDown(player.Controller.ButtonA)){
			if (Shields > Ability1.Cost & !Ability1.Executing){
				Ability1.Begin(GetComponent<ShipAction>());
			}
		}
		if (Input.GetButtonDown(player.Controller.ButtonB)){
			if (Shields > Ability2.Cost & !Ability2.Executing){
				Ability2.Begin(GetComponent<ShipAction>());
			}
		}
		if (Input.GetButtonDown(player.Controller.ButtonX)){
			if (Shields > Ability3.Cost & !Ability3.Executing){
				Ability3.Begin(GetComponent<ShipAction>());
			}
		}
		if (Input.GetButtonDown(player.Controller.ButtonY)){
			if (Shields > Ability4.Cost & !Ability4.Executing){
				Ability4.Begin(GetComponent<ShipAction>());
			}
		}
		if (Input.GetButtonDown(player.Controller.LeftBumper)){
			UnTarget();
		}
	}
	
	void Fire(){
		
		// TODO: Projectile is rotated incorrectly... just rotating the projectileOrigin or the projectile prefab doesn't fix it.
		// NOTE: The "* 2" at the end moves the bullet ahead of the ship enough not to collide with the ship
		Vector3 projectileOrigin = transform.position;
		GameObject projectileGO = (GameObject)Instantiate(
				this.projectilePrefab,
				projectileOrigin,
				Turret.rotation);
		
		IProjectile projectile = projectileGO.GetComponent(typeof(IProjectile)) as IProjectile;
		// TODO: It would be nice to not have to do this. 
		projectileGO.transform.Rotate(new Vector3(90, 0, 0));
		projectile.Direction = Vector3.up;
		projectile.Damage = GetDamage();
	}
	
	void FindNewTarget(){
	
		RaycastHit hitInfo;

		bool rayHit = Physics.Raycast(
				transform.position,
				Turret.forward, 
				out hitInfo, 
				Transform.FindObjectOfType<SceneHandler>().TargetingLayerMask);

		if (rayHit){
			
			/*Vector3 screenPos = Camera.main.WorldToScreenPoint(hitInfo.transform.position);
			if (screenPos.x > 1f | screenPos.x < 0f | screenPos.y > 1f | screenPos.y < 0f){
				return;
			}*/

			string tag = hitInfo.transform.gameObject.tag;
			TargetCursor cursor;
			if (tag == "Enemy" & Target != hitInfo.transform){
				cursor = EnemyCursor;
				PlayerCursor.Tracking = null;
			} else if (tag == "Player" & Target != hitInfo.transform){
				cursor = PlayerCursor;
				EnemyCursor.Tracking = null;
			} else {
				return;
			}
			
			Target = hitInfo.transform;
			cursor.Tracking = hitInfo.transform;
			cursor.ThisRenderer.enabled = true;
		}
	}
	
	void UnTarget(){
	
		Target = null;
		EnemyCursor.GetComponent<TargetCursor>().Tracking = null;
		PlayerCursor.GetComponent<TargetCursor>().Tracking = null;
	}
	
	public int GetDamage(){
	
		return this.Damage + this.DamageMod;
	}
	
	/// <summary>
	/// Updates the player's health and shield bars
	/// </summary>
	void UpdateData() {
		
		float healthRatio = (float)this.Health/(float)this.maxHealth;
		float shieldRatio = (float)this.Shields/(float)this.maxShields;
		
		this.healthBar.GetComponent<Scrollbar>().size = healthRatio;
		this.shieldBar.GetComponent<Scrollbar>().size = shieldRatio;
		
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
		
		// On collision with another destructable object, deal 10% of max health as Damage
		destructable.DamageArmor(Mathf.RoundToInt(this.maxHealth/10f));
	}
}