using UnityEngine;
using System.Collections;

public class FallPiece : MonoBehaviour {
	public FallPiece[] supportingThisPiece;
	public bool bustedYet = false;
	public GameObject spawnUponSplit;
	public GameObject dustEffect;
	private float timeSinceLastDust = 0.0f;
	public bool instantShatter = false;
	private float lastGroundHeightSinceSmokeSpawn = 0.0f;

	private Rigidbody rb = null;

	private Collider fallBackCollider;
	private Vector3 fallBackColliderCenter;

	void UseDynamicCollider(bool useDynamic) {
		if(useDynamic) {
			if(bustedYet) {
				return;
			}
			bustedYet = true;
		}

		MeshCollider staticCollider = GetComponent<MeshCollider>();
		// staticCollider.enabled = !useDynamic;
		if(useDynamic) {
			Destroy(staticCollider);
		}

		if(fallBackCollider) {
			fallBackCollider.enabled = useDynamic;
		}

		if(useDynamic) {
			GameObject spawnedEffect = (GameObject)GameObject.Instantiate(spawnUponSplit,
				(instantShatter ? transform.TransformPoint(fallBackColliderCenter) :
					fallBackColliderCenter),
				fallBackCollider.transform.rotation);

			if(instantShatter == false) {
				spawnedEffect.transform.SetParent(transform, false);
				gameObject.AddComponent<Rigidbody>();
				rb = GetComponent<Rigidbody>();
				rb.mass = 2.0f;
				rb.AddTorque(Random.insideUnitSphere * 1200.0f);
				rb.AddForce(Random.insideUnitSphere * 600.0f);
			} else {
				Destroy(gameObject);
			}

			for(int i = 0; i < supportingThisPiece.Length; i++) {
				if(supportingThisPiece[i]) {
					supportingThisPiece[i].BreakAndRelease();
				}
			}
		}
	}

	float heightOffGroundNow() {
		Vector3 testPt = transform.TransformPoint(fallBackColliderCenter);
		float groundAltitudeHere = 
			Terrain.activeTerrain.SampleHeight(
				testPt
			);
		return (testPt.y-groundAltitudeHere);
	}

	void Start() {
		CapsuleCollider fallBackCollider_capsule = GetComponent<CapsuleCollider>();
		if(fallBackCollider_capsule != null) {
			fallBackCollider = GetComponent<Collider>();
			fallBackColliderCenter = fallBackCollider_capsule.center;
		} else {
			SphereCollider fallBackCollider_sphere = GetComponent<SphereCollider>();
			if(fallBackCollider_sphere != null) {
				fallBackCollider = GetComponent<Collider>();
				fallBackColliderCenter = fallBackCollider_sphere.center;
			} else {
				BoxCollider fallBackCollider_box = GetComponent<BoxCollider>();
				if(fallBackCollider_box != null) {
					fallBackCollider = GetComponent<Collider>();
					fallBackColliderCenter = fallBackCollider_box.center;
				}
			}
		}

		lastGroundHeightSinceSmokeSpawn = heightOffGroundNow();
		UseDynamicCollider(false);
	}

	public void BreakAndRelease() {
		UseDynamicCollider(true);
	}

	void Update() {
		if(bustedYet) {
			timeSinceLastDust += Time.deltaTime;
			float possibleNewHeight = heightOffGroundNow();
			if(possibleNewHeight > lastGroundHeightSinceSmokeSpawn) {
				if(possibleNewHeight < 20.0f && timeSinceLastDust > 0.75f) {
					if(rb.velocity.magnitude > 2.5f) { // so slow roll doesn't keep spawning more
						GameObject.Instantiate(dustEffect,
							transform.TransformPoint(fallBackColliderCenter),
							Quaternion.identity);
					}
					timeSinceLastDust = 0.0f;
				}
			}
			lastGroundHeightSinceSmokeSpawn = possibleNewHeight;
		}
	}
}
