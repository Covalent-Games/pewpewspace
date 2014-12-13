using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SonicDisruption : BaseAbility, IAbility{

	[SerializeField]
	ColliderHelper Sphere;
	
	void Start(){
	
		Resource = Resources.Load("AbilityObjects/ColliderHelper", typeof(GameObject));
		this.Cost = 25f;
		this.Duration = 4f;
		this.PrimaryEffect = 15;
		this.SecondaryEffect = 2;
		this.Condition = Condition.Damage;
	}
	
	public void Begin(ShipObject ship){
		
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
		
		ShipObject target = collider.GetComponent<ShipObject>();
		if (target == null) { return; }
		target.DamageArmor(this.PrimaryEffect, Ship);
		target.GetComponent<ConditionHandler>().ApplyCondition(this.Condition, this.SecondaryEffect, this.Duration);
	}
	
	public void Setup(){
		
		this.Ship.Heat += this.Cost;
		Executing = true;

		var sphere = (GameObject)Instantiate(Resource, Ship.transform.position, Quaternion.identity);
		this.Sphere = sphere.GetComponent<ColliderHelper>();
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