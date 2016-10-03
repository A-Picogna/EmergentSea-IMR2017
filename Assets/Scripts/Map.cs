using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

	public GameObject hexPrefab;

	// size of map in terms of numer of hexagon
	int width = 20;
	int height = 20;
	float xOffset = 0.882f;
	float yOffset = 0.764f;

	// Use this for initialization
	void Start () {
		
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				
				float xPos = x * xOffset;

				if (y % 2 == 1) {
					xPos += xOffset/2f;
				}

				GameObject hex_go = (GameObject) Instantiate (hexPrefab, new Vector3 (xPos, y*yOffset, 0), Quaternion.identity);
				hex_go.name = "Hex_" + x + "_" + y;
				hex_go.transform.SetParent (this.transform);


			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
