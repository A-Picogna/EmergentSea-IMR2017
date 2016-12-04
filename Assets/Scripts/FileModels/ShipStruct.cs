using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ShipStruct
{
	public int food;
	public int gold;
	public int hp;
	public int energyQuantity;
	public int shipX;
	public int shipY;
	public int orientation;



	public bool playable;
	public bool isMoving;
	public string shipName;
	public List<CrewMember> crew = new List<CrewMember>();

}

