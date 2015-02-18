using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EmpowerOther : BaseAbility, IAbility {

	public Object VFX;

	public void Start() {

		Name = "Empower Other";
		Cost = 50f;
		Duration = 1f / 60f;
		string path = "AbilityObjects/EmpowerOtherProjectile";
		Resource = Resources.Load(path, typeof(GameObject));
		VFX = Resources.Load("Effects/EmpowerOthersEffect", typeof(GameObject));
		Duration = 5f;
		Percentage = 0.35f;
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
		yield return null;
		TearDown();
	}

	public void Setup() {

		Executing = true;

		foreach (ShipObject ship in Ship.InRange) {
			if (ship) {
				GameObject projectileGO = (GameObject)Instantiate(
						Resource,
						Ship.transform.position,
						Quaternion.identity);
				EmpowerOtherProjectile projectile = projectileGO.GetComponent<EmpowerOtherProjectile>();

				projectile.Target = ship;
				projectile.DamageModifier = Percentage;
				projectile.Duration = Duration;
				projectile.Effect = VFX;

				StartCoroutine(projectile.TrackToTarget());
			} else {
				Debug.LogWarning("A ship that was 'In Range' was null");
			}
		}

		Ship.Heat += Cost;
	}

	public void TearDown() {

		Executing = false;
		DurationTimer = 0f;
	}

	public override void TriggerEnter(Collider collider) { }

	public override void TriggerStay(Collider collider) { }

	public override void TriggerExit(Collider collider) { }
}