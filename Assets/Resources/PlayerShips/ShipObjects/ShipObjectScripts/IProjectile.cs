using UnityEngine;
using System.Collections;

public interface IProjectile {

	Vector3 Direction {get; set;}
	int Damage {get; set;}
	ShipAction Target { get; set; }
}
