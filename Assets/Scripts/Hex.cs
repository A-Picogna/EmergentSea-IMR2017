using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hex : MonoBehaviour
{

    // Coordinates in the grid (not unity unit)
    public int x;
    public int y;
    private string type;
	public float movementCost;
	public bool isWalkable;
    private bool tag = false;

    public GameObject[] getNeighboursOld(){
        GameObject[] Neighbours = new GameObject[6];
        GameObject leftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + y);
        GameObject rightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + y);
        Neighbours[0] = leftNeighbour;
        Neighbours[1] = rightNeighbour;

        if (y % 2 == 0){
            GameObject upperLeftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + (y + 1));
            GameObject upperRightNeighbour = GameObject.Find("Hex_" + x + "_" + (y + 1));
            GameObject lowerLeftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + (y - 1));
            GameObject lowerRightNeighbour = GameObject.Find("Hex_" + x + "_" + (y - 1));
            Neighbours[2] = upperLeftNeighbour;
            Neighbours[3] = upperRightNeighbour;
            Neighbours[4] = lowerLeftNeighbour;
            Neighbours[5] = lowerRightNeighbour;
        }
        else{
            GameObject upperLeftNeighbour = GameObject.Find("Hex_" + x + "_" + (y + 1));
            GameObject upperRightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + (y + 1));
            GameObject lowerLeftNeighbour = GameObject.Find("Hex_" + x + "_" + (y - 1));
            GameObject lowerRightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + (y - 1));
            Neighbours[2] = upperLeftNeighbour;
            Neighbours[3] = upperRightNeighbour;
            Neighbours[4] = lowerLeftNeighbour;
            Neighbours[5] = lowerRightNeighbour;
        }

        return Neighbours;
    }

    public List<GameObject> getNeighbours(){
        List<GameObject> Neighbours = new List<GameObject>();
        GameObject leftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + y);
        GameObject rightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + y);
        if (leftNeighbour != null) { Neighbours.Add(leftNeighbour); }
        if (rightNeighbour != null) { Neighbours.Add(rightNeighbour); }

        if (y % 2 == 0)
        {
            GameObject upperLeftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + (y + 1));
            GameObject upperRightNeighbour = GameObject.Find("Hex_" + x + "_" + (y + 1));
            GameObject lowerLeftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + (y - 1));
            GameObject lowerRightNeighbour = GameObject.Find("Hex_" + x + "_" + (y - 1));
            if (upperLeftNeighbour != null) { Neighbours.Add(upperLeftNeighbour); }
            if (upperRightNeighbour != null) { Neighbours.Add(upperRightNeighbour); }
            if (lowerLeftNeighbour != null) { Neighbours.Add(lowerLeftNeighbour); }
            if (lowerRightNeighbour != null) { Neighbours.Add(lowerRightNeighbour); }
        }
        else
        {
            GameObject upperLeftNeighbour = GameObject.Find("Hex_" + x + "_" + (y + 1));
            GameObject upperRightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + (y + 1));
            GameObject lowerLeftNeighbour = GameObject.Find("Hex_" + x + "_" + (y - 1));
            GameObject lowerRightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + (y - 1));
            if (upperLeftNeighbour != null) { Neighbours.Add(upperLeftNeighbour); }
            if (upperRightNeighbour != null) { Neighbours.Add(upperRightNeighbour); }
            if (lowerLeftNeighbour != null) { Neighbours.Add(lowerLeftNeighbour); }
            if (lowerRightNeighbour != null) { Neighbours.Add(lowerRightNeighbour); }
        }

        return Neighbours;
    }

    public string gs_type{
        get { return type; }
        set { type = value; }
    }

	public bool gs_tag{
		get { return tag; }
		set { tag = value; }
	}    

	public bool IsWalkable{
		get { return isWalkable; }
		set { isWalkable = value; }
	}

}
