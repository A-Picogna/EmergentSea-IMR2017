using UnityEngine;
using System.Collections;

[System.Serializable]
public class SaveFile
{
	public MapFile map;

	public SaveFile(MapFile map) {
		this.map = map;
	}
}

