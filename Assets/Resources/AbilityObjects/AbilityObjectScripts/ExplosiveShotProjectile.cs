using UnityEngine;
using System.Collections;

public class ExplosiveShotProjectile : MonoBehaviour, IProjectile {

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
			if (speed > 100) {
				Destroy(gameObject);
			}

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

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

		// Only damage enemies within
		foreach (var enemy in enemies) {
			if (Vector3.Distance(enemy.transform.position, transform.position) <= damageRadius) {
				enemy.GetComponent<ShipAction>().DamageShip(this.Damage);
				Debug.Log("Explosive shot hit " + enemy.ToString());
			}
		}

        tracking = false;
        Destroy(gameObject);
    }
}
