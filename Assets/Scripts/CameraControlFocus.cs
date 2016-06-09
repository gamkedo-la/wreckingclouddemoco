using UnityEngine;
using System.Collections;

public class CameraControlFocus : MonoBehaviour {

	public bool isGrounded = true;

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

		if(Input.GetKeyDown(KeyCode.E)) {
			isGrounded = !isGrounded;
			if(isGrounded == false) {
				transform.position += Vector3.up * 10.0f;
			}
		}

		if (recentHit > 0.0f) {
			recentHit -= Time.deltaTime;
			return;
		}

		Vector3 pos = Camera.main.transform.position;
		pos.y = Terrain.activeTerrain.SampleHeight(Camera.main.transform.position)+15.5f;
		if(pos.y > Camera.main.transform.position.y || isGrounded) {
			transform.position += pos - Camera.main.transform.position + 
				(isGrounded ? Vector3.down*8.0f : Vector3.zero);
		}

		panLong -= Input.GetAxis("Mouse Y") * 50.0f * Time.deltaTime;
		panLat += Input.GetAxis("Mouse X") * 50.0f * Time.deltaTime;
		//zoomIn -= Input.GetAxis("Mouse ScrollWheel") * 80.0f * Time.deltaTime;
		// transform.position += Input.GetAxis("Mouse ScrollWheel") * 80.0f * Time.deltaTime * transform.forward;

		if (Input.GetMouseButton(0)) {
			transform.position += 23.0f * Time.deltaTime * transform.forward;
		}
		if (Input.GetMouseButton(1)) {
			transform.position -= 15.0f * Time.deltaTime * transform.forward;
		}

		panLong = Mathf.Clamp(panLong, -89.5f,isGrounded ? 10.0f : 89.5f);
		// zoomIn = Mathf.Clamp(zoomIn,35.0f,120.0f);

		transform.position += Input.GetAxis("Horizontal") * 27.0f * Time.deltaTime * transform.right +
			(isGrounded ? Vector3.zero : Input.GetAxis("Vertical") * 35.0f * Time.deltaTime * Vector3.up);

		transform.rotation = Quaternion.AngleAxis(panLat, Vector3.up)
			* Quaternion.AngleAxis(panLong, Vector3.right);

		Camera.main.transform.position = transform.position;// - transform.forward * zoomIn;
	}
}
