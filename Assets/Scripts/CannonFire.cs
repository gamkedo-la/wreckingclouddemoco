using UnityEngine;
using System.Collections;

public class CannonFire : MonoBehaviour {
	public KeyCode triggerKey;
	public GameObject spawnAttackPrefab;
	Transform fireFrom;

	public bool autoFire = false;
	public float reloadDelay = 0.75f;
	float reloadLeft = 0.0f;

	// Use this for initialization
	void Start () {
		fireFrom = transform.Find("FireFrom");
	}
	
	// Update is called once per frame
	void Update () {
		if(reloadLeft > 0.0f) {
			reloadLeft -= Time.deltaTime;
		}
		if( reloadLeft <= 0.0f &&
			( (autoFire==false && Input.GetKeyDown(triggerKey)) ||
				(autoFire && Input.GetKey(triggerKey)) ) ) {
			GameObject.Instantiate(spawnAttackPrefab, fireFrom.position, fireFrom.rotation);
			reloadLeft += reloadDelay;
		}
	}
}
