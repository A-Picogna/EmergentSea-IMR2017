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
	int nbCasesRemplinit = 8;
	System.Random rand = new System.Random();
	List<int> abcisses = new List<int>();
	List<int> ordonnes = new List<int>();
	Vector3 unitycord = new Vector3(0,0,0);

	// Use this for initialization
	void Start () {
		//selectedUnit.GetComponent<Ship> ().ShipX = selectedUnit.transform.position.x;
		//selectedUnit.GetComponent<Ship> ().ShipY = selectedUnit.transform.position.y;
		initializeMap();
		GeneratePathfindingGraph ();
		generateLand ();

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

		for (int a = 0; a < nbCasesRemplinit; a++){
			GameObject remplacable = GameObject.Find("Hex_" + abcisses[a] + "_" + ordonnes[a]);
			unitycord = remplacable.transform.position;
			Destroy(remplacable);

			GameObject hex_go = (GameObject)Instantiate(landPrefab, unitycord, Quaternion.identity);
			hex_go.name = "Hex_" + abcisses[a] + "_" + ordonnes[a];
			hex_go.GetComponent<Land>().x = abcisses[a];
			hex_go.GetComponent<Land>().y = ordonnes[a];
			hex_go.transform.SetParent(this.transform);
			hex_go.isStatic = true;

		}
		// GameObject.Find ("Hex_" + x + "_" + y);
		// if we replace by a land
		// store coord of the unity world
		// Delete the sea hex and replace it by land hex

		//au lieu de créer un hex_go je crée un objet land ou sea cad :
		//land ou sea ont une certaine probabilité, ca va etre un entier en l'occurence qui divisera 1 pour la proba...
		//si random.Next(0,x) =< entier -- x représentera le nombre de chance au total


		//GameObject remplacable = GameObject.Find("Hex_" + x + "_" + y);
	}

	void initializeMap(){
		graph = new Node[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				// Use the loop for initialise de graph too, we save one loop with this
				graph [x, y] = new Node ();
				graph [x, y].x = x;
				graph [x, y].y = y;

				float xPos = x * xOffset;

				// if we're an odd line, we need to reduce the offset by half
				if (y % 2 == 1) {
					xPos += xOffset/2f;
				}

				// Creation of a new hex
				GameObject hex_go = (GameObject)Instantiate(seaPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);

				// Name the hex according to the grid coordinates
				hex_go.name = "Hex_" + x + "_" + y;

				// Store the grid coord in the hex itself
				hex_go.GetComponent<Sea>().x = x;
				hex_go.GetComponent<Sea>().y = y;

				// set the hex in a parent component, parent this hex to the map object
				hex_go.transform.SetParent (this.transform);

				hex_go.isStatic = true;

			}
		}
	}

	void GeneratePathfindingGraph(){
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
