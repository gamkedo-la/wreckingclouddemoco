using UnityEngine;
using System.Collections;

public class PotentialExploder : MonoBehaviour {
	// explosion
	float range = 8.0f;
	float power = 2500.0f;
	float upwardsPowerBoost = 3.0f;

	int explodeLayer;

	void Start() {
		explodeLayer = LayerMask.NameToLayer("Explosive");
	}

	public bool didBlast = false;
	public void BlastForce() {
		if(didBlast) {
			return;
		}
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, range);
		didBlast = true;
		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody>();
			if(rb != null) {
				rb.AddExplosionForce(power, explosionPos, range, upwardsPowerBoost);
				if(rb.gameObject.layer == explodeLayer) {
					PotentialExploder pe = rb.gameObject.GetComponent<PotentialExploder>();
					if(pe) {
						pe.BlastForce();
					}
				}
			}
		}
		Destroy(gameObject);
	}

}
