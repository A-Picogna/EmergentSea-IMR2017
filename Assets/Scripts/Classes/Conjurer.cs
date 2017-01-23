//@author: R.Baronnet

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Conjurer : CrewMember
{

	public Conjurer() : base(){
		BaseAtk = 50;
		BaseLpMax = 50;
		LpMax = BaseLpMax;
		Lp = LpMax;
		Atk = BaseAtk;
		RecruitmentCost = 100;
		Type = 3;
    }
}