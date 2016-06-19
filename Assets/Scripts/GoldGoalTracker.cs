using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldGoalTracker : MonoBehaviour {
	public static int numberTotal = 0;
	public static int numberPlayerGot = 0;
	public static int numberEnemiesGot = 0;

	public static Text scoreToShow;

	// Use this for initialization
	void Start () {
		numberTotal = 0;
		numberPlayerGot = 0;
		numberEnemiesGot = 0;

		scoreToShow = gameObject.GetComponent<Text>();
		UpdateScoreText();
	}

	public static void AddPlayerGold() {
		numberPlayerGot++;
		UpdateScoreText();
	}

	public static void AddTargetGoldTalley() {
		numberTotal++;
		UpdateScoreText();
	}

	static void UpdateScoreText() {
		if(scoreToShow) {
			scoreToShow.text = ""+numberPlayerGot+" of "+numberTotal+" targets";
		}
	}
}
