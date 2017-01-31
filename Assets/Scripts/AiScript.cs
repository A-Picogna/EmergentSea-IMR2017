using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

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

    private Ship movingShip;

    System.Random rand;

    public bool end;
<<<<<<< Updated upstream
    public InfoPanel panel_message;
=======
    public bool blocked;
>>>>>>> Stashed changes

    public AiScript(/*string difficulty*/)
    {
        blocked = false;
        end = false;
        rand = new System.Random();
        notEnoughGold = false;
        movingShip = null;
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
        /*tmp1.Add("nbship");
        tmp1.Add(3);
        tmp1.Add("nbcrew");
        tmp1.Add("each");
        tmp1.Add(1);
        tmp1.Add("gold");
        tmp1.Add(600);*/
        actions.Add(tmp1);

        for (int i = 0; i < actions.Count; i += 2)
        {
            if((string)actions[i][0] == difficulty)
            {
                aiGame = actions[i + 1];
            }
        }
    }

    public bool turn(Player player, Map map)
    {
        if (movingShip != null && movingShip.CurrentPath != null && movingShip.CurrentPath.Count > 0)
        {
            return true;
        }
        else if(movingShip != null && movingShip.TargetDistance == -1)
        {
            //if the ship still got enough energy, fishing
            movingShip.fishing();
            movingShip.Used = true;
            movingShip = null;
        }
        else
        {
            movingShip = null;
        }
        int state = 0;

        while (state < aiGame.Count)
        {
            //We want the ai to get at least X boats
            if ((string)aiGame[state] == "nbship")
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
                //Debug.Log(ship.TargetDistance);
                if (ship.TargetDistance != -1)
                {
                    //Debug.Log("Ship near");
                    goToTarget(ship, map);
                    movingShip = ship;
                    return true;
                }
                else
                {
                    explore(ship, map);
                    //Debug.Log(ship.CurrentPath.Count);
                    if (ship.CurrentPath.Count > 0)
                    {
                        movingShip = ship;
                        //Debug.Log(movingShip);
                        return true;
                    }
                }
            }
        }
        //End of AI turn
        end = true;
        panel_message = GameObject.Find("txt_genInfo").GetComponent<InfoPanel>();
        panel_message.DisplayInfo("A vous de jouer !", 1);

        return false;
    }

    public void explore(Ship ship, Map map)
    {
        //Debug.Log("exploring");
        bool ok2 = false;
        while (!ok2)
        {
            bool ok = false;
            while (!ok)
            {
                //Set a new direction
                if (ship.DirectionLifeTime == 0)
                {
                    ship.DirectionX = rand.Next(1, map.width);
                    ship.DirectionY = rand.Next(1, map.height);
                    while (map.graph[ship.DirectionX, ship.DirectionY].isWalkable == false)
                    {
                        ship.DirectionX = rand.Next(1, map.width);
                        ship.DirectionY = rand.Next(1, map.height);
                    }
                    ship.DirectionLifeTime = 10;
                }

                //Check if we are at the destination
                if (ship.DirectionX != ship.ShipX || ship.DirectionY != ship.ShipY)
                {
                    ok = true;
                }
                else
                {

                    ship.DirectionLifeTime = 0;
                }
            }

            //Move to destination
            Pathfinder pathfinder = GameObject.Find("Pathfinder").GetComponent<Pathfinder>();
            GameObject hex = GameObject.Find("Hex_" + ship.DirectionX + "_" + ship.DirectionY);
            pathfinder.PathRequest(ship, hex);
            if (ship.CurrentPath != null)
            {
                ok2 = true;
            }
            else
            {
                ship.DirectionLifeTime = 0;
            }
        }
        ship.DirectionLifeTime--;
    }

    public void goToTarget(Ship ship, Map map)
    {
        //Debug.Log("Check if atk is possible");
        //Debug.Log("Distance "+ship.TargetDistance);

        bool canAtk = false;
        Ship target = GameObject.Find("Hex_" + ship.TargetX + "_" + ship.TargetY).GetComponent<Sea>().ShipContained;

        if (target != null)
        {
            if (ship.CurrentPath != null)
            {
                if (ship.CurrentPath.Count > 0)
                {
                    //Put the original destination to walkable and the current place to unwalkable
                    map.graph[ship.CurrentPath[ship.CurrentPath.Count - 1].x, ship.CurrentPath[ship.CurrentPath.Count - 1].y].isWalkable = false;
                    GameObject.Find("Hex_" + ship.CurrentPath[ship.CurrentPath.Count - 1].x + "_" + ship.CurrentPath[ship.CurrentPath.Count - 1].y).GetComponent<Sea>().ShipContained = ship;

                    map.graph[ship.CurrentPath[ship.CurrentPath.Count - 1].x, ship.CurrentPath[ship.CurrentPath.Count - 1].y].isWalkable = true;
                    GameObject.Find("Hex_" + ship.CurrentPath[ship.CurrentPath.Count - 1].x + "_" + ship.CurrentPath[ship.CurrentPath.Count - 1].y).GetComponent<Sea>().RemoveShip();
                }
                ship.DirectionLifeTime = 0;
                ship.CurrentPath = null;
            }
            ship.Used = true;
            if (ship.AtConjurerRange(target))
            {
                canAtk = true;
            }
            if (ship.AtFilibusterRange(target))
            {
                canAtk = true;
            }
            if (ship.AtPowderMonkeyRange(target))
            {
                canAtk = true;
            }
            if (canAtk && ship.EnergyQuantity >= ship.AtkCost)
            {
                //Debug.Log("Attacking target");
                blocked = true;
                ship.Interact(target);
            }
            else if(ship.EnergyQuantity > 0 && ship.TargetDistance > 0)
            { //can still move
                int max = ship.EnergyQuantity;
                List<Node> bestPath = null;
                List<GameObject> neigh = GameObject.Find("Hex_"+ship.TargetX+"_"+ship.TargetY).GetComponent<Hex>().getNeighbours();
                Pathfinder pathfinder = GameObject.Find("Pathfinder").GetComponent<Pathfinder>();
                foreach (GameObject n in neigh)
                {
                    pathfinder.PathRequest(ship, n);
                    if (ship.CurrentPath != null && ship.CurrentPath.Count <= max)
                    {
                        bestPath = ship.CurrentPath;
                    }
                }
                if(bestPath != null)
                {
                    //Debug.Log("Going near the target");
                    ship.CurrentPath = bestPath;
                    ship.TargetDistance = -1;
                    ship.Used = false;
                }
                else
                {
                    //Debug.Log("Can't move or atk anymore");
                    ship.TargetDistance = -1;
                }
            }
            else
            {
                //Debug.Log("Can't move or atk anymore, nearest or no energy");
                ship.TargetDistance = -1;
            }
        }
        else
        {
            //Debug.Log("Nothing to attack");
            ship.TargetDistance = -1;
        }
    }

    public Ship MovingShip
    {
        get { return movingShip; }
        set { movingShip = value; }
    }
}
