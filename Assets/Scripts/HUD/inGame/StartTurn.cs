using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartTurn : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Hide(){
		this.gameObject.GetComponent<Text> ().text = "";
		this.gameObject.SetActive (false);
	}

	public void Show(int turnNumber, Player player){
		string txt = "Tour du joueur " + player.Color.ToString () +"\n"+"Appuyez sur GO pour jouer votre tour";
		this.gameObject.GetComponent<Text> ().text = txt;
		this.gameObject.SetActive (true);
	}

}
