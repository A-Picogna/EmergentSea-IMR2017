//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Filibuster : CrewMember
{

    // Use this for initialization
	public Filibuster() : base(){
        //Initialise the variables
		Lpmax = 200;
		Lp = Lpmax;
		RecruitmentCost = 200;
		Atk = 150;
		Type = 1;
    }
}