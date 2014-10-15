﻿using UnityEngine;
using System.Collections;


public class Destructable : MonoBehaviour {

	public int maxHealth;
	public int maxShields;
	public float baseSpeed;
	
	[SerializeField]
	int health;
	[SerializeField]
	int shields;
	public float Speed;
	
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

	void Update () {	
	}
}
