using UnityEngine;
using System.Collections;

public class EnergyMissilePodsProjectile : MonoBehaviour, IProjectile {

	public ShipObject Target { get; set; }
    bool tracking;
    public int Damage { get; set; }
    public ShipObject Owner;
    public Vector3 Direction { get; set; }
    public Vector3 oldPosition;
    public Vector3 newPosition;

    void Start() {
        SetTarget();
        StartCoroutine(TrackToTarget());
    }

    void SetTarget() {

        // Player has no target
        if (Owner.Target == null && SceneHandler.Enemies.Count > 0) {
            int index = Random.Range(0, SceneHandler.Enemies.Count);
            ShipObject hostileTarget = SceneHandler.Enemies[index];
            this.Target = hostileTarget;
        } else {

            ShipObject target = Owner.Target.GetComponent<ShipObject>();

            // The target is a player
            if (AbilityUtils.IsPlayer(target)) {
                if (SceneHandler.Enemies.Count > 0) {
                    int index = Random.Range(0, SceneHandler.Enemies.Count);
                    ShipObject hostileTarget = SceneHandler.Enemies[index];
                    this.Target = hostileTarget;
                }
            } else {
                this.Target = target;
            }
        }

        if (this.Target == null) {
            Debug.LogError("EnergyMissile target did not succesfully set!");
        }

        //TODO: Get damage from EnergyMissilePods.cs
        this.Damage = 20;

    }

    public IEnumerator TrackToTarget() {

        tracking = true;
        float speed = 15f;

        while (tracking) {

            if (Target == null) {
                
                SetTarget();
                if (Target == null) {
                    Destroy(gameObject);
                    tracking = false;
                }
            } else {
                Direction = Target.transform.position;
                speed += 0.4f;

                transform.position = Vector3.MoveTowards(
                        transform.position,
                        Target.transform.position,
                        Time.deltaTime * speed);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    void OnTriggerEnter(Collider collider) {
		Debug.Log("Colliding");
		GameObject target = collider.gameObject;
		if (target == null)
			Debug.LogError("Collider does not have a gameobject");

        ShipObject shipAction = target.GetComponent<ShipObject>();

        if (shipAction == null) {
			Debug.Log("Shipaction is null");
            return;
        }

        if (AbilityUtils.IsPlayer(shipAction)) {
			Debug.Log("IsPlayer");
            return;
        }

        shipAction.DamageShip(Damage);
        Debug.Log("Dealing " + Damage + " damage");

        tracking = false;
        Destroy(this.gameObject);
    }
}
