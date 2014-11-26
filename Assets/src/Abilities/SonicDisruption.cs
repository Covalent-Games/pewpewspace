using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SonicDisruption : BaseAbility, IAbility{

	[SerializeField]
	AreaOfEffectSphere Sphere;
	
	void Start(){
	
		Resource = Resources.Load("AbilityObjects/AreaOfEffectSphere", typeof(GameObject));
		this.Cost = 25f;
		this.Duration = 4f;
		this.PrimaryEffect = 15;
		this.SecondaryEffect = 2;
		this.Condition = Condition.Damage;
	}
	
	public void Begin(ShipAction ship){
		
		this.Ship = ship;
		this.ShipMove = ship.GetComponent<ShipMovement>();
		this.ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();
		yield return new WaitForFixedUpdate();
		TearDown();
	}
	
	public override void TriggerEnter(Collider collider){
		
		ShipAction target = collider.GetComponent<ShipAction>();
		if (target == null) { return; }
		target.DamageShip(this.PrimaryEffect);
		target.GetComponent<ConditionHandler>().ApplyCondition(this.Condition, this.SecondaryEffect, this.Duration);
	}
	
	public void Setup(){
		
		this.Ship.Dissipation += this.Cost;
		Executing = true;

		var sphere = (GameObject)Instantiate(Resource, Ship.transform.position, Quaternion.identity);
		this.Sphere = sphere.GetComponent<AreaOfEffectSphere>();
		this.Sphere.transform.position = Ship.transform.position;
		this.Sphere.ModifySphereRadius(8);
		this.Sphere.Ability = this;
		//SetupParticleEffect();  // This currently is not working, and the particle system has been removed. 
	}
	
	void SetupParticleEffect(){
	
		Transform particleGO = this.Sphere.transform.FindChild("Particle System");
		ParticleSystem particles = particleGO.GetComponent<ParticleSystem>();
		float speed = 75f;
		float distance = AbilityRadius;
		float time = distance / speed;
		particles.startSpeed = speed;
		particles.startLifetime = time;
		
	}
	
	public void TearDown(){
		
		Executing = false;
		DurationTimer = 0f;
		Destroy(this.Sphere.gameObject);
		this.Sphere = null;
	}
}