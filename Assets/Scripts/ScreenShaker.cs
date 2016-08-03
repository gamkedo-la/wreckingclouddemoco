﻿using UnityEngine;
using System.Collections;

public class ScreenShaker : MonoBehaviour {
	public static ScreenShaker instance;
	float shakeAmt = 0.0f;
	float decayRate = 0.93f;
	float MAX_POWER = 130.0f;
	Vector3 startLocalPos;
	Quaternion startLocalRot;

	void Start () {
		instance = this;
		startLocalPos = transform.localPosition;
		startLocalRot = transform.localRotation;
	}

	void Update() {
		transform.localPosition = startLocalPos + Random.insideUnitSphere * shakeAmt * 0.0125f;
		float rotAmt = Random.Range(-1.0f,1.0f) * shakeAmt * 0.1f;
		transform.localRotation = startLocalRot;
		transform.localRotation *= Quaternion.AngleAxis(rotAmt, Vector3.right);
		transform.localRotation *= Quaternion.AngleAxis(rotAmt, Vector3.up);
	}

	public void BlastShake(float power) {
		shakeAmt += power;
		if(shakeAmt > MAX_POWER) {
			shakeAmt = MAX_POWER;
		}
	}
	
	void FixedUpdate () {
		shakeAmt *= decayRate;
	}
}
