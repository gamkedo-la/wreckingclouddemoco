using UnityEngine;
using System.Collections;

public class CameraControlFocus : MonoBehaviour {

	public bool isGrounded = true;

	Vector3 yankBack;

	public float panLong;
	public float panLat;
	public float zoomIn;

	float recentHit = 0.0f;
	Rigidbody rb;

	/*void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag != "MainCamera") {
			Rigidbody rb = other.GetComponent<Rigidbody>();
			if(rb) {
				rb.AddForceAtPosition((Vector3.up * 5.0f +
					transform.right * Random.RandomRange(-10.0f,10.0f))*600.0f, transform.position);
			}
		}
	}*/

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		rb = GetComponent<Rigidbody>();
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

		/*if(Input.GetKeyDown(KeyCode.E)) {
			isGrounded = !isGrounded;
			if(isGrounded == false) {
				transform.position += Vector3.up * 10.0f;
			}
		}*/

		if (recentHit > 0.0f) {
			recentHit -= Time.deltaTime;
			return;
		}

		Vector3 pos = transform.position;
		pos.y = Terrain.activeTerrain.SampleHeight(transform.position)+15.5f;
		if(pos.y > transform.position.y || isGrounded) {
			transform.position += pos - transform.position + 
				(isGrounded ? Vector3.down*8.0f : Vector3.zero);
		}
		if (EndOfRoundMessage.instance.beenTriggered == false) {

			// panLong -= Input.GetAxis("Mouse Y") * 50.0f * Time.deltaTime;
			// panLat += Input.GetAxis("Mouse X") * 50.0f * Time.deltaTime;
			panLat += Input.GetAxis ("HorizontalP2") * 50.0f * Time.deltaTime;
			//zoomIn -= Input.GetAxis("Mouse ScrollWheel") * 80.0f * Time.deltaTime;
			// transform.position += Input.GetAxis("Mouse ScrollWheel") * 80.0f * Time.deltaTime * transform.forward;

			Vector3 fowardPush = Vector3.zero;

			if (isGrounded == false) {
				transform.position += 40.0f * Time.deltaTime * transform.forward;
			} else {
				if (isGrounded && Input.GetAxis ("VerticalP2") > 0.5f) {
					fowardPush = (isGrounded ? Input.GetAxis ("VerticalP2") : 1.0f) *
					23.0f * Time.deltaTime * transform.forward;
				}
				if (isGrounded && Input.GetAxis ("VerticalP2") < 0.5f) {
					fowardPush = (isGrounded ? -Input.GetAxis ("VerticalP2") : 1.0f) *
					-15.0f * Time.deltaTime * transform.forward;
				}
				rb.AddForce (fowardPush * 800.0f);
			}
		}
		panLong = Mathf.Clamp(panLong, -89.5f,isGrounded ? 10.0f : 89.5f);
		// zoomIn = Mathf.Clamp(zoomIn,35.0f,120.0f);

		/* transform.position += Input.GetAxis("Horizontal") * 27.0f * Time.deltaTime * transform.right +
			(isGrounded ? Vector3.zero : Input.GetAxis("VerticalP2") * 35.0f * Time.deltaTime * Vector3.up); */

		transform.rotation = Quaternion.AngleAxis(panLat, Vector3.up)
			* Quaternion.AngleAxis(panLong, Vector3.right);
	}
}
