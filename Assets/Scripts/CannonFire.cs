using UnityEngine;
using System.Collections;

public class CannonFire : MonoBehaviour {
	public KeyCode triggerKey;
	public GameObject spawnAttackPrefab;
	Transform fireFrom;

	public bool autoFire = false;
	public float reloadDelay = 0.75f;
	float reloadLeft = 0.0f;

	public Material reloadMat;

	// Use this for initialization
	void Start () {
		fireFrom = transform.Find("FireFrom");
		if(reloadMat) {
			reloadMat.color = Color.cyan;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(reloadLeft <= 0.0f && reloadMat) {
			reloadMat.color = Color.Lerp(Color.black, Color.cyan, 0.8f + 0.2f*Mathf.Cos(Time.timeSinceLevelLoad*3.0f));
		}

		if(reloadLeft > 0.0f) {
			reloadLeft -= Time.deltaTime;
		}
		if( reloadLeft <= 0.0f && EndOfRoundMessage.instance.beenTriggered == false &&
			( (autoFire==false && Input.GetKeyDown(triggerKey)) ||
				(autoFire && Input.GetKey(triggerKey)) ) ) {
			GameObject.Instantiate(spawnAttackPrefab, fireFrom.position, fireFrom.rotation);
			if(reloadMat) {
				reloadMat.color = Color.black;
			}
			reloadLeft += reloadDelay;
		}
	}
}
