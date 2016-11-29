using UnityEngine;

[System.Serializable]
public class NodeStruct
{
	public int x;
	public int y;

	public bool isWalkable;
	public string type;

	public bool tag;

	public int SeaFood;
	public int SeaTreasure;

	public NodeStruct(int x, int y, bool isWalkable, string type, bool tag) {
		this.x = x;
		this.y = y;

		this.isWalkable = isWalkable;
		this.type = type;
		this.tag = tag;
	}
}
