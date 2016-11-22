//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class CrewMember
{
	private int lp, 
	lpmax, 
	energyQuantity,
	xp, 
	recruitmentCost, 
	atk, 
	type,
	xpMax,
	level,
	levelMax;

	public CrewMember(){
		EnergyQuantity = 5;
		Xp = 0;
		XpMax = 100;
		Level = 1;
		LevelMax = 10;
	}

	public void gainXP(int value){
		xp += value;
		if (xp >= 100 && level < levelMax) {
			level++;
			xp = xp % 100;

			int tmp_lpmax = Mathf.CeilToInt (lpmax * 1.1f);
			lp = lp + (tmp_lpmax - lpmax);
			lpmax = tmp_lpmax;

			atk = Mathf.CeilToInt (atk * 1.1f);
			energyQuantity = Mathf.CeilToInt (energyQuantity * 1.1f);
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

	public int Lpmax
    {
        get { return lpmax; }
        set { lpmax = value; }
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

	public int Type
	{
		get { return type; }
		set { type = value; }
	}
}