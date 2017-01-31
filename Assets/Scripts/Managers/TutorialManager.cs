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

public class TutorialManager : MonoBehaviour {

	// GameObjects
	public MouseManager mouseManager;
	public GameManager gameManager;
	public PanelHandler panelHandler;
	public StepBox stepInfo;
	public Button nextButton;
	public Button endTurnButton;
	public Projector objProj;
	public Map map;
	public Ship ship;
	public Harbor harbor;

	int step = 0;

	void Start () {
		nextButton.onClick.AddListener( () => NextStep() );	
		mouseManager.mouseManagerEnabled = false;
		endTurnButton.gameObject.SetActive (false);
		harbor = GameObject.Find ("Hex_9_3").GetComponent<Harbor> ();
		GameObject.Find ("btn_return").GetComponent<Button> ().onClick.AddListener( () => SceneManager.LoadScene ("main", LoadSceneMode.Single) );
	}

	// Update is called once per frame
	void Update () {
		switch (step) {
		case 1:
			if (mouseManager.selectedUnit != null) {
				ship = mouseManager.selectedUnit;
				ship.EnergyQuantity = 3;
				panelHandler.updateShipInfo();
				mouseManager.selectedUnit.fishingEnabled = false;
				mouseManager.selectedUnit.attackEnabled = false;
				mouseManager.selectedUnit.moveEnabled = false;
				mouseManager.selectedUnit.tradeEnabled = false;
				mouseManager.selectedUnit.lootEnabled = false;
				NextStep ();
			}
			break;
		case 3:
			if (ship != null && ship.ShipX == 2 && ship.ShipY == 2) {
				map.graph [1, 1].isWalkable = false;
				map.graph [2, 2].isWalkable = false;
				map.graph [2, 3].isWalkable = false;
				map.graph [3, 4].isWalkable = false;
				objProj.transform.position = new Vector3 (0, -5f, 0);
				NextStep ();
			}
			break;
		case 6:
			if (mouseManager.selectedUnit != null && mouseManager.selectedUnit.Gold > 0) {
				map.graph [4, 4].isWalkable = false;
				objProj.transform.position = new Vector3 (0, -5f, 0);
				NextStep ();
			}
			break;
		case 7:
			if (mouseManager.selectedUnit != null && mouseManager.selectedUnit.Food > 0) {
				NextStep ();
			}
			break;
		case 8:
			if (ship != null && ship.ShipX == 7 && ship.ShipY == 4) {
				map.graph [4, 5].isWalkable = false;
				map.graph [5, 5].isWalkable = false;
				map.graph [6, 5].isWalkable = false;
				map.graph [7, 4].isWalkable = false;
				objProj.transform.position = new Vector3 (0, -5f, 0);
				NextStep ();
			}
			break;
		case 9:
			if (harbor.OwnerName != null) {
				map.graph [8, 4].isWalkable = false;
				map.graph [9, 4].isWalkable = false;
				objProj.transform.position = new Vector3 (0, -5f, 0);
				NextStep ();
			}
			break;
		case 10:
			if (ship.IsFullLife()) {
				NextStep ();
			}
			break;
		case 12:
			if (gameManager.players [1].Fleet.Count == 0) {
				NextStep ();
			}
			break;
		case 13:
			break;
		}
	}

	public void InitTutorial(){
		map.graph = new Node[map.width, map.height];
		// the map is fixe, so we need to update the graph in consequence
		foreach (Transform child in map.gameObject.transform) {
			int x = child.GetComponent<Hex> ().x;
			int y = child.GetComponent<Hex> ().y;
			map.graph[x, y] = new Node (x, y, child.position, false,  child.GetComponent<Hex> ().Type);
		}
		map.Size = map.width * map.height;
		map.AddNeighboursToNodes();
		map.generateFood ();
		map.GenerateMapBorder (5);
		GameObject.Find ("Hex_4_5").GetComponent<Sea> ().Treasure_go = GameObject.Find ("Hex_4_5_Treasure");
		GameObject.Find ("Hex_4_5").GetComponent<Sea> ().Treasure = 10000;
	}

	public void NextStep(){
		Debug.Log ("Next Step");
		stepInfo.Next ();
		step++;
		Debug.Log ("Current Step = "+step);
		switch (step) {
		case 1:	
			nextButton.gameObject.SetActive (false);
			mouseManager.mouseManagerEnabled = true;
			break;
		case 2:
			nextButton.gameObject.SetActive (true);
			break;
		case 3:
			nextButton.gameObject.SetActive (false);
			objProj.transform.position = GameObject.Find ("Hex_3_4").transform.position + new Vector3 (0, 5f, 0);
			map.graph [1, 1].isWalkable = true;
			map.graph [2, 2].isWalkable = true;
			map.graph [2, 3].isWalkable = true;
			map.graph [3, 4].isWalkable = true;
			break;
		case 4:
			nextButton.gameObject.SetActive (true);
			break;
		case 5:
			endTurnButton.gameObject.SetActive (true);
			nextButton.gameObject.SetActive (false);
			break;
		case 6:
			objProj.transform.position = GameObject.Find ("Hex_4_4").transform.position + new Vector3 (0, 5f, 0);
			if (gameManager.players[0].Fleet[0] != null) {
				ship.lootEnabled = true;
			}
			map.graph [3, 4].isWalkable = false;
			map.graph [4, 4].isWalkable = true;
			break;
		case 7:
			map.graph [3, 4].isWalkable = false;
			map.graph [4, 4].isWalkable = false;
			map.graph [4, 5].isWalkable = false;
			ship.fishingEnabled = true;
			break;
		case 8:
			objProj.transform.position = GameObject.Find ("Hex_7_4").transform.position + new Vector3 (0, 5f, 0);
			map.graph [4, 5].isWalkable = true;
			map.graph [5, 5].isWalkable = true;
			map.graph [6, 5].isWalkable = true;
			map.graph [7, 4].isWalkable = true;
			break;
		case 9:
			objProj.transform.position = GameObject.Find ("Hex_9_4").transform.position + new Vector3 (0, 5f, 0);
			map.graph [8, 4].isWalkable = true;
			map.graph [9, 4].isWalkable = true;
			break;
		case 10:
			map.graph [8, 4].isWalkable = false;
			map.graph [9, 4].isWalkable = false;
			break;
		case 12:
			mouseManager.selectedUnit.fishingEnabled = true;
			mouseManager.selectedUnit.attackEnabled = true;
			mouseManager.selectedUnit.moveEnabled = true;
			mouseManager.selectedUnit.tradeEnabled = true;
			mouseManager.selectedUnit.lootEnabled = true;
			foreach (Node n in map.graph) {
				if (n.type.Equals ("sea")) {
					n.isWalkable = true;
				}
			}
			mouseManager.pathfinder.PathRequest (gameManager.players [1].Fleet [0], GameObject.Find ("Hex_10_6"));
			mouseManager.pathfinder.PathRequest (gameManager.players [1].Fleet [1], GameObject.Find ("Hex_11_5"));
			mouseManager.pathfinder.PathRequest (gameManager.players [1].Fleet [2], GameObject.Find ("Hex_11_3"));
			gameManager.players [1].Fleet [0].DisplayHp (true);
			gameManager.players [1].Fleet [1].DisplayHp (true);
			gameManager.players [1].Fleet [2].DisplayHp (true);
			break;
		case 13:
			nextButton.gameObject.SetActive (true);
			break;
		case 14:
			nextButton.gameObject.GetComponentInChildren<Text> ().text = "Quitter";
			nextButton.onClick.RemoveAllListeners ();
			nextButton.onClick.AddListener( () => SceneManager.LoadScene ("main", LoadSceneMode.Single) );
			break;
		}
	}

	public void NextTurn(){
		ship.RefuelEnergy ();
		panelHandler.updateShipInfo();
		gameManager.NextTurn ();
		gameManager.ResetFOW ();
		gameManager.RevealAreaAlreadyExplored ();
		gameManager.RevealAreaAroundCurrentPlayerShips ();
		gameManager.CenterCameraOnFirstShip ();
		if (step == 5) {
			NextStep();
		}
		if (step == 11 && ship.Crew.Count == 8) {
			NextStep();
		}
	}

}
