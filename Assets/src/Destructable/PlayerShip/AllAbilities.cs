using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class AbilityUtils {

	public static void UpdateAbilityDictionaries(){
	
		var classes = AbilityUtils.AllTypesDerivedFrom(typeof(IAbility));
		
		foreach(var abilityType in classes){
			//IAbility inst = (IAbility)System.Activator.CreateInstance(T);
			Debug.Log("Adding " + abilityType.Name + " the the dictionary");
			string name = abilityType.Name;
			ShipAction.AbilityDict.Add(name, abilityType);
		}
	}
	
	static IEnumerable<System.Type> AllTypes(){
		
		var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
		
		foreach(var assembly in assemblies){
			var types = assembly.GetTypes();
			foreach(var type in types){
				yield return type;
			}
		}
	}
	
	public static IEnumerable<System.Type> AllTypesDerivedFrom(System.Type aBaseType){
		
		foreach(var T in AllTypes()){
			if(aBaseType.IsAssignableFrom(T) && T != aBaseType)
				yield return T;
		}
	}
}

public class CustomAbilityName : System.Attribute {
	
	public string customName;
	
	
	public CustomAbilityName(string aCustomName){
		
		customName = aCustomName;
	}
}

public enum ShipType {
	
	Guardian,
	Raider,
	Valkyrie,
	Outrunner,
}