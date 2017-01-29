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
	public MouseManager mouseManager;
	public GameManager gameManager;
	public PanelHandler panelHandler;
	public StepBox stepInfo;
	public Button nextButton;
	public Button endTurnButton;
	public Projector objProj;
	public Map map;
	public Ship ship;

	int step = 0;

	void Start () {
		nextButton.onClick.AddListener( () => NextStep() );	
		mouseManager.mouseManagerEnabled = false;
		endTurnButton.gameObject.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		switch (step) {
		case 1:
			if (mouseManager.selectedUnit != null) {
				ship = mouseManager.selectedUnit;
				mouseManager.selectedUnit.fishingEnabled = false;
				mouseManager.selectedUnit.attackEnabled = false;
				mouseManager.selectedUnit.moveEnabled = false;
				mouseManager.selectedUnit.tradeEnabled = false;
				mouseManager.selectedUnit.lootEnabled = false;
				NextStep ();
			}
			break;
		case 3:
			if (mouseManager.selectedUnit != null && mouseManager.selectedUnit.ShipX == 2 && mouseManager.selectedUnit.ShipY == 2) {
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
			break;
		case 8:
			break;
		case 9:
			break;
		case 10:
			break;
		case 11:
			break;
		case 12:
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
			break;
		case 8:
			break;
		case 9:
			break;
		case 10:
			break;
		case 11:
			break;
		case 12:
			break;
		case 13:
			break;
		}
	}

	public void NextTurn(){
		gameManager.NextTurn ();
		gameManager.ResetFOW ();
		gameManager.RevealAreaAlreadyExplored ();
		gameManager.RevealAreaAroundCurrentPlayerShips ();
		gameManager.CenterCameraOnFirstShip ();
		if (step == 5) {
			NextStep();
		}
	}

}
