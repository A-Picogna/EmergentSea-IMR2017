//@author: R.Baronnet

using UnityEngine;
using System.Collections;

[System.Serializable]
public class Admiral : CrewMember
{

    // Use this for initialization
	public Admiral() : base(){
		//Initialise the variables
        //A changer pour la version finale et mettre baseEnergyQuantity à 5
		baseEnergyQuantity = 50;
		energyQuantity = baseEnergyQuantity;
		BaseAtk = 0;
		BaseLpMax = 300;
		LpMax = BaseLpMax;
		Lp = LpMax;
		Atk = BaseAtk;
		RecruitmentCost = 2000;
		Type = 0;
    }
}