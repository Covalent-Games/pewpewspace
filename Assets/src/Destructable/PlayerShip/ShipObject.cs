using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

public class ShipObject : Destructible {
	
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
	public Player PlayerObject;
	public Transform Target;
	public Transform Turret;
	public List<Modifier> ActiveConditions = new List<Modifier>();
	public List<Modifier> ActiveBoons = new List<Modifier>();
	public float FireCost;
	public bool Overheated;
	
	// HUD elements
	public GameObject healthBar;
	public GameObject dissipationBar;
	public GameObject Ability1Icon;
	public GameObject Ability2Icon;
	public GameObject Ability3Icon;
	public GameObject Ability4Icon;
	public TargetCursor EnemyCursor;
	public TargetCursor PlayerCursor;
	
	// Overheat variables
	float coolAmount;
	float overheatTime;
	float overheatTimer;
	float originalSpeed;

	
	public void Start(){
		
		gameObject.AddComponent("ConditionHandler");
		gameObject.AddComponent("BoonHandler");
	
		if (!AbilityUtils.IsPlayer(this)) {
			Turret = transform.FindChild("Turret");
			BaseShipAI ai = GetComponent<BaseShipAI>();
			ai.StartCoroutine(ai.DissipateThreat());
		}
	}
	
	public void SetupPlayer(int playerNumber){
		
		this.PlayerNumber = playerNumber;
		
		GameValues.Players.TryGetValue(playerNumber, out this.PlayerObject);
		if (this.PlayerObject == null){
			Debug.LogWarning("Had to manually create a new Player! Something may be wrong. Ignore this if testing.");
			this.PlayerObject = new Player(playerNumber);
		}
		GetComponent<ShipMovement>().player = this.PlayerObject;
		
		SetUpBaseAttributes();
		FireCost = 0;
		Overheated = false;
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
			GameObject turret = (GameObject)Instantiate(
				Resources.Load("PlayerShips/ShipObjects/Turret"),
				transform.position,
				transform.rotation);
			turret.transform.parent = this.transform;
			
			EnemyCursor = enemyCursor.GetComponent<TargetCursor>();
			PlayerCursor = playerCursor.GetComponent<TargetCursor>();
			Turret = turret.transform;
		}
	}
	
	void AssignAbilities(){
		
		switch (ShipClass){
			case ShipType.Guardian:
				Ability1 = AddAbility("SonicDisruption");
				Ability2 = AddAbility("ShieldCover");
				Ability3 = AddAbility("BullRush");
				Ability4 = AddAbility("SustainDrone");
			break;
			case ShipType.Outrunner:
				Ability1 = AddAbility("SalvageConversionRounds");
				Ability2 = AddAbility("EmpowerOther");
				Ability3 = AddAbility("RepairDrone");
				Ability4 = AddAbility("GoingDark");
				break;
			case ShipType.Raider:
				Ability1 = AddAbility("DeconstructionLaser");
				Ability2 = AddAbility("ReaperMan");
				Ability3 = AddAbility("EnergyMissilePods");
				break;
			case ShipType.Valkyrie:
				Ability1 = AddAbility("DesyncronizationBurst");
				Ability2 = AddAbility("ExplosiveShot");
				Ability3 = AddAbility("RapidFire");
				Ability4 = AddAbility("RadarJam");
				break;
		}	
	}

	IAbility AddAbility(string name){
		
		return (IAbility)gameObject.AddComponent(ShipObject.AbilityDict[name]);
	}
	
	/// <summary>
	/// Assigns the player HUD UI elements
	/// </summary>
	void AcquireHud() {
		
		// TODO: (Jesse) Put this on a "HUD" object in the scene, or even just on the 
		// sceneHandler, and have it just display based on ships available instead of 
		// being attached to the ship itself.
		healthBar = GameObject.Find(string.Format("Player{0}ArmorBar", PlayerNumber));
		dissipationBar = GameObject.Find(string.Format("Player{0}DissipationBar", PlayerNumber));
	}
	
	void UpdateShotTimer(){
		
		this.shotTimer += Time.deltaTime;
	}
	
	void HandleInput(){
		
		triggerValue = Input.GetAxis(PlayerObject.Controller.LeftRightTrigger);
		if (triggerValue < InputCode.AxisThresholdNegative && this.shotTimer >= GetShotTime()){
			this.shotTimer = 0f;
			Fire();
		}
		if (Input.GetButtonDown(PlayerObject.Controller.ButtonA)){
			if (Heat < this.MaxHeat && !Ability1.Executing){
				Ability1.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(PlayerObject.Controller.ButtonB)){
			if (Heat < this.MaxHeat && !Ability2.Executing) {
				Ability2.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(PlayerObject.Controller.ButtonX)){
			if (Heat < this.MaxHeat && !Ability3.Executing) {
				Ability3.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(PlayerObject.Controller.ButtonY)){
			if (Heat < this.MaxHeat && !Ability4.Executing) {
				Ability4.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(PlayerObject.Controller.LeftBumper)){
			UnTarget();
		}
	}

	public float GetShotTime() {

		return 1f / this.shotPerSecond;
	}
	
	/// <summary>
	/// Creates a projectile object facing the same direction as the Turret child object.
	/// </summary>
	public void Fire(){

		Vector3 projectileOrigin = transform.position;
		GameObject projectileGO = (GameObject)Instantiate(
				this.projectilePrefab,
				projectileOrigin,
				Turret.rotation);
		
		IProjectile projectile = projectileGO.GetComponent(typeof(IProjectile)) as IProjectile;
		// TODO: It would be nice to not have to do this. 
		projectileGO.transform.Rotate(new Vector3(90, 0, 0));

		// If the player has no target
		if (Target == null) {
			projectile.Direction = Vector3.up;
		} else if (Target != null && AbilityUtils.IsPlayer(Target.GetComponent<ShipObject>())){
			projectile.Direction = Vector3.up;
		} else {
			projectile.Target = Target.GetComponent<ShipObject>();
		}
		projectile.Damage = GetDamage();
		projectile.Owner = this;

		// TODO: match standard fire heat generation with cooldown
		Heat += FireCost;
	}
	
	void FindNewTarget(){
		
		// 
		if (PlayerObject == null) { return; }

		if (GetComponent<ShipMovement>().AimingTurret | Target == null) {
		RaycastHit hitInfo;

		bool rayHit = Physics.Raycast(
				transform.position,
				Turret.forward, 
				out hitInfo, 
				Transform.FindObjectOfType<SceneHandler>().TargetingLayerMask);

			if (rayHit) {
			string tag = hitInfo.transform.gameObject.tag;
			TargetCursor cursor;
				if (tag == "Enemy" & Target != hitInfo.transform) {
				cursor = EnemyCursor;
				PlayerCursor.Tracking = null;
				} else if (tag == "Player" & Target != hitInfo.transform) {
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
	/// Updates the player's health and dissipation bars
	/// </summary>
	void UpdateHUD() {
		
		float healthRatio = (float)this.Armor/(float)this.MaxArmor;
		float dissipationRatio = this.Heat/this.MaxHeat;
		
		this.healthBar.GetComponent<Image>().fillAmount = healthRatio;
		this.dissipationBar.GetComponent<Image>().fillAmount = dissipationRatio > 1f ? 1f : dissipationRatio;
		
	}
	
	void Update () {
		
		if (this.Heat < this.MaxHeat && !Overheated) {
			UpdateShotTimer();
			FindNewTarget();
			HandleInput();
			base.Update();
		} else {
			Overheat();
		}

		//TODO: Could we change this to UpdateHUD or something similar? UpdateData() seems kind of ambiguous nested in Update(). 
		UpdateHUD();
	}

	public void Overheat() {

		if (!this.Overheated) {
			this.Overheated = true;
			// 1. Slow player by 25%
			originalSpeed = this.Speed;
			this.Speed *= 0.75f;
			float overheatedBy = this.Heat - this.MaxHeat;
			this.Heat = this.MaxHeat;
			// 2. Calculate how long the player will overheat for
			overheatTime = Mathf.Sqrt(overheatedBy);
			Debug.Log("Overheat time = " + overheatTime);
			if (overheatTime < 3f)
				overheatTime = 3f;
			coolAmount = this.MaxHeat * 0.25f;
			overheatTimer = 0f;
		} 
		if (this.Overheated) {
			// 3. Overheat loop
			float cooldown = coolAmount / overheatTime * Time.deltaTime;
			this.Heat -= cooldown;
			overheatTimer += Time.deltaTime;

			if (overheatTimer > overheatTime) {
				// 4. Return player to normal
				this.Speed = originalSpeed;
				this.Overheated = false;
			}
		}
	}
	
	public void AIUpdate(){
		
		UpdateShotTimer();
		// FIXME: this.shotPerSecond is actually seconds per shot, but shots per second for the players. *shrug*.
		if (shotTimer >= shotPerSecond){
			shotTimer = 0f;
			BaseShipAI ai = GetComponent<BaseShipAI>();
			ai.AcquireTarget();
			if (ai.target) {
				Transform turret = transform.FindChild("Turret");
				turret.LookAt(ai.target.transform.position);
				Fire();
			} else {
				Debug.Log(gameObject.name + " has no target this round");
			}
		}
	}
	
	void OnTriggerEnter(Collider collider){
		
		Destructible destructable = collider.GetComponent<Destructible>();
		
		if (destructable == null) { return; }
		
		// On collision with another destructable object, deal 10% of max health as Damage
		destructable.DamageArmor(Mathf.RoundToInt(MaxArmor/10f));
	}
}