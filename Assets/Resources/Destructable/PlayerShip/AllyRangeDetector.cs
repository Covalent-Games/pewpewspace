using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyRangeDetector : MonoBehaviour {

	List<ShipObject> InRange;
	[SerializeField]
	[Tooltip("For testing")]
	int AlliesInRange;

	void Awake() {

		InRange = transform.parent.GetComponent<ShipObject>().InRange;
	}

	void OnTriggerEnter(Collider collider) {

		InRange.Add(collider.GetComponent<ShipObject>());
		AlliesInRange += 1;

	}

	void OnTriggerExit(Collider collider) {

		InRange.Remove(collider.GetComponent<ShipObject>());
		AlliesInRange -= 1;
	}
}
