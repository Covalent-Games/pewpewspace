using UnityEngine;
using System.Collections;

public class TankShip : PlayerShip {

	// Use this for initialization
	void Start () {

		this.Health = 80;
		this.shields = 50;
		this.speed = 0.8f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
