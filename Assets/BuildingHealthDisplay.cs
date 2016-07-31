using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingHealthDisplay : MonoBehaviour {
	public PotentialExploder ShinyBall;
	public PotentialExploder BoxyTree;
	public PotentialExploder Resort;

	private bool roundSummaryShownYet = false;
	private Text display;

	// Use this for initialization
	void Start () {
		display = GetComponent<Text>();
	}

	string ShowIfAround(string baseName, PotentialExploder thisBldg) {
		if(thisBldg == null) {
			return baseName+" was lost!";
		} else {
			return baseName + ": " + thisBldg.HealthPerc();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(roundSummaryShownYet) {
			return;
		}

		int secSurvived =
			Mathf.RoundToInt(Time.realtimeSinceStartup);
		string timeSurvived="";
		if(secSurvived > 60) {
			int minutes = (int)(secSurvived / 60);
			timeSurvived += ""+minutes + ":";
			secSurvived -= minutes * 60;
		}
		if(secSurvived < 10) {
			timeSurvived += "0";
		}
		timeSurvived += ""+secSurvived;

		display.text = ShowIfAround("Chrosph", ShinyBall) +"\n"+
			ShowIfAround("Rectree", BoxyTree) +"\n" +
			ShowIfAround("Sailco", Resort) +"\n" +
			"Time defended: " + timeSurvived;

		if(ShinyBall.hitPoints <= 0 &&
			BoxyTree.hitPoints <= 0 &&
			Resort.hitPoints <= 0) {

			roundSummaryShownYet = true;
			EndOfRoundMessage.instance.OpenPanel("You defended the buildings for " + timeSurvived);
		}
	}
}
