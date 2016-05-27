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
	private CapsuleCollider fallBackCollider;

	void UseDynamicCollider(bool useDynamic) {
		if(useDynamic) {
			if(bustedYet) {
				return;
			}
			bustedYet = true;
		}

		MeshCollider staticCollider = GetComponent<MeshCollider>();
		staticCollider.enabled = !useDynamic;

		fallBackCollider.enabled = useDynamic;

		if(useDynamic) {
			GameObject spawnedEffect = (GameObject)GameObject.Instantiate(spawnUponSplit,
				(instantShatter ? transform.TransformPoint(fallBackCollider.center) :
					fallBackCollider.center),
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
		Vector3 testPt = transform.TransformPoint(fallBackCollider.center);
		float groundAltitudeHere = 
			Terrain.activeTerrain.SampleHeight(
				testPt
			);
		return (testPt.y-groundAltitudeHere);
	}

	void Start() {
		fallBackCollider = GetComponent<CapsuleCollider>();
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
							transform.TransformPoint(fallBackCollider.center),
							Quaternion.identity);
					}
					timeSinceLastDust = 0.0f;
				}
			}
			lastGroundHeightSinceSmokeSpawn = possibleNewHeight;
		}
	}
}
