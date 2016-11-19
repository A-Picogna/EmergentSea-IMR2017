﻿//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Admiral : CrewMember
{

    // Use this for initialization
    public Admiral()
    {
        //Initialise the variables
        Lpmax = 300;
        Lp = Lpmax;
        EnergyQuantity = 300;
        Xp = 0;
        RecruitmentCost = 2000;
        Atk = 0;
    }
}