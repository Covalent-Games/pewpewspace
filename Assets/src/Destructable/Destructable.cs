using UnityEngine;
using System.Collections;


public class Destructable : MonoBehaviour {

	#region Members

	public int maxHealth;
	public float maxDissipation;
	//public int maxShields;
	public float baseSpeed;
	
	[SerializeField]
	int health;
	[SerializeField]
	float dissipation;
	//int shields;
	public float Speed;
	
	float regenTimer;
	
	[SerializeField]
	public bool Invulnerable = false;
	[SerializeField]
	public bool InvulnerableArmor = false;
	[SerializeField]
	public bool InvulnerableDissipation = false;
	//public bool InvulnerableShield = false;
	
	#endregion
	
	#region Properties	
	public int Health {
		get { return this.health; }
		set {
			this.health = value;
			if(this.health <= 0) {
				End ();
			}
			if (this.health > this.maxHealth){
				this.health = this.maxHealth;
			}
		}
	}
	public float Dissipation {
		get {
			return this.dissipation;
		}
		set {
			this.dissipation = value;
			if (this.dissipation < 0){
				this.dissipation = 0f;
			}
		}
	}

	/*
	public int Shields {
		get {
			return this.shields;
		}
		set {
			this.shields = value;
			if (this.shields > this.maxShields){
				this.shields = this.maxShields;
			}
		}
	}
	*/
	#endregion
	
	// Dissipation change: no longer damages shields.
	public int DamageShip(int Damage){
	
		if (this.Invulnerable){ 
				return this.Health;
		}
		if (!this.InvulnerableArmor){
			this.Health -= Damage;
			return this.Health;
		}
		
		return 0;
	}
	
	public int DamageArmor(int Damage){

		if (!this.Invulnerable & !this.InvulnerableArmor){
			this.Health -= Damage;
			return this.Health;
		}

		return this.Health;
	}
	/*
	public int DamageShields(int Damage){
	
		if (!this.Invulnerable & !this.InvulnerableShield){
			this.Shields -= Damage;
			return this.Shields;
		}

		return this.Health;
	}
	*/
	public int RestoreArmor(int restoreAmount){
	
		this.Health += restoreAmount;
		return this.Health;
	}

	public float RestoreDissipation(float cooldown) {

		this.Dissipation -= cooldown;
		return this.Dissipation;
	}

	protected float DissipationCooldown() {

		float cooldown = this.maxDissipation / 10f * Time.deltaTime;
		return RestoreDissipation(cooldown);
	}


	
	/*
	public int RestoreShields(int restoreAmount){
	
		this.Shields += restoreAmount;
		return this.Shields;
	}
	
	protected int ShieldRegen(){
		
		//TODO: Add a simple way of including a modifier, ie +5% shield regen
		int regen = (int)Mathf.Round(this.maxShields/100f);

		return RestoreShields(regen);
	}
	*/

	void Start () {
		
		this.health = this.maxHealth;
	}

	void End(){
		
		//FIXME The comment below is a lie, and I don't currently know the fix. It's a rare bug. 'gameObject'
		// itself is a member of the physical GameObject and so referencing gameObject raises an error.
		// The object sometimes tries to be destroyed twice in one frame. This check prevents that.
		if (gameObject != null){
			Destroy(gameObject);
		}
	}
	
	public void SetUpBaseAttributes(){
	
		this.Health = this.maxHealth;
		this.Dissipation = 0f;
		//this.Shields = this.maxShields;
		this.Speed = this.baseSpeed;
	}

	public void Update () {

		DissipationCooldown();
		//this.regenTimer += Time.deltaTime;
		//if (this.regenTimer > 1) {
		//	DissipationCooldown();
		//	//ShieldRegen();
		//	this.regenTimer = 0f;
		//}
	}
}
