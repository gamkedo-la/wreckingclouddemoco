using UnityEngine;
using System.Collections;

public class ForcePush : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float blastRad = 40.0f;
		Collider[] list = Physics.OverlapSphere(transform.position, blastRad);
		int playerLayer = LayerMask.NameToLayer("Player");
		for(int i = 0; i < list.Length; i++) {
			Rigidbody rb = list[i].GetComponent<Rigidbody>();
			if(rb && list[i].gameObject.layer != playerLayer) {
				rb.AddExplosionForce(2000.0f, transform.position, blastRad);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
