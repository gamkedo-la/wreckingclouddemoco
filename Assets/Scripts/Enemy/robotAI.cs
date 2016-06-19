using UnityEngine;
using System.Collections;

public class robotAI : MonoBehaviour {
	public GameObject nearestGoldBox = null;
	// Use this for initialization
	void Start () {
		StartCoroutine (PickNearestGoldBlock ());
	}

	IEnumerator PickNearestGoldBlock(){
		while (true){
			if (nearestGoldBox == null) {
				nearestGoldBox = GoldGoalTracker.NearestTargetToPoint (gameObject.transform.position);
				if (nearestGoldBox != null) {
//					Debug.Log (nearestGoldBox.name);
				} else {
//					Debug.Log ("No gold block found");
				}
			}
			yield return new WaitForSeconds (0.3f);
		}
	}

	void CaptureBlock(GameObject goldBlock){
		GoldGoalTracker.AddEnemyGold (nearestGoldBox);
		Destroy (goldBlock);
//		Debug.Log ("Destroyed gold box");
	}
	
	// Update is called once per frame
	void Update () {
		if (nearestGoldBox != null) {
			Vector3 eyeLevelTarget = nearestGoldBox.transform.position;
			eyeLevelTarget.y = transform.position.y;
			transform.LookAt (eyeLevelTarget);
			transform.position += transform.forward * 6.0f * Time.deltaTime;
			float distToTarget = Vector3.Distance (transform.position, eyeLevelTarget);
//			Debug.Log (Mathf.FloorToInt (distToTarget));
			if (Input.GetKeyDown (KeyCode.O)|| distToTarget < 30.0f) {
				CaptureBlock (nearestGoldBox);
			} // end of destroyed button pressed
		} // end of nearestGoldBox != null test
	} // end update
}
