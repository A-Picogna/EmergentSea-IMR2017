//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Harbor : Land
{
    private Player owner;
    private string ownerName;
    private float lpCost = 0.5f;
    private float changeFoodGold = 0.1f;

    // Use this for initialization
    void Start()
    {
        type = "harbor";
        owner = null;
        ownerName = null;
    }

    // Update is called once per frame
    void Update()
    {

    }

	public Player Owner
    {
        get { return owner; }
        set { owner = value; }
    }

    public string OwnerName
    {
        get { return ownerName; }
        set { ownerName = value; }
    }

    public void Interact(Ship selected, Node[,] graph)
    {
        //Check if we can iteract with the harbor
        if(CanUse(selected))
        {
            //Afficher panneau port

            //debug start
            var select = "shipyard";
            /*System.Random rand = new System.Random();
            foreach (CrewMember c in selected.Crew)
            {
                Debug.Log("Member HP start : " + c.Lp);
                int lpminus = rand.Next(0, 100);
                if(c.Lp-lpminus < 0)
                {
                    lpminus = c.Lp;
                }
                c.Lp -= lpminus;
                selected.Hp -= lpminus;
                Debug.Log("Member HP : "+c.Lp);             
            }
            Debug.Log("hp ship : "+selected.Hp);
            */
            selected.Gold += 0;
            selected.Food += 0;
            //debug end

            //Shipyard - new ship

            //Store - Selling food
            if (select == "store")
            {
                int price = (int)Mathf.Floor(selected.Food * changeFoodGold);
                selected.Gold += price;
                Debug.Log("Sold " + price / changeFoodGold + " of food for " + price + " gold");
                selected.Food = (int)(selected.Food - (price / changeFoodGold));
            }

            //Tavern - heal
            if (select == "tavern")
            {
                int lpMissing = 0;
                Debug.Log("gold amount : "+selected.Gold);
                int nbMemberNeeding = 0;
                foreach (CrewMember c in selected.Crew)
                {
                    if(c.Lpmax - c.Lp != 0)
                    {
                        nbMemberNeeding += 1;
                    }
                    lpMissing += c.Lpmax - c.Lp;
                }
                Debug.Log("lp missing : " + lpMissing);
                int total = 0;
                if (lpMissing != 0)
                {
                    int price = (int)Mathf.Ceil(lpMissing * lpCost);

                    //if not enough money we heal the most, else we heal full
                    if (selected.Gold < price)
                    {
                        Debug.Log("Not enough money to get a full heal");
                        lpMissing = (int)Mathf.Floor(selected.Gold / lpCost);
                    }

                    Debug.Log("Healed : " + lpMissing);
                    Debug.Log("Cost : " + lpMissing * lpCost);
                    selected.Gold -= (int)Mathf.Ceil(lpMissing * lpCost);
                    selected.Hp += lpMissing;

                    foreach (CrewMember c in selected.Crew)
                    {
                        if (c.Lpmax - c.Lp != 0)
                        {
                            int heal = c.Lpmax - c.Lp;
                            if (heal > lpMissing)
                            {
                                heal = lpMissing;
                            }
                            c.Lp += heal;
                            total += heal;
                            lpMissing -= heal;
                            Debug.Log("Member HP end : " + c.Lp);
                        }
                    }
                }
                else
                {
                    Debug.Log("No lp needed");
                }
                Debug.Log("Realy Healed : " + total);
                Debug.Log("Gold : " + selected.Gold);
                Debug.Log("Hp : "+selected.Hp);
            }

            //DarkArtAcademy - Conjurer
            if(select == "DAA")
            {
                Debug.Log("Nb member : "+selected.Crew.Count);
                Debug.Log("Hp : " + selected.Hp);
                Debug.Log("Gold : " + selected.Gold);
                //8 members max by ship
                if (selected.Crew.Count < 8)
                {
                    Conjurer conjurer = new Conjurer();
                    if(conjurer.RecruitmentCost > selected.Gold)
                    {
                        Debug.Log("Not enough money to recruit a conjurer");
                        conjurer = null;
                    }
                    else
                    {
                        selected.addCrewMember(conjurer);
                        selected.Gold -= conjurer.RecruitmentCost;
                        Debug.Log("Conjurer recuited");
                    }
                }
                else
                {
                    Debug.Log("Too many members in the crew");
                }
                Debug.Log("Nb member : " + selected.Crew.Count);
                Debug.Log("Hp : " + selected.Hp);
                Debug.Log("Gold : " + selected.Gold);
            }

            //Warehouse - PowderMonkey
            if (select == "warehouse")
            {
                Debug.Log("Nb member : " + selected.Crew.Count);
                Debug.Log("Hp : " + selected.Hp);
                Debug.Log("Gold : " + selected.Gold);
                //8 members max by ship
                if (selected.Crew.Count < 8)
                {
                    PowderMonkey PM = new PowderMonkey();
                    if (PM.RecruitmentCost > selected.Gold)
                    {
                        Debug.Log("Not enough money to recruit a PowderMonkey");
                        PM = null;
                    }
                    else
                    {
                        selected.addCrewMember(PM);
                        selected.Gold -= PM.RecruitmentCost;
                        Debug.Log("PowderMonkey recuited");
                    }
                }
                else
                {
                    Debug.Log("Too many members in the crew");
                }
                Debug.Log("Nb member : " + selected.Crew.Count);
                Debug.Log("Hp : " + selected.Hp);
                Debug.Log("Gold : " + selected.Gold);
            }

            //Shallows - Filibuster
            if (select == "shallows")
            {
                Debug.Log("Nb member : " + selected.Crew.Count);
                Debug.Log("Hp : " + selected.Hp);
                Debug.Log("Gold : " + selected.Gold);
                //8 members max by ship
                if (selected.Crew.Count < 8)
                {
                    Filibuster filibuster = new Filibuster();
                    if (filibuster.RecruitmentCost > selected.Gold)
                    {
                        Debug.Log("Not enough money to recruit a filibuster");
                        filibuster = null;
                    }
                    else
                    {
                        selected.addCrewMember(filibuster);
                        selected.Gold -= filibuster.RecruitmentCost;
                        Debug.Log("filibuster recuited");
                    }
                }
                else
                {
                    Debug.Log("Too many members in the crew");
                }
                Debug.Log("Nb member : " + selected.Crew.Count);
                Debug.Log("Hp : " + selected.Hp);
                Debug.Log("Gold : " + selected.Gold);
            }
        }
    }

    public bool CanUse(Ship selected)
    {
        //If a ship is selected
        if (selected != null)
        {
            float dist = Mathf.Abs(Vector3.Distance(transform.position, selected.transform.position));
            //if the harbor is near this selected ship
            if (dist < 1f)
            {
                if (ownerName != null)
                {
                    if (ownerName == selected.Owner.Name)
                    {
                        //Interract with the harbor
                        return true;
                    }
                    else
                    {
                        Debug.Log("This harbor doesn't belong to you");
                        return false;
                    }
                }
                //Taking harbor
                else
                {
                    Debug.Log("No owner, taking it...");
                    owner = selected.Owner;
                    ownerName = selected.Owner.Name;
                    return true;
                }
            }
            else
            {
                Debug.Log("Harbor to far away from you!");
                return false;
            }
        }
        else
        {
            if (ownerName != null)
            {
                Debug.Log("Owner : " + owner.Name);
            }
            else
            {
                Debug.Log("No owner");
            }
            return false;
        }
    }
}