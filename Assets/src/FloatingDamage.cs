using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingDamage : MonoBehaviour {

    public float FadeSpeed;
    float DriftAngle;

    void Start() {

        transform.Rotate(new Vector3(90f, 0f, 0f));
        transform.position += new Vector3(0f, 0f, 1.5f);
        StartCoroutine(Tick());
    }

    private IEnumerator Tick() {

        float ticker = 0f;
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();

        while (ticker < FadeSpeed) {
            ticker += Time.deltaTime;
            canvasGroup.alpha -= Time.deltaTime / .75f;
            transform.position += new Vector3(0f, 0f, 2 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
