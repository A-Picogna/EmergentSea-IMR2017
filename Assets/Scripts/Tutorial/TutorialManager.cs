// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System.Threading;

public class TutorialManager : MonoBehaviour {

	// GameObjects
	public TutorialMouseManager mouseManager;
	public Map map;
	public Button endTurnButton;
	public Text textEndTurnNumber;

	// Prefabs
	public GameObject shipFloatingPanel;
	public GameObject shipPrefab;

	// UI
	public PanelHandler panelHandler;
	private bool checkInit = false;
	private bool gameover = false;

	// Attributes
	public Player currentPlayer;
	List<Player> players;
	int currentPlayerNumber;
	int turnNumber;
	string lastSelected = "";
	private Lang lang;
	private int returnInteractionCode;

	//AI
	AiScript AI;

	// Public attibutes
	public int FleetSize;
	public int GoldAmount;
	public float retributionStrength;

	//GameFile
	public bool loadingMode = false;
	public GameFile game;

	// Use this for initialization

	void Start () {
		initGameManager ();
		gameover = false;
	}

	public void initGameManager() {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		AI = new AiScript();
		turnNumber = 1;
		players = new List<Player>();

		map.graph = new Node[map.width, map.height];
		// the map is fixe, so we need to update the graph in consequence
		foreach (Transform child in map.gameObject.transform) {
			int x = child.GetComponent<Hex> ().x;
			int y = child.GetComponent<Hex> ().y;
			map.graph[x, y] = new Node (x, y, child.position, false,  child.GetComponent<Hex> ().Type);
		}

		// we add ships for the scenario
		this.AddPlayer ("Player", Color.red, true);
		this.AddPlayer ("Computer", Color.blue, false);
		this.createShip (players [0], 0, 0, "PlayerShip_1", "La Perle rouge");
		this.createShip (players [1], 14, 7, "EnemyShip_1", "Le Bluebeard");
		this.createShip (players [1], 15, 5, "EnemyShip_2", "Le Narada");
		this.createShip (players [1], 15, 3, "EnemyShip_3", "Le Titanic");


		currentPlayerNumber = 0;
		currentPlayer = players [currentPlayerNumber];
		endTurnButton.onClick.AddListener(() => NextPlayer());
		textEndTurnNumber.text = "Tour n°" + turnNumber.ToString();
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

	// Update is called once per frame
	void Update () {
		List<Player> playersCopy = players;
		List<Ship> currentPlayerFleet = currentPlayer.Fleet;
		foreach(Player player in playersCopy){
			if ( (player.Fleet == null || player.Fleet.Count == 0) && !gameover) {
				//GameOver (player);
			}
			CheckShipsToDestroy (player);
		}
		if (!checkInit) {
			panelHandler.removeAllShip ();
			foreach (Ship ship in currentPlayerFleet) {
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
	}

	public Ship createShip(Player player, int x, int y, string goName, string name){
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
		player.Fleet.Add (ship);
		mouseManager.map.graph [x, y].isWalkable = false;
		GameObject.Find("Hex_" + x + "_" + y).GetComponent<Sea>().ShipContained = ship;
		player.NbTotalShip++;
		return ship;
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
		ship.Die ();
	}

	void GameOver(Player player){
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

	void NextTurn(){
		turnNumber++;
		textEndTurnNumber.text = "Tour n°" + turnNumber.ToString();
	}

	void NextPlayer()
	{
		ResetFOW();
		RevealAreaAlreadyExplored();
		RevealAreaAroundCurrentPlayerShips();

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
					//Debug.Log(harbor.RemainingBuildingTime);
				}
				else
				{
					harbor.Build(map);
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
				NextPlayer();
			}
		}
		checkInit = false;
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
				currentHex = GameObject.Find("Hex_" + n.x + "_" + n.y);
				currentHex.GetComponent<Hex>().setVisibility(1);
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

	private void CenterCameraOnFirstShip(){
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
}
