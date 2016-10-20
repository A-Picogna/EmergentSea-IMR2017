﻿
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
    List<GameObject> GroupLand = new List<GameObject>();
    List<GameObject> GroupNeighbours = new List<GameObject>();
    Vector3 unitycord = new Vector3(0, 0, 0);
    GameObject land_go;

    // Use this for initialization
    void Start()
    {

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
                GameObject hex_go = (GameObject)Instantiate(seaPrefab, new Vector3(xPos, 0, y * zOffset), Quaternion.identity);

                // Name the hex according to the grid coordinates
                hex_go.name = "Hex_" + x + "_" + y;

                // Store the grid coord in the hex itself
                hex_go.GetComponent<Sea>().x = x;
                hex_go.GetComponent<Sea>().y = y;

                //set the type of hexagon
                hex_go.GetComponent<Sea>().gs_type = "Sea";


                // set the hex in a parent component, parent this hex to the map object
                hex_go.transform.SetParent(this.transform);

                hex_go.isStatic = true;

            }
        }

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
            hex_go.name = "Hex_" + abcisses[a] + "_" + ordonnes[a];
            hex_go.GetComponent<Land>().x = abcisses[a];
            hex_go.GetComponent<Land>().y = ordonnes[a];
            hex_go.GetComponent<Land>().gs_type = "Land";
            hex_go.transform.SetParent(this.transform);
            hex_go.isStatic = true;
            List<GameObject> Neighbours = hex_go.GetComponent<Hex>().getNeighbours();

            var nonremplacable = rand.Next(0, Neighbours.Count);
            Neighbours.RemoveAt(nonremplacable);
            FirstStep = Neighbours;

            for (int i = 0; i < FirstStep.Count; i++)
            {
                if (FirstStep[i].GetComponent<Hex>().gs_type == "Sea")
                {

                    var abs = FirstStep[i].GetComponent<Hex>().x;
                    var ord = FirstStep[i].GetComponent<Hex>().y;
                    unitycord = FirstStep[i].transform.position;
                    Destroy(FirstStep[i]);
                    land_go = (GameObject)Instantiate(landPrefab, unitycord, Quaternion.identity);
                    land_go.name = "Hex_" + abs + "_" + ord;
                    land_go.GetComponent<Land>().x = abs;
                    land_go.GetComponent<Land>().y = ord;
                    land_go.GetComponent<Land>().gs_type = "Land";
                    land_go.transform.SetParent(this.transform);
                    land_go.isStatic = true;
                    //NextNeigbours[i,] = land_go.GetComponent<Hex>().getNeighbours();
                    //Debug.Log(NextNeigbours);
                }
                List<GameObject> NextNeighbours = land_go.GetComponent<Hex>().getNeighbours();

                    var k = 0;
                    while (k < 3)
                    {
                        if(NextNeighbours != null)
                        { 
                            var Nextnonremplacable = rand.Next(0, NextNeighbours.Count);
                            NextNeighbours.RemoveAt(Nextnonremplacable);
                        }
                        k = k + 1;
                    }
                    for (var j = 0; j < NextNeighbours.Count; j++)
                    {
                        if (FirstStep[i].GetComponent<Hex>().gs_type == "Sea")
                        {
                            var Nextabs = NextNeighbours[j].GetComponent<Hex>().x;
                            var Nextord = NextNeighbours[j].GetComponent<Hex>().y;
                            unitycord = NextNeighbours[j].transform.position;
                            Destroy(NextNeighbours[j]);
                            GameObject Next_land_go = (GameObject)Instantiate(landPrefab, unitycord, Quaternion.identity);
                            Next_land_go.name = "Hex_" + Nextabs + "_" + Nextord;
                            Next_land_go.GetComponent<Land>().x = Nextabs;
                            Next_land_go.GetComponent<Land>().y = Nextord;
                            hex_go.GetComponent<Land>().gs_type = "Land";
                            Next_land_go.transform.SetParent(this.transform);
                            Next_land_go.isStatic = true;
                        }
                    }
                
            }
            /*
            //Vérification Map
            var verif = false;
            var l = 0;
            var m = 0;
            while (verif != true && l < width)
            {
                m = 0;
                while (verif != true && m < height)
                {
                    //if hex l_m est un sea
                    //{
                    //GroupLand.Add(Hex(l_m));
                    //verif = true;
                    //hex(l_m).tag = true;
                    //}
                }
            }
            while (GroupLand.Count > 0)
            {

                //GroupNeighbours = GroupLand[0].GetComponent<Hex>().getNeighbours();

                for (var j = 0; j < GroupNeighbours.Count; j++)
                {
                    //if GroupNeighbours[j]==sea && tag ==false
                    //{
                    //GroupLand.add(GroupNeighbours[j]);
                    //GroupNeighbours[j].tag = true;
                    //}
                }

            }
            */



        }

        // Update is called once per frame
    }
}