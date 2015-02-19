using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioLibrary : MonoBehaviour {

	// PLEASE LIST ALL AUDIOCLIPS ALPHABETICALLY

	public AudioClip DefaultTurret;
	public AudioClip Explosion_01;

	public static void Play(AudioClip clip) {

		AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
	}

	void Awake() {

		DontDestroyOnLoad(transform.gameObject);
	}
}

