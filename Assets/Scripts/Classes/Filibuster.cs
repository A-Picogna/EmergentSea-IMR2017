//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Filibuster : CrewMember
{

    // Use this for initialization
    public Filibuster(){
        //Initialise the variables
		Lpmax = 200;
		Lp = Lpmax;
		EnergyQuantity = 100;
		Xp = 0;
		RecruitmentCost = 200;
		Atk = 150;
    }
}