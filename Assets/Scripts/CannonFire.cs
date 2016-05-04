using UnityEngine;
using System.Collections;

public class CannonFire : MonoBehaviour {
	public KeyCode triggerKey;
	public GameObject spawnAttackPrefab;
	Transform fireFrom;

	// Use this for initialization
	void Start () {
		fireFrom = transform.Find("FireFrom");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(triggerKey)) {
			GameObject.Instantiate(spawnAttackPrefab, fireFrom.position, fireFrom.rotation);
		}
	}
}
