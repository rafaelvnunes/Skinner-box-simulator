using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Timers;

[RequireComponent(typeof(NavMeshAgent))]
public class Main : MonoBehaviour {
	public Lever LeverScript;
	public Transform Lever;
	public Animator animator;
	[HideInInspector]
	public bool NotEating,
		FirstEat;
	[HideInInspector]
	public float propensity = 0f;
	[HideInInspector]
	public float lasteat;
	[HideInInspector]
	public Vector3 actual = new Vector3();
	[HideInInspector]
	public bool ready = false;
	[HideInInspector]
	public int fSeconds, fMinutes, fHours;

	NavMeshAgent agent;
	float kh,
		elapsedTime = 0f;
	bool firsttime;
	int seconds, minutes, hours;
	Vector3 nav_lever = new Vector3 ();
	Vector3[] vertex;
	Text TotalTime,
		EatTime,
		Eat;

	// Use this for initialization
	void Start () {
		TotalTime = GameObject.Find ("Tempo_Total").GetComponent<Text> ();
		EatTime = GameObject.Find ("Tempo_Comida").GetComponent<Text> ();
		Eat = GameObject.Find ("X_Comeu").GetComponent<Text> ();
		Eat.text = "Comeu: 0 vezes.";
		EatTime.text = "Ainda não comeu";
		agent = GetComponent<NavMeshAgent>();
		vertex = new Vector3[NavMesh.CalculateTriangulation().vertices.Length];
		NavMesh.CalculateTriangulation ().vertices.CopyTo (vertex, 0);
		actual.Set (agent.nextPosition.x, agent.nextPosition.y, agent.nextPosition.z);
		nav_lever.Set (Lever.position.x, agent.nextPosition.y, vertex [0].z);
		firsttime = true;
		NotEating = true;
		FirstEat = true;
		StartCoroutine (Hungry ());
		StartCoroutine (Idle ());
		StartCoroutine (APP_Time ());
	}

	Vector3 RandomNextPoint () {
		float z = Random.Range(vertex[0].z, vertex[1].z);
		float x = Random.Range(vertex[0].x, vertex[2].x);
		actual.Set (x, agent.nextPosition.y, z);
		return actual;
	}

	public IEnumerator Idle () {
		while (true) {
			if (NotEating) {
				if (Vector3.Distance (agent.nextPosition, actual) <= .1f) {
					float idletime = Random.Range (0, 5);
					animator.Play ("Idle");
					if (!actual.Equals (nav_lever))
						yield return new WaitForSecondsRealtime (idletime);
					agent.destination = RandomNextPoint ();
					animator.Play ("Andar");
					if (propensity >= 1f) {
						agent.destination = nav_lever;
						actual = nav_lever;
						if (agent.nextPosition.Equals (nav_lever)) {
							Vector3 relative = Lever.transform.position - transform.position;
							relative.y = 0f;
							Quaternion rotation = Quaternion.LookRotation (relative);
							Quaternion starting_r = transform.rotation;
							while (Quaternion.Angle (transform.rotation, rotation) != 0f) {
								elapsedTime += Time.deltaTime;
								transform.rotation = Quaternion.Slerp (starting_r, rotation, elapsedTime);
								yield return new WaitForEndOfFrame ();
							}
							elapsedTime = 0f;
							propensity = 0f;
							animator.Play ("Subir");
							yield return new WaitForSecondsRealtime (1.666f);
							Lever.GetComponent<Animator> ().Play ("Idle");
						}
					}
				}
			} else {
				GameObject Food = GameObject.FindWithTag ("Comida");
				if (Vector3.Distance (agent.nextPosition, actual) <= .1f) {
					Vector3 relativef = Food.transform.position - transform.position;
					relativef.y = 0f;
					Quaternion rotationf = Quaternion.LookRotation (relativef);
					Quaternion starting_rf = transform.rotation;
					while (Quaternion.Angle (transform.rotation, rotationf) != 0f) {
						elapsedTime += Time.deltaTime;
						transform.rotation = Quaternion.Slerp (starting_rf, rotationf, elapsedTime);
						yield return new WaitForEndOfFrame ();
					}
					elapsedTime = 0f;
				}
			}
			yield return null;
		}
	}

	IEnumerator Hungry () {
		while (true) {
			yield return new WaitForSecondsRealtime (1f);
			if (firsttime) {
				kh = lasteat / 900000; // 900.000 = 22 min, 90.000 = 7 min
			} else
				kh = lasteat / 360000;
			propensity += kh;
			yield return null;
		}
	}

	IEnumerator APP_Time () {
		while (true) {
			yield return new WaitForSecondsRealtime (1f);
			seconds++;
			if (seconds >= 60) {
				minutes++;
				seconds = 0;
				if (minutes >= 60) {
					hours++;
					minutes = 0;
				}
			}
			TotalTime.text = "Duração: " + hours + "h" + minutes + "m" + seconds + "s.";
			lasteat++;
			if (!FirstEat) {
				if (lasteat >= 60) {
					fMinutes = (int)lasteat / 60;
					fSeconds = (int)lasteat % 60;
					if (fMinutes >= 60) {
						fHours = fMinutes / 60;
						fMinutes = fMinutes % 60;
					}
				} else
					fSeconds = (int)lasteat;
				EatTime.text = "Comeu há: " + fHours + "h" + fMinutes + "m" + fSeconds + "s.";
			}
			yield return null;
		}
	}
}