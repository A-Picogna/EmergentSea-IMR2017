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
    public bool harbor = false;
    public Harbor currentHarbor;

	// UI
	public PanelHandler panelHandler;

	// Attributes
	public Vector2 mousePos;
	public Texture2D mainCursorTexture;
	public Texture2D attackCursorTexture;
	public Texture2D interactCursorTexture;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(mainCursorTexture, Vector2.zero, CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {

		/*
		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1)) {
			if (!EventSystem.current.IsPointerOverGameObject ()) {
				RayCast ();
			}
		}*/

		if (!EventSystem.current.IsPointerOverGameObject ()) {
			RayCast ();
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
			if (Input.GetMouseButtonDown (0)) {
				mousePos = Input.mousePosition;
			}			
			GameObject ourHitObject = hitInfo.collider.transform.parent.gameObject;
			if (ourHitObject.GetComponent<Sea> () != null) {
				// If we are over a sea hex
				if (ourHitObject.GetComponent<Sea> ().ShipContained != null) {
					// If we are over a sea hex which contain a ship
					MouseOver_HexUnit (ourHitObject);
				} else if (ourHitObject.GetComponent<Sea> ().Treasure_go != null) {
					// If we are over a sea hex which contain a treasure
					MouseOver_Treasure (ourHitObject);
				} else {
					// If we are over a sea hex which contain a treasure
					MouseOver_VoidSea (ourHitObject);
				}
			} else if (ourHitObject.GetComponent<Harbor> () != null) {
				// if we are over an harbor hex
				MouseOver_Harbor (ourHitObject);
			} else if (ourHitObject.GetComponent<Ship> () != null) {
				// if we are over a unit
				MouseOver_Unit (ourHitObject);
			} else {
				// if we are over nothing important
				MouseOver_NothingImportant ();
			}
		}
	}

	void MouseOver_NothingImportant(){
		Cursor.SetCursor(mainCursorTexture, Vector2.zero, CursorMode.Auto);
		if (Input.GetMouseButtonUp (0)) {
			if (Vector2.Distance (mousePos, Input.mousePosition) < 10f) {
				selectedUnit = null;
				panelHandler.hideAllBottom ();
			}
		}
	}

	void MouseOver_HexUnit(GameObject ourHitObject){
		if (Input.GetMouseButtonUp (0)) {
			selectedUnit = ourHitObject.GetComponent<Sea> ().ShipContained;
			selectionCircle.transform.position = selectedUnit.transform.position+new Vector3(0,5f,0);
			panelHandler.updateShip ();
		}

		if (Input.GetMouseButtonUp (1)) {
			Ship target = ourHitObject.GetComponent<Sea>().ShipContained;
			selectedUnit.Interact(target);
			panelHandler.hidePanelHarbor();
		}
	}

	void MouseOver_Treasure(GameObject ourHitObject){
		if (Input.GetMouseButtonUp (1)) {
			Sea target = ourHitObject.GetComponent<Sea> ();
			selectedUnit.HoistTreasure (target);
			panelHandler.hidePanelHarbor();
		}
	}

	void MouseOver_Harbor(GameObject ourHitObject){
		if (Input.GetMouseButtonUp (1)) {
			harbor = ourHitObject.GetComponent<Harbor> ().Interact (selectedUnit, map);
			if (harbor) {
				currentHarbor = ourHitObject.GetComponent<Harbor> ().getHarbor ();
			}
		}
	}

	void MouseOver_Unit(GameObject ourHitObject) {
		Cursor.SetCursor(interactCursorTexture, Vector2.zero, CursorMode.Auto);
		//Debug.Log("Raycast hit: " + ourHitObject.name );
		if (Input.GetMouseButtonUp(0)) {
			selectedUnit = ourHitObject.GetComponent<Ship> ();
			selectionCircle.transform.position = selectedUnit.transform.position+new Vector3(0,5f,0);
			panelHandler.updateShip ();
		}

		if (Input.GetMouseButtonUp (1)) {
			Ship target = ourHitObject.GetComponent<Ship> ();
			selectedUnit.Interact (target);
			panelHandler.hidePanelHarbor();
		}
	}

	void MouseOver_VoidSea(GameObject ourHitObject) {

		Cursor.SetCursor(mainCursorTexture, Vector2.zero, CursorMode.Auto);

		if (Input.GetMouseButtonUp (1)) {
			if (selectedUnit != null && selectedUnit.Playable) {
				pathfinder.PathRequest (selectedUnit, ourHitObject);
				panelHandler.hidePanelHarbor();
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			if (Vector2.Distance (mousePos, Input.mousePosition) < 10f) {
				selectedUnit = null;
				panelHandler.hideAllBottom ();
			}
		}
	}

}
