using UnityEngine;
using System.Collections;

public class TerminalVelocity : MonoBehaviour {

	public float maxSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(rigidbody.velocity.magnitude > maxSpeed)
		{
			rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
		}
	
	}
}
