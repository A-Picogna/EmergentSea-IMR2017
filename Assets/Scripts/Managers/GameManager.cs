// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System.Threading;
using UnityEngine.SceneManagement;

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
	private bool gameover = false;

	// Attributes
	public Player currentPlayer;
	public List<Player> players;
	int currentPlayerNumber;
	int turnNumber;
	System.Random rand;
    string lastSelected = "";
	bool aiTurn;
	private Lang lang;
	private int returnInteractionCode;
	private bool isTutorial = false;

    //AI
    AiScript AI;
    public bool aiIsPlaying;
    public int waiting;

	// Public attibutes
	public int FleetSize;
	public int GoldAmount = 0;
	public float retributionStrength;

	//GameFile
	public bool loadingMode = false;
	public GameFile game;

	// Use this for initialization

	void Start () {
		if (SceneManager.GetActiveScene().name.Equals("map_tutorial")){
			isTutorial = true;
			LaunchTutorial();
		} else {
			isTutorial = false;
			if (!loadingMode) {
				initGameManager ();
			} else {
				loadGameManager (game);
			}
		}
		gameover = false;
	}

	public void initGameManager() {
        waiting = 0;
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		aiTurn = false;
		aiIsPlaying = false;
		AI = new AiScript();
		rand = new System.Random();
		turnNumber = 1;

		players = new List<Player>();
		this.AddPlayer ("Player1", Color.red, true);
		this.AddPlayer ("Player2", Color.blue, true);
		currentPlayerNumber = 0;
		currentPlayer = players [currentPlayerNumber];

		endTurnButton.onClick.AddListener(() => NextPlayer());
		textEndTurnNumber.text = "Tour n°" + turnNumber.ToString();

		if (map.loadedMap != null && map.loadedMap.boatPreset == false) {
			AddShips (FleetSize);
		} else {
			loadShip (map.loadedMap);
		}

		foreach(Player player in players){
			foreach (Ship ship in player.Fleet) {
				ship.UpdateShipHp ();
			}
		}

		if (currentPlayer.Fleet != null && currentPlayer.Fleet.Count > 0) {
			foreach (Ship ship in currentPlayer.Fleet) {
				ship.Playable = true;
			}
		}
		checkInit = false;
		// We init the fow
		ResetFOW ();
		RevealAreaAroundCurrentPlayerShips ();
		CenterCameraOnFirstShip ();
		WelcomeMessage ();
	}

	public void loadGameManager(GameFile game) {
        waiting = 0;
		this.lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		this.aiTurn = false;
		this.aiIsPlaying = false;
		this.AI = new AiScript();
		this.rand = new System.Random();
		this.turnNumber = game.turnNumber;

		this.players = new List<Player> ();
		foreach(PlayerStruct playerSaved in game.players) {
			List<Node> exploredHexBuffer = new List<Node>();
			foreach (NodeStruct n in playerSaved.exploredHex) {
				exploredHexBuffer.Add(map.graph [n.x, n.y]);
			}

			Player p = new Player (playerSaved, exploredHexBuffer);
			this.players.Add(p);

			foreach(HarborStruct h in playerSaved.harbors) {
				GameObject go = GameObject.Find ("Hex_" + h.x + "_" + h.y);
				Harbor hh = go.GetComponent<Harbor>();
				hh.Load (h, p);
			}
		}

		this.currentPlayerNumber = game.currentPlayerNumber;
		this.currentPlayer = players [currentPlayerNumber];

		endTurnButton.onClick.AddListener(() => NextPlayer());
		textEndTurnNumber.text = "Tour n°" + turnNumber.ToString();

		//AddShips (FleetSize);
		loadShip(game);

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
		//checkInit = false;
		ResetFOW ();
		RevealAreaAlreadyExplored ();
		RevealAreaAroundCurrentPlayerShips ();
		CenterCameraOnFirstShip ();
	}

	public GameFile saveGameManager() {
		GameFile gameFile = new GameFile();

		gameFile.turnNumber = this.turnNumber;
		gameFile.currentPlayerNumber = this.currentPlayerNumber;


		gameFile.players = new List<PlayerStruct> ();

		foreach (Player p in this.players) {
			PlayerStruct ps = new PlayerStruct (p);
			foreach (Ship s in p.Fleet) {
				ps.fleet.Add (s.SaveShip ());
			}
			foreach (Harbor h in p.Harbors) {
				ps.harbors.Add (h.Save());
			}
			gameFile.players.Add (ps);
		}

		return gameFile;
	}

	// Update is called once per frame
	void Update () {
		List<Player> playersCopy = players;
		List<Ship> currentPlayerFleet = currentPlayer.Fleet;
		foreach(Player player in playersCopy){
			/*
			if ( (player.Fleet == null || player.Fleet.Count == 0) && (player.Harbors == null || player.Harbors.Count == 0) && !gameover) {
				GameOver (player);
			}
			*/
			if ( (player.Fleet == null || player.Fleet.Count == 0) && !gameover) {
				GameOver (player);
			}
			CheckShipsToDestroy (player);
		}
		if (!checkInit) {
			panelHandler.removeAllShip ();
			if (!aiTurn) {
				foreach (Ship ship in currentPlayerFleet) {
					panelHandler.addShip (ship);
				}
				panelHandler.refreshListShipDisplay ();
				checkInit = true;
			} else {
				panelHandler.refreshListShipDisplay ();
				checkInit = true;
			}
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
			returnInteractionCode = mouseManager.currentHarbor.doAction(mouseManager.selectedUnit, map, panelHandler.panelHarbor.GetComponent<HarborPanel>().buttonClicked);
			switch (returnInteractionCode) {
			case 1:
				mouseManager.ip.DisplayInfo (lang.getString("notEnoughGold"), 6f);
				break;
			case 2:
				mouseManager.ip.DisplayInfo (lang.getString("shipIsFull"), 6f);
				break;
			case 3:
				mouseManager.ip.DisplayInfo (lang.getString("noPlaceToBuildAShip"), 6f);
				break;
			case 4:
				mouseManager.ip.DisplayInfo (lang.getString("alreadyBuildingShip"), 6f);
				break;
			case 5:
				mouseManager.ip.DisplayInfo (lang.getString("notEnoughFood"), 6f);
				break;
			case 6:
				mouseManager.ip.DisplayInfo (lang.getString("noHealNeeded"), 6f);
				break;
			}

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
        if(AI.end)
        {
            AI.end = false;
            waiting = 100;
        }

        if (waiting > 0)
        {
            Debug.Log("Waiting");
            waiting--;
        }
        else
        {
            if (aiTurn)
            {
                aiTurn = false;
                NextPlayer();
            }
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
		ship.name = ship.name + rand.Next (1000000000);
		ship.Die ();
	}

	void GameOver(Player player){
		if (!isTutorial) {
			gameover = true;
			if (!player.IsHuman) {
				GameObject.Find ("txt_gameoverLabel").GetComponent<Text> ().text = lang.getString ("gameover_winnerLabel");
				GameObject.Find ("txt_gameover").GetComponent<Text> ().text = lang.getString ("gameover_winner");
			} else {
				GameObject.Find ("txt_gameoverLabel").GetComponent<Text> ().text = lang.getString ("gameover_looserLabel");
				GameObject.Find ("txt_gameover").GetComponent<Text> ().text = lang.getString ("gameover_looser");
			}
			GameObject.Find ("GameoverCanvas").GetComponent<GameoverManager> ().Pause ();
		}
	}

	public void NextTurn(){
		turnNumber++;
		textEndTurnNumber.text = "Tour n°" + turnNumber.ToString();
	}

	void NextPlayer()
	{
        if (!aiIsPlaying)
        {
            ResetFOW();
            RevealAreaAlreadyExplored();
            RevealAreaAroundCurrentPlayerShips();

            AI.MovingShip = null;
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
                    harbor.RemainingBuildingTime--;
                    //Debug.Log(harbor.RemainingBuildingTime);
                }
            }
        }
        if (currentPlayer.Fleet != null && currentPlayer.Fleet.Count > 0) {
			if(currentPlayer.IsHuman)
            {
                // We reset fow for next player
                ResetFOW ();
                RevealAreaAlreadyExplored ();
                RevealAreaAroundCurrentPlayerShips();
				CenterCameraOnFirstShip ();
                //Debug.Log("Human turn");
				if (turnNumber == 1) {
					WelcomeMessage ();
				}
                foreach (Ship ship in currentPlayer.Fleet)
                {
                    ship.Playable = true;
                    ship.RefuelEnergy();
				}
            }
            else
            {
                //Debug.Log("AI playing...");
                if (!aiIsPlaying)
                {
                    foreach (Ship ship in currentPlayer.Fleet)
                    {
                        ship.Used = false;
                        ship.TargetDistance = -1;
                        ship.RefuelEnergy();
                    }
                    RevealAreaAroundCurrentPlayerShips();
                }
                if(AI.MovingShip != null && AI.MovingShip.TargetDistance != -1)
                {
                    //Calculate new path
                    Debug.Log("Change path");
                    AI.goToTarget(AI.MovingShip, map);
                    aiIsPlaying = true;
                }
                else
                {
                    aiIsPlaying = AI.turn(currentPlayer, map);
                }
                aiTurn = true;
            }
		}
		checkInit = false;
	}

	void AddShips(int n){
		int playerCount = 0;
		foreach (Player player in players) {
			int count = 0;
			float minDistance = Mathf.Max(map.width/2, map.height/2);
			float maxDistance = Mathf.Max(map.width/5, map.height/5);
			while (count < n) {
				int x = rand.Next (1, mouseManager.map.width);
				int y = rand.Next (1, mouseManager.map.height);
				if (mouseManager.map.graph [x, y].type == "sea" && mouseManager.map.graph [x, y].tag && mouseManager.map.graph [x, y].isWalkable) {
					if (count == 0) {
						if (playerCount == 0) {
							createShip (player, x, y);
							count++;
						} else {
							bool farEnough = true;
							for (int i = 0; i < playerCount; i++) {								
								if (Vector3.Distance (mouseManager.map.graph [x, y].worldPos, mouseManager.map.graph [players[i].Fleet [0].ShipX, players[i].Fleet [0].ShipY].worldPos) < minDistance) {
									farEnough = false;
								}
							}
							if (farEnough) {
								createShip (player, x, y);
								count++;
							}
						}
					} else {
						if (Vector3.Distance (mouseManager.map.graph [x, y].worldPos, mouseManager.map.graph [player.Fleet [0].ShipX, player.Fleet [0].ShipY].worldPos) < maxDistance) {
							createShip (player, x, y);
							count++;
						}
					}
				}
			}
			playerCount++;
		}
	}

	public Ship createShip(Player player, int x, int y, string goName = "", string name = ""){
		if (goName.Equals("")) goName = "Ship_" + player.Name + "_" + player.NbTotalShip;
		if (name.Equals("")) name = player.Name + "_Ship_" + player.NbTotalShip;
		GameObject ship_go = (GameObject)Instantiate (shipPrefab, mouseManager.map.graph [x, y].worldPos, Quaternion.identity);
		ship_go.name = goName;
		ship_go.GetComponent<Ship> ().ShipX = x;
		ship_go.GetComponent<Ship> ().ShipY = y;
		ship_go.GetComponent<Ship> ().ShipName = name;
		ship_go.GetComponent<Ship> ().Gold = GoldAmount;
		ship_go.GetComponentInChildren<MeshRenderer> ().material.color = player.Color;
		Ship ship = ship_go.GetComponent<Ship> ();
		ship.Owner = player;
		ship.addCrewMember(new Filibuster());
		ship.addCrewMember(new PowderMonkey());
		ship.addCrewMember(new Conjurer());
		ship.setRandomName ();
		player.Fleet.Add (ship);
		mouseManager.map.graph [x, y].isWalkable = false;
		GameObject.Find("Hex_" + x + "_" + y).GetComponent<Sea>().ShipContained = ship;
		player.NbTotalShip++;
		return ship;
	}

	public void loadShip(GameFile game) {
		foreach (PlayerStruct p in game.players) {
			Player pp = this.players.Find (x => x.Name.Equals(p.name));
			foreach (ShipStruct s in p.fleet) {
				GameObject ship_go = (GameObject)Instantiate (shipPrefab, mouseManager.map.graph [s.shipX, s.shipY].worldPos, Quaternion.identity);
				ship_go.name = "Ship_" + p.name + "_" + pp.NbTotalShip;
				Ship ss = (ship_go.GetComponent<Ship> ());
				ss.LoadShip (s);
				ship_go.GetComponentInChildren<MeshRenderer> ().material.color = p.color;
				ss.Owner = pp;
				pp.Fleet.Add (ss);
				mouseManager.map.graph [s.shipX, s.shipY].isWalkable = false;
				GameObject.Find("Hex_" + s.shipX + "_" + s.shipY).GetComponent<Sea>().ShipContained = ss;
				pp.NbTotalShip++;
			}
		}
	}

	public void loadShip(MapFile mapSaved) {
		foreach (PlayerStruct p in mapSaved.playerPreset) {
			Player pp = this.players.Find (x => x.Name.Equals(p.name));
			foreach (ShipStruct s in p.fleet) {
				GameObject ship_go = (GameObject)Instantiate (shipPrefab, mouseManager.map.graph [s.shipX, s.shipY].worldPos, Quaternion.identity);
				ship_go.name = "Ship_" + p.name + "_" + pp.NbTotalShip;
				Ship ss = (ship_go.GetComponent<Ship> ());
				ss.LoadShip (s);
				ship_go.GetComponentInChildren<MeshRenderer> ().material.color = p.color;
				ss.Owner = pp;
				ss.Gold = GoldAmount;
				pp.Fleet.Add (ss);
				mouseManager.map.graph [s.shipX, s.shipY].isWalkable = false;
				GameObject.Find("Hex_" + s.shipX + "_" + s.shipY).GetComponent<Sea>().ShipContained = ss;
				pp.NbTotalShip++;
			}
		}
	}

	public void ResetFOW(){
		if (currentPlayer.IsHuman)
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
        //Debug.Log("Revealing");
		Player cp = currentPlayer;
		foreach (Ship ship in cp.Fleet) {
			GameObject currentHex = (GameObject) GameObject.Find ("Hex_" + ship.ShipX + "_" + ship.ShipY);
			List<GameObject> neighbours;
			MeshRenderer[] meshRenderers;
			Node newNode;
			// Reveal Ship and ship Hex
			if (currentPlayer.IsHuman){
				ship.DisplayHp(true);
			}
			neighbours = currentHex.GetComponent<Hex> ().getNLevelOfNeighbours (0, 3);
			foreach (GameObject n in neighbours) {
				if (currentPlayer.IsHuman){
					n.GetComponent<Hex>().setVisibility(2);
				}
				newNode = new Node(n.GetComponent<Hex>().x, n.GetComponent<Hex>().y, new Vector3(0,0,0), false, "map");
                //Solution à verifier
                float distance = Mathf.Abs(Vector3.Distance(currentHex.transform.position, n.transform.position));
                ship.getTarget(n,(int)Mathf.Floor(distance));
				ship.DisplayTargetHp (n);

				if (!currentPlayer.ExploredHex.Exists (e => e.x == newNode.x && e.y == newNode.y)) {
					currentPlayer.ExploredHex.Add(newNode);
				}
			}
			newNode = new Node(ship.ShipX, ship.ShipY, new Vector3(0,0,0), false, "ship");

            if (!currentPlayer.ExploredHex.Exists (e => e.x == newNode.x && e.y == newNode.y)) {
				currentPlayer.ExploredHex.Add(newNode);
			}
		}
	}

	public void RevealAreaAlreadyExplored(){
		if (currentPlayer.IsHuman)
        {
            Player cp = currentPlayer;
            GameObject currentHex;
            MeshRenderer[] meshRenderers;
            foreach (Node n in currentPlayer.ExploredHex)
            {
				GameObject.Find("Hex_" + n.x + "_" + n.y).GetComponent<Hex>().setVisibility(1);
            }
        }
	}

	private void WelcomeMessage(){
		Color32 color = currentPlayer.Color;
		string hexaCodeColor = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		//Debug.Log (hexaCodeColor);
		InfoBox ib = GameObject.Find ("InfoBoxCanvas").GetComponent<InfoBox> ();
		ib.DisplayText(lang.getString ("infoMessage_welcome_joueur") + 
			"<color=#"+ hexaCodeColor + "ff>" + 
			lang.getString ("color_"+hexaCodeColor) + 
			"</color>" + 
			lang.getString ("infoMessage_welcome_objectif1")
			);
	}

	public void CenterCameraOnFirstShip(){
		if (currentPlayer.Fleet.Count > 0) {
			Ship ship = currentPlayer.Fleet [0];
			GameObject cam = GameObject.Find ("Main Camera");
			cam.transform.position = new Vector3 (
				mouseManager.map.graph [ship.ShipX, ship.ShipY].worldPos.x, 
				cam.transform.position.y,
				mouseManager.map.graph [ship.ShipX, ship.ShipY].worldPos.z - 10
			);		
		}
	}

	public Player AddPlayer(string name, Color color, bool type){
		Player newPlayer = new Player (type, color, name);
		players.Add (newPlayer);
		return newPlayer;
	}

	public Player GetPlayerByName (string name){
		Player result = null;
		for (int i = 0; i < players.Count; i++) {
			if (players [i].name.Equals (name)) {
				result = players [i];
			}
		}
		//Debug.Log (result);
		return result;
	}

	public int GetPlayerIndexByName (string name){
		int result = -1;
		for (int i = 0; i < players.Count; i++) {
			if (players [i].name.Equals (name)) {
				result = i;
			}
		}
		return result;
	}

	public void LaunchTutorial(){		
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		GameObject.Find("TutorialManager").GetComponent<TutorialManager>().InitTutorial();

		waiting = 0;
		currentPlayerNumber = 0;
		turnNumber = 1;

		aiTurn = false;
		aiIsPlaying = false;

		AI = new AiScript();
		rand = new System.Random();
		players = new List<Player>();

		// we add ships for the scenario
		AddPlayer ("Player", Color.red, true);
		AddPlayer ("Computer", Color.blue, false);
		currentPlayer = players [currentPlayerNumber];

		endTurnButton.onClick.AddListener( () => GameObject.Find("TutorialManager").GetComponent<TutorialManager>().NextTurn() );
		textEndTurnNumber.text = "Tour n°" + turnNumber.ToString();

		createShip (players [0], 1, 1, "PlayerShip_1", "La Perle rouge");
		createShip (players [1], 14, 7, "EnemyShip_1", "Le Bluebeard");
		createShip (players [1], 15, 5, "EnemyShip_2", "Le Narada");
		createShip (players [1], 15, 3, "EnemyShip_3", "Le Titanic");

		foreach(Player player in players){
			foreach (Ship ship in player.Fleet) {
				ship.UpdateShipHp ();
			}
		}

		if (currentPlayer.Fleet != null && currentPlayer.Fleet.Count > 0) {
			foreach (Ship ship in currentPlayer.Fleet) {
				ship.Playable = true;
			}
		}

		checkInit = false;
		ResetFOW ();
		RevealAreaAroundCurrentPlayerShips ();
		CenterCameraOnFirstShip ();
		Color32 color = currentPlayer.Color;
		string hexaCodeColor = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		//Debug.Log (hexaCodeColor);
	}

}
