using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class StartTurn : MonoBehaviour {

	private Lang lang;

	// Use this for initialization
	void Start () {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Hide(){
		this.transform.Find("txt_startTurn").GetComponent<Text> ().text = "";
		this.gameObject.SetActive (false);
	}

	public void Show(int turnNumber, Player player){
		Color32 color = player.Color;
		string hexaCodeColor = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		string txt = "Tour " + turnNumber.ToString() + " - Joueur " +
			"<color=#"+ hexaCodeColor + "ff>" + 
			lang.getString ("color_"+hexaCodeColor) +
			"</color>" +
			"\n"+ 
			"Appuyez sur GO pour jouer votre tour";
		this.transform.Find("txt_startTurn").GetComponent<Text> ().text = txt;
		this.gameObject.SetActive (true);
	}

}
