using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class StepBox : MonoBehaviour {

	private Lang lang;
	int step = 0;

	// Use this for initialization
	void Start () {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		DisplayStep (step);
	}

	// Update is called once per frame
	void Update () {

	}

	public void DisplayStep(int stepNumber){
		string tmp = "tuto_step_" + stepNumber;
		this.transform.FindChild ("Text").GetComponent<Text> ().text = lang.getString(tmp);
	}

	public void Next(){
		step++;
		DisplayStep (step);
	}
}
