using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeCanvas : MonoBehaviour {

	Image FadeBlack;
	Image FadeWhite;

	void Awake() {

		FadeBlack = transform.FindChild("FadeBlack").GetComponent<Image>();
		FadeWhite = transform.FindChild("FadeWhite").GetComponent<Image>();
	}

	public void FadeToBlack() {

		FadeBlack.enabled = true;
		StartCoroutine(Fade());
	}

	public void FadeToWhite() {

		FadeWhite.enabled = true;
		StartCoroutine(Fade());
	}

	IEnumerator Fade() {

		CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

		while (canvasGroup.alpha < 1f) {
			// FixedUpdate happens 60/sec, so 1 second lerp would be 1 sec divided by 60
			canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, Time.deltaTime);
			yield return new WaitForFixedUpdate();
		}
	}
}
