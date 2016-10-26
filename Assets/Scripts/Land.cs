//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Land : Hex
{
    // Use this for initialization
    void Start()
	{
		isWalkable = false;
		movementCost = Mathf.Infinity;
		type = "land";
    }

    // Update is called once per frame
    void Update()
    {

    }
}