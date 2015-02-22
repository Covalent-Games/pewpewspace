using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BullRush : BaseAbility, IAbility {

	private Vector3 moveTowards;

	void Start() {

		Cost = 50f;
		Duration = 0.3f;
		Name = "Bull Rush";
		Resource = Resources.Load("Effects/BullRushEffect");
	}

	public void Begin(ShipObject ship) {

		this.Ship = ship;
		this.ShipMove = ship.GetComponent<ShipMovement>();
		this.ShipClass = ship.ShipClass;
		StartCoroutine(Execute());
		DisplayName(Name);
	}

	public IEnumerator Execute() {

		Setup();

		GameObject BullRushEffectGO = (GameObject)Instantiate(Resource, Ship.transform.position, Ship.transform.rotation);
		BullRushEffectGO.transform.parent = Ship.transform;
		BullRushEffectGO.transform.Rotate(180, 0, 0);
		BullRushEffectGO.transform.localPosition = new Vector3(0f, 0f, 1.05f);

		while (DurationTimer < Duration) {
			DurationTimer += Time.deltaTime;
			ShipMove.MoveShip(Vector3.MoveTowards(Ship.transform.position, moveTowards, Time.deltaTime * ShipMove.moveSpeed * 4));
			yield return new WaitForEndOfFrame();
		}

		TearDown();

		BullRushEffectGO.GetComponent<ParticleSystem>().Stop();
		yield return new WaitForSeconds(BullRushEffectGO.GetComponent<ParticleSystem>().startLifetime);
		Destroy(BullRushEffectGO);

	}

	public void Setup() {

		Executing = true;
		moveTowards = new Vector3(0, 0, 20f) + Ship.transform.position;
		ShipMove.moveEnabled = false;
		Ship.Invulnerable = true;
		Ship.Heat += Cost;
	}

	public void TearDown() {

		Executing = false;
		ShipMove.moveEnabled = true;
		Ship.Invulnerable = false;
		DurationTimer = 0f;
	}
}