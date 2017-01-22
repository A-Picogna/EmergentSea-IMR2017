using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class InfoBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisplayText(string text){
		this.GetComponent<Canvas> ().enabled = true;
		this.transform.FindChild("pnl_infoBox01").transform.FindChild ("Text").GetComponent<Text> ().text = text;
	}

	public void CloseBox(){
		this.transform.FindChild("pnl_infoBox01").transform.FindChild ("Text").GetComponent<Text> ().text = "";
		this.GetComponent<Canvas> ().enabled = false;
	}
}
