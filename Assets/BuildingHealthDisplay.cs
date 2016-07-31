using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingHealthDisplay : MonoBehaviour {
	public PotentialExploder ShinyBall;
	public PotentialExploder BoxyTree;
	public PotentialExploder Resort;

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
		display.text = ShowIfAround("Chrosph", ShinyBall) +"\n"+
			ShowIfAround("Rectree", BoxyTree) +"\n" +
				ShowIfAround("Sailco", Resort);
	}
}
