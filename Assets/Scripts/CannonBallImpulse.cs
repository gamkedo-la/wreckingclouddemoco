using UnityEngine;
using System.Collections;

public class CannonBallImpulse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.up * 9000.0f);
	}
}
