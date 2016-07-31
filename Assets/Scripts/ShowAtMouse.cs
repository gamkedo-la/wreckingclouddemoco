using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShowAtMouse : MonoBehaviour {
	Image aimer;

	public Transform bodyTransform;
	public Transform leftArmWep;
	public Transform rightArmWep;

	int aimerMask;

	// Use this for initialization
	void Start () {
		aimer = GetComponent<Image>();
		aimerMask = ~LayerMask.GetMask("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if(Cursor.lockState == CursorLockMode.Locked) {
			Cursor.lockState = CursorLockMode.Confined;
			Cursor.visible = false;
		}

		RaycastHit rhInfo;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Vector3 goalPt;
		Quaternion targetDir;
		if(Physics.Raycast(ray, out rhInfo, 200.0f, aimerMask)) {
			goalPt = rhInfo.point;
		} else {
			goalPt = Camera.main.transform.position + ray.direction * 100.0f;
		}

		Vector3 relPos = bodyTransform.InverseTransformPoint(goalPt);
		float nearestAim = 8.0f;
		if(relPos.z < nearestAim) {
			relPos.z = nearestAim;
			goalPt = bodyTransform.TransformPoint(relPos);
		}

		targetDir = Quaternion.LookRotation(goalPt - leftArmWep.position);
		leftArmWep.rotation = Quaternion.Slerp(leftArmWep.rotation, targetDir, Time.deltaTime * 10.0f);
		targetDir = Quaternion.LookRotation(goalPt - rightArmWep.position);
		rightArmWep.rotation = Quaternion.Slerp(rightArmWep.rotation, targetDir, Time.deltaTime * 10.0f);

		Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		aimer.transform.position = mousePosition;

	}
}
