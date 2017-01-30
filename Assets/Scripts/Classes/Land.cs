//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Land : Hex {
    // Use this for initialization
	private bool isCoast = false;

    void Awake()
	{
		isWalkable = false;
		movementCost = Mathf.Infinity;
		type = "land";
    }

    // Update is called once per frame
    void Update()
    {

    }

	public bool IsCoast {
		get {return isCoast;}
		set {isCoast = value;}
	}
}