﻿using UnityEngine;
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
	public float Damage;
	/// <summary>
	/// The damage modifier. Change this to modify damage. Do not access Damage directly.
	/// </summary>
	public float DamageMod = 0f;
	public float triggerValue;
	public int PlayerNumber;

	IAbility Ability1;
	IAbility Ability2;
	IAbility Ability3;
	IAbility Ability4;
	public ShipType ShipClass;
	public Player PlayerObject;
	public ShipMovement Movement;
	public Transform Target;
	public Transform Turret;
	public List<ShipObject> InRange = new List<ShipObject>();
	public List<Modifier> ActiveConditions = new List<Modifier>();
	public List<Modifier> ActiveBoons = new List<Modifier>();
	public float FireCost;

	// HUD elements
	public Image HealthBar;
	public Image DissipationBar;
	public TargetCursor EnemyCursor;
	public TargetCursor PlayerCursor;

	// Overheat variables
	public bool Overheated;
	float coolAmount;
	float overheatTime;
	float overheatTimer;
	float originalSpeed;

	#region Audio

	AudioClip SFX_DefaultTurret;

	#endregion



	public void Start() {

		gameObject.AddComponent("ConditionHandler");
		gameObject.AddComponent("BoonHandler");

		if (!AbilityUtils.IsPlayer(this)) {
			Turret = transform.FindChild("Turret");
			BaseShipAI ai = GetComponent<BaseShipAI>();
			ai.StartCoroutine(ai.DissipateThreat());
		}

		GetAudioReferences();
	}

	public void SetupPlayer(int playerNumber) {

		PlayerNumber = playerNumber;

		GameValues.Players.TryGetValue(playerNumber, out PlayerObject);
		if (PlayerObject == null) {
			Debug.LogWarning("Had to manually create a new Player! Something may be wrong. Ignore this if testing.");
			PlayerObject = new Player(playerNumber);
		}
		Movement = GetComponent<ShipMovement>();
		Movement.player = PlayerObject;

		SetUpBaseAttributes();
		FireCost = 0;
		Overheated = false;
		AcquireHud();
		AssignAbilities();

		enabled = true;
		GetComponent<ShipMovement>().enabled = true;

		// Initialize targeting cursors. TODO: This might need to be somewhere else. More graphics may eventually be needed.
		if (AbilityUtils.IsPlayer(this)) {
			GameObject enemyCursor = (GameObject)Instantiate(
				Resources.Load("Art/GUIPrefabs/TargetEnemyCursorObject"),
				Vector3.zero,
				Quaternion.Euler(new Vector3(90, 0, 0)));
			GameObject playerCursor = (GameObject)Instantiate(
				Resources.Load("Art/GUIPrefabs/TargetPlayerCursorObject"),
				Vector3.zero,
				Quaternion.Euler(new Vector3(90, 0, 0)));
			GameObject turret = (GameObject)Instantiate(
				Resources.Load("PlayerShips/Turret"),
				transform.position,
				transform.rotation);
			turret.transform.parent = transform;

			EnemyCursor = enemyCursor.GetComponent<TargetCursor>();
			PlayerCursor = playerCursor.GetComponent<TargetCursor>();
			Turret = turret.transform;
			StartCoroutine(DelayLoad());
		}
	}

	IEnumerator DelayLoad() {

		yield return new WaitForSeconds(0.1f);
		transform.FindChild("AllyRangeDetector").GetComponent<SphereCollider>().enabled = true;
	}

	/// <summary>
	/// Assigns the player HUD UI elements
	/// </summary>
	void AcquireHud() {

		// TODO: (Jesse) Put this on a "HUD" object in the scene, or even just on the 
		// sceneHandler, and have it just display based on ships available instead of 
		// being attached to the ship itself.
		GameObject healthGO = GameObject.Find(string.Format("Player{0}ArmorBar", PlayerNumber));
		GameObject heatGo = GameObject.Find(string.Format("Player{0}DissipationBar", PlayerNumber));
		HealthBar = healthGO.GetComponent<Image>();
		DissipationBar = heatGo.GetComponent<Image>();
	}

	void AssignAbilities() {

		switch (ShipClass) {
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
				Ability4 = AddAbility("CarpetBomb");
				break;
			case ShipType.Valkyrie:
				Ability1 = AddAbility("DesynchronizationBurst");
				Ability2 = AddAbility("ExplosiveShot");
				Ability3 = AddAbility("RapidFire");
				Ability4 = AddAbility("RadarJam");
				break;
		}
	}

	IAbility AddAbility(string name) {

		return (IAbility)gameObject.AddComponent(ShipObject.AbilityDict[name]);
	}

	/// <summary>
	/// Assigns all AudioLibrary AuidClip references to class members.
	/// </summary>
	void GetAudioReferences() {

		AudioLibrary library = FindObjectOfType<AudioLibrary>();

		if (AbilityUtils.IsPlayer(this)) {
			SFX_DefaultTurret = library.DefaultTurret;
		}

		SFX_Explosion = library.Explosion_01;
	}

	void HandleInput() {

		triggerValue = Input.GetAxis(PlayerObject.Controller.LeftRightTrigger);
		if (triggerValue < InputCode.AxisThresholdNegative && shotTimer >= GetShotTime()) {
			shotTimer = 0f;
			Fire();
		}
		if (Input.GetButtonDown(PlayerObject.Controller.ButtonA)) {
			if (Heat < MaxHeat && !Ability1.Executing) {
				Ability1.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(PlayerObject.Controller.ButtonB)) {
			if (Heat < MaxHeat && !Ability2.Executing) {
				Ability2.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(PlayerObject.Controller.ButtonX)) {
			if (Heat < MaxHeat && !Ability3.Executing) {
				Ability3.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(PlayerObject.Controller.ButtonY)) {
			if (Heat < MaxHeat && !Ability4.Executing) {
				Ability4.Begin(GetComponent<ShipObject>());
			}
		}
		if (Input.GetButtonDown(PlayerObject.Controller.LeftBumper)) {
			UnTarget();
		}
	}

	public float GetShotTime() {

		return 1f / shotPerSecond;
	}


	void UpdateShotTimer() {

		shotTimer += Time.deltaTime;
	}

	/// <summary>
	/// Creates a projectile object facing the same direction as the Turret child object.
	/// </summary>
	public void Fire() {

		Vector3 projectileOrigin = transform.position;
		GameObject projectileGO = (GameObject)Instantiate(
				projectilePrefab,
				projectileOrigin,
				Turret.rotation);

		IProjectile projectile = projectileGO.GetComponent(typeof(IProjectile)) as IProjectile;
		// TODO: It would be nice to not have to do  
		projectileGO.transform.Rotate(new Vector3(90, 0, 0));

		// If the player has no target
		if (Target == null) {
			projectile.Direction = Vector3.up;
		} else {
			projectile.Target = Target.GetComponent<ShipObject>();
		}
		projectile.Damage = GetDamage();
		projectile.Owner = this;

		// TODO: match standard fire heat generation with cooldown
		Heat += FireCost;

		AudioLibrary.Play(SFX_DefaultTurret);
	}

	void FindNewTarget() {

		// 
		if (PlayerObject == null) { return; }

		if (Movement.AimingTurret | Target == null) {
			RaycastHit hitInfo;

			bool rayHit = Physics.Raycast(
					transform.position,
					Turret.forward,
					out hitInfo,
					Mathf.Infinity,
					Transform.FindObjectOfType<SceneHandler>().TargetingLayerMask);

			if (rayHit) {
				string tag = hitInfo.transform.gameObject.tag;
				TargetCursor cursor;
				if (tag == "Enemy" & Target != hitInfo.transform) {
					cursor = EnemyCursor;
					PlayerCursor.Tracking = null;
				} else {
					return;
				}

				Target = hitInfo.transform;
				cursor.Tracking = hitInfo.transform;

				cursor.ThisRenderer.enabled = true;
			}
		}
	}

	void UnTarget() {

		Target = null;
		EnemyCursor.GetComponent<TargetCursor>().Tracking = null;
		PlayerCursor.GetComponent<TargetCursor>().Tracking = null;
	}

	public float GetDamage() {

		return Damage + DamageMod;
	}

	/// <summary>
	/// Updates the player's health and dissipation bars
	/// </summary>
	void UpdateHUD() {

		float remainingArmor = (float)(MaxArmor - Armor);

		float healthRatio = remainingArmor / (float)MaxArmor;
		float dissipationRatio = Heat / MaxHeat;

		HealthBar.fillAmount = healthRatio;
		DissipationBar.fillAmount = dissipationRatio > 1f ? 1f : dissipationRatio;

	}

	public override void Update() {

		if (Heat < MaxHeat && !Overheated) {
			UpdateShotTimer();
			FindNewTarget();
			HandleInput();
			base.Update();
		} else {
			Overheat();
		}

		UpdateHUD();
	}

	public void Overheat() {

		if (!Overheated) {
			Overheated = true;
			// 1. Slow player by 25%
			originalSpeed = Speed;
			Speed *= 0.75f;
			float overheatedBy = Heat - MaxHeat;
			Heat = MaxHeat;
			// 2. Calculate how long the player will overheat for
			overheatTime = Mathf.Sqrt(overheatedBy);
			if (overheatTime < 3f)
				overheatTime = 3f;
			coolAmount = MaxHeat * 0.25f;
			overheatTimer = 0f;
		}
		if (Overheated) {
			// 3. Overheat loop
			float cooldown = coolAmount / overheatTime * Time.deltaTime;
			Heat -= cooldown;
			overheatTimer += Time.deltaTime;

			if (overheatTimer > overheatTime) {
				// 4. Return player to normal
				Speed = originalSpeed;
				Overheated = false;
			}
		}
	}

	void OnTriggerEnter(Collider collider) {

		Destructible destructable = collider.GetComponent<Destructible>();

		if (destructable) {
			// On collision with another destructable object, deal 15% of max health as Damage
			destructable.DamageArmor(Mathf.RoundToInt(MaxArmor * 0.15f));
		}

	}
}