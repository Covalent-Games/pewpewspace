using UnityEngine;

public class TurretSpawner : MonoBehaviour {

	public GameObject TurretPrefab;

	void Awake() {

		foreach (Transform child in transform) {
			if (child.tag == "TurretLocation") {
				var turret = Instantiate(
						TurretPrefab,
						child.transform.position,
						Quaternion.identity) as GameObject;
				turret.transform.parent = transform;
			}
		}
	}
}
