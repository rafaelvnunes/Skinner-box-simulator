using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Timers;

public class Food : MonoBehaviour {
	static int TimesEat = 0;

	NavMeshAgent agent;
	Animator M_Rato;
	Main main;
	GameObject rato;
	Vector3 Edge;
	Text Eat;

	void Start () {
		Eat = GameObject.Find ("X_Comeu").GetComponent<Text> ();
		rato = GameObject.Find("Rato");
		M_Rato = rato.GetComponentInParent<Animator> ();
		agent = rato.GetComponent<NavMeshAgent> ();
		main = rato.GetComponent<Main> ();
		Edge.Set (-82.3f, 11f, 112.7f);
	}

	IEnumerator OnTriggerEnter (Collider other) {
		if(other.CompareTag("Boca")){
			M_Rato.Play ("Comer");
			yield return new WaitForSecondsRealtime (0.833f);
			main.NotEating = true;
			yield return new WaitForEndOfFrame ();
			Destroy (gameObject);
			main.lasteat = 0;
			main.fMinutes = 0;
			main.fHours = 0;
			TimesEat++;
			if (TimesEat > 0)
				main.FirstEat = false;
			Eat.text = "Comeu: " + TimesEat + " vezes.";
		}
		yield return null;
	}

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.CompareTag ("FoodBay")) {
			main.NotEating = false;
			agent.destination = Edge;
			main.actual = Edge;
		}
	}
}