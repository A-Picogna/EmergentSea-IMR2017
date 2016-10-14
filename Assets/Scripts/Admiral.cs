//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Admiral : CrewMember
{

    // Use this for initialization
    void Start()
    {
        //Initialise the variables
		Lpmax = 300;
		Lp = Lpmax;
		EnergyQuantity = 300;
		Xp = 0;
		RecruitmentCost = 0;
		Atk = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}