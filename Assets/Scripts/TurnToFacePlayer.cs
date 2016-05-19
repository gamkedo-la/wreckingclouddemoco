using UnityEngine;
using System.Collections;

public class TurnToFacePlayer : MonoBehaviour {
	Vector3 aimTargetPos;
	// Use this for initialization
	void Start () {
		aimTargetPos = Camera.main.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(aimTargetPos);
	}

	void FixedUpdate() {
		float chaseK = 0.992f;

		aimTargetPos = aimTargetPos * chaseK +
			Camera.main.transform.position * (1.0f - chaseK);
		aimTargetPos.y = transform.position.y; // to remain planer in base pivot
	}
}
