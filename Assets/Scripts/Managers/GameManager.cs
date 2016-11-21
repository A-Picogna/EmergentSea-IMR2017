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
	public GameObject shipFloatingPanel;

	// UI
	public PanelHandler panelHandler;
	private bool checkInit = false;

	// Attributes
	public Player currentPlayer;
	List<Player> players;
	int currentPlayerNumber;
	int turnNumber;
	System.Random rand;
    string lastSelected = "";


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
		AddShips (10);
		if (currentPlayer.Fleet != null && currentPlayer.Fleet.Count > 0) {
			foreach (Ship ship in currentPlayer.Fleet) {
				ship.Playable = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		List<Player> playersCopy = players;
		List<Ship> currentPlayerFleet = currentPlayer.Fleet;
		foreach(Player player in playersCopy){
			if ( (player.Fleet == null || player.Fleet.Count == 0) && (player.Harbors == null || player.Harbors.Count == 0)) {
				GameOver ();
			}
			CheckShipsToDestroy (player);
		}
		if (!checkInit) {
			panelHandler.removeAllShip ();
			foreach(Ship ship in currentPlayerFleet){
				panelHandler.addShip (ship);
			}
			panelHandler.refreshListShipDisplay ();
			checkInit = true;
		}
        if(mouseManager.harbor == true)
        {
            panelHandler.showPanelHarbor();
            lastSelected = mouseManager.selectedUnit.ShipName;
            mouseManager.harbor = false;
        }
		if (mouseManager.selectedUnit == null) {
			panelHandler.hidePanelUnkown ();
			panelHandler.hidePanelShip ();
            panelHandler.hidePanelHarbor();
        } else if (mouseManager.selectedUnit != null && mouseManager.selectedUnit.GetType() == typeof(Ship) && mouseManager.selectedUnit.Playable) {
			panelHandler.hidePanelUnkown ();
            if(mouseManager.selectedUnit.ShipName != lastSelected)
            {
                panelHandler.hidePanelHarbor();
            }
		} else if (mouseManager.selectedUnit != null && mouseManager.selectedUnit.GetType() == typeof(Ship) && !mouseManager.selectedUnit.Playable) {
			panelHandler.hidePanelShip ();
			panelHandler.initPanelEnnemyShip ();
            panelHandler.hidePanelHarbor();
		}
	}

	void CheckShipsToDestroy(Player player){
		Ship shipToDestroy = null;
		foreach (Ship ship in player.Fleet) {
			if (ship.Hp <= 0) {
				shipToDestroy = ship;
			}
		}
		if (shipToDestroy != null){
			player.Fleet.Remove (shipToDestroy);
			DestroyShip (shipToDestroy);
		}
	}

	void DestroyShip(Ship ship){
		GameObject.Find ("Hex_" + ship.ShipX + "_" + ship.ShipY).GetComponent<Sea> ().RemoveShip ();
		mouseManager.map.graph [ship.ShipX, ship.ShipY].isWalkable = true;
		ship.name = ship.name + rand.Next (1000000000);
		ship.Die ();
		//GameObject.DestroyObject (ship.gameObject);
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
        Debug.Log("Turn number : " + turnNumber);
        foreach (Harbor harbor in currentPlayer.Harbors)
        {
            if (harbor.Building)
            {
                if (harbor.RemainingBuildingTime > 1)
                {
                    harbor.RemainingBuildingTime--;
                    Debug.Log(harbor.RemainingBuildingTime);
                }
                else
                {
                    harbor.Build(map);
                }
            }
        }
        if (currentPlayer.Fleet != null && currentPlayer.Fleet.Count > 0) {
			foreach (Ship ship in currentPlayer.Fleet) {
				ship.Playable = true;
			}
		}
	}

	void AddShips(int n){
		foreach (Player player in players) {
			for (int count = 1; count <= n; count++) {
				int x = rand.Next (1, mouseManager.map.width);
				int y = rand.Next (1, mouseManager.map.height);
				if (mouseManager.map.graph [x, y].type == "sea" && mouseManager.map.graph [x, y].tag && mouseManager.map.graph [x, y].isWalkable) {
					GameObject ship_go = (GameObject)Instantiate (shipPrefab, mouseManager.map.graph [x, y].worldPos, Quaternion.identity);
					ship_go.name = "Ship_" + player.Name + "_" + player.NbTotalShip;
					ship_go.GetComponent<Ship> ().ShipX = x;
					ship_go.GetComponent<Ship> ().ShipY = y;
					ship_go.GetComponent<Ship> ().ShipName = player.Name + "_Ship_" + player.NbTotalShip;
					ship_go.GetComponentInChildren<MeshRenderer> ().material.color = player.Color;
					Ship ship = ship_go.GetComponent<Ship> ();
					ship.Owner = player;
					CrewMember cm = new Conjurer ();
					cm.Lp = 80;
					cm.Lpmax = 100;
					cm.Xp = 20;
					ship.addCrewMember(cm);
					ship.addCrewMember(new Conjurer());
					ship.addCrewMember(new Filibuster());
					ship.addCrewMember(new PowderMonkey());
					if (count > 5) {
						cm = new PowderMonkey ();
						cm.Lp = 75;
						cm.Xp = 40;
						ship.addCrewMember (cm);
					}
					ship.addCrewMember(new PowderMonkey());
					player.Fleet.Add (ship);
					mouseManager.map.graph [x, y].isWalkable = false;
					GameObject.Find("Hex_" + x + "_" + y).GetComponent<Sea>().ShipContained = ship;
                    player.NbTotalShip++;
				}
			}
		}
	}
}
