using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour {

	public void OpenStart(int thisScene) {
		SceneManager.LoadScene(thisScene);
	}
}
