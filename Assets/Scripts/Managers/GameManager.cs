// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	// GameObjects
	public MouseManager mouseManager;
	public Map map;

	// Prefabs
	public GameObject shipPrefab;
	public GameObject mapPrefab;
	public GameObject hexPrefab;
	public GameObject landPrefab;
	public GameObject seaPrefab;

	// Attributes
	Player currentPlayer;
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
		currentPlayer = players [0];
		AddSomeTestShip ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach(Player player in players){
			if (player.Fleet != null && player.Fleet.Count > 0) {
				foreach (Ship ship in player.Fleet) {
					if (Vector3.Distance (ship.transform.position, ship.destination) < 0.1f){
						mouseManager.map.graph [ship.ShipX, ship.ShipY].isWalkable = false;
					}
				}
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
					mouseManager.map.graph [x, y].isWalkable = false;
					count++;
				}
			}
		}
	}
}
