using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartUp : MonoBehaviour {
	
	public bool Debugging;
	public string levelToLoad;


	void Awake(){
		
		if (Debugging){
			Debug.Log("Loading Ability Components");
		}
		
		AbilityUtils.UpdateAbilityDictionaries();
		
		if (Debugging){
			foreach(var entry in ShipAction.AbilityDict){
				Debug.Log(string.Format("Name: {0}, Ability: {1}", entry.Key, entry.Value));
			}
		}
		
		if (Debugging){
			Debug.Log("Loading " + levelToLoad);
		}
		Application.LoadLevel(levelToLoad);
	}
}