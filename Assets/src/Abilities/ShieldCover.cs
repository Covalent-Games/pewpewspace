using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShieldCover : BaseAbility, IAbility {

	GameObject Shield;

	public void Start() {

		Name = "Shield Cover";
		Cost = 60;
		string path = "AbilityObjects/ShieldCoverObject";
		Resource = Resources.Load(path, typeof(GameObject));
		Duration = 3f;
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
		while (DurationTimer < Duration) {
			DurationTimer += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
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
		DurationTimer = 0f;
		Destroy(Shield);
	}

	public override void TriggerEnter(Collider collider) { }

	public override void TriggerStay(Collider collider) { }

	public override void TriggerExit(Collider collider) { }
}