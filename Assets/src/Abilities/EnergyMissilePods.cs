using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EnergyMissilePods : BaseAbility, IAbility {

    GameObject CustomProjectile;
    GameObject OriginalProjectile;

    public void Start() {

        Cost = 20f;
        Duration = 1f / 60f;
        string path = "PlayerShips/ShipObjects/EnergyMissilePodsProjectile";
        CustomProjectile = (GameObject)Resources.Load(path, typeof(GameObject));
    }

    public void Begin(ShipObject ship) {
        Debug.Log("Energy Missile engaged!");
        Ship = ship;
        CustomProjectile.GetComponent<EnergyMissilePodsProjectile>().Owner = ship;
        ShipMove = ship.GetComponent<ShipMovement>();
        ShipClass = ship.ShipClass;
        Toggle = !Toggle;
        Debug.Log("Energy Missile Rounds is on: " + Toggle);
        StartCoroutine(Execute());
    }


    public IEnumerator Execute() {

        if (Toggle) {
            Setup();
        }

		float originalCost = Ship.fireCost;
		Ship.fireCost = this.Cost;

        while (Toggle) {
            // TODO: This might not be responsive enough at 1 second.
            yield return new WaitForSeconds(1f);
            if (Ship.MaxHeat - Ship.Heat < Cost) {
                Toggle = false;
                TearDown();
                yield break;
            }
        }

		Ship.fireCost = originalCost;

        TearDown();
    }

    public void Setup() {
        Debug.Log("Setting up");
        OriginalProjectile = Ship.projectilePrefab;
        Ship.projectilePrefab = CustomProjectile;
    }

    public void TearDown() {

        Ship.projectilePrefab = OriginalProjectile;
    }

    public void TriggerEnter(Collider collider) { }

    public void TriggerStay(Collider collider) { }

    public void TriggerExit(Collider collider) { }
}