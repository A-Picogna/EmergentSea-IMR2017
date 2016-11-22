//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Conjurer : CrewMember
{

	public Conjurer() : base(){
		Lpmax = 50;
		Lp = Lpmax;
		RecruitmentCost = 200;
		Atk = 80;
		Type = 3;
    }
}