﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CannonFire : MonoBehaviour {
	public KeyCode triggerKey;
	public GameObject spawnAttackPrefab;
	Transform fireFrom;

	public bool autoFire = false;
	public float reloadDelay = 0.75f;
	float reloadLeft = 0.0f;

	public Material reloadMat;
	private Text rechargeUI;
	private string baseReloadMsg;

	// Use this for initialization
	void Start () {
		fireFrom = transform.Find("FireFrom");
		if(reloadMat) {
			reloadMat.color = Color.cyan;
			GameObject rechargeUIGO = GameObject.Find("FusionRecharge");
			if(rechargeUIGO) {
				rechargeUI = rechargeUIGO.GetComponent<Text>();
				baseReloadMsg = rechargeUI.text;
				rechargeUI.enabled = false;
			}
		}
	}

	IEnumerator updateRechargeTime() {
		while(reloadLeft > 0.1f) {
			rechargeUI.text = baseReloadMsg + ": " + reloadLeft.ToString("F1");
			yield return new WaitForSeconds(0.1f);
		}
		rechargeUI.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(reloadLeft <= 0.0f && reloadMat) {
			reloadMat.color = Color.Lerp(Color.black, Color.cyan, 0.8f + 0.2f*Mathf.Cos(Time.timeSinceLevelLoad*3.0f));
		}

		if(reloadLeft > 0.0f) {
			reloadLeft -= Time.deltaTime;
		}

		bool triggerNow;

		if(triggerKey != KeyCode.None) {
			triggerNow = ((autoFire == false && Input.GetKeyDown(triggerKey)) ||
				(autoFire && Input.GetKey(triggerKey)));
		} else {
			triggerNow = ((autoFire == false && Input.GetMouseButtonDown(0)) ||
				(autoFire && Input.GetMouseButton(0)));
		}

		if( reloadLeft <= 0.0f && EndOfRoundMessage.instance.beenTriggered == false &&
			triggerNow ) {
			GameObject.Instantiate(spawnAttackPrefab, fireFrom.position, fireFrom.rotation);
			reloadLeft += reloadDelay;
			if(reloadMat) {
				reloadMat.color = Color.black;
				rechargeUI.enabled = true;
				StartCoroutine(updateRechargeTime());
			}
		}
	}
}
