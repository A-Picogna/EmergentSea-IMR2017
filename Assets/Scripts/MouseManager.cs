// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour {

	public Ship selectedUnit;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// WITH 3D OBJECTS :
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo)) {			
			GameObject ourHitObject = hitInfo.collider.transform.parent.gameObject;
			if (ourHitObject.GetComponent<Hex> () != null) {
				MouseOver_Hex (ourHitObject);
			} else if (ourHitObject.GetComponent<Ship> () != null) {
				MouseOver_Unit (ourHitObject);
			}
		}

	}

	void MouseOver_Hex(GameObject ourHitObject) {
		//Debug.Log("Raycast hit: " + ourHitObject.name );
		if (Input.GetMouseButtonDown(0)) {

			// Check if we get the rights Neighbours
			GameObject[] Neighbours = ourHitObject.GetComponent<Hex> ().getNeighboursOld();
			for (int i = 0; i < Neighbours.Length; i++) {
				if (Neighbours[i] != null) {
					Debug.Log (Neighbours[i].name);
				}
			}

			MeshRenderer mr = ourHitObject.GetComponentInChildren<MeshRenderer> ();
			if (mr.material.color == Color.red) {
				mr.material.color = Color.white;
			} else {
				mr.material.color = Color.red;
			}

			if (selectedUnit != null) {
				//selectedUnit.destination = DijkstraPathfindingTo(ourHitObject.transform.position.x, ourHitObject.transform.position.z);
			}
		}

	}

	void MouseOver_Unit(GameObject ourHitObject) {
		//Debug.Log("Raycast hit: " + ourHitObject.name );
		if (Input.GetMouseButtonDown(0)) {
			selectedUnit = ourHitObject.GetComponent<Ship> ();
		}
	}
}
