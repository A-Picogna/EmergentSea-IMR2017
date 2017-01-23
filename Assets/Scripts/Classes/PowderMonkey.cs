//@author: R.Baronnet

using UnityEngine;
using System.Collections;

[System.Serializable]
public class PowderMonkey : CrewMember
{

    // Use this for initialization
	public PowderMonkey() : base(){
		//Initialise the variables
		BaseAtk = 70;
		BaseLpMax = 100;
		LpMax = BaseLpMax;
		Lp = LpMax;
		Atk = BaseAtk;
		RecruitmentCost = 125;
		Type = 2;
    }
}