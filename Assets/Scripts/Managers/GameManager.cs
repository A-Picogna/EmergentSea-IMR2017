// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {

	// GameObjects
	public MouseManager mouseManager;
	public Map map;
	public Button endTurnButton;
	public Text textEndTurnNumber;

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
    bool aiTurn;

    //AI
    AiScript AI;
    bool aiIsPlaying;


	// Use this for initialization

	void Start () {
        aiTurn = false;
        aiIsPlaying = false;
        AI = new AiScript();
		rand = new System.Random();
		turnNumber = 1;
		players = new List<Player>();
		players.Add (new Player ("Humain", Color.red, "Player1"));
		players.Add (new Player ("IA", Color.blue, "Player2"));
		currentPlayerNumber = 0;
		currentPlayer = players [currentPlayerNumber];
		endTurnButton.onClick.AddListener(() => NextPlayer());
		textEndTurnNumber.text = "Tour n°" + turnNumber.ToString();
		AddShips (5);
		foreach(Player player in players){
			foreach (Ship ship in currentPlayer.Fleet) {
				ship.UpdateShipHp ();
			}
		}
		if (currentPlayer.Fleet != null && currentPlayer.Fleet.Count > 0) {
			foreach (Ship ship in currentPlayer.Fleet) {
				ship.Playable = true;
			}
		}
		// We init the fow
		ResetFOW ();
		RevealAreaAroundCurrentPlayerShips ();
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
        if(panelHandler.panelHarbor.GetComponent<HarborPanel>().selected)
        {
            panelHandler.panelHarbor.GetComponent<HarborPanel>().selected = false;
            mouseManager.currentHarbor.doAction(mouseManager.selectedUnit, map, panelHandler.panelHarbor.GetComponent<HarborPanel>().buttonClicked);
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

        if (aiTurn)
        {
            aiTurn = false;
            NextPlayer();
        }
    }

	void CheckShipsToDestroy(Player player){
		Ship shipToDestroy = null;
		foreach (Ship ship in player.Fleet) {
			if (ship.Hp < 0) {
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
	}

	void GameOver(){

	}

	void NextTurn(){
		turnNumber++;
		textEndTurnNumber.text = "Tour n°" + turnNumber.ToString();
	}

	void NextPlayer(){
        if (!aiIsPlaying)
        {
            mouseManager.selectedUnit = null;
            if (currentPlayer.Fleet != null && currentPlayer.Fleet.Count > 0)
            {
                foreach (Ship ship in currentPlayer.Fleet)
                {
                    ship.Playable = false;
                }
            }
            currentPlayerNumber = (currentPlayerNumber + 1) % players.Count;
            if (currentPlayerNumber == 0)
            {
                NextTurn();
            }
            currentPlayer = players[currentPlayerNumber];
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
        }
        if (currentPlayer.Fleet != null && currentPlayer.Fleet.Count > 0) {
            if(currentPlayer.Type == "Humain")
            {
                Debug.Log("Human turn");
                foreach (Ship ship in currentPlayer.Fleet)
                {
                    ship.Playable = true;
                    ship.RefuelEnergy();
                }
            }
            else
            {
                Debug.Log("AI playing...");
                if (!aiIsPlaying)
                {
                    foreach (Ship ship in currentPlayer.Fleet)
                    {
                        ship.Used = false;
                        ship.RefuelEnergy();
                    }
                }
                aiIsPlaying = AI.turn(currentPlayer, map);
                aiTurn = true;
            }
		}
		checkInit = false;
		// We reset fow for next player
		ResetFOW ();
		RevealAreaAlreadyExplored ();
		RevealAreaAroundCurrentPlayerShips ();
	}

	void AddShips(int n){
		foreach (Player player in players) {
			int count = 1;
			while (count <= n) {
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
					ship.addCrewMember(new Filibuster());
					player.Fleet.Add (ship);
					mouseManager.map.graph [x, y].isWalkable = false;
					GameObject.Find("Hex_" + x + "_" + y).GetComponent<Sea>().ShipContained = ship;
                    player.NbTotalShip++;
					count++;
				}
			}
		}
	}

	public void ResetFOW(){
        if (!aiIsPlaying)
        {
            map = mouseManager.map;
            GameObject currentHex;
            for (int x = 0; x < map.width; x++)
            {
                for (int y = 0; y < map.height; y++)
                {
                    currentHex = (GameObject)GameObject.Find("Hex_" + x + "_" + y);
                    currentHex.GetComponent<Hex>().setVisibility(0);
                }
            }
        }
	}

	public void RevealAreaAroundCurrentPlayerShips(){
		Player cp = currentPlayer;
		foreach (Ship ship in cp.Fleet) {
			GameObject currentHex = (GameObject) GameObject.Find ("Hex_" + ship.ShipX + "_" + ship.ShipY);
			List<GameObject> firstNeighboursToReveal;
			List<GameObject> secondNeighboursToReveal;
			List<GameObject> thirdNeighboursToReveal;
			MeshRenderer[] meshRenderers;
			Node newNode;

            // Reveal Ship and ship Hex
            if (!aiIsPlaying)
            {
                currentHex.GetComponent<Hex>().setVisibility(2);
            }
			newNode = new Node(ship.ShipX, ship.ShipY, new Vector3(0,0,0), false, "ship");
			if (!currentPlayer.ExploredHex.Exists (e => e.x == newNode.x && e.y == newNode.y)) {
				currentPlayer.ExploredHex.Add(newNode);
			}

			// Reveal 1st Neighbours
			firstNeighboursToReveal = currentHex.GetComponent<Hex>().getNeighbours();
			foreach (GameObject n1 in firstNeighboursToReveal) {
                if (!aiIsPlaying)
                {
                    n1.GetComponent<Hex>().setVisibility(2);
                }
				newNode = new Node(n1.GetComponent<Hex>().x, n1.GetComponent<Hex>().y, new Vector3(0,0,0), false, "map");
				if (!currentPlayer.ExploredHex.Exists (e => e.x == newNode.x && e.y == newNode.y)) {
					currentPlayer.ExploredHex.Add(newNode);
				}
				// Reveal 2nd Neighbours
				secondNeighboursToReveal = n1.GetComponent<Hex> ().getNeighbours ();
				foreach (GameObject n2 in secondNeighboursToReveal) {
                    if (!aiIsPlaying)
                    {
                        n2.GetComponent<Hex>().setVisibility(2);
                    }
					newNode = new Node(n2.GetComponent<Hex>().x, n2.GetComponent<Hex>().y, new Vector3(0,0,0), false, "map");
					if (!currentPlayer.ExploredHex.Exists (e => e.x == newNode.x && e.y == newNode.y)) {
						currentPlayer.ExploredHex.Add(newNode);
					}
					// Reveal 3rd Neighbours
					thirdNeighboursToReveal = n2.GetComponent<Hex> ().getNeighbours ();
					foreach (GameObject n3 in thirdNeighboursToReveal) {
                        if (!aiIsPlaying)
                        {
                            n3.GetComponent<Hex>().setVisibility(2);
                        }
						newNode = new Node(n3.GetComponent<Hex>().x, n3.GetComponent<Hex>().y, new Vector3(0,0,0), false, "map");
						if (!currentPlayer.ExploredHex.Exists (e => e.x == newNode.x && e.y == newNode.y)) {
							currentPlayer.ExploredHex.Add(newNode);
						}
					}
				}
			}
		}
	}

	public void RevealAreaAlreadyExplored(){
        if (!aiIsPlaying)
        {
            Player cp = currentPlayer;
            GameObject currentHex;
            MeshRenderer[] meshRenderers;
            foreach (Node n in currentPlayer.ExploredHex)
            {
                currentHex = (GameObject)GameObject.Find("Hex_" + n.x + "_" + n.y);
                currentHex.GetComponent<Hex>().setVisibility(1);
            }
        }
	}

}
