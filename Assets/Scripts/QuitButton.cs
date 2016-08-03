using UnityEngine;
using System.Collections;

public class QuitButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(Application.loadedLevel != 0) {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;

				Application.LoadLevel(0);
			} else {
				Application.Quit();
			}
		}
	}
}
