
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
    Vector3 unitycord = new Vector3(0,0,0);

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

                // set the hex in a parent component, parent this hex to the map object
                hex_go.transform.SetParent(this.transform);

                hex_go.isStatic = true;

            }
        }

        for (int k=0; k<nbCasesRemplinit ;k++)
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
            hex_go.transform.SetParent(this.transform);
            hex_go.isStatic = true;
            List<GameObject> Neighbours = hex_go.GetComponent<Hex>().getNeighbours();
            
            var nonremplacable = rand.Next(0,Neighbours.Count);
            Neighbours.RemoveAt(nonremplacable);
            FirstStep = Neighbours;
            Debug.Log(FirstStep.Count);
            for (int i = 0; i < FirstStep.Count; i++)
            {
                //if (FirstStep[i] != LAAAND) { JE FAIS LA SUITE}
                var abs = FirstStep[i].GetComponent<Hex>().x;
                var ord = FirstStep[i].GetComponent<Hex>().y;
                unitycord = FirstStep[i].transform.position;
                Destroy(FirstStep[i]);
                GameObject land_go = (GameObject)Instantiate(landPrefab, unitycord, Quaternion.identity);
                land_go.name = "Hex_" + abs + "_" + ord;
                land_go.GetComponent<Land>().x = abs;
                land_go.GetComponent<Land>().y = ord;
                land_go.transform.SetParent(this.transform);
                land_go.isStatic = true;
                //NextNeigbours[i,] = land_go.GetComponent<Hex>().getNeighbours();
                //Debug.Log(NextNeigbours);
                List<GameObject> NextNeighbours = land_go.GetComponent<Hex>().getNeighbours();
                var k = 0;
                while (k < 3)
                {
                    var Nextnonremplacable = rand.Next(0, NextNeighbours.Count);
                    NextNeighbours.RemoveAt(Nextnonremplacable);
                    k = k + 1;
                }
                for (var j=0;j<NextNeighbours.Count;j++)
                {
                    //if (FirstStep[i] != LAAAND) { JE FAIS LA SUITE}
                    var Nextabs = NextNeighbours[j].GetComponent<Hex>().x;
                    var Nextord = NextNeighbours[j].GetComponent<Hex>().y;
                    unitycord = NextNeighbours[j].transform.position;
                    Destroy(NextNeighbours[j]);
                    GameObject Next_land_go = (GameObject)Instantiate(landPrefab, unitycord, Quaternion.identity);
                    Next_land_go.name = "Hex_" + abs + "_" + ord;
                    Next_land_go.GetComponent<Land>().x = abs;
                    Next_land_go.GetComponent<Land>().y = ord;
                    Next_land_go.transform.SetParent(this.transform);
                    Next_land_go.isStatic = true;
                }
            }
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

    // Update is called once per frame
    void Update()
    {

    }
}



