// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Map : MonoBehaviour {

	public GameObject hexPrefab;
	public GameObject landPrefab;
	public GameObject seaPrefab;

	// Map in graph to calculate pathfinding
	public Node[,] graph;

	// size of map in terms of numer of hexagon
	public int width = 20;
	public int height = 20;
	float xOffset = 0.882f;
	float zOffset = 0.764f;

	// Use this for initialization
	void Start () {

		//selectedUnit.GetComponent<Ship> ().ShipX = selectedUnit.transform.position.x;
		//selectedUnit.GetComponent<Ship> ().ShipY = selectedUnit.transform.position.y;

		
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				
				float xPos = x * xOffset;

				// if we're an odd line, we need to reduce the offset by half
				if (y % 2 == 1) {
					xPos += xOffset/2f;
				}

				// Creation of a new hex
				GameObject hex_go = (GameObject) Instantiate (hexPrefab, new Vector3 (xPos, 0, y*zOffset), Quaternion.identity);

				// Name the hex according to the grid coordinates
				hex_go.name = "Hex_" + x + "_" + y;

				// Store the grid coord in the hex itself
				hex_go.GetComponent<Hex> ().x = x;
				hex_go.GetComponent<Hex> ().y = y;

				// set the hex in a parent component, parent this hex to the map object
				hex_go.transform.SetParent (this.transform);

				hex_go.isStatic = true;

			}
		}
		GeneratePathfindingGraph ();

		// We stat to generate some land
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				// GameObject.Find ("Hex_" + x + "_" + y);
				// if we replace by a land
				// store coord of the unity world
				// Delete the sea hex and replace it by land hex
			}
		}


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void GeneratePathfindingGraph(){
		
		// Initialize the array
		graph = new Node[width, height];

		// Initialize each Node
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				graph [x, y] = new Node ();
				graph [x, y].x = x;
				graph [x, y].y = y;
			}
		}

		// Creation of the graph with neighbours
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				// First we find the gameobject at te current coord
				// Then we get his neighbour with the Hex function
				GameObject currentHex = GameObject.Find ("Hex_" + x + "_" + y);
				GameObject[] currentHexNeighbours = currentHex.GetComponent<Hex> ().getNeighboursOld();
				for (int i = 0; i < currentHexNeighbours.Length; i++) {
					if (currentHexNeighbours [i] != null) {
						graph [x, y].neighbours.Add (graph [
							currentHexNeighbours [i].GetComponent<Hex>().x,
							currentHexNeighbours [i].GetComponent<Hex>().y
						]);
					}
				}
			}
		}
	}


}
