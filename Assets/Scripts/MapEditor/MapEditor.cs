using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	private string selectedType = "sea";

	void Start () {
		size = width * height;
		InitializeMap();
	
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
	}

	void ReplaceElement(string newElementType){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast (ray, out hitInfo)) {			
			GameObject ourHitObject = hitInfo.collider.transform.parent.gameObject;
			Vector3 worldPos = ourHitObject.transform.position;	
			int x = ourHitObject.GetComponent<Hex> ().x;
			int y = ourHitObject.GetComponent<Hex> ().y;
			switch (newElementType) {
			case "sea":
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
			case "land":
				Destroy (ourHitObject);	
				GameObject land_go = (GameObject)Instantiate (landPrefab, worldPos, Quaternion.identity);
				graph [x,y] = new Node (x, y, worldPos, false, "sea");
				land_go.name = "Hex_" + x + "_" + y;
				land_go.GetComponent<Land> ().x = x;
				land_go.GetComponent<Land> ().y = y;
				land_go.transform.SetParent(this.transform);
				land_go.isStatic = true;
				DrawEdgesLines(land_go);
				break;
			case "harbor":
				Destroy (ourHitObject);	
				GameObject harbor_go;
				graph [x,y] = new Node (x, y, worldPos, false, "harbor");
				//harbor_go.name = "Hex_" + x + "_" + y;
				//harbor_go.GetComponent<Harbor> ().x = x;
				//harbor_go.GetComponent<Harbor> ().y = y;
				//harbor_go.transform.SetParent(this.transform);
				//harbor_go.isStatic = true;
				//drawEdgesLines(harbor_go);
				break;
			case "ship":
				GameObject ship_go = (GameObject)Instantiate (shipPrefab, worldPos, Quaternion.identity);
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
}
