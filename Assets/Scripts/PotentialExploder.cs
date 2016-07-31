using UnityEngine;
using System.Collections;

public class PotentialExploder : MonoBehaviour {
	// explosion
	float range = 8.0f;
	float power = 2500.0f;
	float upwardsPowerBoost = 3.0f;
	public GameObject fireParticles;
	public GameObject smokeParticles;
	public int damage = 10;

	public int hitPoints = 20;
	private int maxHP = 20;

	int explodeLayer;

	void Start() {
		explodeLayer = LayerMask.NameToLayer("Explosive");
		maxHP = hitPoints;
	}

	public string HealthPerc() {
		int fakePerc = 100 * hitPoints / maxHP;
		return ""+fakePerc+"%";
	}

	public void ApplyDamage(int thisMany) {
		hitPoints -= thisMany;
		if(hitPoints < 0) {
			BlastForce();
		}
	}

	public bool didBlast = false;
	public void BlastForce(bool doRemove=true) {
		if(didBlast) {
			return;
		}
		didBlast = true;
		SoundSet.PlayClipByName("Explosion with Metal Debris", Random.Range(0.7f, 1.0f));
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explosionPos, range);

		foreach (Collider hit in colliders) {
			Rigidbody rb = hit.GetComponent<Rigidbody>();
			if(rb != null) {
				rb.AddExplosionForce(power, explosionPos, range, upwardsPowerBoost);

				FallPiece fpScript = rb.GetComponent<FallPiece>();
				if(fpScript) {
					fpScript.BreakAndRelease(damage);
				} else {
					PotentialExploder peScript = rb.GetComponent<PotentialExploder>();
					if(peScript && peScript != this) {
						peScript.ApplyDamage(damage);
					}
				}

				/*if(rb.gameObject.layer == explodeLayer) {
					PotentialExploder pe = rb.gameObject.GetComponent<PotentialExploder>();
					if(pe) {
						pe.BlastForce();
					}
				}*/
			}
		}
		if (fireParticles) {
			GameObject fire = Instantiate (fireParticles, gameObject.transform.position, Quaternion.identity) as GameObject;
			Destroy (fire, 6);
		}
		if (smokeParticles) {
			GameObject smoke = Instantiate (smokeParticles, gameObject.transform.position, Quaternion.identity) as GameObject;
			Destroy (smoke, 6);
		}
		if(doRemove) {
			Destroy(gameObject);
		} else {
			StartCoroutine(clearReblast());
		}
	}

	IEnumerator clearReblast() {
		yield return new WaitForSeconds(0.2f);
		didBlast = false;
	}
}
