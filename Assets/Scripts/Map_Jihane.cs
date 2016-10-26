
// @Author Jihane Ababou

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Map_Jihane : MonoBehaviour
{
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
    int nbCasesRemplinit = 10;
    System.Random rand = new System.Random();
    List<int> abcisses = new List<int>();
    List<int> ordonnes = new List<int>();
    List<GameObject> FirstStep = new List<GameObject>();
    List<GameObject> GroupSea = new List<GameObject>();
    List<GameObject> GroupNeighbours = new List<GameObject>();
    Vector3 unitycord = new Vector3(0, 0, 0);
    GameObject land_go;
	public bool mapfausse = false;

    // Use this for initialization
    void Start()
	{
		//mapfausse = false;
		initializeMap ();
        generateLand();

        verifMap();
		Debug.Log(mapfausse);
		/*
		while (mapfausse == true) 
		{
			generateLand();
			verifMap();
		}
		*/
    }

	void initializeMap(){
		graph = new Node[width, height];

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{

				float xPos = x * xOffset;

				// if we're an odd line, we need to reduce the offset by half
				if (y % 2 == 1)
				{
					xPos += xOffset / 2f;
				}

				// Creation of a new hex
				Vector3 worldPosition = new Vector3(xPos, 0, y * zOffset);
				GameObject hex_go = (GameObject)Instantiate(seaPrefab, worldPosition, Quaternion.identity);

				graph [x, y] = new Node (x, y, worldPosition, true, "sea");


				// Name the hex according to the grid coordinates
				hex_go.name = "Hex_" + x + "_" + y;

				// Store the grid coord in the hex itself
				hex_go.GetComponent<Hex>().x = x;
				hex_go.GetComponent<Hex>().y = y;

				// set the hex in a parent component, parent this hex to the map object
				hex_go.transform.SetParent(this.transform);

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
			Node node = graph [abcisses [a], ordonnes [a]];
			graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");

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
				if(graph[abs,ord].type.Equals("sea"))
                {
                    unitycord = FirstStep[i].transform.position;
                    Destroy(FirstStep[i]);
                    land_go = (GameObject)Instantiate(landPrefab, unitycord, Quaternion.identity);
					// UPDATE NODES
					node = graph [abs, ord];
					graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");

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
                while (k < 3)
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
					if(graph[Nextabs,Nextord].type.Equals("sea"))
                    {
                        
                        unitycord = NextNeighbours[j].transform.position;
                        Destroy(NextNeighbours[j]);
                        //Debug.Log(NextNeighbours[j].GetComponent<Hex>().gs_type);

                        GameObject Next_land_go = (GameObject)Instantiate(landPrefab, unitycord, Quaternion.identity);
						// UPDATE NODES
						node = graph [Nextabs, Nextord];
						graph [node.x, node.y] = new Node (node.x, node.y, node.worldPos, false, "land");

                        Next_land_go.name = "Hex_" + Nextabs + "_" + Nextord;
                        Next_land_go.GetComponent<Hex>().x = Nextabs;
                        Next_land_go.GetComponent<Hex>().y = Nextord;
                        

                        Next_land_go.transform.SetParent(this.transform);
                        Next_land_go.isStatic = true;

                    }
                    
                }

            }
            
        }
        
        
    }//fin générationland
    //Vérification Map

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
        Debug.Log(GroupSea.Count);

        while (l < width)
        {
			mapfausse = false;
            m = 0;
            while (m < height)
            {
                //GameObject firstSea = GameObject.Find("Hex_" + l + "_" + m);
				//Debug.Log (graph [l, m].tag);
				if (graph[l,m].type.Equals("sea") & !graph[l,m].tag)
                {
					mapfausse = true;
					break;

                }
            m=m+1;
            }
			if (mapfausse) 
			{
				break;
			}
            l=l+1;

        }
    }
    
}