using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BaseAbility: MonoBehaviour{
	
	public ShipType ShipClass;
	protected ShipAction Ship;
	protected ShipMovement ShipMove;
	public bool Executing {get; set;}
	public int Cost {get; set;}
	public int Level = 1;
	public Object Resource;
	
	// These exist to outline what the current abilities can handle and to keep naming uniform.
	public Condition Condition;
	public Boon Boon;
	public bool Toggle;
	public int PrimaryEffect = 0;
	public int SecondaryEffect = 0;
	public float Percentage = 0f;
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

	public static bool IsPlayer(ShipAction shipAction){
	
		int value = (int)shipAction.ShipClass;

		if (value >= 0 & value <= 20){
			return true;
		} else {
			return false;
		}
	}

	public static void UpdateAbilityDictionaries(){
	
		var classes = AbilityUtils.AllTypesDerivedFrom(typeof(IAbility));
		
		foreach(var abilityType in classes){
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

