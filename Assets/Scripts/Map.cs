// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Map : MonoBehaviour {

	public GameObject hexPrefab;
	public GameObject landPrefab;
	public GameObject seaPrefab;
    public GameObject coastPrefab;
    public GameObject harborPrefab;
	public GameObject foodPrefab;
	public GameObject treasurePrefab;
    public GameObject shipPrefab;

    // Map in graph to calculate pathfinding
    public Node[,] graph;

	// size of map in terms of numer of hexagon
	public int width;
	public int height;
	public int nbCasesRemplinit;
	public int nombreCasesTreasure;
	public int nombreCasesFood;
	public int tresorMin;
	public int tresorMax;
	public int foodQuantityMax;
	public int foodQuantityMin;

	int size;
	int regenerateCount;
	float xOffset = Mathf.Sqrt(3)/2;
	float zOffset = 0.75f;
	bool mapFausse;

	List<int> abcisses;
	List<int> ordonnes;
	List<Node> FirstStep;
	List<Node> GroupSea;
	List<Node> GroupNeighbours;
	List<Node> GroupLand;
	List<List<Node>> GroupListPossibleHarbor;

	Vector3 worldCoordFood;
	Vector3 worldCoordTreasure;
	Vector3 worldCoord;

	GameObject land_go;
	System.Random rand;

    // Use this for initialization
    void Start () {

	}

	public void LaunchMapGeneration () {
		size = width * height;
		rand = new System.Random();
		GroupLand = new List<Node>();
		GroupListPossibleHarbor = new List<List<Node>>();
		worldCoord = new Vector3(0, 0, 0);
		mapFausse = false;
		regenerateCount = 1;
		worldCoordFood = new Vector3(0, 0, 0);
		worldCoordTreasure = new Vector3(0, 0, 0);

		// Init map
		InitializeMap();
		// Generate some lands
		GenerateLand();
		// Check if there is no sea prisonner
		mapFausse = VerifMap();
		//Debug.Log(mapFausse);

		while (mapFausse && regenerateCount <= 100){
			//Debug.Log ("Map fausse, changement n°"+regenerateCount);
			InitializeMap();
			GenerateLand();
			mapFausse = VerifMap();
			regenerateCount++;
		}
		mapFausse = false;

		InstantiateMap ();

		//Generate coast and harbor
		generateHarbor();

		generateFood ();
		generateTreasure ();

		// Add neighbours
		AddNeighboursToNodes ();
	}

	public void LaunchMapLoading(MapFile saveMap) {

		LoadMap (saveMap);

		this.size = this.width * this.height;
		rand = new System.Random();
		//GroupLand = new List<Node>();						// On a déjà crée les harbors, pas la paine
		//GroupListPossibleHarbor = new List<List<Node>>(); // On a déjà crée les harbors, pas la peine
		worldCoord = new Vector3(0, 0, 0);
		//mapFausse = false;
		//regenerateCount = 1;
		worldCoordFood = new Vector3(0, 0, 0);
		worldCoordTreasure = new Vector3(0, 0, 0);

		InstantiateMap ();

		//Generate coast and harbor --Already generated---
		//generateHarbor();

		//generateFood ();
		//loadFood();
		// Because we set nothing for the treasure, he start to generate treasures infinitely
		//generateTreasure ();
		//loadTreasure();

		loadFoodAndTreasures (saveMap);

		// Add neighbours
		AddNeighboursToNodes ();
	}

	public void LaunchEditorMap() {
		//////////// POUR L'EDITEUR
		size = width * height;
		rand = new System.Random();
		GroupLand = new List<Node>();
		GroupListPossibleHarbor = new List<List<Node>>();
		worldCoord = new Vector3(0, 0, 0);
		mapFausse = false;
		regenerateCount = 1;
		worldCoordFood = new Vector3(0, 0, 0);
		worldCoordTreasure = new Vector3(0, 0, 0);

		// Init map
		InitializeMap();

		// INVOCATIOOOOON
		InstantiateMap ();

		// La nourriture
		generateFood ();

		// Touche finale
		AddNeighboursToNodes ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void GenerateLand(){
		abcisses = new List<int>();
		ordonnes = new List<int>();
		FirstStep = new List<Node>();
		
		for (int k = 0; k < nbCasesRemplinit; k++){
			int x = rand.Next(0, width);
			int y = rand.Next(0, height);
			abcisses.Add(x);
			ordonnes.Add(y);
		}

		// We stat to generate some land
        //Only sea now
		for (int a = 0; a < nbCasesRemplinit; a++){
			
			worldCoord = graph[abcisses[a], ordonnes[a]].worldPos;
			// UPDATE NODES
			Node node = graph [abcisses [a], ordonnes [a]];
			graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");

			List<Node> Neighbours = graph [node.x, node.y].getNodesNeighbours(graph);
			int nonRemplacable = rand.Next(0, Neighbours.Count);
			Neighbours.RemoveAt(nonRemplacable);
			FirstStep = Neighbours;

			for (int i = 0; i < FirstStep.Count; i++){
				int abs = FirstStep [i].x;
				int ord = FirstStep [i].y;
				if(graph[abs,ord].type.Equals("sea")){
					
					worldCoord = FirstStep[i].worldPos;
					// UPDATE NODES
					node = graph [abs, ord];
					graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");
				}
				List<Node> NextNeighbours = graph [node.x, node.y].getNodesNeighbours(graph);

				int k = 0;
				while (k < 2){
					if (NextNeighbours.Count > 0)	{
						int Nextnonremplacable = rand.Next(0, NextNeighbours.Count);
						NextNeighbours.RemoveAt(Nextnonremplacable);
					}
					k = k + 1;
				}
				for (int j = 0; j < NextNeighbours.Count; j++){
					int Nextabs = NextNeighbours[j].x;
					int Nextord = NextNeighbours[j].y;
					if(graph[Nextabs,Nextord].type.Equals("sea")){
						worldCoord = NextNeighbours[j].worldPos;

						// UPDATE NODES
						node = graph [Nextabs, Nextord];
						graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");

					}
				}
			}
		}
	}

    public void generateHarbor()
    {
        // We start to generate some harbor
        int idGroupLand = 0;
        for (int a = 0; a < nbCasesRemplinit; a++)
        {
            //Get the neighbours
            Node nodeHarbor = graph[abcisses[a], ordonnes[a]];

            //If we haven't see this land before
            if (nodeHarbor.idGroupLand == -1)
            {
                List<Node> ListPossibleHarbor = new List<Node>();
                GroupLand.Add(nodeHarbor);
                while (GroupLand.Count > 0)
                {
                    if (GroupLand[0].idGroupLand == -1)
                    {
                        List<Node> Neighbours = GroupLand[0].getLandNodesNeighbours(graph);
                        GroupLand.AddRange(Neighbours);
                        //Set the idGroupLand
                        GroupLand[0].idGroupLand = idGroupLand;

                        //Check if the node have sea neighbours
                        List<Node> SeaNeighbours = GroupLand[0].getSeaNodesNeighbours(graph);
                        //If yes we change the prefab
                        if (SeaNeighbours.Count != 0)
                        //put the SeaNeighbours.Count != 0 to SeaNeighbours.Count > 1 ?
                        {
                            //check if the sea isn't alone surrunded by land
                            bool okCoast = false;
                            for(int nbSea=0; nbSea < SeaNeighbours.Count;nbSea++)
                            {
                                if(SeaNeighbours[nbSea].tag)
                                {
                                    okCoast = true;
                                    break;
                                }
                            }
                            if (okCoast)
                            {
                                GameObject remplacable = GameObject.Find("Hex_" + GroupLand[0].x + "_" + GroupLand[0].y);
                                worldCoord = remplacable.transform.position;
                                remplacable.name = remplacable.name + "_trash";
                                Destroy(remplacable);
                                
                                GameObject hex_go = (GameObject)Instantiate(coastPrefab, worldCoord, Quaternion.identity);

                                hex_go.name = "Hex_" + GroupLand[0].x + "_" + GroupLand[0].y;
                                hex_go.GetComponent<Hex>().x = GroupLand[0].x;
                                hex_go.GetComponent<Hex>().y = GroupLand[0].y;
								hex_go.GetComponent<Land> ().IsCoast = true;
                                hex_go.transform.SetParent(this.transform);
                                hex_go.isStatic = true;

                                drawEdgesLines(hex_go);
                                //Add it to the list of possible harbor
                                if (GroupLand[0].getSeaNodesNeighbours(graph).Count() > 1)
                                {
                                    ListPossibleHarbor.Add(GroupLand[0]);
                                }
                            }
                        }
                    }
                    GroupLand.Remove(GroupLand[0]);
                }
                //Add the list of possible harbor to the grouplist of possible harbor
                GroupListPossibleHarbor.Add(ListPossibleHarbor);
                idGroupLand++;
            }
        }

        //idGroupLand-1 = number of island in the map !
        //Debug.Log(idGroupLand);

        /*Maybe later: if the number of possible harbor for a group of land is higher than the medium number of possible harbor we can create 2 harbor instead of 1
        int moy = 0;
        for (int test=0;test<idGroupLand;test++)
        {
            moy += GroupListPossibleHarbor[test].Count;
            Debug.Log("Groupe de terre "+test+" possede "+GroupListPossibleHarbor[test].Count+" plages (nb ports possibles)");
        }
        Debug.Log("Moyenne du nombre de ports possible par groupe de terre : " + moy / (idGroupLand - 1));
         */

        //Create an harbor for a group of land
        for (int island = 0; island < idGroupLand; island++)
        {
            int NodeHarbor = rand.Next(0, GroupListPossibleHarbor[island].Count);
            //Debug.Log("Ile : " + island + " port en x : " + GroupListPossibleHarbor[island][NodeHarbor].x + " en y : " + GroupListPossibleHarbor[island][NodeHarbor].y);
            
            //Destroy doesn't work !
            GameObject remplacable = GameObject.Find("Hex_" + GroupListPossibleHarbor[island][NodeHarbor].x + "_" + GroupListPossibleHarbor[island][NodeHarbor].y);
            worldCoord = remplacable.transform.position;
            remplacable.name = remplacable.name + "_trash";
            Destroy(remplacable);

            //Change this for the harbor prefab !
            GameObject hex_go = (GameObject)Instantiate(harborPrefab, worldCoord, Quaternion.identity);

            hex_go.name = "Hex_" + GroupListPossibleHarbor[island][NodeHarbor].x + "_" + GroupListPossibleHarbor[island][NodeHarbor].y;
            hex_go.GetComponent<Hex>().x = GroupListPossibleHarbor[island][NodeHarbor].x;
            hex_go.GetComponent<Hex>().y = GroupListPossibleHarbor[island][NodeHarbor].y;
            hex_go.transform.SetParent(this.transform);
            hex_go.isStatic = true;
            //ship_go.GetComponentInChildren<MeshRenderer>().material.color = owner.Color;
            drawEdgesLines(hex_go);
            //Update the graph
            graph[GroupListPossibleHarbor[island][NodeHarbor].x, GroupListPossibleHarbor[island][NodeHarbor].y] = new Node(GroupListPossibleHarbor[island][NodeHarbor].x, GroupListPossibleHarbor[island][NodeHarbor].y, GroupListPossibleHarbor[island][NodeHarbor].worldPos, false, "harbor");
        }
    }

    void InitializeMap(){
		graph = new Node[width, height];
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {

				// Use the loop for initialise de graph too, we save one loop with this
				float xPos = x * xOffset;

				// if we're an odd line, we need to reduce the offset by half
				if (y % 2 == 1) {
					xPos += xOffset / 2f;
				}

				Vector3 worldPosition = new Vector3 (xPos, 0, y * zOffset);
				graph [x, y] = new Node (x, y, worldPosition, true, "sea");
			}
		}
	}

	void InstantiateMap(){
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				string hexType = graph [x, y].type;
				GameObject hex_go = null;
				switch (hexType) {
				case "sea":
					hex_go = (GameObject)Instantiate (seaPrefab, graph [x, y].worldPos, Quaternion.identity);
					break;
				case "land":
					hex_go = (GameObject)Instantiate (landPrefab, graph [x, y].worldPos, Quaternion.identity);
					break;
				case "harbor":
					hex_go = (GameObject)Instantiate (harborPrefab, graph [x, y].worldPos, Quaternion.identity);
					break;
				}
				drawEdgesLines(hex_go);
				hex_go.name = "Hex_" + x + "_" + y;
				hex_go.GetComponent<Hex> ().x = x;
				hex_go.GetComponent<Hex> ().y = y;
				hex_go.transform.SetParent (this.transform);
				hex_go.isStatic = true;
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
				drawEdgesLines(hex_go);
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
		GroupSea = new List<Node>();
		GroupNeighbours = new List<Node>();
		bool verif = false;
		int compteur = 0;

		// We get the first sea we found
		for (int f = 0; f < width; f++) {
			for (int g = 0; g < height; g++) {
				if(graph[f,g].type.Equals("sea")){
					graph[f,g].tag = true;
					GroupSea.Add(graph[f,g]);
					verif = true;
					break;
				}
			}
			if (verif) {
				break;
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

		for (int l = 0; l < width; l++) {
			mapFausse = false;
			for (int m = 0; m < height; m++) {
				if (graph[l,m].type.Equals("sea") & !graph[l,m].tag){
					mapFausse = true;
					compteur++;
					if (compteur > 3 | (graph [l, m].x == 0 & graph [l, m].y == 0)) {
						break;
					}
				}
			}
			if (mapFausse & compteur > 3){
				break;
			}
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


	public void generateFood()
	{
		for (int absFood = 0; absFood < width; absFood++) {
			for (int ordFood = 0; ordFood < height; ordFood++) {
				if (graph [absFood, ordFood].type.Equals ("sea") & graph [absFood, ordFood].tag == true) {
					worldCoordFood = graph [absFood, ordFood].worldPos;
					GameObject caseFood = GameObject.Find ("Hex_" + absFood + "_" + ordFood);
					caseFood.GetComponent<Sea> ().FoodQuantity = rand.Next (foodQuantityMin, foodQuantityMax);
					//Instantiate (foodPrefab, worldCoordFood, Quaternion.identity);
				}
			}
		}
	}
	public void generateTreasure()
	{
		for (int c = 0; c < nombreCasesTreasure; c++) {
			int abs = rand.Next(0, width);
			int ord = rand.Next(0, height);
			if (graph [abs, ord].type.Equals ("sea") & graph [abs, ord].tag == true) {
				worldCoordTreasure = graph [abs, ord].worldPos;
				graph [abs, ord].isWalkable = false;
				GameObject caseTreasure = GameObject.Find ("Hex_" + abs + "_" + ord);
				GameObject tres = (GameObject) Instantiate (treasurePrefab, worldCoordTreasure, Quaternion.identity);
				tres.name = caseTreasure.name+"_Treasure"+rand.Next(0,1000000000);
				tres.transform.SetParent (caseTreasure.transform);
				caseTreasure.GetComponent<Sea> ().AddTreasure (rand.Next (tresorMin, tresorMax), tres);
			} else {
				c--;
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

				Debug.Log ("(i * this.height) + j=" + ((i * this.height) + j).ToString());
			}
		}


	}

}
