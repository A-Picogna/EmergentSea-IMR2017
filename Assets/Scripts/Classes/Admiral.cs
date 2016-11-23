//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Admiral : CrewMember
{

    // Use this for initialization
    public Admiral(){
		//Initialise the variables
		BaseAtk = 0;
		BaseLpMax = 300;
		LpMax = BaseLpMax;
		Lp = LpMax;
		Atk = BaseAtk;
		RecruitmentCost = 2000;
		Type = 0;
    }
}