using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// A collection of values accessible acrossed scenes.
/// </summary>
public static class GameValues {

	public static int NumberOfPlayers;
	public static Dictionary<int, Player> Players = new Dictionary<int, Player>();
	public static string PreviousScene;
	public static string NextScene;

	public struct TestingValues {
		public static bool DisableHeat;
		public static bool DisablePlayerArmor;
		public static bool DisableEnemyArmor;
	}

}
