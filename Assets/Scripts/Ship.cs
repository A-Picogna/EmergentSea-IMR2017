using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {

	private List<Node> currentPath = null;
	private int food;
	private int gold;
	private int hp;
	private int energyQuantity;
	private int shipX = -1;
	private int shipY = -1;
	bool playable;
	private string shipName;
	private List<CrewMember> crew = new List<CrewMember>();
	public Vector3 destination;

	// Use this for initialization
	void Start () {
		//There is always an amiral when the ship is construct so we create one and add it to the ship
		Admiral admiral = new Admiral();
		addCrewMember(admiral);

		//Let's make the ship start with 20 energy
		energyQuantity = 100000000;
		//Let's make the ship start with 100 gold and food
		food = 100;
		gold = 100;
		//Let's put the ship's hp to 2000 if there is a lp notion for the ship
		//hp = 2000;
		destination = transform.position;
	}

	// Update is called once per frame
	void Update () {		
		if (currentPath != null) {
			if (Vector3.Distance (transform.position, destination) < 0.1f) {
				MoveShipToNextHex ();
			}
		}
		transform.position = Vector3.Lerp(transform.position, destination, 5f * Time.deltaTime);
	}

	public void MoveShipToNextHex(){
		// here we control the remaining energy quantity before moving
		if(currentPath==null)
			return;

		if(energyQuantity <= 0)
			return;
		
		if (currentPath.Count > 0) {
			energyQuantity -= 1;
			shipX = currentPath [0].x;
			shipY = currentPath [0].y;
			currentPath.RemoveAt (0);
			destination = GameObject.Find ("Hex_" + shipX + "_" + shipY).transform.position;
		} else {
			currentPath = null;
		}
	}

	public int calculateEQmax()
	{
		int EQMax = 0;
		foreach(CrewMember member in crew)
		{
			EQMax += member.EnergyQuantity;
		}
		return EQMax;
	}

	public bool hoistTreasure()
	{
		//if enough energy
		if (energyQuantity >= 3)
		{
			//debug test of a treasure value of 100
			gold += 100;

			//to implement : remove treasure from the hex
			//decrement quantiteEnergie, to check, debug value
			energyQuantity -= 3;

			return true;
		}
		else
		{ return false; }
	}

	public void harborDock()
	{
		//toCheck
	}

	/*public void interagirBateau()
    {
        //toCheck
    }*/

	//updateOr, updateNourriture seems useless

	public bool fishing()
	{
		//if enough energy
		if(energyQuantity >= 5)
		{
			//debug test of fishing value of 100
			food += 100;

			//toImplement : modify foodQuantity of the hex
			//decrement energyQuantity, to check, debug value
			//Or set it to 0 to make the ship unable to move
			energyQuantity -= 5;

			return true;
		}
		else
		{ return false; }
	}

	public void addCrewMember(CrewMember member)
	{
		crew.Add(member);
	}

	public void removeCrewMember(CrewMember member)
	{
		crew.Remove(member);
	}





	// ====================
	// GETTER & SETTER 
	// ====================

	public int Food
	{
		get { return food; }
		set { food = value; }
	}

	public int Gold
	{
		get { return gold; }
		set { gold = value; }
	}

	public int Hp
	{
		get { return hp; }
		set { hp = value; }
	}

	public int EnergyQuantity
	{
		get { return energyQuantity; }
		set { energyQuantity = value; }
	}

	public List<CrewMember> Crew
	{
		get { return crew; }
		set { crew = value; }
	}

	public void moveShip(int x, int y)
	{
		//tocheck
	}

	public int ShipX
	{
		get { return shipX; }
		set { shipX = value; }
	}

	public int ShipY
	{
		get { return shipY; }
		set { shipY = value; }
	}

	public string ShipName
	{
		get { return shipName; }
		set { shipName = value; }
	}

	public List<Node> CurrentPath
	{
		get { return currentPath; }
		set { currentPath = value; }
	}

	public bool Playable
	{
		get { return playable; }
		set { playable = value; }
	}

	// ====================
	// ====================



}
