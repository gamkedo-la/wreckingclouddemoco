using UnityEngine;
using System.Collections;

public class DriveWrecking : MonoBehaviour {
	public bool isPiloting = true;

	public float panLong;
	public float panLat;

	private float panLongV;
	private float panLatV;
	private float velFalloff = 0.92f;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		if(isPiloting == false && EndOfRoundMessage.instance.beenTriggered == false) {
			if(Input.GetKey(KeyCode.C)) {
				// transform.position += transform.forward * Time.deltaTime * 40.0f;
				CameraControlFocus ccf = Camera.main.transform.GetComponentInParent<CameraControlFocus>();
				ccf.enabled = false;

				Transform camPos = transform.Find("PilotCamPos");
				Camera.main.transform.position = camPos.position;
				Camera.main.transform.rotation = camPos.rotation;
				Camera.main.transform.parent = transform;

				for(int i = 0; i < Camera.main.transform.childCount; i++) {
					Camera hasCamTest = Camera.main.transform.GetChild(i).GetComponent<Camera>();
					if(hasCamTest == null) {
						Camera.main.transform.GetChild(i).gameObject.SetActive(false);
					}
				}
				isPiloting = true;
			}
		} else if(EndOfRoundMessage.instance.beenTriggered == false) {
			panLongV -= Input.GetAxis("Mouse Y") * 2.0f * Time.deltaTime;
			panLatV += Input.GetAxis("Mouse X") * 3.0f * Time.deltaTime;

			panLong += panLongV;
			panLat += panLatV;

			transform.position += transform.right * Input.GetAxis("HorizontalP2") * 12.0f * Time.deltaTime;
			transform.position += transform.forward * Input.GetAxis("VerticalP2") * 25.0f * Time.deltaTime;

			panLong = Mathf.Clamp(panLong, -29.5f, 29.5f);

			transform.rotation = Quaternion.AngleAxis(panLat, Vector3.up)
				* Quaternion.AngleAxis(panLong, Vector3.right);
		}
	}

	void FixedUpdate() {
		panLongV *= velFalloff;
		panLatV *= velFalloff;
	}
}
