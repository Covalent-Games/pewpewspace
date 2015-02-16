using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Destructible : MonoBehaviour {

	#region Members

	public List<List<ShipObject>> Containers = new List<List<ShipObject>>();

	public float MaxArmor;
	public float MaxHeat;
	public float baseSpeed;

	[SerializeField]
	float armor;
	[SerializeField]
	float heat;
	public float Speed;
	public bool CanBeTargetted = true;
	public bool CanTarget = true;

	[SerializeField]
	public bool Invulnerable = false;
	[SerializeField]
	public bool InvulnerableArmor = false;
	[SerializeField]
	public bool InvulnerableDissipation = false;

	GameObject FloatingDamage;
	public GameObject FloatingText;

	protected AudioClip SFX_Explosion;

	// Delegate for destroy callback
	public delegate void OnDestroyDelegate();
	public OnDestroyDelegate OnDestroy;



	#endregion

	#region Properties
	public float Armor {
		get {
			return this.armor;
		}
		set {
			this.armor = value;
			if (this.armor <= 0f) {
				End();
			}
			if (this.armor > this.MaxArmor) {
				this.armor = this.MaxArmor;
			}
		}
	}
	public float Heat {
		get {
			return this.heat;
		}
		set {
			this.heat = value;
			if (this.heat < 0) {
				this.heat = 0f;
			}
		}
	}

	#endregion

	void Awake() {

		FloatingDamage = Resources.Load("GUIPrefabs/FloatingDamage") as GameObject;
		FloatingText = Resources.Load("GUIPrefabs/FloatingText") as GameObject;
	}

	void Start() {

		this.armor = this.MaxArmor;
	}

	/// <summary>
	/// Deals non tracked damage. The offender will not be recorded.
	/// </summary>
	/// <param name="damage">The amount of damage.</param>
	/// <returns></returns>
	public float DamageArmor(float damage) {

		if (!this.Invulnerable) {
			this.Armor -= damage;

			DisplayFloatingDamage(damage);

			return this.Armor;
		}

		return this.Armor;
	}

	/// <summary>
	/// Deals tracked damage. The offender will be recorded.
	/// </summary>
	/// <param name="damage">The amount of damage.</param>
	/// <param name="offender">The ShipObject that dealt the damage.</param>
	/// <returns></returns>
	public float DamageArmor(float damage, ShipObject offender) {

		float armor = DamageArmor(damage);
		BaseShipAI ai = GetComponent<BaseShipAI>();
		if (ai != null) {
			if (ai.ThreatTable.ContainsKey(offender)) {
				ai.ThreatTable[offender] += Mathf.RoundToInt(damage);
			} else {
				ai.ThreatTable.Add(offender, Mathf.RoundToInt(damage));
			}
		}

		return armor;
	}

	public float RestoreArmor(float restoreAmount) {

		this.Armor += restoreAmount;
		return this.Armor;
	}

	public float RestoreDissipation(float cooldown) {

		this.Heat -= cooldown;
		return this.Heat;
	}

	protected float DissipationCooldown() {

		float cooldown = this.MaxHeat / 10f * Time.deltaTime;
		return RestoreDissipation(cooldown);
	}

	// TODO: This should be more generic, provide an optional text argument, and hover direction.
	private void DisplayFloatingDamage(float damage) {

		GameObject guiElement = (GameObject)Instantiate(FloatingDamage, transform.position, Quaternion.identity);
		Transform textTransform = guiElement.transform.FindChild("Text");
		textTransform.GetComponent<Text>().text = Mathf.RoundToInt(damage).ToString();
	}

	public virtual void End() {

		//FIXME The comment below is a lie, and I don't currently know the fix. It's a rare bug. 'gameObject'
		// itself is a member of the physical GameObject and so referencing gameObject raises an error.
		// The object sometimes tries to be destroyed twice in one frame. This check prevents that.
		if (gameObject != null) {
			// Remove the object from all associated lists.
			foreach (var container in Containers) {
				container.Remove(GetComponent<ShipObject>());
			}
			// OnDestroy callback
			if (OnDestroy != null) {
				OnDestroy();
			}
			var explosions = GameObject.FindObjectOfType<SceneHandler>().Explosions;
			var explosion = explosions[Random.Range(1, explosions.Count) - 1];
			Instantiate(explosion, transform.position, Quaternion.identity);

			if (SFX_Explosion) {
				AudioLibrary.Play(SFX_Explosion);
			}

			Destroy(gameObject);
		}
	}

	public void SetUpBaseAttributes() {

		this.Armor = this.MaxArmor;
		this.Heat = 0f;
		//this.Shields = this.maxShields;
		this.Speed = this.baseSpeed;
	}

	/// <summary>
	/// Adds this entity to containers. If the entity is destroyed it will be removed from all containers.
	/// </summary>
	/// <param name="containers">Any number of containers to add this entity to.</param>
	/// <returns>Exhaustive list of all containers this entity is referenced.</returns>
	public List<List<ShipObject>> AddContainers(params List<ShipObject>[] containers) {

		ShipObject ship = GetComponent<ShipObject>();

		foreach (var container in containers) {
			container.Add(ship);
			Containers.Add(container);
		}

		return Containers;
	}

	public virtual void Update() {

		DissipationCooldown();
	}
}
