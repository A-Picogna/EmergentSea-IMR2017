//@author: R.Baronnet

using UnityEngine;
using System.Collections;

[System.Serializable]
public class CrewMember
{
	protected int baseLpMax,
	lp, 
	lpMax, 
	baseEnergyQuantity,
	energyQuantity,
	xp, 
	allxp,
	recruitmentCost, 
	baseAtk,
	atk, 
	type,
	xpMax,
	level,
	levelMax;

	public CrewMember(){
		baseEnergyQuantity = 2;
		energyQuantity = baseEnergyQuantity;
		xp = 0;
		level = 1;
		levelMax = 10;
	}

	public void gainXP(int value){
		if (level < levelMax) {
			xp += value;
			if (xp > 100) {
				level++;
				xp -= 100;
				int tmp = Mathf.RoundToInt (baseLpMax * (1f + (0.05f * (level - 1)) ) );
				lp = lp + (tmp - baseLpMax);
				lpMax = lpMax + (tmp - baseLpMax);

				atk = Mathf.RoundToInt (baseAtk * (1f + (0.05f * (level - 1)) ) );
				energyQuantity = Mathf.RoundToInt (baseEnergyQuantity * (1f + (0.05f * (level - 1)) ) );
			}
		}
	}

	public int XpMax
	{
		get { return xpMax; }
		set { xpMax = value; }
	}

	public int Level
	{
		get { return level; }
		set { level = value; }
	}

	public int LevelMax
	{
		get { return levelMax; }
		set { levelMax = value; }
	}

	public int Lp
	{
		get { return lp; }
		set { lp = value; }
	}

	public int BaseLpMax
	{
		get { return baseLpMax; }
		set { baseLpMax = value; }
	}

	public int LpMax
    {
		get { return lpMax; }
        set { lpMax = value; }
    }

	public int EnergyQuantity
    {
        get { return energyQuantity; }
        set { energyQuantity = value; }
    }

	public int Xp
	{
		get { return xp; }
		set { xp = value; }
	}

	public int Allxp
    {
		get { return allxp; }
		set { allxp = value; }
    }

	public int RecruitmentCost
    {
        get { return recruitmentCost; }
        set { recruitmentCost = value; }
	}

	public int Atk
	{
		get { return atk; }
		set { atk = value; }
	}

	public int BaseAtk
	{
		get { return baseAtk; }
		set { baseAtk = value; }
	}

	public int Type
	{
		get { return type; }
		set { type = value; }
	}
}