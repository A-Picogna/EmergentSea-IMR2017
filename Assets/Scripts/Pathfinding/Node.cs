using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {
	public int x;
	public int y;
	public float gCost;
	public float hCost;
	public bool isWalkable;
	public string type;
	public Node parent;
	public List<Node> neighbours;
	public Vector3 worldPos;

	public Node(int _x, int _y, Vector3 _worldPos, bool _isWalkable, string type) {
		neighbours = new List<Node> ();
		x = _x;
		y = _y;
		worldPos = _worldPos;
		isWalkable = _isWalkable;
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


}
