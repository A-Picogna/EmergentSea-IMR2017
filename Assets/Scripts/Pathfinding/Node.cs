using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node : IHeapItem<Node>{
	
	public int x;
	public int y;
	public float gCost;
	public float hCost;
	public bool isWalkable;
	public string type;
	public Node parent;
	public List<Node> neighbours;
	public Vector3 worldPos;
	public bool tag;
	int heapIndex;

	public Node(int _x, int _y, Vector3 _worldPos, bool _isWalkable, string _type) {
		neighbours = new List<Node> ();
		x = _x;
		y = _y;
		worldPos = _worldPos;
		isWalkable = _isWalkable;
		type = _type;
		tag = false;
	}

	public float DistanceTo(Node n){
		return Vector2.Distance(
			new Vector2(worldPos.x, worldPos.z),
			new Vector2(n.worldPos.x, n.worldPos.z)
		);
	}

	public float fCost{
		get {
			return gCost + hCost;
		}
	}

	public List<Node> getNodesNeighbours(Node[,] graph){
		int width = graph.GetLength (0);
		int height = graph.GetLength (1);
		List<Node> neighbours = new List<Node>();
		if (x-1 >= 0) neighbours.Add (graph [x-1, y]);
		if (x+1 < width) neighbours.Add (graph [x+1, y]);
		if (y % 2 == 0) {
			if (x-1 >= 0 && y+1 < height) neighbours.Add (graph [x - 1, y + 1]);
			if (y+1 < height) neighbours.Add (graph [x, y+1]);
			if (x-1 >= 0 && y-1 >= 0) neighbours.Add (graph [x-1, y-1]);
			if (y-1 >= 0) neighbours.Add (graph [x, y-1]);
		} else {
			if (y+1 < height) neighbours.Add (graph [x, y+1]);
			if (x+1 < width && y+1 < height) neighbours.Add (graph [x+1, y+1]);
			if ( y-1 >= 0) neighbours.Add (graph [x, y-1]);
			if (x+1 < width && y-1 >= 0) neighbours.Add (graph [x+1, y-1]);
		}
		return neighbours;
	}

	public List<Node> getSeaNodesNeighbours(Node[,] graph){
		List<Node> neighbours = getNodesNeighbours(graph);
		List<Node> seaNeighbours = new List<Node>();
		foreach (Node node in neighbours) {
			// if it is a sea, we add it to a new list
			if (node.type == "sea") {
				seaNeighbours.Add (node);
			}
		}
		return seaNeighbours;
	}

	public List<Node> getLandNodesNeighbours(Node[,] graph){
		List<Node> neighbours = getNodesNeighbours(graph);
		List<Node> landNeighbours = new List<Node>();
		foreach (Node node in neighbours) {
			// if it is a land, we add it to a new list
			if (node.type == "land") {
				landNeighbours.Add (node);
			}
		}
		return landNeighbours;
	}

	public int HeapIndex{
		get{ return heapIndex; }
		set{heapIndex = value;}
	}

	public int CompareTo(Node nodeToCompare){
		int compare = fCost.CompareTo (nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo (nodeToCompare.hCost);
		}
		return -compare;
	}
}
