using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartUp : MonoBehaviour {

	public bool Debugging;
	public string levelToLoad;
	public Transform HUD;
	public Transform MenuObject;
	public Transform AudioHandler;


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
			Debug.Log("Loading " + levelToLoad);
		}
		Application.LoadLevel(levelToLoad);
	}
}