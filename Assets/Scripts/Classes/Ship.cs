﻿using UnityEngine;
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
	/*
	 * Orientation in degree
	 * 0 : right
	 * 60 : upper-right
	 * 120 : upper-left
	 * 180 : left
	 * 240 : lower-left
	 * 300 : lower-right
	 */
	private int orientation = 0;
	bool playable;
	private string shipName;
	private List<CrewMember> crew = new List<CrewMember>();
	public Vector3 destination;
	public GameObject fireBolt;

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
		transform.rotation = Quaternion.Euler(new Vector3(0, -orientation, 0));
	}

	public void MoveShipToNextHex(){
		// here we control the remaining energy quantity before moving
		if(currentPath==null)
			return;

		if(energyQuantity <= 0)
			return;
		
		if (currentPath.Count > 0) {
			Vector3 prevPos = transform.position;
			energyQuantity -= 1;
			shipX = currentPath [0].x;
			shipY = currentPath [0].y;
			currentPath.RemoveAt (0);
			destination = GameObject.Find ("Hex_" + shipX + "_" + shipY).transform.position;
			orientation = Angle360RoundToNeareast60(prevPos, destination);
		} else {
			currentPath = null;
		}
	}

	int Angle360RoundToNeareast60(Vector3 prev, Vector3 dest){		
		Vector3 toOther = (prev - dest).normalized;
		int angle = Mathf.RoundToInt((Mathf.Atan2(toOther.z, toOther.x) * Mathf.Rad2Deg + 180) / 60) * 60;
		if (angle == 360) {
			angle = 0;
		}
		return angle;
	}

	int Angle360(Vector3 prev, Vector3 dest){		
		Vector3 toOther = (prev - dest).normalized;
		int angle = Mathf.RoundToInt((Mathf.Atan2(toOther.z, toOther.x) * Mathf.Rad2Deg + 180));
		return angle;
	}


	public bool AtFilibusterRange(Ship target){
		float distance = Mathf.Abs (Vector3.Distance (transform.position, target.transform.position));
		// 1 Hex dist
		if (distance < 1f) {
			return true;
		} else {
			return false;
		}
	}

	public bool AtPowderMonkeyRange(Ship target){
		float distance = Mathf.Abs (Vector3.Distance (transform.position, target.transform.position));
		Debug.Log (distance);
		// 3 Hex dist
		if (distance > 2.9f) {
			return false;
		} else {
			bool result = false;
			Node[,] tmpGraph = GameObject.Find ("Map").GetComponent<Map> ().graph;
			Node originNode = tmpGraph [shipX, shipY];
			Node targetNode = tmpGraph [target.ShipX, target.ShipY];
			List<Node> neighbours = new List<Node> ();
			List<Node> neighbours2 = new List<Node> ();
			int angle = Angle360 (targetNode.worldPos, transform.position);
			// oriented toward right or left
			if (orientation == 0 || orientation == 180) {
				if (angle >= 60 && angle <= 120 || angle >= 240 && angle <= 300) {
					result = true;
				}
			}
			// oriented toward upper right or lower left
			if (orientation == 60 || orientation == 240) {
				if (angle >= 120 && angle <= 180 || angle >= 300 && angle <= 360) {
					result = true;
				}
			}
			// oriented toward upper left or lower right
			if (orientation == 120 || orientation == 300) {
				if (angle >= 0 && angle <= 60 || angle >= 180 && angle <= 240) {
					result = true;
				}
			}
			return result;
		}
	}

	public bool AtConjurerRange(Ship target){
		float distance = Mathf.Abs (Vector3.Distance (transform.position, target.transform.position));
		// 2 Hex dist
		if (distance < 2f) {
			return true;
		} else {
			return false;
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

	public int Orientation
	{
		get { return orientation; }
		set { orientation = value; }
	}

	// ====================
	// ====================



}