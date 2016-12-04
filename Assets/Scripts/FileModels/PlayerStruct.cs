using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerStruct
{
	public string type, name;
	public Color color;
	public List<ShipStruct> fleet;
	public List<HarborStruct> harbors;
	// public int nbTotalShip;
	public List<NodeStruct> exploredHex;

	public PlayerStruct(Player p) {
		this.type = p.Type;
		this.name= p.Name;
		this.color= p.Color;

		this.fleet=new List<ShipStruct>();
//TODO To init		
		this.harbors=new List<HarborStruct>();
		foreach (Harbor h in p.Harbors) {
			this.harbors.Add (h.Save());
		}
		this.exploredHex = new List<NodeStruct>();
		foreach(Node n in p.ExploredHex) {
			exploredHex.Add (new NodeStruct (n.x, n.y));
		}
	}

}

