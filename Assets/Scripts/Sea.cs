//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Sea : Hex
{
	private int treasure, foodQuantity;
	public Ship shipContained;

    // Use this for initialization
    void Start()
	{
		isWalkable = true;
		movementCost = 1;
		type = "sea";
		shipContained = null;
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

    public void updateFoodQuantity(){
        //Set some formula to decrement the foodQuantity
        foodQuantity = 0;
    }
}