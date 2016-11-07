// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	// GameObjects
	public MouseManager mouseManager;
	public Map map;
	public Button endTurnButton;

	// Prefabs
	public GameObject shipPrefab;
	public GameObject mapPrefab;
	public GameObject hexPrefab;
	public GameObject landPrefab;
	public GameObject seaPrefab;
    public GameObject harborPrefab;
    public GameObject coastPrefab;

	// Attributes
	Player currentPlayer;
	int currentPlayerNumber;
	List<Player> players;
	int turnNumber;
	System.Random rand;


	// Use this for initialization

	void Start () {
		rand = new System.Random();
		turnNumber = 1;
		players = new List<Player>();
		players.Add (new Player ("Humain", Color.red, "Player1"));
		players.Add (new Player ("IA", Color.blue, "Player2"));
		currentPlayerNumber = 0;
		currentPlayer = players [currentPlayerNumber];
		endTurnButton.onClick.AddListener(() => EndTurn());
		AddSomeTestShip ();
		if (currentPlayer.Fleet != null && currentPlayer.Fleet.Count > 0) {
			foreach (Ship ship in currentPlayer.Fleet) {
				ship.Playable = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Player player in players){
			if ( (player.Fleet == null || player.Fleet.Count == 0) && (player.Harbors == null || player.Harbors.Count == 0)) {
				GameOver ();
			}
		}
	}

	void GameOver(){

	}

	void EndTurn(){
		if (currentPlayer.Fleet != null && currentPlayer.Fleet.Count > 0) {
			foreach (Ship ship in currentPlayer.Fleet) {
				ship.Playable = false;
			}
		}
		currentPlayerNumber = (currentPlayerNumber + 1) % players.Count;
		if (currentPlayerNumber == 0)
			turnNumber++;
		currentPlayer = players [currentPlayerNumber];
		Debug.Log("Its turn of player : "+currentPlayer.Name);
		Debug.Log("Turn number : "+turnNumber);
		if (currentPlayer.Fleet != null && currentPlayer.Fleet.Count > 0) {
			foreach (Ship ship in currentPlayer.Fleet) {
				ship.Playable = true;
			}
		}
	}

	void AddSomeTestShip(){
		foreach (Player player in players) {
			int count = 1;
			// We create 3 ship for each player
			while (count <= 3) {
				int x = rand.Next (1, mouseManager.map.width);
				int y = rand.Next (1, mouseManager.map.height);
				if (mouseManager.map.graph [x, y].type == "sea" && mouseManager.map.graph [x, y].tag) {
					GameObject ship_go = (GameObject)Instantiate (shipPrefab, mouseManager.map.graph [x, y].worldPos, Quaternion.identity);
					ship_go.name = "Ship_" + x + "_" + y;
					ship_go.GetComponent<Ship> ().ShipX = x;
					ship_go.GetComponent<Ship> ().ShipY = y;
					ship_go.GetComponent<Ship> ().ShipName = player.Name + "_Ship_" + count.ToString ();
					ship_go.GetComponentInChildren<MeshRenderer> ().material.color = player.Color;
					Ship ship = ship_go.GetComponent<Ship> ();
					player.Fleet.Add (ship);
					mouseManager.map.graph [x, y].isWalkable = false;
					GameObject.Find("Hex_" + x + "_" + y).GetComponent<Sea>().ShipContained = ship;
					count++;
				}
			}
		}
	}
}
