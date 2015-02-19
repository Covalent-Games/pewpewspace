using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShieldCover : BaseAbility, IAbility {

	GameObject Shield;

	public void Start() {

		Name = "Shield Cover";
		Cost = 60;
		string path = "Abilities/ShieldCover/ShieldCoverObject";
		Resource = Resources.Load(path, typeof(GameObject));
		Duration = 2.2f;
	}

	public void Begin(ShipObject ship) {

		Ship = ship;
		ShipMove = ship.GetComponent<ShipMovement>();
		ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
		DisplayName(Name);
	}

	public IEnumerator Execute() {

		Setup();
		yield return new WaitForSeconds(Duration);
		TearDown();
	}

	public void Setup() {

		Executing = true;
		Shield = (GameObject)Instantiate(Resource, transform.position, Quaternion.identity);
		Shield.transform.parent = transform;
		ShieldCoverObject shield = Shield.GetComponent<ShieldCoverObject>();
		shield.Size = new Vector3(15f, 10f, 15f);
		shield.Invulnerable = true;
		Ship.Heat += Cost;

	}

	public void TearDown() {

		Executing = false;
		//HACK to prevent collision from breaking
		GetComponent<MeshCollider>().convex = false;
		Destroy(Shield);
		StartCoroutine(EnableMeshCollider());
	}

	//HACK to prevent collision from breaking
	IEnumerator EnableMeshCollider() {

		yield return null;
		GetComponent<MeshCollider>().convex = true;
	}

	public override void TriggerEnter(Collider collider) { }

	public override void TriggerStay(Collider collider) { }

	public override void TriggerExit(Collider collider) { }
}