//@author: R.Baronnet

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
		RecruitmentCost = 0;
		Atk = 0;
		Type = 0;
    }
}