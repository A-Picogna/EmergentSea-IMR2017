// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MouseManager : MonoBehaviour {

	// GameObjects
	public Map map;
	public Ship selectedUnit;
	private GameObject ourHitObject;

	// Attributes
	public Vector2 mousePos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonUp (0) || Input.GetMouseButtonUp (1)) {
			RayCast ();
		}

		if (selectedUnit != null) {
			GameObject.Find ("Projector").transform.position = selectedUnit.transform.position + new Vector3 (0, 5f, 0);
		} else {
			GameObject.Find ("Projector").transform.position = new Vector3 (0, -5f, 0);
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
			if (Vector2.Distance (mousePos, Input.mousePosition) < 10f) {
				selectedUnit = null;
			}
		}

		if (Input.GetMouseButtonUp (1)) {
			if (selectedUnit != null && ourHitObject.GetComponent<Hex> ().IsWalkable) {
				Debug.Log (map.graph [ourHitObject.GetComponent<Hex> ().x, ourHitObject.GetComponent<Hex> ().y].isWalkable);
				int x = selectedUnit.GetComponent<Ship> ().ShipX;
				int y = selectedUnit.GetComponent<Ship> ().ShipY;
				bool pathExist = AstarPathfindingTo(ourHitObject.GetComponent<Hex> ().x, ourHitObject.GetComponent<Hex> ().y);
				if (pathExist) {
					map.graph [x, y].isWalkable = true;
				}
			}
		}

	}

	void MouseOver_Unit(GameObject ourHitObject) {
		//Debug.Log("Raycast hit: " + ourHitObject.name );
		if (Input.GetMouseButtonUp(0)) {
			selectedUnit = ourHitObject.GetComponent<Ship> ();
			GameObject.Find ("Projector").transform.position = selectedUnit.transform.position+new Vector3(0,5f,0);
		}
	}

	public bool AstarPathfindingTo(int x, int y){
		Node startNode = map.graph [
			selectedUnit.GetComponent<Ship> ().ShipX,
			selectedUnit.GetComponent<Ship> ().ShipY
		];
		Node targetNode = map.graph [
			x,
			y
		];
		Heap<Node> openSet = new Heap<Node>(map.Size);
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node node = openSet.RemoveFirst();
			closedSet.Add(node);

			if (node == targetNode) {
				RetracePath(startNode,targetNode);
				return true;
			}

			float newCostToNeighbour;
			foreach (Node neighbour in node.neighbours) {
				if (!neighbour.isWalkable || closedSet.Contains(neighbour)) {
					continue;
				}
				newCostToNeighbour = (node.gCost + node.DistanceTo(neighbour));
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = neighbour.DistanceTo (targetNode);
					neighbour.parent = node;

					if (!openSet.Contains (neighbour))
						openSet.Add (neighbour);
					else
						openSet.UpdateItem (neighbour);
				}
			}
		}
		return false;
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();
		selectedUnit.GetComponent<Ship> ().CurrentPath = path;
	}

}
