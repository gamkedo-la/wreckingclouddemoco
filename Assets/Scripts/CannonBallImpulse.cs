﻿using UnityEngine;
using System.Collections;

public class CannonBallImpulse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.up * 27000.0f);
	}
}
