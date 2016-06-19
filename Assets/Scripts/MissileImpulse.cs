﻿using UnityEngine;
using System.Collections;

public class MissileImpulse : MonoBehaviour {
	Rigidbody rb;
	PotentialExploder selfExplode;

	// movement
	float maxSpeed = 80.0f;
	float accel = 4.3f;
	float speedNow = 4.0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		selfExplode = GetComponent<PotentialExploder>();

		Destroy(gameObject, 15.0f);
	}
	
	// Update is called once per frame
	void Update () {
		rb.velocity = transform.up * speedNow;
		transform.Rotate(0.0f, 130.0f * Time.deltaTime, 0.0f);
	}

	void FixedUpdate() {
		speedNow += accel;
		if(speedNow > maxSpeed) {
			speedNow = maxSpeed;
		}
	}

	public void OnCollisionEnter(Collision hitFacts) {
		if (hitFacts.collider.gameObject.layer == LayerMask.NameToLayer ("Player")) {
			return;
		}
		if(transform.childCount > 0) {
			Transform effectsChild = transform.GetChild(0);
			TrailRenderer trScript = effectsChild.GetComponent<TrailRenderer>();
			ParticleSystem psScript = effectsChild.GetComponent<ParticleSystem>();
			ParticleSystem.EmissionModule tempEmit = psScript.emission;
			tempEmit.enabled = false;
			Destroy(effectsChild.gameObject, psScript.startLifetime);
			selfExplode.BlastForce();
			effectsChild.parent = null;
			Destroy(gameObject);

			if(hitFacts.collider.gameObject.layer == LayerMask.NameToLayer("GoldPrize")) {
				GoldGoalTracker.AddPlayerGold();
			}

			FallPiece fpScript = hitFacts.collider.GetComponent<FallPiece>();
			if(fpScript) {
				fpScript.BreakAndRelease();
			} else if(RailShotMotion.isStopShotLayer( hitFacts.gameObject.layer ) == false) {
				Destroy(hitFacts.collider.gameObject);
			}
		}
	}
}
