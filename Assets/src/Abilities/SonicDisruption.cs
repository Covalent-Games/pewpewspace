using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class SonicDisruption : BaseAbility, IAbility{

	[SerializeField]
	ColliderHelper Sphere;
	
	void Start(){
	
		Resource = Resources.Load("AbilityObjects/ColliderHelper", typeof(GameObject));
		Cost = 25f;
		Duration = 4f;
		PrimaryEffect = 15;
		SecondaryEffect = 2;
		Condition = Condition.Damage;
	}
	
	public void Begin(ShipObject ship){
		
		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
	}
	
	public IEnumerator Execute(){
		
		Setup();
		yield return null;
		TearDown();
	}
	
	public override void TriggerEnter(Collider collider){
		
		ShipObject target = collider.GetComponent<ShipObject>();
		if (target == null) { return; }
		target.DamageArmor(this.PrimaryEffect, Ship);
		target.GetComponent<ConditionHandler>().ApplyCondition(
				this.Condition, 
				AbilityID.SonicDisruption, 
				this.SecondaryEffect, 
				this.Duration);
	}
	
	public void Setup(){
		
		Ship.Heat += this.Cost;
		Executing = true;

		var sphere = (GameObject)Instantiate(Resource, Ship.transform.position, Quaternion.identity);
		sphere.transform.Rotate(Vector3.right, 90f);
		Sphere = sphere.GetComponent<ColliderHelper>();
		Sphere.transform.position = Ship.transform.position;
		Sphere.ModifySphereRadius(8);
		Sphere.Ability = this;
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
		Sphere = null;
	}
}