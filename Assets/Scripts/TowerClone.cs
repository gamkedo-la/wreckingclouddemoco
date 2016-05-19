using UnityEngine;
using System.Collections;

public class TowerClone : MonoBehaviour {
	int cubesWide;
	int cubesLong;
	int cubesTall;

	void Start() {
		cubesWide = 3+Random.Range(0,2);
		cubesLong = 4+Random.Range(0,2);
		cubesTall = 5+Random.Range(0,5);
	}

	// Use this for initialization
	void LateUpdate () {
		GameObject newParent = new GameObject(name);
		for(int w = 0; w < cubesWide; w++) {
			for(int l = 0; l < cubesLong; l++) {
				for(int t = 0; t < cubesTall; t++) {
					GameObject clonedGO = (GameObject)GameObject.Instantiate(gameObject,
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
					wasTC.enabled = false;
					Destroy(wasTC);
				} // end of for-t
			} // end of for-l
		} // end of for-w

		Destroy(gameObject); // otherwise will overlap generated one at 0,0,0

	} // end of start

}
