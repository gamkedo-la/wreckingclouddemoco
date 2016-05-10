using UnityEngine;
using System.Collections;

public class RailShotMotion : MonoBehaviour {
	Rigidbody rb;

	// movement
	float maxSpeed = 2000.0f;

	void Start () {
		rb = GetComponent<Rigidbody>();
		RaycastHit[] allHits = Physics.RaycastAll(transform.position, transform.up);
		for(int i = 0; i < allHits.Length; i++) {
			if(allHits[i].collider.gameObject.layer != LayerMask.NameToLayer("Terrain")) {
				Destroy(allHits[i].collider.gameObject);
			}
		}
	}

	void Update () {
		rb.velocity = transform.up * maxSpeed;
	}

	public void OnCollisionEnter(Collision hitFacts) {
		if(transform.childCount > 0) {
			if(hitFacts.gameObject.layer != LayerMask.NameToLayer("Terrain")) {
				Destroy(hitFacts.collider.gameObject);
			} else {
				Transform effectsChild = transform.GetChild(0);
				TrailRenderer trScript = effectsChild.GetComponent<TrailRenderer>();
				Destroy(effectsChild.gameObject, trScript.time);
				effectsChild.parent = null;
				Destroy(gameObject);
			}
		}
	}
}
