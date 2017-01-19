using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AddShipPanel : MonoBehaviour {

	int x;
	int y;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void addShip(int x, int y){
		this.x = x;
		this.y = y;
		Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); 
		this.GetComponent<Canvas> ().enabled = true;
	}

	public void validateShip(){
		string shipName = GameObject.Find ("inputTxt_shipName").GetComponent<Text> ().text;
		string shipOwner = GameObject.Find ("input_owner").GetComponent<Dropdown> ().options[GameObject.Find ("input_owner").GetComponent<Dropdown> ().value].text;
		Debug.Log (shipName);
		Debug.Log (shipOwner);
		Player player = GameObject.Find ("GameManager").GetComponent<GameManager> ().GetPlayerByName (shipOwner);
		Debug.Log (player);
		GameObject.Find ("MapEditor").GetComponent<MapEditor> ().createShip (player, x, y, shipName);
		this.GetComponent<Canvas> ().enabled = false;
	}

}

