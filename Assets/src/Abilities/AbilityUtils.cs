using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BaseAbility: MonoBehaviour{
	
	public ShipType ShipClass;
	protected ShipAction Ship;
	protected ShipMovement ShipMove;
	public Condition Condition;
	public Boon Boon;
	public bool Executing {get; set;}
	public int Cost {get; set;}
	public int Level = 1;
	public int PrimaryEffect = 0;
	public int SecondaryEffect = 0;
	public int Damage = 0;
	public int ArmorRepair = 0;
	public int ShieldRepair = 0;
	public float Duration = 0f;
	public float DurationTimer = 0f;
	
	public float AbilityRadius;

	public virtual void TriggerEnter(Collider collider){}
	
	public virtual void TriggerStay(Collider collider){}
	
	public virtual void TriggerExit(Collider collider){}
}

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
	Drone,
}

// Order should match that of Boon
public enum Condition {

	Damage,	
}

// Order should match that of Condition
public enum Boon {
	
	Damage,
	
}

