using UnityEngine;
using System.Collections;

public class CameraControlFocus : MonoBehaviour {

	public float panLong;
	public float panLat;
	public float zoomIn;

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
		panLong -= Input.GetAxis("Mouse Y") * 100.0f * Time.deltaTime;
		panLat += Input.GetAxis("Mouse X") * 100.0f * Time.deltaTime;
		//zoomIn -= Input.GetAxis("Mouse ScrollWheel") * 80.0f * Time.deltaTime;
		transform.position += Input.GetAxis("Mouse ScrollWheel") * 80.0f * Time.deltaTime * transform.forward;

		panLong = Mathf.Clamp(panLong,-3.0f,89.5f);
		zoomIn = Mathf.Clamp(zoomIn,35.0f,120.0f);

		transform.position += Input.GetAxis("Horizontal") * 30.0f * Time.deltaTime * transform.right +
			Input.GetAxis("Vertical") * 30.0f * Time.deltaTime * Vector3.up;

		transform.rotation = Quaternion.AngleAxis(panLat, Vector3.up) *
			Quaternion.AngleAxis(panLong, Vector3.right);

		Camera.main.transform.position = transform.position - transform.forward * zoomIn;
	}
}
