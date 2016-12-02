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
	public Projector pathProjector;
	public Pathfinder pathfinder;
	private GameObject ourHitObject;
    public bool harbor = false;
    public Harbor currentHarbor;
	private GameManager gameManager;

	// UI
	public PanelHandler panelHandler;

	// Attributes
	public Vector2 mousePos;
	public Texture2D mainCursorTexture;
	public Texture2D attackCursorTexture;
	public Texture2D tradeCursorTexture;
	public Texture2D harborCursorTexture;
	public Texture2D fishingCursorTexture;
	public Texture2D treasureCursorTexture;

	// Use this for initialization
	void Start () {
		Cursor.SetCursor(mainCursorTexture, Vector2.zero, CursorMode.Auto);
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager> ();
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
		} else {
			Cursor.SetCursor(mainCursorTexture, Vector2.zero, CursorMode.Auto);
			panelHandler.hidePanelHelper();
			panelHandler.hidePanelHelper2();
			panelHandler.hidePanelHelper3();
		}

		if (selectedUnit != null) {
			selectionCircle.transform.position = selectedUnit.transform.position + new Vector3 (0, 5f, 0);
		} else {
			selectionCircle.transform.position = new Vector3 (0, -5f, 0);
			pathProjector.transform.position = new Vector3 (0, -5f, 0);
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
				Cursor.SetCursor(mainCursorTexture, Vector2.zero, CursorMode.Auto);
				panelHandler.hidePanelHelper();
				panelHandler.hidePanelHelper2();
				panelHandler.hidePanelHelper3();
			}
		} else {
			// if we are over nothing important
			MouseOver_NothingImportant ();
		}
	}

	void MouseOver_NothingImportant(){
		Cursor.SetCursor(mainCursorTexture, Vector2.zero, CursorMode.Auto);
		panelHandler.hidePanelHelper();
		panelHandler.hidePanelHelper2();
		panelHandler.hidePanelHelper3();
		pathProjector.transform.position = new Vector3 (0, -5f, 0);
		if (Input.GetMouseButtonUp (0)) {
			if (Vector2.Distance (mousePos, Input.mousePosition) < 10f) {
				selectedUnit = null;
				panelHandler.hideAllBottom ();
				panelHandler.hideAllModals ();
			}
		}
	}

	void MouseOver_HexUnit(GameObject ourHitObject){
		Ship target = ourHitObject.GetComponent<Sea> ().ShipContained;

		if (selectedUnit != null) {

			if (target.Owner.Name.Equals (gameManager.currentPlayer.Name)) {
				if (target == selectedUnit) {
					Cursor.SetCursor (fishingCursorTexture, new Vector2 (fishingCursorTexture.width / 2, fishingCursorTexture.height / 2), CursorMode.Auto);
					panelHandler.changeTextHelper (11);
					panelHandler.refreshHelper ();
					panelHandler.showPanelHelper ();
				} else {
					Cursor.SetCursor (tradeCursorTexture, new Vector2 (tradeCursorTexture.width / 2, tradeCursorTexture.height / 2), CursorMode.Auto);
					panelHandler.changeTextHelper (10);
					panelHandler.refreshHelper ();
					panelHandler.showPanelHelper ();
				}
			} else {
				Cursor.SetCursor (attackCursorTexture, new Vector2 (attackCursorTexture.width / 2, attackCursorTexture.height / 2), CursorMode.Auto);
				panelHandler.changeTextHelper (3);
				panelHandler.refreshHelper ();
				panelHandler.showPanelHelper ();
			}

			if (Input.GetMouseButtonUp (1)) {
				panelHandler.hideAllModals ();
				selectedUnit.Interact (target);
				panelHandler.hidePanelHarbor ();
			}
		} else {
			if (target.Owner.Name.Equals (gameManager.currentPlayer.Name)) {
				panelHandler.changeTextHelper (9);
				panelHandler.refreshHelper ();
				panelHandler.showPanelHelper ();
			} else {
				panelHandler.changeTextHelper (4);
				panelHandler.refreshHelper ();
				panelHandler.showPanelHelper ();
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			selectedUnit = ourHitObject.GetComponent<Sea> ().ShipContained;
			selectionCircle.transform.position = selectedUnit.transform.position+new Vector3(0,5f,0);
			panelHandler.updateShip ();
			panelHandler.hideAllModals ();
		}
	}

	void MouseOver_Treasure(GameObject ourHitObject){
		if (selectedUnit != null) {
			panelHandler.changeTextHelper (6);
			panelHandler.refreshHelper ();
			panelHandler.showPanelHelper ();
			Cursor.SetCursor (treasureCursorTexture, new Vector2 (treasureCursorTexture.width / 2, treasureCursorTexture.height / 2), CursorMode.Auto);
			if (Input.GetMouseButtonUp (1)) {
				Sea target = ourHitObject.GetComponent<Sea> ();
				selectedUnit.HoistTreasure (target);
				panelHandler.hidePanelHarbor ();
				panelHandler.hideAllModals ();
			}
		} else {
			panelHandler.changeTextHelper (5);
			panelHandler.refreshHelper ();
			panelHandler.showPanelHelper ();
		}
	}

	void MouseOver_Harbor(GameObject ourHitObject){
		if (selectedUnit != null) {
			Cursor.SetCursor (harborCursorTexture, new Vector2 (harborCursorTexture.width / 2, harborCursorTexture.height / 2), CursorMode.Auto);
			panelHandler.changeTextHelper (8);
			panelHandler.refreshHelper ();
			panelHandler.showPanelHelper ();
			if (Input.GetMouseButtonUp (1)) {
				panelHandler.hideAllModals ();
				harbor = ourHitObject.GetComponent<Harbor> ().Interact (selectedUnit, map);
				if (harbor) {
					currentHarbor = ourHitObject.GetComponent<Harbor> ().getHarbor ();
				}
			}
		} else {
			panelHandler.changeTextHelper (7);
			panelHandler.refreshHelper ();
			panelHandler.showPanelHelper ();
		}
	}

	void MouseOver_Unit(GameObject ourHitObject) {
		Ship target = ourHitObject.GetComponent<Ship> ();

		if (selectedUnit != null) {
			if (!target.Owner.Name.Equals (gameManager.currentPlayer.Name)) {
				panelHandler.changeTextHelper (3);
				panelHandler.refreshHelper ();
				panelHandler.showPanelHelper ();
			} else {
				panelHandler.hidePanelHelper ();
			}
		} else {
			if (!target.Owner.Name.Equals (gameManager.currentPlayer.Name)) {
				panelHandler.changeTextHelper (4);
				panelHandler.refreshHelper ();
				panelHandler.showPanelHelper ();
			} else {
				panelHandler.changeTextHelper (9);
				panelHandler.refreshHelper ();
				panelHandler.showPanelHelper ();
			}
		} 

		if (selectedUnit != null) {
			if (target.Owner.Name.Equals (gameManager.currentPlayer.Name)) {
				if (target == selectedUnit) {
					Cursor.SetCursor (fishingCursorTexture, new Vector2 (fishingCursorTexture.width / 2, fishingCursorTexture.height / 2), CursorMode.Auto);
					panelHandler.changeTextHelper (11);
					panelHandler.refreshHelper ();
					panelHandler.showPanelHelper ();
				} else {
					Cursor.SetCursor (tradeCursorTexture, new Vector2 (tradeCursorTexture.width / 2, tradeCursorTexture.height / 2), CursorMode.Auto);
					panelHandler.changeTextHelper (10);
					panelHandler.refreshHelper ();
					panelHandler.showPanelHelper ();
				}
			} else {
				Cursor.SetCursor (attackCursorTexture, new Vector2 (attackCursorTexture.width / 2, attackCursorTexture.height / 2), CursorMode.Auto);
				panelHandler.changeTextHelper (3);
				panelHandler.refreshHelper ();
				panelHandler.showPanelHelper ();
			}
			if (Input.GetMouseButtonUp (1)) {
				panelHandler.hideAllModals ();
				selectedUnit.Interact (target);
				panelHandler.hidePanelHarbor();
			}
		}
		if (Input.GetMouseButtonUp(0) && target.Owner.Name.Equals (gameManager.currentPlayer.Name)) {
			panelHandler.hideAllModals ();
			selectedUnit = target;
			selectionCircle.transform.position = selectedUnit.transform.position+new Vector3(0,5f,0);
			panelHandler.updateShip ();
		}
	}

	void MouseOver_VoidSea(GameObject ourHitObject) {

		Cursor.SetCursor(mainCursorTexture, Vector2.zero, CursorMode.Auto);
		panelHandler.hidePanelHelper();
		panelHandler.hidePanelHelper2();
		panelHandler.hidePanelHelper3();

		if (selectedUnit != null) {
			pathProjector.transform.position = ourHitObject.transform.position+new Vector3(0,5f,0);
		}

		if (Input.GetMouseButtonUp (1)) {
			if (selectedUnit != null && selectedUnit.Playable) {
				panelHandler.hideAllModals ();
				pathfinder.PathRequest (selectedUnit, ourHitObject);
				panelHandler.hidePanelHarbor();
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			if (Vector2.Distance (mousePos, Input.mousePosition) < 10f) {
				selectedUnit = null;
				panelHandler.hideAllModals ();
				panelHandler.hideAllBottom ();
			}
		}
	}

}
