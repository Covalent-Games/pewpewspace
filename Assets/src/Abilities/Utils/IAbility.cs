using UnityEngine;
using System.Collections;

public interface IAbility {
	
	float Cost {get; set;}
	bool Executing {get; set;}
	void Begin(ShipObject ship);
	void Setup();
	void TearDown();
	IEnumerator Execute();
}