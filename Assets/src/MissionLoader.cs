using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionLoader : MonoBehaviour {

	public string SceneToLoad;

	/// <summary>
	/// Loads the scene specified in the member SceneToLoad.
	/// </summary>
	internal void LoadMission() {
		
		Application.LoadLevel(SceneToLoad);
	}
}
