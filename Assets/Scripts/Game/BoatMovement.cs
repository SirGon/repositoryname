using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMovement : MonoBehaviour {

	private Rigidbody rb;

    public float velocity;
    public float acceleration = 50f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey(KeyCode.I)) {
			//Debug.Log ("entro 1");
			//velocity += acceleration;
			rb.AddForce (transform.forward * velocity);
		}
		if (Input.GetKey(KeyCode.J)) {
			//Debug.Log ("entro 1");
			transform.Rotate(transform.up * -1f);
		}
		if (Input.GetKey(KeyCode.L)) {
			//Debug.Log ("entro 1");
			transform.Rotate(transform.up * +1f);
		}
	}
}
