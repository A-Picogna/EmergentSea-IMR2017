﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class MapEditor : MonoBehaviour {

	public GameObject hexPrefab;
	public GameObject landPrefab;
	public GameObject seaPrefab;
	public GameObject coastPrefab;
	public GameObject harborPrefab;
	public GameObject treasurePrefab;
	public GameObject shipPrefab;
	public GameObject shipHUD;
	public int width;
	public int height;

	private int size;
	public Node[,] graph;
	private float xOffset = Mathf.Sqrt(3)/2;
	private float zOffset = 0.75f;
	private GameObject newHex;
	private Vector3 mousePos;
	private int selectedType = 0;
	System.Random rand;

	public Button btn_selectSea;
	public Button btn_selectLand;
	public Button btn_selectCoast;
	public Button btn_selectHarbor;
	public Button btn_selectTreasure;
	public Button btn_selectShip;
	private Lang lang;

	public Texture2D seaHexCursor;
	public Texture2D landHexCursor;
	public Texture2D coastHexCursor;
	public Texture2D harborHexCursor;
	public Texture2D treasureHexCursor;
	public Texture2D shipCursor;

	Vector3 worldCoordTreasure;

	void Start () {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		size = width * height;
		btn_selectSea = (Button) GameObject.Find("btn_selectSea").GetComponent<Button>();
		btn_selectLand = (Button) GameObject.Find("btn_selectLand").GetComponent<Button>();
		btn_selectCoast = (Button) GameObject.Find("btn_selectCoast").GetComponent<Button>();
		btn_selectHarbor = (Button) GameObject.Find("btn_selectHarbor").GetComponent<Button>();
		btn_selectTreasure = (Button) GameObject.Find("btn_selectTreasure").GetComponent<Button>();
		btn_selectShip = (Button) GameObject.Find("btn_selectShip").GetComponent<Button>();

		shipHUD = (GameObject) GameObject.Find ("HUDAddShip");
		/*shipHUD.transform.Find ("txt_menuLabel").GetComponent<Text> ().text = lang.getString("editor_addShipHUD_menuLabel");
		shipHUD.transform.Find ("Placeholder").GetComponent<Text> ().text = lang.getString("player")+" 1";
		shipHUD.transform.Find ("input_owner").GetComponent<Dropdown> ().options[0].text = lang.getString("player")+" 2";
		shipHUD.transform.Find ("input_owner").GetComponent<Dropdown> ().options[1].text = lang.getString("editor_addShipHUD_shipName");
		shipHUD.transform.Find ("input_owner").GetComponentInChildren<Text> ().text = lang.getString("editor_addShipHUD_shipOwner");
		shipHUD.transform.Find ("btn_ok").GetComponentInChildren<Text> ().text = lang.getString("editor_addShipHUD_validate");*/

		btn_selectSea.onClick.AddListener (() => setListener (0, seaHexCursor));
		btn_selectLand.onClick.AddListener(() => setListener (1, landHexCursor));
		btn_selectCoast.onClick.AddListener(() => setListener (2, coastHexCursor));
		btn_selectHarbor.onClick.AddListener(() => setListener (3, harborHexCursor));
		btn_selectTreasure.onClick.AddListener(() => setListener (4, treasureHexCursor));
		btn_selectShip.onClick.AddListener(() => setListener (5, shipCursor));

		//Button btn_save = GameObject.Find ("btn_save").GetComponent<Button>();
		//btn_save.onClick.AddListener (() => {
		//	LoadManager.instance.savePrefabricatedMapEditor ("test");
		//});
		InfoPanel ip = GameObject.Find ("txt_genInfo").GetComponent<InfoPanel> ();
		ip.DisplayInfo(lang.getString ("mapEditor_firstExplaination"), 20f); 
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			mousePos = Input.mousePosition;
		}
		if (Input.GetMouseButtonUp (0)) {
			if (Vector2.Distance (mousePos, Input.mousePosition) < 10f) {
				ReplaceElement (selectedType);
			}
		}
		if (Input.GetMouseButton (1)) {
			ReplaceElement (selectedType);
		}
	}

	public void setListener(int i, Texture2D cursor){
		selectedType = i;
		Cursor.SetCursor (cursor, new Vector2 (cursor.width / 2, cursor.height / 2), CursorMode.Auto);
	}

	void ReplaceElement(int newElementType){
		/*
		 * 0 : Sea
		 * 1 : Land
		 * 2 : Coast
		 * 3 : Harbor
		 * 4 : Treasure
		 */
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast (ray, out hitInfo)) {			
			GameObject ourHitObject = hitInfo.collider.transform.parent.gameObject;
			Vector3 worldPos = ourHitObject.transform.position;	
			int x = ourHitObject.GetComponent<Hex> ().x;
			int y = ourHitObject.GetComponent<Hex> ().y;
			switch (newElementType) {
			case 0:
				Destroy (ourHitObject);	
				GameObject sea_go = (GameObject)Instantiate (seaPrefab, worldPos, Quaternion.identity);
				graph [x,y] = new Node (x, y, worldPos, true, "sea");
				sea_go.name = "Hex_" + x + "_" + y;
				sea_go.GetComponent<Sea> ().x = x;
				sea_go.GetComponent<Sea> ().y = y;
				sea_go.transform.SetParent(this.transform);
				sea_go.isStatic = true;
				graph [x, y].isWalkable = true;
				DrawEdgesLines(sea_go);
				break;
			case 1:
				Destroy (ourHitObject);	
				GameObject land_go = (GameObject)Instantiate (landPrefab, worldPos, Quaternion.identity);
				graph [x,y] = new Node (x, y, worldPos, false, "land");
				land_go.name = "Hex_" + x + "_" + y;
				land_go.GetComponent<Land> ().x = x;
				land_go.GetComponent<Land> ().y = y;
				land_go.transform.SetParent(this.transform);
				land_go.isStatic = true;
				graph [x, y].isWalkable = false;
				DrawEdgesLines(land_go);
				break;
			case 2:
				Destroy (ourHitObject);	
				GameObject coast_go = (GameObject)Instantiate (coastPrefab, worldPos, Quaternion.identity);
				graph [x,y] = new Node (x, y, worldPos, false, "land");
				coast_go.name = "Hex_" + x + "_" + y;
				coast_go.GetComponent<Land> ().x = x;
				coast_go.GetComponent<Land> ().y = y;
				coast_go.GetComponent<Land> ().IsCoast = true;
				coast_go.transform.SetParent(this.transform);
				coast_go.isStatic = true;
				graph [x, y].isWalkable = false;
				DrawEdgesLines(coast_go);
				break;
			case 3:
				Destroy (ourHitObject);	
				GameObject harbor_go = (GameObject)Instantiate (harborPrefab, worldPos, Quaternion.identity);
				graph [x,y] = new Node (x, y, worldPos, false, "harbor");
				harbor_go.name = "Hex_" + x + "_" + y;
				harbor_go.GetComponent<Harbor> ().x = x;
				harbor_go.GetComponent<Harbor> ().y = y;
				harbor_go.transform.SetParent(this.transform);
				harbor_go.isStatic = true;
				graph [x, y].isWalkable = false;
				DrawEdgesLines(harbor_go);
				break;
			case 4:
				if (ourHitObject.GetComponent<Sea> () != null) {
					if (ourHitObject.GetComponent<Sea> ().Treasure_go != null) {
						DestroyImmediate (ourHitObject.GetComponent<Sea> ().Treasure_go);
						ourHitObject.GetComponent<Sea> ().RemoveTreasure();
					}
					if (ourHitObject.GetComponent<Sea> ().ShipContained != null) {
						DestroyImmediate (ourHitObject.GetComponent<Sea> ().ShipContained);
						ourHitObject.GetComponent<Sea> ().RemoveShip();
					}
					GameObject treasure_go = (GameObject)Instantiate (treasurePrefab, worldPos, Quaternion.identity);
					treasure_go.name = ourHitObject.GetComponent<Sea> ().name + "_Treasure";
					treasure_go.transform.SetParent (ourHitObject.GetComponent<Sea> ().transform);
					ourHitObject.GetComponent<Sea> ().GetComponent<Sea> ().AddTreasure (1000, treasure_go);
					graph [ourHitObject.GetComponent<Sea> ().x, ourHitObject.GetComponent<Sea> ().y].isWalkable = false;
				}
				break;
			case 5:
				if (ourHitObject.GetComponent<Sea> () != null) {
					if (ourHitObject.GetComponent<Sea> ().Treasure_go != null) {
						DestroyImmediate (ourHitObject.GetComponent<Sea> ().Treasure_go);
						ourHitObject.GetComponent<Sea> ().RemoveTreasure();
					}
					if (ourHitObject.GetComponent<Sea> ().ShipContained != null) {
						DestroyImmediate (ourHitObject.GetComponent<Sea> ().ShipContained);
						ourHitObject.GetComponent<Sea> ().RemoveShip();
					}
					ShowShipHUD();
					//createShip (player, ourHitObject.GetComponent<Sea> ().x, ourHitObject.GetComponent<Sea> ().y);
				}
				break;
			}
		}		
	}

	public void ShowShipHUD(){
		shipHUD.SetActive(true);

	}

	public Ship createShip(Player player, int x, int y){
		GameObject ship_go = (GameObject)Instantiate (shipPrefab, this.graph [x, y].worldPos, Quaternion.identity);
		ship_go.name = "Ship_" + player.Name + "_" + player.NbTotalShip;
		ship_go.GetComponent<Ship> ().ShipX = x;
		ship_go.GetComponent<Ship> ().ShipY = y;
		ship_go.GetComponent<Ship> ().ShipName = player.Name + "_Ship_" + player.NbTotalShip;
		ship_go.GetComponent<Ship> ().Gold = 0;
		ship_go.GetComponentInChildren<MeshRenderer> ().material.color = player.Color;
		Ship ship = ship_go.GetComponent<Ship> ();
		ship.Owner = player;
		ship.addCrewMember(new Filibuster());
		ship.addCrewMember(new Filibuster());
		//if (player.Type.Equals ("IA")) {
		ship.addCrewMember(new PowderMonkey());
		ship.addCrewMember(new Conjurer());
		ship.addCrewMember(new PowderMonkey());
		ship.addCrewMember(new Conjurer());
		//}
		foreach (CrewMember cm in ship.Crew) {
			cm.Lp = rand.Next (1, cm.LpMax);
		}
		player.Fleet.Add (ship);
		this.graph [x, y].isWalkable = false;
		GameObject.Find("Hex_" + x + "_" + y).GetComponent<Sea>().ShipContained = ship;
		player.NbTotalShip++;
		return ship;
	}

	public void InitializeMap(){
		graph = new Node[width, height];
		List<Vector3> V3LinesPositions = new List<Vector3>();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				float xPos = x * xOffset;
				if (y % 2 == 1) {
					xPos += xOffset / 2f;
				}
				Vector3 worldPosition = new Vector3 (xPos, 0, y * zOffset);
				GameObject hex_go = (GameObject)Instantiate (seaPrefab, worldPosition, Quaternion.identity);
				graph [x, y] = new Node (x, y, worldPosition, false, "sea");
				graph [x, y].tag = true;
				graph [x, y].isWalkable = true;
				DrawEdgesLines(hex_go);
				hex_go.name = "Hex_" + x + "_" + y;
				hex_go.GetComponent<Hex> ().x = x;
				hex_go.GetComponent<Hex> ().y = y;
				hex_go.transform.SetParent (this.transform);
				hex_go.isStatic = true;
			}
		}
	}

	public void LoadMapRoutine(MapFile SavedMap) {
		LoadMap (SavedMap);
		InstantiateMap (SavedMap);
		loadFoodAndTreasures (SavedMap);
	}

	public void LoadMap(MapFile SavedMap) {
		this.height = SavedMap.height;
		Debug.Log ("Height: "+this.height.ToString());
		this.width = SavedMap.width;
		Debug.Log ("Width: "+this.width.ToString());

		this.graph = new Node[width, height];

		int index = 0;
		for (int i = 0; i < this.width; i++) {
			for (int j = 0; j < this.height; j++) {
				index = (i * this.height) + j;
				Debug.Log (SavedMap.graph [index].type);

				// Use the loop for initialise de graph too, we save one loop with this
				float xPos = SavedMap.graph[index].x * xOffset;

				// if we're an odd line, we need to reduce the offset by half
				if (SavedMap.graph[index].y % 2 == 1) {
					xPos += xOffset / 2f;
				}

				Vector3 worldPosition = new Vector3 (xPos, 0, SavedMap.graph[index].y * zOffset);
				Debug.Log (worldPosition.ToString ());
				this.graph [i, j] = new Node(SavedMap.graph[index].x,
					SavedMap.graph[index].y,
					worldPosition,
					SavedMap.graph[index].isWalkable,
					SavedMap.graph[index].type);

				this.graph [i, j].tag = SavedMap.graph [index].tag;
			}
		}
	}

	void InstantiateMap(MapFile mapSaved) {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				string hexType = graph [x, y].type;
				GameObject hex_go = null;
				switch (hexType) {
				case "sea":
					hex_go = (GameObject)Instantiate (seaPrefab, graph [x, y].worldPos, Quaternion.identity);
					break;
				case "land":
					int index = (x * this.height) + y;
					if(mapSaved.graph[index].LandIsCoast)
						hex_go = (GameObject)Instantiate (coastPrefab, graph [x, y].worldPos, Quaternion.identity);
					else 
						hex_go = (GameObject)Instantiate (landPrefab, graph [x, y].worldPos, Quaternion.identity);
					break;
				case "harbor":
					hex_go = (GameObject)Instantiate (harborPrefab, graph [x, y].worldPos, Quaternion.identity);
					break;
				}
				DrawEdgesLines(hex_go);
				hex_go.name = "Hex_" + x + "_" + y;
				hex_go.GetComponent<Hex> ().x = x;
				hex_go.GetComponent<Hex> ().y = y;
				hex_go.transform.SetParent (this.transform);
				hex_go.isStatic = true;
			}
		}
	}

	void loadFoodAndTreasures(MapFile saveMap) {
		Sea SeaBuffer;
		Land LandBuffer;
		GameObject caseTreasure;
		GameObject tres;
		int index;
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (graph [x, y].type == "sea") {
					index = (x * this.height) + y;

					SeaBuffer = (GameObject.Find ("Hex_" + x + "_" + y)).GetComponent<Sea> ();

					// On remet le bon nombre de nourriture
					SeaBuffer.FoodQuantity = saveMap.graph [index].SeaFood;

					if (saveMap.graph [index].SeaTreasure > 0) {
						// On ajoute un trésor ! :)
						worldCoordTreasure = graph [x,y].worldPos;
						graph [x,y].isWalkable = false;
						caseTreasure = GameObject.Find ("Hex_" + x + "_" + y);
						tres = (GameObject) Instantiate (treasurePrefab, worldCoordTreasure, Quaternion.identity);
						tres.name = caseTreasure.name+"_Treasure"+rand.Next(0,1000000000);
						tres.transform.SetParent (caseTreasure.transform);
						SeaBuffer.AddTreasure (saveMap.graph[index].SeaTreasure, tres);
					}
				}
				if (graph [x, y].type == "land") {
					index = (x * this.height) + y;

					LandBuffer = (GameObject.Find ("Hex_" + x + "_" + y)).GetComponent<Land> ();
					LandBuffer.IsCoast = saveMap.graph[index].LandIsCoast;

				}
			}
		}
	}

	void DrawEdgesLines(GameObject go){
		Vector3 worldPosition = go.transform.position;
		LineRenderer lineRenderer = go.AddComponent<LineRenderer> (); // Add or get LineRenderer component to game object
		lineRenderer.SetWidth(0.01f, 0.01f);
		lineRenderer.SetVertexCount(7);  // 6+1 since vertex 6 has to connect to vertex 1
		lineRenderer.SetColors(Color.black, Color.black);
		//lineRenderer.material.color = Color.black;
		lineRenderer.material = new Material (Shader.Find("Sprites/Default"));
		for (int i = 0; i < 7; i++) {
			// Note for unknown reason, the y value have to set to 0.06f to be align with the hex
			Vector3 pos = new Vector3(Hex_corner(worldPosition.x, worldPosition.z, i)[0], 0.06f, Hex_corner(worldPosition.x, worldPosition.z, i)[1]) ; // Positions of hex vertices
			lineRenderer.SetPosition(i, pos);
		}
	}

	float[] Hex_corner(float x, float y, int i){
		int angle_deg = 60 * i   + 30;
		float angle_rad = Mathf.PI / 180 * angle_deg;
		float[] res = new float[2];
		res[0] = x + 0.5f * Mathf.Cos (angle_rad);
		res[1] = y + 0.5f * Mathf.Sin (angle_rad);
		return res;
	}

	public MapFile SaveMap() {
		int index = 0;
		MapFile mapSaved = new MapFile();
		mapSaved.height = this.height;
		Debug.Log ("Height: "+this.height.ToString());
		mapSaved.width = this.width;
		Debug.Log ("Width: "+this.width.ToString());
		mapSaved.graph = new NodeStruct[(this.height * this.width)];

		Debug.Log ("MapSaved_size=" + (this.height * this.width));

		int k = 0;
		Sea SeaBuffer;
		Land LandBuffer;
		for (int i = 0; i < this.width; i++) {
			for (int j = 0; j < this.height; j++) {
				index = (i * this.height) + j;

				mapSaved.graph [index] = new NodeStruct(this.graph [i, j].x, 
					this.graph [i, j].y, 
					this.graph [i, j].isWalkable, 
					this.graph [i, j].type,
					this.graph [i, j].tag
				);
				if(this.graph[i,j].type == "sea") {
					SeaBuffer = (GameObject.Find ("Hex_" + i + "_" + j)).GetComponent<Sea> ();

					mapSaved.graph [index].SeaFood = SeaBuffer.FoodQuantity;
					mapSaved.graph [index].SeaTreasure = SeaBuffer.Treasure;
				}
				if (this.graph [i, j].type == "land") {
					LandBuffer = (GameObject.Find ("Hex_" + i + "_" + j)).GetComponent<Land> ();
					mapSaved.graph [index].LandIsCoast = LandBuffer.IsCoast;
				}

			}
		}

		return mapSaved;
	}
}
