using UnityEngine;
using System.Collections;

public class Mission {

	public int NumberOfWaves { get; set; }
	public int EnemiesPerWave { get; set; }
	public float TimeBetweenWaves { get; set; }
	public WinCondition WinCondition { get; set; }

}
