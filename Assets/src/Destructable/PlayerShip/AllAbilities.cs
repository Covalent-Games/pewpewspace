using UnityEngine;
using System.Collections;

public static class AbilityUtils {

	public static void UpdateAbilityDictionaries(){
	
		ShipAction.AbilityDict.Add("BullRush", new BullRush());
	}
}

public enum ShipType {
	
	Guardian,
	Raider,
	Valkyrie,
	Outrunner,
}