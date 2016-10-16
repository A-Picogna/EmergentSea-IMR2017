using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {

	private List<Node> currentPath = null;
	private int food, gold, lp, energyQuantity, shipX, shipY;
	private string shipName;
	private ArrayList crew = new ArrayList();
	public Vector3 destination;
	float speed = 2;

	// Use this for initialization
	void Start () {
		//There is always an amiral when the ship is construct so we create one and add it to the ship
		Admiral admiral = new Admiral();
		addCrewMember(admiral);

		//Let's make the ship start with 100 gold and food
		food = 100;
		gold = 100;

		//Let's put the ship's lp to 2000 if there is a lp notion for the ship
		//lp = 2000;
		destination = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 direction = destination - transform.position;
		Vector3 velocity = direction.normalized * speed * Time.deltaTime;
		velocity = Vector3.ClampMagnitude (velocity, direction.magnitude);
		transform.Translate (velocity);
	
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

	public int Lp
	{
		get { return lp; }
		set { lp = value; }
	}

	public int EnergyQuantity
	{
		get { return energyQuantity; }
		set { energyQuantity = value; }
	}

	public ArrayList Crew
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

	// ====================
	// ====================



}
