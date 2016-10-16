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
		
		/* Is the mouse over a Unity UI Element?
		if(EventSystem.current.IsPointerOverGameObject()) {
			// It is, so let's not do any of our own custom
			// mouse stuff, because that would be weird.

			// NOTE!  We might want to ask the system WHAT KIND
			// of object we're over -- so for things that aren't
			// buttons, we might not actually want to bail out early.

			return;
		}*/

		// only work in orthographic mode :
		// Vector3 worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);

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
			GameObject[] Neighbours = ourHitObject.GetComponent<Hex> ().getNeighbours();
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
				selectedUnit.destination = ourHitObject.transform.position;
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
