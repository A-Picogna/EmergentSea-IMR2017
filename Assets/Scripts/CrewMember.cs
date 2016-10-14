//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class CrewMember : MonoBehaviour
{
    private int lp, lpmax, energyQuantity, xp, recruitmentCost, atk;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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

    public void updateLp(int add) //is it usefull with the set fonction?
    {
        //add can be positive or negative
        lp = lp + add;
    }

    public void updateEnergyQuantity(int add) //is it usefull with the set fonction?
    {
        //add can be positive or negative
        energyQuantity = energyQuantity + add;
    }

    public void updateXp(int add) //is it usefull with the set fonction?
    {
        //add positive
        xp = xp + add;
    }
}