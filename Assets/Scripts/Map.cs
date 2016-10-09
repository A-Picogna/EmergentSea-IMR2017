// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public GameObject hexPrefab;

	// size of map in terms of numer of hexagon
	public int width = 20;
	public int height = 20;
	float xOffset = 0.882f;
	float zOffset = 0.764f;

	// Use this for initialization
	void Start () {
		
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

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
