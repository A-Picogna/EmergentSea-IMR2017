//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Conjurer : CrewMember
{

	public Conjurer() : base(){
		BaseAtk = 80;
		BaseLpMax = 50;
		LpMax = BaseLpMax;
		Lp = LpMax;
		Atk = BaseAtk;
		RecruitmentCost = 150;
		Type = 3;
    }
}