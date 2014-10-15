using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GuardianAbility {
	
	public ShipType Type = ShipType.Guardian;
}

public class BullRush : GuardianAbility, IAbility{
	
	public void Start(ShipAction ship){

		Debug.Log("BUULLLRRUUSSHHH!!! ... finally...");
	}
}