using UnityEngine;
using System.Collections;

public class ReaperManProjectile : MonoBehaviour {

    public ShipObject Target;
    bool tracking;
    public int damage;

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

        ShipObject shipAction = collider.GetComponent<ShipObject>();

        if (shipAction == null)
        {
            return;
        }

        if (AbilityUtils.IsPlayer(shipAction)) {
            return;
        }

        shipAction.DamageShip(damage);

        tracking = false;
        Destroy(gameObject);
    }
}
