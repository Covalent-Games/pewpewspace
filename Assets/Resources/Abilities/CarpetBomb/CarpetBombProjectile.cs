using UnityEngine;
using System.Collections;

public class CarpetBombProjectile : Projectile, IProjectile {

	bool tracking;
	float speed = 15f;

	// Use this for initialization
	void Start () {

		StartCoroutine(Launch());
	}

	public IEnumerator Launch() {

		tracking = true;

		while (tracking) {

			speed += 0.4f;

			Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

			if (viewPos.x > 1 | viewPos.x < 0) { Destroy(gameObject); }
			if (viewPos.y > 1 | viewPos.y < 0) { Destroy(gameObject); }

			transform.position = transform.position + (transform.forward * Time.deltaTime * speed);
			yield return new WaitForEndOfFrame();
		}
	}

	void OnTriggerEnter(Collider collider) {

		ShipObject shipObject = collider.GetComponent<ShipObject>();

		if (shipObject == null) {
			return;
		}

		if (AbilityUtils.IsPlayer(shipObject)) {
			return;
		}

		shipObject.Armor -= Damage;

		tracking = false;
		Destroy(gameObject);
	}
}
