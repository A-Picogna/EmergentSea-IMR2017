//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Sea : Hex
{
    private int treasure, foodQuantity;
    // Use this for initialization
    void Start()
	{
		this.IsWalkable = true;
		this.movementCost = 1;

    }

    // Update is called once per frame
    void Update()
    {

	}

	public int Treasure
	{
		get { return treasure; }
		set { treasure = value; }
	}

    public int FoodQuantity
    {
        get { return foodQuantity; }
        set { foodQuantity = value; }
    }

    public void updateFoodQuantity()
    {
        //Set some formula to decrement the foodQuantity
        foodQuantity = 0;
    }
}