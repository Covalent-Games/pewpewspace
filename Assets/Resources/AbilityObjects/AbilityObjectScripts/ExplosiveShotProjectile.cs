using UnityEngine;
using System.Collections;

public class ExplosiveShotProjectile : Projectile, IProjectile {

	public int Damage { get; set; }
	public Vector3 Direction { get; set; }
	bool tracking;
	public float damageRadius;


	// Use this for initialization
	void Start () {

		StartCoroutine(Launch());
	}

	public IEnumerator Launch() {

		tracking = true;
		float speed = 15f;


		while (tracking) {

			speed += 0.4f;

			Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
			// If outside of screen, destroy
			if (viewPos.x > 1 | viewPos.x < 0) { Destroy(gameObject);}
			if (viewPos.y > 1 | viewPos.y < 0) { Destroy(gameObject);}

			transform.position = transform.position + (transform.forward * Time.deltaTime * speed);
			yield return new WaitForFixedUpdate();
		}

	}
	
    void OnTriggerEnter(Collider collider) {

        ShipAction shipAction = collider.GetComponent<ShipAction>();

        if (shipAction == null) {
            return;
        }

        if (AbilityUtils.IsPlayer(shipAction)) {
            return;
        }

		// Only damage enemies within blast radius
		foreach (var enemy in SceneHandler.Enemies) {
			if (Vector3.Distance(enemy.transform.position, transform.position) <= damageRadius) {
				enemy.GetComponent<ShipAction>().DamageShip(this.Damage);
				Debug.Log("Explosive shot hit " + enemy.ToString());
			}
		}

        tracking = false;
        Destroy(gameObject);
    }
}
