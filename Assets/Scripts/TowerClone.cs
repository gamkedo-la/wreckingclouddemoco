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
	private bool useTileKinds;

	void Start() {
		cubesWide = 3+Random.Range(0,2);
		cubesLong = 4+Random.Range(0,2);
		cubesTall = 5+Random.Range(0,5);

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
								} else {
									preFabHere = topEdgePrefab;
								}
							} else if(isLongSide) {
								preFabHere = topCornerPrefab;
							} else {
								preFabHere = topPrefab;
							}
						} else if(isWideSide) {
							if(isLongSide) {
								preFabHere = sideCornerPrefab;
							} else {
								preFabHere = sidePrefab;
							}
						} else if(isLongSide) {
							preFabHere = sidePrefab;
						} else {
							preFabHere = insidePrefab;
						}
					}

					GameObject clonedGO = (GameObject)GameObject.Instantiate(preFabHere,
						transform.position +
						w * transform.localScale.x * 1.01f * transform.right +
						l * transform.localScale.z * 1.01f * transform.forward +
						t * transform.localScale.y * 1.01f * transform.up,
						transform.rotation);
					clonedGO.transform.parent = newParent.transform;
					if(Random.Range(0, 255) < 2) {
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
