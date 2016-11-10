// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour {

	// GameObjects
	public Map map;
	public Ship selectedUnit;
	public Projector selectionCircle;
	public Pathfinder pathfinder;
	private GameObject ourHitObject;

	// Attributes
	public Vector2 mousePos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1)) {
			if (!EventSystem.current.IsPointerOverGameObject ()) {
				RayCast ();
			}
		}

		if (selectedUnit != null) {
			selectionCircle.transform.position = selectedUnit.transform.position + new Vector3 (0, 5f, 0);
		} else {
			selectionCircle.transform.position = new Vector3 (0, -5f, 0);
		}
	}

	void RayCast(){
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;

		if (Physics.Raycast (ray, out hitInfo)) {			
			GameObject ourHitObject = hitInfo.collider.transform.parent.gameObject;
			if (ourHitObject.GetComponent<Hex> () != null) {
				MouseOver_Hex (ourHitObject);
			} else if (ourHitObject.GetComponent<Ship> () != null) {
				MouseOver_Unit (ourHitObject);
			}
		} else {
			MouseOver_Nothing ();
		}
	}

	void MouseOver_Nothing(){
		if (Input.GetMouseButtonDown (0)) {
			mousePos = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0)) {
			if (Vector2.Distance (mousePos, Input.mousePosition) < 10f) {
				selectedUnit = null;
			}
		}
	}

	void MouseOver_Hex(GameObject ourHitObject) {
		if (Input.GetMouseButtonDown (0)) {
			mousePos = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0)) {
			if (ourHitObject.GetComponent<Sea> () != null && ourHitObject.GetComponent<Sea> ().ShipContained != null) {
				selectedUnit = ourHitObject.GetComponent<Sea> ().ShipContained;
				selectionCircle.transform.position = selectedUnit.transform.position+new Vector3(0,5f,0);
			} else {
				if (Vector2.Distance (mousePos, Input.mousePosition) < 10f) {
					selectedUnit = null;
				}
			}
		}

		if (Input.GetMouseButtonUp (1)) {
			if (ourHitObject.GetComponent<Sea> () != null && ourHitObject.GetComponent<Sea> ().ShipContained != null) {
				if (selectedUnit.AtFilibusterRange(ourHitObject.GetComponent<Sea> ().ShipContained)){
					Debug.Log ("Filibusters at range, ready to aboard !");
				}
				if (selectedUnit.AtConjurerRange(ourHitObject.GetComponent<Sea> ().ShipContained)){
					Debug.Log ("Conjurer at range, ready to cast !");
				}
				if (selectedUnit.AtPowderMonkeyRange(ourHitObject.GetComponent<Sea> ().ShipContained)){
					Debug.Log ("Canon at range, ready to fire !");
				}
			} else { 
				if (selectedUnit != null && selectedUnit.Playable) {
					pathfinder.PathRequest (selectedUnit, ourHitObject);
				}
			}
		}
	}

	void MouseOver_Unit(GameObject ourHitObject) {
		//Debug.Log("Raycast hit: " + ourHitObject.name );
		if (Input.GetMouseButtonUp(0)) {
			selectedUnit = ourHitObject.GetComponent<Ship> ();
			selectionCircle.transform.position = selectedUnit.transform.position+new Vector3(0,5f,0);
		}

		if (Input.GetMouseButtonUp (1)) {
			if (selectedUnit.AtFilibusterRange(ourHitObject.GetComponent<Ship> ())){
				Debug.Log ("Filibusters at range, ready to aboard !");
			}
			if (selectedUnit.AtConjurerRange(ourHitObject.GetComponent<Ship> ())){
				Debug.Log ("Conjurer at range, ready to cast !");
			}
			if (selectedUnit.AtPowderMonkeyRange(ourHitObject.GetComponent<Ship> ())){
				Debug.Log ("Canon at range, ready to fire !");
			}
		}
	}

}
