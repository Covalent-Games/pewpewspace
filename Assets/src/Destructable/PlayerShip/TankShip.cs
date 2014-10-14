using UnityEngine;
using System.Collections;

public class TankShip : Destructable {

	// Use this for initialization
	void Start () {

		this.Health = this.maxHealth;
		this.Shields = this.maxShields;
		this.Speed = 0.8f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
