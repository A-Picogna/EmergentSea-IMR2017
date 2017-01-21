//@author: R.Baronnet

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Conjurer : CrewMember
{

	public Conjurer() : base(){
		BaseAtk = 30;
		BaseLpMax = 50;
		LpMax = BaseLpMax;
		Lp = LpMax;
		Atk = BaseAtk;
		RecruitmentCost = 150;
		Type = 3;
    }
}