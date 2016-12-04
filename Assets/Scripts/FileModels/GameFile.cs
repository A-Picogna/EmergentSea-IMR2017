using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameFile
{
	public int turnNumber;
	public int currentPlayerNumber;
	public List<PlayerStruct> players;
}

