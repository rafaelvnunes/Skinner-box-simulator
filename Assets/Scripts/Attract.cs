using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attract : MonoBehaviour {
	public Main main;
	Animator box;
	AudioSource audio;
	bool firstatempt;

	void Start () {
		box = GetComponent<Animator> ();
		audio = GetComponentInChildren<AudioSource> ();
		firstatempt = true;
	}

	public void Incentivos (string Component) {
		if (firstatempt) {
			main.propensity += 0.5f;
			firstatempt = false;
		} else {
			main.propensity += 0.75f;
		}

		if ("Red_Light".Equals (Component)) {
			StartCoroutine (Wait ("A_Red_Light", 0.517f));
		}
		if ("Blue_Light".Equals (Component)) {
			StartCoroutine (Wait ("A_Blue_Light", 0.517f));
		}
		if ("Speaker".Equals (Component)) {
			audio.Play ();
		}
	}

	IEnumerator Wait (string animName, float time) {
		box.Play (animName);
		yield return new WaitForSecondsRealtime (time);
		box.Play ("Idle");
		yield return null;
	}
}