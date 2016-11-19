//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Conjurer : CrewMember
{

    public Conjurer(){
		Lpmax = 50;
		Lp = Lpmax;
		EnergyQuantity = 50;
		Xp = 0;
		RecruitmentCost = 200;
		Atk = 80;
    }
}