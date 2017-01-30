//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Sea : Hex {
	
	private int treasure = 0;
	private int foodQuantity = 0;
	private GameObject treasure_go = null;
	private Ship shipContained = null;

    // Use this for initialization
    void Awake()
	{
		isWalkable = true;
		movementCost = 1;
		type = "sea";
    }

    // Update is called once per frame
    void Update()
    {

	}

	public int Treasure{
		get { return treasure; }
		set { treasure = value; }
	}

	public int FoodQuantity{
		get { return foodQuantity; }
		set { foodQuantity = value; }
	}

	public Ship ShipContained{
		get { 
			return shipContained; 
		}
		set {
			if (shipContained == null) {
				shipContained = value;
				isWalkable = false;
			}
		}
	}

	// Remove the ship from the current hex
	public void RemoveShip(){
		shipContained = null;
		isWalkable = true;
	}

	// Remove the ship from the current hex
	public void RemoveTreasure(){
		GameObject map = GameObject.Find ("Map");
		if (map != null) {
			map.GetComponent<Map> ().graph [x, y].isWalkable = true;
		} else {
			map = GameObject.Find ("MapEditor");
			map.GetComponent<MapEditor> ().graph [x, y].isWalkable = true;
		}
		treasure_go = null;
		treasure = 0;
		isWalkable = true;
	}

	public void AddTreasure(int val, GameObject tres){
		treasure_go = tres;
		treasure = val;
		isWalkable = false;
	}

	public GameObject Treasure_go{
		get { return treasure_go; }
		set { treasure_go = value; }
	}

    public void updateFoodQuantity(){
        //Set some formula to decrement the foodQuantity
        foodQuantity = 0;
    }
}