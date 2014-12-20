using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sequence_01_01 : MonoBehaviour, IMissionSequence {

	public bool Running { get; set; }
	public GameObject DronePrefab;
	List<ShipObject> SpawnedEntities = new List<ShipObject>();

	public void Start() {

		Running = true;
		Debug.Log(name + " has started");
		StartCoroutine(ExecuteSequence());
	}

	public IEnumerator ExecuteSequence() {

		yield return new WaitForSeconds(3f);

		for (int i = 0; i < 5; i++) {
			float xpos = Random.Range(0f, 1f);
			float ypos = Random.Range(1.02f, 1.08f);
			Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(xpos, ypos, 40f));

			GameObject newEnemyGO = (GameObject)Instantiate(
					DronePrefab,
					spawnPosition,
					Quaternion.LookRotation(Vector3.back));

			ShipObject newEnemy = newEnemyGO.GetComponent<ShipObject>();
			newEnemy.AddContainers(SceneHandler.Enemies, SpawnedEntities);
		}

		while (SpawnedEntities.Count > 0) {
			// Wait for half a second before checking again since this doesn't need to be too accurate.
			yield return new WaitForSeconds(0.5f);
		}

		Debug.Log(name + " is done");
		Running = false;
	}
}
