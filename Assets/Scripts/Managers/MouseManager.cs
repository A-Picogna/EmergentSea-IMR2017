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
				panelHandler.hideAllBottom ();
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
				panelHandler.updateShip ();
			} else {
				if (Vector2.Distance (mousePos, Input.mousePosition) < 10f) {
					selectedUnit = null;
					panelHandler.hideAllBottom ();
				}
			}
		}

		if (Input.GetMouseButtonUp (1)) {
            if (ourHitObject.GetComponent<Harbor>() != null)
            {
                harbor = ourHitObject.GetComponent<Harbor>().Interact(selectedUnit, map);
                if (harbor)
                {
                    currentHarbor = ourHitObject.GetComponent<Harbor>().getHarbor();
                }
            }
            else
            {
                panelHandler.hidePanelHarbor();
                if (ourHitObject.GetComponent<Sea>() != null)
                {
                    if (ourHitObject.GetComponent<Sea>().ShipContained != null)
                    {
                        Ship target = ourHitObject.GetComponent<Sea>().ShipContained;
                        selectedUnit.Interact(target);
                    }
                    else
                    {
                        if (ourHitObject.GetComponent<Sea>().Treasure_go != null)
                        {
                            Sea target = ourHitObject.GetComponent<Sea>();
                            selectedUnit.HoistTreasure(target);
                        }
                        else
                        {
                            if (selectedUnit != null && selectedUnit.Playable)
                            {
                                pathfinder.PathRequest(selectedUnit, ourHitObject);
                            }
                        }
                    }
                }
            }
		}
	}

	void MouseOver_Unit(GameObject ourHitObject) {
		//Debug.Log("Raycast hit: " + ourHitObject.name );
		if (Input.GetMouseButtonUp(0)) {
			selectedUnit = ourHitObject.GetComponent<Ship> ();
			selectionCircle.transform.position = selectedUnit.transform.position+new Vector3(0,5f,0);
			panelHandler.updateShip ();
		}

		if (Input.GetMouseButtonUp (1)) {
			Ship target = ourHitObject.GetComponent<Ship> ();
			selectedUnit.Interact (target);
		}
	}

}
