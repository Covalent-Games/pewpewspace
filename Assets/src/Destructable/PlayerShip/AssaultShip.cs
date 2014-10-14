using UnityEngine;
using System.Collections;

public class AssaultShip : Destructable {

	// Use this for initialization
	void Start () {
		
		this.Health = this.maxHealth;
		this.Shields = this.maxShields;
		this.Speed = 1.2f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
