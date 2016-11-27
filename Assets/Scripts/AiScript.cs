using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiScript {
    public string difficulty;
    private List<ArrayList> actions = new List<ArrayList>();
    private ArrayList aiGame;
    //private ArrayList foes = new ArrayList();
    private int shipPrice;
    private int filibusterPrice;
    private int conjurerPrice;
    private int powderMonkeyPrice;
    //Change the 2 next values, have to get them by an harbor
    private float lpCost; //0.5 => 2pv = 1gold, 2 => 2pv = 4gold
    private float changeFoodGold; //0.5 => 100 food sold = 50 gold, 2 => 100 food = 200 gold

    private bool notEnoughGold;

    public AiScript(/*string difficulty*/)
    {
        notEnoughGold = false;
        //Set the price
        Admiral a = new Admiral();
        shipPrice = a.RecruitmentCost;
        a = null;
        Filibuster f = new Filibuster();
        filibusterPrice = f.RecruitmentCost;
        f = null;
        Conjurer c = new Conjurer();
        conjurerPrice = c.RecruitmentCost;
        c = null;
        PowderMonkey p = new PowderMonkey();
        powderMonkeyPrice = p.RecruitmentCost;
        p = null;

        difficulty = "default";
        //Creating the list of difficulties
        ArrayList tmp0 = new ArrayList();
        tmp0.Add("default");
        actions.Add(tmp0);
        ArrayList tmp1 = new ArrayList();
        tmp1.Add("nbboat");
        tmp1.Add(3);
        tmp1.Add("nbcrew");
        tmp1.Add("each");
        tmp1.Add(1);
        tmp1.Add("gold");
        tmp1.Add(600);
        actions.Add(tmp1);

        for (int i = 0; i < actions.Count; i += 2)
        {
            if((string)actions[i][0] == difficulty)
            {
                aiGame = actions[i + 1];
            }
        }
    }

    public void turn(Player player)
    {
        Debug.Log("AI turn");
        int state = 0;

        while (state <= aiGame.Count)
        {
            //We want the ai to get at least X boats
            if ((string)aiGame[state] == "nbboat")
            {
                int nbBuilding = 0;
                foreach (Harbor harbor in player.Harbors)
                {
                    if (harbor.Building)
                    {
                        nbBuilding += 1;
                    }
                }
                //If the ai doesn't have this amount of boats
                if (player.Fleet.Count + nbBuilding < (int)aiGame[state + 1])
                {
                    //doAction
                }
                else
                {
                    state += 2;
                }
            }
            //We want the ai to get, by ship, at least X member ...
            else if ((string)aiGame[state] == "nbcrew")
            {
                // ... for each type of member
                if ((string)aiGame[state + 1] == "each")
                {
                    foreach (Ship ship in player.Fleet)
                    {
                        bool ok = true;
                        if (!ship.Used)
                        {
                            if (ship.NbConjurer < (int)aiGame[state + 2])
                            {
                                ok = false;
                            }
                            if (ship.NbFilibuster < (int)aiGame[state + 2])
                            {
                                ok = false;
                            }
                            if (ship.NbPowderMonkey < (int)aiGame[state + 2])
                            {
                                ok = false;
                            }
                        }
                        if (!ok)
                        {
                            ship.Used = true;
                            //doAction
                        }
                    }
                }
                // ... for the total crewmember amount
                else if ((string)aiGame[state + 1] == "all")
                {
                    foreach (Ship ship in player.Fleet)
                    {
                        if (!ship.Used)
                        {
                            if (ship.Crew.Count < (int)aiGame[state + 2])
                            {
                                ship.Used = true;
                                //doAction
                            }
                        }
                    }
                }
                // ... as filibuster
                else if ((string)aiGame[state + 1] == "filibuster")
                {
                    foreach (Ship ship in player.Fleet)
                    {
                        if (!ship.Used)
                        {
                            if (ship.NbFilibuster < (int)aiGame[state + 2])
                            {
                                ship.Used = true;
                                //doAction
                            }
                        }
                    }
                }
                // ... as conjurer
                else if ((string)aiGame[state + 1] == "conjurer")
                {
                    foreach (Ship ship in player.Fleet)
                    {
                        if (!ship.Used)
                        {
                            if (ship.NbConjurer < (int)aiGame[state + 2])
                            {
                                ship.Used = true;
                                //doAction
                            }
                        }
                    }
                }
                // ... as powdermonkey
                else if ((string)aiGame[state + 1] == "powdermonkey")
                {
                    foreach (Ship ship in player.Fleet)
                    {
                        if (!ship.Used)
                        {
                            if (ship.NbPowderMonkey < (int)aiGame[state + 2])
                            {
                                ship.Used = true;
                                //doAction
                            }
                        }
                    }
                }
                state += 3;
            }
            //We want the AI to get, by ship, at least X money (gold + gold conversion of food)
            else if((string)aiGame[state] == "gold")
            {
                foreach (Ship ship in player.Fleet)
                {
                    if (!ship.Used)
                    {
                        if(ship.Gold + (int)Mathf.Floor(ship.Food * changeFoodGold) < (int)aiGame[state + 1])
                        {
                            ship.Used = true;
                            //doAction
                        }
                    }
                }
                state += 2;
            }
            else
            {
                state += 1;
            }
        }

        //For all remaining ships
        foreach (Ship ship in player.Fleet)
        {
            if(!ship.Used)
            {
                //doAction
                ship.Used = true;
            }
        }
    }
}
