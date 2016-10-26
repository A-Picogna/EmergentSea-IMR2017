﻿// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public class Map : MonoBehaviour {

	public GameObject hexPrefab;
	public GameObject landPrefab;
	public GameObject seaPrefab;

	// Map in graph to calculate pathfinding
	public Node[,] graph;

	// size of map in terms of numer of hexagon
	public int width = 20;
	public int height = 20;
	//float xOffset = 0.882f;
	//float zOffset = 0.764f;
	float xOffset = Mathf.Sqrt(3)/2;
	float zOffset = 0.75f;
	int nbCasesRemplinit = 10;
	System.Random rand = new System.Random();
	List<int> abcisses = new List<int>();
	List<int> ordonnes = new List<int>();
	List<GameObject> FirstStep = new List<GameObject>();
	Vector3 unitycord = new Vector3(0,0,0);
	public Material lineMat;

	// Use this for initialization
	void Start () {
		
		// RESPECT THIS STRIC ORDER
		// Init map
		initializeMap();
		// Generate some lands
		generateLand ();
		// Add neighbours
		AddNeighboursToNodes ();




	}
	// Update is called once per frame
	void Update () {
	
	}

	void generateLand(){
		for (int k=0; k<nbCasesRemplinit ;k++){
			int x = rand.Next(0, width);
			int y = rand.Next(0, height);
			//Debug.Log(x.ToString());
			abcisses.Add(x);
			ordonnes.Add(y);
		}

		// We stat to generate some land

		//GameObject remplacable = GameObject.Find("Hex_" + x + "_" + y);

		for (int a = 0; a < nbCasesRemplinit; a++) {
			GameObject remplacable = GameObject.Find ("Hex_" + abcisses [a] + "_" + ordonnes [a]);
			unitycord = remplacable.transform.position;
			Destroy (remplacable);

			GameObject hex_go = (GameObject)Instantiate (landPrefab, unitycord, Quaternion.identity);

			// UPDATE NODES
			Node node = graph [abcisses [a], ordonnes [a]];
			graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");

			// Add line to the edge of the Hex
			drawEdgesLines(hex_go);

			hex_go.name = "Hex_" + abcisses [a] + "_" + ordonnes [a];
			hex_go.GetComponent<Land> ().x = abcisses [a];
			hex_go.GetComponent<Land> ().y = ordonnes [a];
			hex_go.transform.SetParent (this.transform);
			hex_go.isStatic = true;
			List<GameObject> Neighbours = hex_go.GetComponent<Hex> ().getNeighbours ();
			FirstStep = Neighbours;
			for (int i = 0; i < FirstStep.Count; i++) {
				
				var abs = FirstStep [i].GetComponent<Hex> ().x;
				var ord = FirstStep [i].GetComponent<Hex> ().y;
				GameObject land_go = FirstStep [i];

				if (rand.Next (1, 101) <= 50) {
					unitycord = FirstStep [i].transform.position;
					Destroy (FirstStep [i]);
					land_go = (GameObject)Instantiate (landPrefab, unitycord, Quaternion.identity);

					// UPDATE NODES
					node = graph [abs, ord];
					graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");

					// Add line to the edge of the Hex
					drawEdgesLines(land_go);

					land_go.name = "Hex_" + abs + "_" + ord;
					land_go.GetComponent<Land> ().x = abs;
					land_go.GetComponent<Land> ().y = ord;
					land_go.transform.SetParent (this.transform);
					land_go.isStatic = true;
				}

				List<GameObject> NextNeighbours = land_go.GetComponent<Hex> ().getNeighbours ();

				for (int j = 0; j < NextNeighbours.Count; j++) {
					//if (FirstStep[i] != LAAAND) { JE FAIS LA SUITE}
					if (rand.Next (1, 101) <= 25) {
						int NextAbs = NextNeighbours [j].GetComponent<Hex> ().x;
						int NextOrd = NextNeighbours [j].GetComponent<Hex> ().y;
						unitycord = NextNeighbours [j].transform.position;
						Destroy (NextNeighbours [j]);
						GameObject Next_land_go = (GameObject)Instantiate (landPrefab, unitycord, Quaternion.identity);

						// UPDATE NODES
						node = graph [NextAbs, NextOrd];
						graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");

						// Add line to the edge of the Hex
						drawEdgesLines(Next_land_go);

						Next_land_go.name = "Hex_" + NextAbs + "_" + NextOrd;
						Next_land_go.GetComponent<Land> ().x = NextAbs;
						Next_land_go.GetComponent<Land> ().y = NextOrd;
						Next_land_go.transform.SetParent (this.transform);
						Next_land_go.isStatic = true;
					}
				}
			}
		}
	}

	void initializeMap(){
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



}
