using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node {
	public int x;
	public int y;
	public List<Node> neighbours;

	public Node() {
		neighbours = new List<Node> ();
	}

	public float DistanceTo(Node n){
		if(n == null) {
			Debug.LogError("WTF?");
		}
		return Vector2.Distance(
			new Vector2(x, y),
			new Vector2(n.x, n.y)
		);
	}
}
