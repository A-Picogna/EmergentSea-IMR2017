using UnityEngine;
using System.Collections;

[System.Serializable]
public class SaveFile
{
	public MapFile map;
	public MapFile startMap;
	public GameFile game;

	public SaveFile(MapFile map, GameFile game, MapFile startMap) {
		this.map = map;
		this.startMap = startMap;
		this.game = game;
	}
}

