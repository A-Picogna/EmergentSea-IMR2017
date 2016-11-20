//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class PowderMonkey : CrewMember
{

    // Use this for initialization
    public PowderMonkey(){
        //Initialise the variables
		Lpmax = 100;
		Lp = Lpmax;
		EnergyQuantity = 60;
		Xp = 0;
		RecruitmentCost = 70;
		Atk = 100;
		Type = 2;
    }
}