using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CannonFire : MonoBehaviour {
	public KeyCode triggerKey;
	public GameObject spawnAttackPrefab;
	Transform fireFrom;
	public string fireSoundName;

	public bool autoFire = false;
	public float reloadDelay = 0.75f;
	float reloadLeft = 0.0f;

	public Material reloadMat;
	private Text rechargeUI;
	private string baseReloadMsg;

	public GameObject startChargeEffect;
	public Transform chargeEffectLoc;
	public SpinBarrel spinner;
	public Transform fireParticleParent;
	private ParticleSystem[] fireParticles;

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
		if(fireParticleParent) {
			fireParticles = fireParticleParent.GetComponentsInChildren<ParticleSystem>();
			for(int i = 0; i < fireParticles.Length; i++) {
				ParticleSystem.EmissionModule emitter = fireParticles[i].emission;
				emitter.enabled = false;
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

	IEnumerator delayedBlast() {
		if(startChargeEffect) {
			GameObject tempGO = GameObject.Instantiate(startChargeEffect, chargeEffectLoc.position, chargeEffectLoc.rotation) as GameObject;
			tempGO.transform.parent = chargeEffectLoc;
		}
		yield return new WaitForSeconds(2.0f);
		GameObject.Instantiate(spawnAttackPrefab, fireFrom.position, fireFrom.rotation);
	}

	void Shoot () {
		if(fireSoundName.Length > 1) {
			SoundSet.PlayClipByName(fireSoundName, Random.Range(0.9f, 1.0f));
		}
		GameObject.Instantiate(spawnAttackPrefab, fireFrom.position, fireFrom.rotation);
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


		if(EndOfRoundMessage.instance.beenTriggered == false &&
		   triggerNow) {
			if(reloadLeft <= 0.0f) {
				reloadLeft += reloadDelay;
				if(reloadMat) {
					reloadMat.color = Color.black;
					rechargeUI.enabled = true;
					SoundSet.PlayClipByName("WCPowerUp", 1.0f);
					StartCoroutine(updateRechargeTime());
					StartCoroutine(delayedBlast());
				} else {
					if(spinner) {
						spinner.spinTorque(true);
						if(spinner.FastEnoughToFire()) {
							if(fireParticleParent) {
								for(int i = 0; i < fireParticles.Length; i++) {
									ParticleSystem.EmissionModule emitter = fireParticles[i].emission;
									emitter.enabled = true;
								}
							}
							Shoot();
						}
					} else { // simpler when no spinning barrel
						Shoot();
					}
				}
			}
		} else {
			if(spinner) {
				spinner.spinTorque(false);
				if(fireParticleParent) {
					for(int i = 0; i < fireParticles.Length; i++) {
						ParticleSystem.EmissionModule emitter = fireParticles[i].emission;
						emitter.enabled = false;
					}
				}
			}
		}
	}
}
