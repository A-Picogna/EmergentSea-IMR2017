//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Filibuster : CrewMember
{

    // Use this for initialization
	public Filibuster() : base(){
		//Initialise the variables
		BaseAtk = 150;
		BaseLpMax = 200;
		LpMax = BaseLpMax;
		Lp = LpMax;
		Atk = BaseAtk;
		RecruitmentCost = 200;
		Type = 1;
    }
}