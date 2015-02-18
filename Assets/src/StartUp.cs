using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartUp : MonoBehaviour {

	public bool Debugging;
	public string levelToLoad;
	public Transform HUD;
	public Transform MenuObject;
	public Transform AudioHandler;
	[Tooltip("Modify MissionPackages/MissionTester.prefab with any mission or sequence to test.")]
	public bool RunMissionTester;
	[Range(1, 4)]
	[Tooltip("Only applicable when Run Mision Tester is checked.")]
	public int NumberOfTestPlayers;


	void Awake() {

		DontDestroyOnLoad(HUD.gameObject);
		DontDestroyOnLoad(MenuObject.gameObject);
		DontDestroyOnLoad(AudioHandler.gameObject);

		if (Debugging) {
			Debug.Log("Loading Ability Components");
		}

		AbilityUtils.UpdateAbilityDictionaries();

		if (Debugging) {
			foreach (var entry in ShipObject.AbilityDict) {
				Debug.Log(string.Format("Name: {0}, Ability: {1}", entry.Key, entry.Value));
			}
		}

		if (Debugging) {
			if (RunMissionTester) {
				Debug.Log("Running Mission Tester");
			} else {
				Debug.Log("Loading " + levelToLoad);
			}
		}

		if (RunMissionTester) {
			GameValues.NextScene = "MissionTest";
			for (int i = 1; i <= NumberOfTestPlayers; i++) {
				GameValues.Players.Add(i, new Player(i));
			}
			GameValues.NumberOfPlayers = NumberOfTestPlayers;
			Application.LoadLevel("ShipSelection");
		} else {
			Application.LoadLevel(levelToLoad);
		}
	}

}