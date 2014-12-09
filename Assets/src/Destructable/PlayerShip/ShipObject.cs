using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.UI;

public class ShipObject : Destructable {
	
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
	public Transform Turret;
	public List<Condition> ActiveConditions = new List<Condition>();
	public List<Boon> ActiveBoons = new List<Boon>();
	public float fireCost;
	public bool overheated;
	
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
		}
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
		this.fireCost = this.maxDissipation / 10f * this.shotPerSecond;
		//Debug.Log("Fire cost = " + fireCost);
		this.overheated = false;
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
				Ability2 = AddAbility("BullRush");
				Ability3 = AddAbility("SustainDrone");
			break;
			case ShipType.Outrunner:
				Ability1 = AddAbility("SalvageConversionRounds");
				Ability2 = AddAbility("EmpowerOther");
				Ability3 = AddAbility("BatteryDrone");
				break;
			case ShipType.Raider:
                Ability1 = AddAbility("DeconstructionLaser");
                Ability2 = AddAbility("ReaperMan");
                Ability3 = AddAbility("EnergyMissilePods");
				break;
			case ShipType.Valkyrie:
                Ability1 = AddAbility("DesyncronizationBurst");
				Ability2 = AddAbility("ExplosiveShot");
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
		
		triggerValue = Input.GetAxis(player.Controller.LeftRightTrigger);
		if (triggerValue < InputCode.AxisThresholdNegative && this.shotTimer >= this.shotPerSecond){
			this.shotTimer = 0f;
			Fire();
		}
		if (Input.GetButtonDown(player.Controller.ButtonA)){
			if (Dissipation < this.maxDissipation && !Ability1.Executing){
				Ability1.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(player.Controller.ButtonB)){
			if (Dissipation < this.maxDissipation && !Ability2.Executing) {
				Ability2.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(player.Controller.ButtonX)){
			if (Dissipation < this.maxDissipation && !Ability3.Executing) {
				Ability3.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(player.Controller.ButtonY)){
			if (Dissipation < this.maxDissipation && !Ability4.Executing) {
				Ability4.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(player.Controller.LeftBumper)){
			UnTarget();
		}
	}
	
	void Fire(){

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

		// TODO: match standard fire heat generation with cooldown
		Dissipation += this.fireCost;
	}
	
	void FindNewTarget(){
		
		// 
		if (player == null) { return; }

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
		
		float healthRatio = (float)this.Health/(float)this.maxHealth;
		float dissipationRatio = this.Dissipation/this.maxDissipation;
		
		this.healthBar.GetComponent<Image>().fillAmount = healthRatio;
		this.dissipationBar.GetComponent<Image>().fillAmount = dissipationRatio > 1f ? 1f : dissipationRatio;
		
	}
	
	void Update () {
		
		if (this.Dissipation < this.maxDissipation && !overheated) {
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

		if (!this.overheated) {
			this.overheated = true;
			//this.dissipationBar.GetComponentInChildren<Animator>().enabled = true;
			//Animation animation = this.dissipationBar.GetComponentInChildren<Animation>();
			//Debug.Log("animation = " + animation);
			//bool animationWorked = animation.Play();
			//if (!animationWorked) {
			//	Debug.Log("Animation did not play");
			//}
			// 1. Slow player by 25%
			originalSpeed = this.Speed;
			this.Speed *= 0.75f;
			float overheatedBy = this.Dissipation - this.maxDissipation;
			this.Dissipation = this.maxDissipation;
			// 2. Calculate how long the player will overheat for
			overheatTime = Mathf.Sqrt(overheatedBy);
			Debug.Log("Overheat time = " + overheatTime);
			if (overheatTime < 3f)
				overheatTime = 3f;
			coolAmount = this.maxDissipation * 0.25f;
			overheatTimer = 0f;
		} 
		if (this.overheated) {
			// 3. Overheat loop
			//Debug.Log("cooling..." + overheatTimer);
			float cooldown = coolAmount / overheatTime * Time.deltaTime;
			this.Dissipation -= cooldown;
			overheatTimer += Time.deltaTime;

			if (overheatTimer > overheatTime) {
				// 4. Return player to normal
				//this.dissipationBar.GetComponentInChildren<Animator>().enabled = false;
				//this.dissipationBar.GetComponentInChildren<Animator>().gameObject.GetComponent<Image>().color = Color.white;
				//this.dissipationBar.GetComponentInChildren<Animation>().Stop();
				this.Speed = originalSpeed;
				this.overheated = false;
			}
		}
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