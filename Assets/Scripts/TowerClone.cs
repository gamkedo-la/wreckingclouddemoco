using UnityEngine;
using System.Collections;

public class TowerClone : MonoBehaviour {
	int cubesWide;
	int cubesLong;
	int cubesTall;

	public GameObject sideCornerPrefab;
	public GameObject topEdgePrefab;
	public GameObject topCornerPrefab;
	public GameObject insidePrefab;
	public GameObject topPrefab;
	public GameObject sidePrefab;
	public GameObject goldPrizePrefab;
	private bool useTileKinds;
	private bool addToGoldList = false;

	public static int blocksSinceGold = 0; // ensures rand odds don't keep us from getting too few blocks total

	void Start() {
		cubesWide = 3+Random.Range(0,2);
		cubesLong = 4+Random.Range(0,2);
		cubesTall = 5+Random.Range(0,6);

		useTileKinds = sideCornerPrefab && topEdgePrefab && topCornerPrefab &&
		insidePrefab && topPrefab && sidePrefab;
	}

	// Use this for initialization
	void LateUpdate () {
		GameObject newParent = new GameObject(name);
		for(int w = 0; w < cubesWide; w++) {
			for(int l = 0; l < cubesLong; l++) {
				for(int t = 0; t < cubesTall; t++) {
					GameObject preFabHere;
					Quaternion rotBy = Quaternion.AngleAxis(-90.0f,Vector3.right);

					if(useTileKinds == false) {
						preFabHere = gameObject;
					} else{
						bool isTopLayer = (t == cubesTall - 1);
						bool isWideSide = (w == 0 || w == cubesWide - 1);
						bool isLongSide = (l == 0 || l == cubesLong - 1);

						if(isTopLayer) {
							if(isWideSide) {
								if(isLongSide) {
									preFabHere = topCornerPrefab;
									if(l == 0 && w == 0) {
										rotBy *= Quaternion.AngleAxis(90.0f, Vector3.forward);
									} else if(l == cubesLong - 1 && w == 0) {
										rotBy *= Quaternion.AngleAxis(180.0f, Vector3.forward);
									} else if(l == cubesLong - 1 && w == cubesWide - 1) {
										rotBy *= Quaternion.AngleAxis(270.0f, Vector3.forward);
									}
								} else {
									preFabHere = topEdgePrefab;
									if(w == 0) {
										rotBy *= Quaternion.AngleAxis(180.0f,Vector3.up);
										rotBy *= Quaternion.AngleAxis(180.0f,Vector3.right);
									}								}
							} else if(isLongSide) {
								preFabHere = topEdgePrefab;

								if(l == 0) {
									rotBy *= Quaternion.AngleAxis(90.0f, Vector3.forward);
								} else {
									rotBy *= Quaternion.AngleAxis(-90.0f, Vector3.forward);
								}
							} else {
								preFabHere = topPrefab;

								rotBy = Quaternion.AngleAxis(
									Random.Range(0,4)*90.0f,Vector3.up) * rotBy;
							}
						} else if(isWideSide) {
							if(isLongSide) {
								preFabHere = sideCornerPrefab;
								if(l == 0 && w == 0) {
									rotBy *= Quaternion.AngleAxis(90.0f, Vector3.forward);
								} else if(l == cubesLong - 1 && w == 0) {
									rotBy *= Quaternion.AngleAxis(180.0f, Vector3.forward);
								} else if(l == cubesLong - 1 && w == cubesWide - 1) {
									rotBy *= Quaternion.AngleAxis(270.0f, Vector3.forward);
								}
							} else {
								preFabHere = sidePrefab;

								if(w == 0) {
									rotBy *= Quaternion.AngleAxis(180.0f,Vector3.up);
									rotBy *= Quaternion.AngleAxis(180.0f,Vector3.right);
								}
							}
						} else if(isLongSide) {
							preFabHere = sidePrefab;
							if(l == 0) {
								rotBy *= Quaternion.AngleAxis(90.0f, Vector3.forward);
							} else {
								rotBy *= Quaternion.AngleAxis(-90.0f, Vector3.forward);
							}
						} else {
							if (EndOfRoundMessage.instance.isCombatMode == false && 
								(Random.Range (0, 100) < 6 || blocksSinceGold>30)) {
								preFabHere = goldPrizePrefab;
								addToGoldList = true;
								blocksSinceGold = 0;
							} else {
								blocksSinceGold++;
								preFabHere = insidePrefab;	
							}

							rotBy = Quaternion.AngleAxis(
								Random.Range(0,4)*90.0f,Vector3.up) * rotBy;
						}
					}

					GameObject clonedGO = (GameObject)GameObject.Instantiate(preFabHere,
						transform.position +
						w * transform.localScale.x * 1.01f * transform.right +
						l * transform.localScale.z * 1.01f * transform.forward +
						t * transform.localScale.y * 1.01f * transform.up,
						transform.rotation * rotBy);
					clonedGO.transform.parent = newParent.transform;
					if (addToGoldList) {
						GoldGoalTracker.AddTargetGoldTalley(clonedGO.gameObject);
						addToGoldList = false;
					} else	if(Random.Range(0, 255) < 2) {
						clonedGO.layer = LayerMask.NameToLayer("Explosive");
					}
					TowerClone wasTC = clonedGO.GetComponent<TowerClone>();
					if(wasTC) {
						wasTC.enabled = false;
						Destroy(wasTC);
					}
				} // end of for-t
			} // end of for-l
		} // end of for-w

		Destroy(gameObject); // otherwise will overlap generated one at 0,0,0

	} // end of start

}
