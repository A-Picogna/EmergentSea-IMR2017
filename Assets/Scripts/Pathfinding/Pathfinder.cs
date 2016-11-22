using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Pathfinder : MonoBehaviour {

	// GameObjects
	public Map map;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	// We receive a path request form the mouse manager
	public void PathRequest(Ship selectedUnit, GameObject destinationObject){
		
		int x = selectedUnit.GetComponent<Ship> ().ShipX;
		int y = selectedUnit.GetComponent<Ship> ().ShipY;
		int destX = destinationObject.GetComponent<Hex> ().x;
		int destY = destinationObject.GetComponent<Hex> ().y;
		List<Node> prevPath = selectedUnit.CurrentPath;
		bool pathExist = AstarPathfindingTo (selectedUnit, destX, destY);

		if (selectedUnit.GetComponent<Ship> ().CurrentPath.Count <= selectedUnit.GetComponent<Ship> ().EnergyQuantity) {
			// If a path is found, we do something
			if (pathExist) {
				// if the unit was not moving, we free its spot and lock its destination spot
				if (prevPath == null || prevPath.Count == 0) {
					map.graph [x, y].isWalkable = true;
					GameObject.Find ("Hex_" + x + "_" + y).GetComponent<Sea> ().RemoveShip ();
					map.graph [destX, destY].isWalkable = false;
					GameObject.Find ("Hex_" + destX + "_" + destY).GetComponent<Sea> ().ShipContained = selectedUnit;
					// if the unit was moving, we free the previous destination spot and we lock the new one
				} else {
					map.graph [prevPath.Last ().x, prevPath.Last ().y].isWalkable = true;
					GameObject.Find ("Hex_" + prevPath.Last ().x + "_" + prevPath.Last ().y).GetComponent<Sea> ().RemoveShip ();
					map.graph [destX, destY].isWalkable = false;
					GameObject.Find ("Hex_" + destX + "_" + destY).GetComponent<Sea> ().ShipContained = selectedUnit;
				}
			}
		} else {
			selectedUnit.GetComponent<Ship> ().CurrentPath = null;
		}
	}

	public bool AstarPathfindingTo(Ship selectedUnit, int x, int y){
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
				RetracePath(startNode,targetNode, selectedUnit);
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

	void RetracePath(Node startNode, Node endNode, Ship selectedUnit) {
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
