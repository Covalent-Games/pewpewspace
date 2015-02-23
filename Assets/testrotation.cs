using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class testrotation : MonoBehaviour {

	public Quaternion NextRotation;

	void Start() {
		NextRotation = Random.rotation;
	}

	void Update() {

		if (transform.rotation == NextRotation) {
			NextRotation = Random.rotation;
		} else {
			transform.rotation = Quaternion.Lerp(transform.rotation, NextRotation, Time.deltaTime * 3);
		}
	}
}
