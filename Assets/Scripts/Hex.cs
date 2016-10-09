using UnityEngine;
using System.Collections;

public class Hex : MonoBehaviour {

	// Coordinates in the grid (not unity unit)
	public int x;
	public int y;

	public Hex[] getNeighbours(){
		
		GameObject leftNeighbour = GameObject.Find("Hex_" + (x-1) + "_" + y);
		GameObject rightNeighbour = GameObject.Find("Hex_" + (x+1) + "_" + y);

		if (y % 2 == 0) {
			GameObject upperLeftNeighbour = GameObject.Find ("Hex_" + (x-1) + "_" + (y+1));
			GameObject upperRightNeighbour = GameObject.Find ("Hex_" + x + "_" + (y+1));
			GameObject lowerLeftNeighbour = GameObject.Find ("Hex_" + (x-1) + "_" + (y-1));
			GameObject lowerRightNeighbour = GameObject.Find ("Hex_" + x + "_" + (y-1));
		} else {
			GameObject upperLeftNeighbour = GameObject.Find ("Hex_" + x + "_" + (y+1));
			GameObject upperRightNeighbour = GameObject.Find ("Hex_" + (x+1) + "_" + (y+1));
			GameObject lowerLeftNeighbour = GameObject.Find ("Hex_" + x + "_" + (y+1));
			GameObject lowerRightNeighbour = GameObject.Find ("Hex_" + (x+1) + "_" + (y-1));
		}

		return null;
	}

}
