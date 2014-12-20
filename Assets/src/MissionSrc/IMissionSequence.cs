using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IMissionSequence {

	bool Running { get; set; }

	IEnumerator ExecuteSequence();
}
