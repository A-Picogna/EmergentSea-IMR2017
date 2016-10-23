using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {
	public int x;
	public int y;
	public List<Node> neighbours;
	public float gCost;
	public float hCost;
	public bool walkable;
	public Node parent;

	public Node() {
		neighbours = new List<Node> ();
	}

	public float DistanceTo(Node n){
		return Vector2.Distance(
			new Vector2(x, y),
			new Vector2(n.x, n.y)
		);
	}

	public float fCost{
		get {
			return gCost + hCost;
		}
	}
}
