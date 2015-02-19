using UnityEngine;
using System.Collections;

public class TargetCursor : MonoBehaviour {

	public Transform Tracking;
	public SpriteRenderer ThisRenderer;

	void Awake () {
		
		ThisRenderer = GetComponent<SpriteRenderer>();
	}
	
	void Update () {
	
		if (Tracking == null & ThisRenderer.enabled == true){
			ThisRenderer.enabled = false;
		} else if (Tracking != null){
			transform.position = Tracking.position;
		}
	}
}
