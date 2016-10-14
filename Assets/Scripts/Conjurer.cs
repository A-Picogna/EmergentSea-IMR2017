//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Conjurer : CrewMember
{

    // Use this for initialization
    void Start()
    {
        //Initialise the variables
		Lpmax = 50;
		Lp = Lpmax;
		EnergyQuantity = 50;
		Xp = 0;
		RecruitmentCost = 200;
		Atk = 90;
    }

    // Update is called once per frame
    void Update()
    {

    }
}