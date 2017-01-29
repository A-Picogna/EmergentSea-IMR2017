using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MapFile
{
	public int width, height;
	public NodeStruct[] graph;

	public bool boatPreset;
	public List<PlayerStruct> playerPreset;
}


