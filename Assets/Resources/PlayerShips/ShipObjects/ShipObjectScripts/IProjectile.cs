using UnityEngine;
using System.Collections;

public interface IProjectile {

	Vector3 Direction {get; set;}
	float Damage {get; set;}
	ShipObject Target { get; set; }

	ShipObject Owner { get; set; }
}
