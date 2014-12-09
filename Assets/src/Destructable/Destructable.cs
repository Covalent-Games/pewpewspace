using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Destructable : MonoBehaviour {

	#region Members

	public List<ShipObject> Container = new List<ShipObject>();

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
	public int DamageShip(int damage){
	
		if (this.Invulnerable){ 
				return this.Health;
		}
		if (!this.InvulnerableArmor){
			this.Health -= damage;

            DisplayFloatingDamage(damage);

			return this.Health;
		}
		
		return 0;
	}
	
	public int DamageArmor(int damage){

		if (!this.Invulnerable & !this.InvulnerableArmor){
			this.Health -= damage;

            DisplayFloatingDamage(damage);

			return this.Health;
		}

		return this.Health;
	}

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


    private void DisplayFloatingDamage(int damage) {

        GameObject guiElement = (GameObject)Instantiate(Resources.Load("GUIPrefabs/FloatingDamage"), transform.position, Quaternion.identity);
        Transform textTransform = guiElement.transform.FindChild("Text");
        textTransform.GetComponent<Text>().text = damage.ToString();
    }

	void Start () {
		
		this.health = this.maxHealth;
	}

	void End(){
		
		//FIXME The comment below is a lie, and I don't currently know the fix. It's a rare bug. 'gameObject'
		// itself is a member of the physical GameObject and so referencing gameObject raises an error.
		// The object sometimes tries to be destroyed twice in one frame. This check prevents that.
		if (gameObject != null){
			Container.Remove(GetComponent<ShipObject>());
			var explosions = GameObject.FindObjectOfType<SceneHandler>().Explosions;
			var explosion = explosions[Random.Range(1, explosions.Count) - 1];
			Instantiate(explosion, transform.position, Quaternion.identity);
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
