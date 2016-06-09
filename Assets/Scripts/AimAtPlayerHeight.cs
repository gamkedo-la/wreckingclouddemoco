using UnityEngine;
using System.Collections;

public class AimAtPlayerHeight : MonoBehaviour {
	Vector3 aimTargetPos;
	float aimY;
	// Use this for initialization
	void Start () {
		aimTargetPos = transform.position + transform.forward * 30.0f;
		aimTargetPos.y = aimY;
		aimY = Camera.main.transform.position.y;
	}

	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Slerp(transform.rotation,
			Quaternion.LookRotation(aimTargetPos-transform.position), Time.deltaTime * 1.0f);
	}

	void FixedUpdate() {
		float chaseK = 0.992f;

		aimTargetPos = transform.position + transform.forward * 30.0f;
		aimY = aimY * chaseK +
			Camera.main.transform.position.y * (1.0f - chaseK);
		aimTargetPos.y = aimY;
	}
}
