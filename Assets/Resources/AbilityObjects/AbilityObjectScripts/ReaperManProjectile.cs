using UnityEngine;
using System.Collections;

public class ReaperManProjectile : MonoBehaviour {

    public ShipObject Target;
    bool tracking;
    public int damage;
	public ShipObject Owner;

    public IEnumerator TrackToTarget()
    {

        tracking = true;
        float speed = 15f;

        while (tracking)
        {

            if (Target == null)
            {
                tracking = false;
                Destroy(gameObject);
                yield break;
            }

            speed += 0.4f;

            transform.position = Vector3.MoveTowards(
                    transform.position,
                    Target.transform.position,
                    Time.deltaTime * speed);

            yield return new WaitForFixedUpdate();
        }
    }

    void OnTriggerEnter(Collider collider)
    {

        ShipObject shipObject = collider.GetComponent<ShipObject>();

        if (shipObject == null)
        {
            return;
        }

        if (AbilityUtils.IsPlayer(shipObject)) {
            return;
        }

        shipObject.DamageArmor(damage, Owner);

        tracking = false;
        Destroy(gameObject);
    }
}
