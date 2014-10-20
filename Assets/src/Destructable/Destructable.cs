using UnityEngine;
using System.Collections;


public class Destructable : MonoBehaviour {

	#region Members

	public int maxHealth;
	public int maxShields;
	public float baseSpeed;
	
	[SerializeField]
	int health;
	[SerializeField]
	int shields;
	public float Speed;
	
	float regenTimer;
	
	[SerializeField]
	public bool Invulnerable = false;
	[SerializeField]
	public bool InvulnerableArmor = false;
	[SerializeField]
	public bool InvulnerableShield = false;
	
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
	
	#endregion
	
	public int DamageShip(int damage){
	
		if (this.Invulnerable){ 
			if (this.Shields != 0){
				return this.Shields; 
			} else {
				return this.Health;
			}
		}
		if (this.Shields > 0 && !this.InvulnerableShield){
			this.Shields -= damage;
			return this.Shields;
		} else if (!this.InvulnerableArmor){
			this.Health -= damage;
			return this.Health;
		}
		
		return 0;
	}
	
	public int DamageArmor(int damage){

		if (!this.Invulnerable & !this.InvulnerableArmor){
			Debug.Log(this.InvulnerableArmor.ToString() + this.Invulnerable.ToString());
			this.Health -= damage;
			return this.Health;

		return this.Health;
	}
	
	public int DamageShields(int damage){
	
		if (!this.Invulnerable & !this.InvulnerableShield){
			this.Shields -= damage;
			return this.Shields;

		return this.Health;
	}
	
	public int RestoreArmor(int restoreAmount){
	
		this.Health += restoreAmount;
		return this.Health;
	}
	
	public int RestoreShields(int restoreAmount){
	
		this.Shields += restoreAmount;
		return this.Shields;
	}
	
	protected int ShieldRegen(){
		
		//TODO: Add a simple way of including a modifier, ie +5% shield regen
		int regen = (int)Mathf.Round(this.maxShields/100f);

		return RestoreShields(regen);
	}

	void Start () {
		
		this.health = this.maxHealth;
	}

	void End(){
		
		Destroy(gameObject);
	}
	
	public void SetUpBaseAttributes(){
	
		this.Health = this.maxHealth;
		this.Shields = this.maxShields;
		this.Speed = this.baseSpeed;
	}

	public void Update () {
	
		this.regenTimer += Time.deltaTime;
		if (this.regenTimer > 1) {
			ShieldRegen();
			this.regenTimer = 0f;
		}
	}
}
