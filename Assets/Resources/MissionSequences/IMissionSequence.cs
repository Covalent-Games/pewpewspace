using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public interface IMissionSequence {

	bool Running { get; set; }
	BaseMission ParentMission { get; set; }
	Canvas DialogueCanvas { get; set; }
	Text DialogueText { get; set; }

	/// <summary>
	/// Sets up the sequence with commonly used objects.
	/// </summary>
	/// <param name="parentMission">The mission that creates this sequence.</param>
	void Init(BaseMission baseMission);
	bool IsConcurrent();
	IEnumerator ExecuteSequence();
	void Finish();
}
