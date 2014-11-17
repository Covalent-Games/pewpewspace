using UnityEngine;
using System.Collections;


public enum ShipType {
	
	Guardian,
	Raider,
	Valkyrie,
	Outrunner,
	Drone = 21, // Creating a buffer in case we add more base classes
}

// Order should match that of Boon
public enum Condition {
	
	Damage,	
    Speed,
}

// Order should match that of Condition
public enum Boon {
	
	Damage,
	
}

public enum MissionName {

	Mission1,
}