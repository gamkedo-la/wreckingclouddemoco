using UnityEngine;
using System.Collections;

public class CannonBallImpulse : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.AddForce((Quaternion.AngleAxis(Random.Range(-5.0f,5.0f),Vector3.right)*
			Quaternion.AngleAxis(Random.Range(-4.0f,4.0f),Vector3.up)*
			transform.up) * 2000.0f);
		Destroy(gameObject, 7.0f); // just so they don't pile up forever
	}
}
