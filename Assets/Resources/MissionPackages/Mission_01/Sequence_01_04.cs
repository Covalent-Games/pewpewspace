using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sequence_01_04 : BaseSequence {

	// How many have been spawned already.
	int SpawnCount = 0;
	int GoalCounter = 0;
	[Tooltip("How many enemies to destroy before progressing")]
	public int GoalTarget;
	public int MaxEnemiesOnScreen;
	bool CountingEnabled = true;
	public float SpawnDelay;
	public GameObject EnemyPrefab;
	public GameObject Freighter;
	public float EnemyArmor;
	bool NotYetWinning = true;
	List<ShipObject> SpawnedEntities = new List<ShipObject>();

	public override IEnumerator ExecuteSequence() {

		StartCoroutine(SpawnOverTime());

		GoalTarget *= GameValues.Players.Count;
		GameObject freighter = (GameObject)Instantiate(Freighter, new Vector3(0f, -5f, 75f), Quaternion.identity);

		StartCoroutine(MoveFreighter(freighter, new Vector3(0, -5, 2)));

		// First Freighter Segment.
		while (GoalCounter <= GoalTarget) {
			// Wait .5 seconds before checking again since this doesn't need to be too accurate.
			yield return new WaitForSeconds(0.5f);
		}

		GoalCounter = 0;
		StartCoroutine(MoveFreighter(freighter, new Vector3(0, -5, -34)));

		// Second Freighter Segment.
		while (GoalCounter <= GoalTarget) {
			// Wait .5 seconds before checking again since this doesn't need to be too accurate.
			yield return new WaitForSeconds(0.5f);
		}

		GoalCounter = 0;
		StartCoroutine(MoveFreighter(freighter, new Vector3(0, -5, -86)));

		freighter.GetComponentInChildren<KingOfTheHill>().OnWin += Win;
		// Third Freighter Segment.
		while (NotYetWinning) {
			// Wait .5 seconds before checking again since this doesn't need to be too accurate.
			yield return new WaitForSeconds(0.5f);
		}

		Finish();
	}

	/// <summary>
	/// Spawns enemies over the duration of the sequence.
	/// </summary>
	IEnumerator SpawnOverTime() {

		float xpos;
		float ypos;

		while (enabled) {
			if (SpawnCount < MaxEnemiesOnScreen) {
				switch (SpawnCount % (int)Random.Range(1f, 6f)) {
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
				// Add OnDestroy callback.
				newEnemy.OnDestroy += IncreaseGoalCounter;
				newEnemy.AddContainers(SceneHandler.Enemies, SpawnedEntities);
				SpawnCount += 1;
			}
			Debug.Log(string.Format("Spawning an enemy. SpawnCount: {0}, OnScreen: {1}", SpawnCount, MaxEnemiesOnScreen));
			Debug.Log("Pausing for " + SpawnDelay + " seconds");
			yield return new WaitForSeconds(SpawnDelay);
		}
	}

	IEnumerator MoveFreighter(GameObject freighter, Vector3 targetPosition) {

		CountingEnabled = false;

		while (freighter.transform.position != targetPosition) {
			freighter.transform.position = Vector3.MoveTowards(
					freighter.transform.position, targetPosition, Time.deltaTime * 5);
			yield return null;
		}

		CountingEnabled = true;
	}

	/// <summary>
	/// Increases the counter for how many enemies have to be destroyed to progress.
	/// </summary>
	public void IncreaseGoalCounter() {
		Debug.Log("Dude died, decreasing count");
		SpawnCount -= 1;
		if (CountingEnabled) {
			GoalCounter += 1;
		}
	}

	public void Win() {

		NotYetWinning = false;
	}
}
