using UnityEngine;

public class TurnLeft : MonoBehaviour {
	public Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("w")) {
			anim.Play ("Armature|WalkCycle");
		}
			if (Input.GetKeyUp ("w")) {
				anim.Play ("Idle");
		}
	}
}
