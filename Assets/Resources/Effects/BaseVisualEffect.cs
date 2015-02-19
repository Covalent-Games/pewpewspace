using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseVisualEffect : MonoBehaviour {

	void Start() {

		StartCoroutine(End());
	}

	IEnumerator End() {

		yield return new WaitForSeconds(10f);
		Destroy(gameObject);
	}
}
