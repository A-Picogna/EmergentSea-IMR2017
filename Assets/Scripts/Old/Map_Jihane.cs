// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.SceneManagement;

public class Map_Jihane : MonoBehaviour {

	public GameObject hexPrefab;
	public GameObject landPrefab;
	public GameObject seaPrefab;

	// Map in graph to calculate pathfinding
	public Node[,] graph;

	// size of map in terms of numer of hexagon
	public int width;
	public int height;
	public int size;
	int nbCasesRemplinit;
	float xOffset = Mathf.Sqrt(3)/2;
	float zOffset = 0.75f;
	bool mapFausse = false;
	List<int> abcisses;
	List<int> ordonnes;
	List<GameObject> FirstStep;
	List<Node> GroupSea;
	List<Node> GroupNeighbours;
	Vector3 worldCoord;
	GameObject land_go;
	System.Random rand;

	// Use this for initialization
	void Start () {

		nbCasesRemplinit = 10;
		size = width * height;
		rand = new System.Random();
		abcisses = new List<int>();
		ordonnes = new List<int>();
		FirstStep = new List<GameObject>();
		GroupSea = new List<Node>();
		GroupNeighbours = new List<Node>();
		worldCoord = new Vector3(0, 0, 0);

		// Init map
		InitializeMap();
		// Generate some lands
		GenerateLand();
		// Check if there is no sea prisonner
		mapFausse = VerifMap();
		Debug.Log(mapFausse);
		Debug.Log(SceneManager.GetActiveScene().name);
		if (mapFausse){
			Debug.Log ("je vais etre changee");
			List<GameObject> children = new List<GameObject>();
			foreach (Transform child in this.transform){
				children.Add(child.gameObject);
			}
			children.ForEach(child => Destroy(child));
			Application.LoadLevel("map");
		}

		// Add neighbours
		AddNeighboursToNodes ();

	}
	// Update is called once per frame
	void Update () {

	}

	public void GenerateLand(){

		for (int k = 0; k < nbCasesRemplinit; k++){
			int x = rand.Next(0, width);
			int y = rand.Next(0, height);
			abcisses.Add(x);
			ordonnes.Add(y);
		}

		// We stat to generate some land
		for (int a = 0; a < nbCasesRemplinit; a++){
			GameObject remplacable = GameObject.Find("Hex_" + abcisses[a] + "_" + ordonnes[a]);
			worldCoord = remplacable.transform.position;
			Destroy(remplacable);

			GameObject hex_go = (GameObject)Instantiate(landPrefab, worldCoord, Quaternion.identity);

			// UPDATE NODES
			Node node = graph [abcisses [a], ordonnes [a]];
			graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");

			// Add line to the edge of the Hex
			drawEdgesLines(hex_go);

			hex_go.name = "Hex_" + abcisses[a] + "_" + ordonnes[a];
			hex_go.GetComponent<Hex>().x = abcisses[a];
			hex_go.GetComponent<Hex>().y = ordonnes[a];
			hex_go.transform.SetParent(this.transform);
			hex_go.isStatic = true;

			List<GameObject> Neighbours = hex_go.GetComponent<Hex>().getNeighbours();
			int nonRemplacable = rand.Next(0, Neighbours.Count);
			Neighbours.RemoveAt(nonRemplacable);
			FirstStep = Neighbours;

			for (int i = 0; i < FirstStep.Count; i++){
				int abs = FirstStep[i].GetComponent<Hex>().x;
				int ord = FirstStep[i].GetComponent<Hex>().y;
				if(graph[abs,ord].type.Equals("sea")){

					worldCoord = FirstStep[i].transform.position;
					Destroy(FirstStep[i]);
					land_go = (GameObject)Instantiate(landPrefab, worldCoord, Quaternion.identity);

					// UPDATE NODES
					node = graph [abs, ord];
					graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");

					// Add line to the edge of the Hex
					drawEdgesLines(land_go);

					land_go.name = "Hex_" + abs + "_" + ord;
					land_go.GetComponent<Hex>().x = abs;
					land_go.GetComponent<Hex>().y = ord;
					land_go.transform.SetParent(this.transform);
					land_go.isStatic = true;
				}
				List<GameObject> NextNeighbours = land_go.GetComponent<Hex>().getNeighbours();

				int k = 0;
				while (k < 2){
					if (NextNeighbours != null)	{
						int Nextnonremplacable = rand.Next(0, NextNeighbours.Count);
						NextNeighbours.RemoveAt(Nextnonremplacable);
					}
					k = k + 1;
				}
				for (int j = 0; j < NextNeighbours.Count; j++){
					int Nextabs = NextNeighbours[j].GetComponent<Hex>().x;
					int Nextord = NextNeighbours[j].GetComponent<Hex>().y;
					if(graph[Nextabs,Nextord].type.Equals("sea")){
						worldCoord = NextNeighbours[j].transform.position;
						Destroy(NextNeighbours[j]);

						GameObject Next_land_go = (GameObject)Instantiate(landPrefab, worldCoord, Quaternion.identity);

						// UPDATE NODES
						node = graph [Nextabs, Nextord];
						graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");

						// Add line to the edge of the Hex
						drawEdgesLines(Next_land_go);

						Next_land_go.name = "Hex_" + Nextabs + "_" + Nextord;
						Next_land_go.GetComponent<Hex>().x = Nextabs;
						Next_land_go.GetComponent<Hex>().y = Nextord;
						Next_land_go.transform.SetParent(this.transform);
						Next_land_go.isStatic = true;

					}
				}
			}
		}
	}

	void InitializeMap(){
		graph = new Node[width, height];
		List<Vector3> V3LinesPositions = new List<Vector3>();
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				// Use the loop for initialise de graph too, we save one loop with this
				float xPos = x * xOffset;

				// if we're an odd line, we need to reduce the offset by half
				if (y % 2 == 1) {
					xPos += xOffset / 2f;
				}

				// Creation of a new hex
				Vector3 worldPosition = new Vector3 (xPos, 0, y * zOffset);
				GameObject hex_go = (GameObject)Instantiate (seaPrefab, worldPosition, Quaternion.identity);
				graph [x, y] = new Node (x, y, worldPosition, true, "sea");

				// Add line to the edge of the Hex
				drawEdgesLines(hex_go);

				// Name the hex according to the grid coordinates
				hex_go.name = "Hex_" + x + "_" + y;
				// Store the grid coord in the hex itself
				hex_go.GetComponent<Sea> ().x = x;
				hex_go.GetComponent<Sea> ().y = y;
				// set the hex in a parent component, parent this hex to the map object
				hex_go.transform.SetParent (this.transform);
				hex_go.isStatic = true;

			}
		}
	}

	void drawEdgesLines(GameObject go){
		Vector3 worldPosition = go.transform.position;
		LineRenderer lineRenderer = go.AddComponent<LineRenderer> (); // Add or get LineRenderer component to game object
		lineRenderer.SetWidth(0.01f, 0.01f);
		lineRenderer.SetVertexCount(7);  // 6+1 since vertex 6 has to connect to vertex 1
		lineRenderer.SetColors(Color.black, Color.black);
		//lineRenderer.material.color = Color.black;
		lineRenderer.material = new Material (Shader.Find("Sprites/Default"));
		for (int i = 0; i < 7; i++) {
			// Note for unknown reason, the y value have to set to 0.06f to be align with the hex
			Vector3 pos = new Vector3(hex_corner(worldPosition.x, worldPosition.z, i)[0], 0.06f, hex_corner(worldPosition.x, worldPosition.z, i)[1]) ; // Positions of hex vertices
			lineRenderer.SetPosition(i, pos);
		}
	}

	public bool VerifMap(){
		bool verif = false;
		int l = 0;
		int m = 0;
		int compteur = 0;

		// We get the first sea we found
		while (verif != true && l < width){
			m = 0;
			while (verif != true && m < height){
				if(graph[l,m].type.Equals("sea")){
					graph[l,m].tag = true;
					GroupSea.Add(graph[l,m]);
					verif = true;
				}
			}
		}

		// We find all sea connected to this first sea
		for (int i = 0; i < GroupSea.Count; i++){
			GroupNeighbours = GroupSea[i].getNodesNeighbours(graph);
			for (int j = 0; j < GroupNeighbours.Count; j++){
				int x = GroupNeighbours[j].x;
				int y = GroupNeighbours [j].y;
				if(graph[x,y].type.Equals("sea") & graph[x,y].tag==false){
					GroupSea.Add(GroupNeighbours[j]);
					graph[x,y].tag = true;
				}
			}
		}

		while (l < width){
			mapFausse = false;
			m = 0;
			while (m < height){
				if (graph[l,m].type.Equals("sea") & !graph[l,m].tag){
					mapFausse = true;
					compteur++;
					if (compteur > 3 | (graph [l, m].x == 0 & graph [l, m].y == 0)) {
						break;
					}
				}
				m=m+1;
			}
			if (mapFausse & compteur > 3){
				break;
			}
			l=l+1;
		}
		return mapFausse;
	}

	// Add the neighbours to each node, for each node AFTER the land generation
	void AddNeighboursToNodes(){
		// Creation of the graph with neighbours
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				if (x-1 >= 0) graph [x, y].neighbours.Add (graph [x-1, y]);
				if (x+1 < width) graph [x, y].neighbours.Add (graph [x+1, y]);
				if (y % 2 == 0) {
					if (x-1 >= 0 && y+1 < height) graph [x, y].neighbours.Add (graph [x - 1, y + 1]);
					if (y+1 < height) graph [x, y].neighbours.Add (graph [x, y+1]);
					if (x-1 >= 0 && y-1 >= 0) graph [x, y].neighbours.Add (graph [x-1, y-1]);
					if (y-1 >= 0) graph [x, y].neighbours.Add (graph [x, y-1]);
				} else {
					if (y+1 < height) graph [x, y].neighbours.Add (graph [x, y+1]);
					if (x+1 < width && y+1 < height) graph [x, y].neighbours.Add (graph [x+1, y+1]);
					if ( y-1 >= 0) graph [x, y].neighbours.Add (graph [x, y-1]);
					if (x+1 < width && y-1 >= 0) graph [x, y].neighbours.Add (graph [x+1, y-1]);
				}
			}
		}
	}

	float[] hex_corner(float x, float y, int i){
		int angle_deg = 60 * i   + 30;
		float angle_rad = Mathf.PI / 180 * angle_deg;
		float[] res = new float[2];
		res[0] = x + 0.5f * Mathf.Cos (angle_rad);
		res[1] = y + 0.5f * Mathf.Sin (angle_rad);
		return res;
	}

	public int Size{
		get{ return size; }
	}


}
