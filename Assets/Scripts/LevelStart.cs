using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;

public class LevelStart : MonoBehaviour {

	public void OpenStart() {
		SceneManager.LoadScene(1);
	}
}
