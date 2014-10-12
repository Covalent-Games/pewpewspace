using UnityEngine;
using System.Collections;

public class AssaultShip : PlayerShip {

	// Use this for initialization
	void Start () {
	
		this.Health = 50;
		this.shields = 30;
		this.speed = 1.2f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
