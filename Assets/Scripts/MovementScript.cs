using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class MovementScript : MonoBehaviour
{
	public float movementSpeed = 1000.0f;
	public float clockwise = 5.0f;
	public float counterClockwise = -5.0f;

	void Start () {

	}

	void Update () {
		if(Input.GetKey(KeyCode.W)) {
			transform.position -= transform.up* Time.deltaTime * movementSpeed;
		}
		else if(Input.GetKey(KeyCode.S)) {
			transform.position += transform.up* Time.deltaTime * movementSpeed;
		}


		if(Input.GetKey(KeyCode.E)) {
			transform.Rotate(0, 0,Time.deltaTime * clockwise);
		}
		else if(Input.GetKey(KeyCode.Q)) {
			transform.Rotate(0, 0,Time.deltaTime * counterClockwise);
		}
	}
}