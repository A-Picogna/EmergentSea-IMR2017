// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MouseManager : MonoBehaviour {

	public Ship selectedUnit;
	public Map map;
	public Vector2 mousePos;

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
		if (selectedUnit != null) {
			GameObject.Find ("Projector").transform.position = selectedUnit.transform.position + new Vector3 (0, 5f, 0);
		} else {
			GameObject.Find ("Projector").transform.position = new Vector3 (0, -5f, 0);
		}

	}

	void MouseOver_Hex(GameObject ourHitObject) {
		//Debug.Log("Raycast hit: " + ourHitObject.name );
		if (Input.GetMouseButtonDown (0)) {
			mousePos = Input.mousePosition;
		}

		if (Input.GetMouseButtonUp (0)) {
			if (Vector2.Distance (mousePos, Input.mousePosition) < 10f) {
				selectedUnit = null;
			}
		}

		if (Input.GetMouseButton (1)) {
			// Check if we get the rights Neighbours
			GameObject[] Neighbours = ourHitObject.GetComponent<Hex> ().getNeighboursOld();
			for (int i = 0; i < Neighbours.Length; i++) {
				if (Neighbours[i] != null) {
					//Debug.Log (Neighbours[i].name);
				}
			}

			MeshRenderer mr = ourHitObject.GetComponentInChildren<MeshRenderer> ();
			/*
			if (mr.material.color == Color.red) {
				mr.material.color = Color.white;
			} else {
				mr.material.color = Color.red;
			}
			*/


			if (selectedUnit != null && ourHitObject.GetComponent<Hex> ().IsWalkable) {
				AstarPathfindingTo(ourHitObject.GetComponent<Hex> ().x, ourHitObject.GetComponent<Hex> ().y);
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

	float costToEnterTile(int x, int y){
		return GameObject.Find ("Hex_" + x + "_" + y).GetComponent<Hex>().movementCost;
	}

	public void AstarPathfindingTo(int x, int y){
		Node startNode = map.graph [
			selectedUnit.GetComponent<Ship> ().ShipX,
			selectedUnit.GetComponent<Ship> ().ShipY
		];
		Node targetNode = map.graph [
			x,
			y
		];
		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node node = openSet[0];
			for (int i = 1; i < openSet.Count; i ++) {
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
						node = openSet[i];
				}
			}

			openSet.Remove(node);
			closedSet.Add(node);

			if (node == targetNode) {
				RetracePath(startNode,targetNode);
				return;
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

					if (!openSet.Contains(neighbour))
						openSet.Add(neighbour);
				}
			}
		}
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

	/*
	public void DijkstraPathfindingTo(int x, int y){
		selectedUnit.GetComponent<Ship>().CurrentPath = null;
		List<Node> currentPath = null;

		Dictionary<Node, float> dist = new Dictionary<Node, float> ();
		Dictionary<Node, Node> prev = new Dictionary<Node, Node> ();

		List<Node> unvisited = new List<Node> ();

		Node source = map.graph [
			selectedUnit.GetComponent<Ship> ().ShipX,
			selectedUnit.GetComponent<Ship> ().ShipY
		];

		Node target = map.graph [
			x,
			y
		];
		dist [source] = 0;
		prev [source] = null;

		// Initialize everything with infinity distance
		foreach (Node v in map.graph) {
			if (v != source) {
				dist [v] = Mathf.Infinity;
				prev [v] = null;
			}
			unvisited.Add (v);
		}

		while (unvisited.Count > 0) {

			// u is going to be the unvisited node with the smallest distance
			Node u = null;

			foreach (Node possibleU in unvisited) {
				if (u == null || dist[possibleU] < dist[u]) {
					u = possibleU;
				}
			}

			if (u == target) {
				break; // Exit the loop!
			}

			unvisited.Remove (u);

			foreach(Node v in u.neighbours){
				//float alt = dist [u] + u.DistanceTo (v);
				float alt = dist [u] + costToEnterTile(v.x,v.y);
				if (alt < dist [v]) {
					dist [v] = alt;
					prev [v] = u;
				}
			}
		}

		// when we're here, we found the shortest route or there is no route at all

		if (prev [target] == null) {
			// No route!
			return;
		}

		currentPath = new List<Node> ();

		Node curr = target;

		while (curr != null) {
			currentPath.Add (curr);
			curr = prev [curr];
		}

		currentPath.Reverse ();

		selectedUnit.GetComponent<Ship>().CurrentPath = currentPath;
	}
	*/

}
