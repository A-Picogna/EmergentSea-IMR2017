using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Map_Romain : MonoBehaviour
{
    public GameObject hexPrefab;
    public GameObject landPrefab;
    public GameObject seaPrefab;
    //public GameObject harborPrefab;

    // Map in graph to calculate pathfinding
    public Node[,] graph;

    // size of map in terms of numer of hexagon
    public int width = 20;
    public int height = 20;
    float xOffset = 0.882f;
    float zOffset = 0.764f;
    int nbCasesRemplinit;
    System.Random rand;
    List<int> abcisses;
    List<int> ordonnes;
    List<GameObject> FirstStep;
    List<GameObject> GroupSea;
    List<GameObject> GroupNeighbours;
    List<Node> GroupLand;
    List<List<Node>> GroupListPossibleHarbor;
    Vector3 unitycord;
    GameObject land_go;
	bool mapFausse = false;
	int compt = 1;

    // Use this for initialization
    void Start()
	{
		nbCasesRemplinit = 10;
		rand = new System.Random();
		abcisses = new List<int>();
		ordonnes = new List<int>();
		FirstStep = new List<GameObject>();
		GroupSea = new List<GameObject>();
        GroupLand = new List<Node>();
        GroupListPossibleHarbor = new List<List<Node>>();
        GroupNeighbours = new List<GameObject>();
		unitycord = new Vector3(0, 0, 0);
		// RESPECT THIS STRIC ORDER
		// Init map
		initializeMap();
		// Generate some lands
		generateLand();
		// Add neighbours
		AddNeighboursToNodes ();
        generateHarbor();
		// Check if there is no sea prisonner
        verifMap();
		Debug.Log(mapFausse);

		/*if (mapFausse && compt >0) 
		{
			compt--;
			Debug.Log ("je vais etre changee");
            var children = new List<GameObject>();
            foreach (Transform child in this.transform)
            {
                children.Add(child.gameObject);
            }
            children.ForEach(child => Destroy(child));

            //Restart();
        }*/

    }
	void Restart()
	{
		Start();
	}

	void initializeMap(){
		graph = new Node[width, height];
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

    public void generateLand()
    {

        for (int k = 0; k < nbCasesRemplinit; k++)
        {
            int x = rand.Next(0, width);
            int y = rand.Next(0, height);
            abcisses.Add(x);
            ordonnes.Add(y);
        }


        // We stat to generate some land
        for (int a = 0; a < nbCasesRemplinit; a++)
        {
            GameObject remplacable = GameObject.Find("Hex_" + abcisses[a] + "_" + ordonnes[a]);
            unitycord = remplacable.transform.position;
            Destroy(remplacable);

            GameObject hex_go = (GameObject)Instantiate(landPrefab, unitycord, Quaternion.identity);

            // UPDATE NODES
            Node node = graph[abcisses[a], ordonnes[a]];
            graph[node.x, node.y] = new Node(node.x, node.y, node.worldPos, false, "land");

            hex_go.name = "Hex_" + abcisses[a] + "_" + ordonnes[a];
            hex_go.GetComponent<Hex>().x = abcisses[a];
            hex_go.GetComponent<Hex>().y = ordonnes[a];
            hex_go.transform.SetParent(this.transform);
            hex_go.isStatic = true;
            List<GameObject> Neighbours = hex_go.GetComponent<Hex>().getNeighbours();

            var nonremplacable = rand.Next(0, Neighbours.Count);
            Neighbours.RemoveAt(nonremplacable);
            FirstStep = Neighbours;

            for (int i = 0; i < FirstStep.Count; i++)
            {
                //(FirstStep[i].GetComponent<Hex>().Type.Equals("sea"))
                var abs = FirstStep[i].GetComponent<Hex>().x;
                var ord = FirstStep[i].GetComponent<Hex>().y;
                if (graph[abs, ord].type.Equals("sea"))
                {
                    unitycord = FirstStep[i].transform.position;
                    Destroy(FirstStep[i]);
                    land_go = (GameObject)Instantiate(landPrefab, unitycord, Quaternion.identity);
                    // UPDATE NODES
                    node = graph[abs, ord];
                    graph[node.x, node.y] = new Node(node.x, node.y, node.worldPos, false, "land");

                    land_go.name = "Hex_" + abs + "_" + ord;
                    land_go.GetComponent<Hex>().x = abs;
                    land_go.GetComponent<Hex>().y = ord;

                    land_go.transform.SetParent(this.transform);
                    land_go.isStatic = true;
                    //NextNeigbours[i,] = land_go.GetComponent<Hex>().getNeighbours();
                    //Debug.Log(NextNeigbours);
                }
                List<GameObject> NextNeighbours = land_go.GetComponent<Hex>().getNeighbours();

                var k = 0;
                while (k < 2)
                {
                    if (NextNeighbours != null)
                    {
                        var Nextnonremplacable = rand.Next(0, NextNeighbours.Count);
                        NextNeighbours.RemoveAt(Nextnonremplacable);
                    }
                    k = k + 1;
                }
                for (var j = 0; j < NextNeighbours.Count; j++)
                {
                    //if (FirstStep[i].GetComponent<Hex>().Type.Equals("sea"))
                    var Nextabs = NextNeighbours[j].GetComponent<Hex>().x;
                    var Nextord = NextNeighbours[j].GetComponent<Hex>().y;
                    if (graph[Nextabs, Nextord].type.Equals("sea"))
                    {

                        unitycord = NextNeighbours[j].transform.position;
                        Destroy(NextNeighbours[j]);
                        //Debug.Log(NextNeighbours[j].GetComponent<Hex>().gs_type);

                        GameObject Next_land_go = (GameObject)Instantiate(landPrefab, unitycord, Quaternion.identity);
                        // UPDATE NODES
                        node = graph[Nextabs, Nextord];
                        graph[node.x, node.y] = new Node(node.x, node.y, node.worldPos, false, "land");

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
                        {
                            //maybe check if the sea isn't alone surrunded by land (put the SeaNeighbours.Count != 0 to SeaNeighbours.Count > 1 ?)
                            //Destroy doesn't work !
                            GameObject remplacable = GameObject.Find("Hex_" + GroupLand[0].x + "_" + GroupLand[0].y);
                            unitycord = remplacable.transform.position;
                            Destroy(remplacable);

                            //Change this for the coast prefab !
                            GameObject hex_go = (GameObject)Instantiate(hexPrefab, unitycord, Quaternion.identity);

                            hex_go.name = "Hex_" + GroupLand[0].x + "_" + GroupLand[0].y;
                            hex_go.GetComponent<Hex>().x = GroupLand[0].x;
                            hex_go.GetComponent<Hex>().y = GroupLand[0].y;
                            hex_go.transform.SetParent(this.transform);
                            hex_go.isStatic = true;

                            //Add it to the list of possible harbor
                            ListPossibleHarbor.Add(GroupLand[0]);
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
        /*for (int island = 0; island < idGroupLand; island++)
        {
            int NodeHarbor = rand.Next(0, GroupListPossibleHarbor[island].Count);
            Debug.Log("Ile : " + island + " port en x : " + GroupListPossibleHarbor[island][NodeHarbor].x + " en y : " + GroupListPossibleHarbor[island][NodeHarbor].y);
            
            //Destroy doesn't work !
            GameObject remplacable = GameObject.Find("Hex_" + GroupListPossibleHarbor[island][NodeHarbor].x + "_" + GroupListPossibleHarbor[island][NodeHarbor].y);
            unitycord = remplacable.transform.position;
            Destroy(remplacable);

            //Change this for the harbor prefab !
            /*GameObject hex_go = (GameObject)Instantiate(hexPrefab, unitycord, Quaternion.identity);

            hex_go.name = "Hex_" + GroupListPossibleHarbor[island][NodeHarbor].x + "_" + GroupListPossibleHarbor[island][NodeHarbor].y;
            hex_go.GetComponent<Hex>().x = GroupListPossibleHarbor[island][NodeHarbor].x;
            hex_go.GetComponent<Hex>().y = GroupListPossibleHarbor[island][NodeHarbor].y;
            hex_go.transform.SetParent(this.transform);
            hex_go.isStatic = true;

            //Update the graph
            graph[GroupListPossibleHarbor[island][NodeHarbor].x, GroupListPossibleHarbor[island][NodeHarbor].y] = new Node(GroupListPossibleHarbor[island][NodeHarbor].x, GroupListPossibleHarbor[island][NodeHarbor].x, GroupListPossibleHarbor[island][NodeHarbor].worldPos, false, "harbor");*/
        //}
        //fin générationland
        //Vérification Map
    }

    public void verifMap()
    {
        var verif = false;
        var l = 0;
        var m = 0;
        var r = 0;

        while (verif != true && l < width)
        {
            m = 0;
            while (verif != true && m < height)
            {
                GameObject firstSea = GameObject.Find("Hex_" + l + "_" + m);
                //if (firstSea.GetComponent<Hex>().gs_type == "Sea")
				if(graph[l,m].type.Equals("sea"))
                {
                    Debug.Log("je suis une mer à true");
					graph[l,m].tag = true;
                    GroupSea.Add(firstSea);
                    verif = true;
                }
            }
        }
        for (r=0; r< GroupSea.Count; r++)
        {
            GroupNeighbours = GroupSea[r].GetComponent<Hex>().getNeighbours();
            for (var j = 0; j < GroupNeighbours.Count; j++)
            {
                //if (GroupNeighbours[j].GetComponent<Hex>().gs_type == "Sea" && GroupNeighbours[j].GetComponent<Hex>().gs_tag == false)
				int xx = GroupNeighbours[j].GetComponent<Hex>().x;
				int yy = GroupNeighbours [j].GetComponent<Hex>().y;
				if(graph[xx,yy].type.Equals("sea") & graph[xx,yy].tag==false)
                {
					//Debug.Log ("encore des mers à true !!!");
                    GroupSea.Add(GroupNeighbours[j]);
					graph[xx,yy].tag = true;
                }
            }
        }

        while (l < width)
        {
			mapFausse = false;
            m = 0;
            while (m < height)
            {
                //GameObject firstSea = GameObject.Find("Hex_" + l + "_" + m);
				//Debug.Log (graph [l, m].tag);
				if (graph[l,m].type.Equals("sea") & !graph[l,m].tag)
                {
					mapFausse = true;
					break;

                }
            m=m+1;
            }
			if (mapFausse) 
			{
				break;
			}
            l=l+1;

        }
    }

	// Add the neighbours to each node, for each node AFTER the land generation
	// This function is only usefull for pathfinding, do not use anywhere else !!!!!!!
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

}

//Notes romain :
//Lorsqu'on pop un bateau il faut verifier que la case de pop ai des cases adjacentes d'eau
//Node xxxxx.getSeaNodesNeighbours(graph) > 0