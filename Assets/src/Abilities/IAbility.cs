using UnityEngine;
using System.Collections;

public interface IAbility {
	
	int Cost {get; set;}
	void Begin(ShipAction ship);
	void Setup();
	void TearDown();
	IEnumerator Execute();
}