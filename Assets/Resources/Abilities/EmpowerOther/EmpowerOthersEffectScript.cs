using UnityEngine;
using System.Collections;

public class EmpowerOthersEffectScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

		StartCoroutine(BeginEffect());
		StartCoroutine(EndEffect());
	}
	
	// Update is called once per frame
	public IEnumerator BeginEffect() {

		while (enabled) {
			this.transform.Rotate(new Vector3(0, 1, 0), Time.deltaTime);
			yield return null;
		}
	
	}

	public IEnumerator EndEffect() {

		yield return new WaitForSeconds(1.5f);
		Destroy(gameObject);
	}
}
