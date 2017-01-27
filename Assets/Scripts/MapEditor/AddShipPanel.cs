using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

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
		string shipOwner = "Player"+(GameObject.Find ("input_owner").GetComponent<Dropdown> ().value+1);
		Player player = GameObject.Find("MapEditor").GetComponent<MapEditor>().GetPlayerByName (shipOwner);
		GameObject.Find ("MapEditor").GetComponent<MapEditor> ().createShip (player, x, y, shipName);
		this.GetComponent<Canvas> ().enabled = false;
	}
}

