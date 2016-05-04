using UnityEngine;
using System.Collections;

public class MissileImpulse : MonoBehaviour {
	Rigidbody rb;

	// explosion
	float range = 8.0f;
	float power = 2500.0f;
	float upwardsPowerBoost = 3.0f;

	// movement
	float maxSpeed = 80.0f;
	float accel = 4.3f;
	float speedNow = 4.0f;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		rb.velocity = transform.up * speedNow;
	}

	void FixedUpdate() {
		speedNow += accel;
		if(speedNow > maxSpeed) {
			speedNow = maxSpeed;
		}
	}

	public void BlastForce() {
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, range);
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody>();
			if (rb != null)
				rb.AddExplosionForce(power, explosionPos, range, upwardsPowerBoost);
		}
	}

	public void OnCollisionEnter(Collision hitFacts) {
		if(transform.childCount > 0) {
			Transform effectsChild = transform.GetChild(0);
			TrailRenderer trScript = effectsChild.GetComponent<TrailRenderer>();
			Destroy(effectsChild.gameObject, trScript.time);
			BlastForce();
			effectsChild.parent = null;
			Destroy(gameObject);
			if(hitFacts.gameObject.layer != LayerMask.NameToLayer("Terrain")) {
				Destroy(hitFacts.collider.gameObject);
			}
		}
	}
}
