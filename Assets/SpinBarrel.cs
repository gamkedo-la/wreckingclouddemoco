using UnityEngine;
using System.Collections;

public class SpinBarrel : MonoBehaviour {
	float spinPower = 0.0f;
	float slowDownDecay = 0.955f;
	float shotsReady = 0;
	float minSpinToFire = 400.0f;
	bool spinningUp = false;

	// Use this for initialization
	void Start () {
		spinningUp = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(spinPower > 10.0f) {
			Debug.Log(Mathf.Round(spinPower));
		}*/
		transform.Rotate(0.0f, 0.0f, -spinPower * Time.deltaTime);
	}

	public bool FastEnoughToFire() {
		return spinPower > minSpinToFire;
	}

	public void spinTorque(bool powered) {
		spinningUp = powered;
	}

	void FixedUpdate() {
		if(spinningUp) {
			spinPower += 1000.0f;
		} else {
			spinPower *= slowDownDecay;
		}
	}
}
