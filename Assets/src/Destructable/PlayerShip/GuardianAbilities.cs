using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GuardianAbility {
	
	public ShipType Type = ShipType.Guardian;
	public int ShieldCost = 0;
	protected ShipAction Ship;
}

public class BullRush : GuardianAbility, IAbility{
	
	public int ShieldCost = 50;
	
	public void Start(ShipAction ship){
		
		Debug.Log("BULLRUSH!");
		
	}
}