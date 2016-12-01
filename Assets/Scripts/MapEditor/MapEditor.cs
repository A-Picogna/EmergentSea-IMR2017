using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapEditor : MonoBehaviour {

	public GameObject hexPrefab;
	public GameObject landPrefab;
	public GameObject seaPrefab;
	public GameObject coastPrefab;
	public GameObject harborPrefab;
	public GameObject treasurePrefab;
	public GameObject shipPrefab;
	public int width;
	public int height;

	private int size;
	private Node[,] graph;
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

	void Start () {
		size = width * height;
		InitializeMap();
		btn_selectSea = (Button) GameObject.Find("btn_selectSea").GetComponent<Button>();
		btn_selectLand = (Button) GameObject.Find("btn_selectLand").GetComponent<Button>();
		btn_selectCoast = (Button) GameObject.Find("btn_selectCoast").GetComponent<Button>();
		btn_selectHarbor = (Button) GameObject.Find("btn_selectHarbor").GetComponent<Button>();
		btn_selectTreasure = (Button) GameObject.Find("btn_selectTreasure").GetComponent<Button>();

		btn_selectSea.onClick.AddListener(() => selectedType = 0);
		btn_selectLand.onClick.AddListener(() => selectedType = 1);
		btn_selectCoast.onClick.AddListener(() => selectedType = 2);
		btn_selectHarbor.onClick.AddListener(() => selectedType = 3);
		btn_selectTreasure.onClick.AddListener(() => selectedType = 4);

		Button btn_save = GameObject.Find ("btn_save").GetComponent<Button>();
		btn_save.onClick.AddListener (() => {
			LoadManager.instance.savePrefabricatedMapEditor ("test");
		});
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
				DrawEdgesLines(harbor_go);
				break;
			case 4:
				if (ourHitObject.GetComponent<Sea> () != null) {
					GameObject treasure_go = (GameObject)Instantiate (treasurePrefab, worldPos, Quaternion.identity);
					treasure_go.name = ourHitObject.GetComponent<Sea> ().name + "_Treasure";
					treasure_go.transform.SetParent (ourHitObject.GetComponent<Sea> ().transform);
					ourHitObject.GetComponent<Sea> ().GetComponent<Sea> ().AddTreasure (1000, treasure_go);
					graph [ourHitObject.GetComponent<Sea> ().x, ourHitObject.GetComponent<Sea> ().y].isWalkable = false;
				}
				break;
			}
		}		
	}

	void InitializeMap(){
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
				DrawEdgesLines(hex_go);
				hex_go.name = "Hex_" + x + "_" + y;
				hex_go.GetComponent<Hex> ().x = x;
				hex_go.GetComponent<Hex> ().y = y;
				hex_go.transform.SetParent (this.transform);
				hex_go.isStatic = true;
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
