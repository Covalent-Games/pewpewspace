using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BaseSequence : MonoBehaviour, IMissionSequence {

	public bool Running { get; set; }
	public BaseMission ParentMission { get; set; }
	public Canvas DialogueCanvas { get; set; }
	public Text DialogueText { get; set; }

	/// <summary>
	/// Sets up the sequence with commonly used objects.
	/// </summary>
	/// <param name="parentMission">The mission that creates this sequence.</param>
	public virtual void Init(BaseMission parentMission) {

		Debug.Log(name + " has initialized.");

		Running = true;
		ParentMission = parentMission;
		DialogueCanvas = ParentMission.DialogueCanvas;
		DialogueText = ParentMission.DialogueText;

		StartCoroutine(ExecuteSequence());
	}

	public virtual void Finish() {

		Debug.Log(name + " has finished.");
		Running = false;
	}

	public virtual IEnumerator ExecuteSequence() {

		yield break;
	}
}
