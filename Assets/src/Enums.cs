using UnityEngine;
using System.Collections;
using System.Xml.Serialization;


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
	Targeting,
}

// Order should match that of Condition
public enum Boon {
	
	Damage,
	FireRate,
	
}

public enum MissionName {

	Mission1,
}

public enum WinCondition {

	AllEnemiesKilled = 0,
}

public enum AbilityID {

	None,
	BatteryDrone,
	BullRush,
	DeconLaser,
	DesyncBurst,
	EmpowerOther,
	EnergyMissilePods,
	GoingDark,
	ReaperMan,
	SalvageConversionRounds,
	ShieldCover,
	SonicDisruption,
	SustainDrone,
 }
