using UnityEngine;
using System.Collections;

public class PlayerChaser : MonoBehaviour {
	float chaseToRangeMin = 35.0f;
	float chaseToRangeMax = 50.0f;
	float speedMin = 40.0f;
	float speedMax = 55.0f;

	float tooClose = 25.0f;
	float chaseToRange = 30.0f; // based on min and max above, varies per enemy
	float driveSpeed = 50.0f;
	Vector3 aimTargetPos;

	Vector3 flyToPt;

	void newGoal() {
		Vector3 randGoalOffset;
		randGoalOffset = Random.onUnitSphere;
		randGoalOffset.y *= 0.4f; // compress vertical offset range
		flyToPt = Camera.main.transform.position
			+ randGoalOffset.normalized * chaseToRange;
	}

	IEnumerator changeFlyToPt() {
		while(true) {
			newGoal();
			yield return new WaitForSeconds( Random.Range(2.5f,6.0f) );
		}
	}

	// Use this for initialization
	void Start () {
		chaseToRange = Random.Range(chaseToRangeMin, chaseToRangeMax);
		driveSpeed = Random.Range(speedMin, speedMax);

		aimTargetPos = Camera.main.transform.position;
		StartCoroutine(changeFlyToPt());
	}

	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, Camera.main.transform.position) < tooClose) {
			newGoal();
			transform.position += transform.forward * driveSpeed * Time.deltaTime;
		} else if(Vector3.Distance(transform.position, flyToPt) > chaseToRange) {
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(flyToPt-transform.position), Time.deltaTime * 1.0f);
			transform.position += transform.forward * driveSpeed * Time.deltaTime;
		} else {
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(aimTargetPos-transform.position), Time.deltaTime * 1.0f);
		}
	}

	void FixedUpdate() {
		float chaseK = 0.992f;

		aimTargetPos = aimTargetPos * chaseK +
			Camera.main.transform.position * (1.0f - chaseK);
		aimTargetPos.y = transform.position.y; // to remain planer in base pivot
	}
}
