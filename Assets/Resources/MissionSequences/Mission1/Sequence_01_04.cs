using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sequence_01_04 : BaseSequence {

	public int SpawnQuantity;
	public GameObject EnemyPrefab;
	public GameObject Freighter;
	public float EnemyArmor;
	List<ShipObject> SpawnedEntities = new List<ShipObject>();

	public override IEnumerator ExecuteSequence() {

		yield return new WaitForSeconds(3f);

		for (int i = 0; i < SpawnQuantity; i++) {

			float xpos;
			float ypos;
			switch (i % SpawnQuantity) {
				default:
					xpos = Random.Range(0f, 1f);
					ypos = Random.Range(1.02f, 1.08f);
					break;
				case 0:
					ypos = Random.Range(0f, 1f);
					xpos = Random.Range(1.02f, 1.08f);
					break;
				case 1:
					ypos = Random.Range(0f, 1f);
					xpos = Random.Range(-0.02f, -0.08f);
					break;
			}


			var viewport = new Vector3(xpos, ypos, Camera.main.transform.position.y);
			Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(viewport);

			GameObject newEnemyGO = (GameObject)Instantiate(
					EnemyPrefab,
					spawnPosition,
					Quaternion.identity);

			ShipObject newEnemy = newEnemyGO.GetComponent<ShipObject>();
			newEnemy.MaxArmor = EnemyArmor;
			newEnemy.AddContainers(SceneHandler.Enemies, SpawnedEntities);
		}

		GameObject freighter = (GameObject)Instantiate(Freighter, new Vector3(0f, 0f, 75f), Quaternion.identity);
		Vector3 targetPosition = new Vector3(0f, 0f, 2f);

		while (freighter.transform.position != targetPosition) {
			freighter.transform.position = Vector3.MoveTowards(
					freighter.transform.position, targetPosition, Time.deltaTime * 5);
			yield return new WaitForEndOfFrame();
		}

		while (SpawnedEntities.Count > 0) {
			// Wait .5 seconds before checking again since this doesn't need to be too accurate.
			yield return new WaitForSeconds(0.5f);
		}

		Finish();
	}
}
