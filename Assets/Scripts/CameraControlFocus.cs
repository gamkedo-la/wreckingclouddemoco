using UnityEngine;
using System.Collections;

public class CameraControlFocus : MonoBehaviour {

	Vector3 yankBack;

	public float panLong;
	public float panLat;
	public float zoomIn;

	float recentHit = 0.0f;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag != "MainCamera") {
			transform.position = yankBack;
			recentHit = 0.1f;
		}
	}

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		yankBack = transform.position;
		recentHit = 0.0f;
	}

	void FixedUpdate() {
		float kValYank = 0.65f;
		yankBack = yankBack * kValYank +
			transform.position * (1.0f-kValYank);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Minus)) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		if (recentHit > 0.0f) {
			recentHit -= Time.deltaTime;
			return;
		}

		Vector3 pos = Camera.main.transform.position;
		pos.y = Terrain.activeTerrain.SampleHeight(Camera.main.transform.position)+15.5f;
		if(pos.y > Camera.main.transform.position.y) {
			transform.position += pos - Camera.main.transform.position;
		}

		panLong -= Input.GetAxis("Mouse Y") * 100.0f * Time.deltaTime;
		panLat += Input.GetAxis("Mouse X") * 100.0f * Time.deltaTime;
		//zoomIn -= Input.GetAxis("Mouse ScrollWheel") * 80.0f * Time.deltaTime;
		// transform.position += Input.GetAxis("Mouse ScrollWheel") * 80.0f * Time.deltaTime * transform.forward;

		if (Input.GetMouseButton(0)) {
			transform.position += 90.0f * Time.deltaTime * transform.forward;
		}
		if (Input.GetMouseButton(1)) {
			transform.position -= 50.0f * Time.deltaTime * transform.forward;
		}

		panLong = Mathf.Clamp(panLong,-89.5f,89.5f);
		// zoomIn = Mathf.Clamp(zoomIn,35.0f,120.0f);

		transform.position += Input.GetAxis("Horizontal") * 70.0f * Time.deltaTime * transform.right +
			Input.GetAxis("Vertical") * 45.0f * Time.deltaTime * Vector3.up;

		transform.rotation = Quaternion.AngleAxis(panLat, Vector3.up) *
			Quaternion.AngleAxis(panLong, Vector3.right);

		Camera.main.transform.position = transform.position;// - transform.forward * zoomIn;
	}
}
