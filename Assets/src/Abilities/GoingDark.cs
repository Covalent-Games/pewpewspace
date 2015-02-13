using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GoingDark : BaseAbility, IAbility {

	Object VFX;

	public void Start() {

		Name = "Goind Dark";
		Cost = 35f;
		Duration = 3f;
		Resource = Resources.Load("AbilityObjects/GoingDarkVeil");
		VFX = Resources.Load("Effects/GoingDarkEffect");
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

		//MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
		//renderer.material.color.a = 0.3f;

		gameObject.renderer.material.SetColor("_Color", new Color(gameObject.renderer.material.color.r, gameObject.renderer.material.color.g, gameObject.renderer.material.color.b, 0.2f));

		
		//GameObject veil = (GameObject)Instantiate(Resource, transform.position, Quaternion.identity);
		//veil.transform.Rotate(Vector3.right, 90f);

		while (DurationTimer < Duration) {
			DurationTimer += Time.deltaTime;
			//veil.transform.position = transform.position + new Vector3(0, .5f, 0);
			yield return new WaitForEndOfFrame();
		}
		gameObject.renderer.material.SetColor("_Color", new Color(gameObject.renderer.material.color.r, gameObject.renderer.material.color.g, gameObject.renderer.material.color.b, 1f));

		//Destroy(veil);
		TearDown();
	}

	public void Setup() {

		Executing = true;
		Ship.Heat += Cost;
		Ship.CanBeTargetted = false;

		StartCoroutine(ApplyVFX());

		// Reduce all current enemy's threat table entries for this player to 0.
		foreach (ShipObject enemy in SceneHandler.Enemies.ToArray()) {
			BaseShipAI ai = enemy.GetComponent<BaseShipAI>();
			var table = ai.ThreatTable;
			foreach (ShipObject ship in new List<ShipObject>(table.Keys)) {
				if (ship == Ship) {
					table[ship] = 0;
				}
			}

			if (ai.BaseShip.Target == Ship.transform) {
				ai.BaseShip.Target = null;
			}
		}
	}

	public void TearDown() {

		Ship.CanBeTargetted = true;
		Executing = false;
		DurationTimer = 0f;
	}

	public IEnumerator ApplyVFX() {

		GameObject effect = (GameObject)Instantiate(VFX, transform.position, Quaternion.identity);
		effect.transform.parent = transform;
		//effect.transform.
		effect.transform.Translate(0f, 0.75f, 1.4f);
		effect.transform.Rotate(new Vector3(180, 0, 0));
		yield return new WaitForSeconds(2f);
		Destroy(effect);
	}

	public override void TriggerEnter(Collider collider) { }

	public override void TriggerStay(Collider collider) { }

	public override void TriggerExit(Collider collider) { }
}