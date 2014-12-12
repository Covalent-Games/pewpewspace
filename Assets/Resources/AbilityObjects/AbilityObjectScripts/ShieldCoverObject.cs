using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShieldCoverObject : Destructable {

	public Vector3 Size;

	void Start() {

		StartCoroutine(Grow());
	}

	IEnumerator Grow() {

		while (true) {
			transform.localScale = Vector3.Lerp(transform.localScale, Size, Time.deltaTime * 10);
			yield return new WaitForEndOfFrame();
		}
	}
}
