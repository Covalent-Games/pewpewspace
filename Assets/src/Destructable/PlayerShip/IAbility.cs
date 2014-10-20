using UnityEngine;
using System.Collections;

public interface IAbility {
	
	void Begin(ShipAction ship);
	void Setup();
	void TearDown();
}