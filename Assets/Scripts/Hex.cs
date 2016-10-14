using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour {

	// Coordinates in the grid (not unity unit)
	public int x;
	public int y;
	private string type;

	public GameObject[] getNeighbours(){

		GameObject[] Neighbours = new GameObject[6];
		GameObject leftNeighbour = GameObject.Find("Hex_" + (x-1) + "_" + y);
		GameObject rightNeighbour = GameObject.Find("Hex_" + (x+1) + "_" + y);
		Neighbours [0] = leftNeighbour;
		Neighbours [1] = rightNeighbour;

		if (y % 2 == 0) {
			GameObject upperLeftNeighbour = GameObject.Find ("Hex_" + (x-1) + "_" + (y+1));
			GameObject upperRightNeighbour = GameObject.Find ("Hex_" + x + "_" + (y+1));
			GameObject lowerLeftNeighbour = GameObject.Find ("Hex_" + (x-1) + "_" + (y-1));
			GameObject lowerRightNeighbour = GameObject.Find ("Hex_" + x + "_" + (y-1));
			Neighbours [2] = upperLeftNeighbour;
			Neighbours [3] = upperRightNeighbour;
			Neighbours [4] = lowerLeftNeighbour;
			Neighbours [5] = lowerRightNeighbour;
		} else {
			GameObject upperLeftNeighbour = GameObject.Find ("Hex_" + x + "_" + (y+1));
			GameObject upperRightNeighbour = GameObject.Find ("Hex_" + (x+1) + "_" + (y+1));
			GameObject lowerLeftNeighbour = GameObject.Find ("Hex_" + x + "_" + (y-1));
			GameObject lowerRightNeighbour = GameObject.Find ("Hex_" + (x+1) + "_" + (y-1));
			Neighbours [2] = upperLeftNeighbour;
			Neighbours [3] = upperRightNeighbour;
			Neighbours [4] = lowerLeftNeighbour;
			Neighbours [5] = lowerRightNeighbour;
		}

		return Neighbours;
	}

	public string gs_type
	{
		get { return type; }
		set { type = value; }
	}

}
