using UnityEngine;

[System.Serializable]
public class NodeStruct
{
	public int x;
	public int y;

	public bool isWalkable;
	public string type;

	public NodeStruct(int x, int y, bool isWalkable, string type) {
		this.x = x;
		this.y = y;

		this.isWalkable = isWalkable;
		this.type = type;
	}
}
