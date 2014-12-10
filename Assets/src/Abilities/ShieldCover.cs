using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ShieldCover : BaseAbility, IAbility {

	GameObject Shield;

	public void Start() {

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
		shield.InvulnerableArmor = true;
		
		
	}

	public void TearDown() {

		Executing = false;
		DurationTimer = 0f;
		Destroy(Shield);
	}

	public void TriggerEnter(Collider collider) { }

	public void TriggerStay(Collider collider) { }

	public void TriggerExit(Collider collider) { }
}