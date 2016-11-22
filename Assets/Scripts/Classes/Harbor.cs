//@author: R.Baronnet

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Harbor : Land
{
    private Player owner;
    private string ownerName;
    private float lpCost = 0.5f;
    private float changeFoodGold = 0.1f;
    private int buildingTime = 2;
    private bool building;
    private int remainingBuildingTime;
    private GameObject buildingShip;
    private int buildingShipX;
    private int buildingShipY;
    private string buildingName;

    // Use this for initialization
    void Start()
    {
        type = "harbor";
        owner = null;
        ownerName = null;
        building = false;
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

    public bool Interact(Ship selected, Map map)
    {
        Node[,] graph = map.graph;
        //Check if we can iteract with the harbor
        if (CanUse(selected))
        {
            //Afficher panneau port
            return true;
        }
        else
        {
            return false;
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
                    owner.Harbors.Add(this);
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

    public void Build(Map map)
    {
        building = false;
        string shipName = buildingShip.name;
        buildingShip.name = buildingShip.name + "_trash";
        Destroy(buildingShip);
        
        GameObject ship_go = (GameObject)Instantiate(map.shipPrefab, map.graph[buildingShipX, buildingShipY].worldPos, Quaternion.identity);
        ship_go.name = shipName;
        ship_go.GetComponent<Ship>().ShipX = buildingShipX;
        ship_go.GetComponent<Ship>().ShipY = buildingShipY;
        ship_go.GetComponent<Ship>().ShipName = buildingName;
        ship_go.GetComponentInChildren<MeshRenderer>().material.color = owner.Color;
        Ship ship = ship_go.GetComponent<Ship>();
        ship.Owner = owner;
        owner.Fleet.Add(ship);
        GameObject.Find("Hex_" + buildingShipX + "_" + buildingShipY).GetComponent<Sea>().ShipContained = ship;
    }

    public bool Building
    {
        get { return building; }
        set { building = value; }
    }

    public int RemainingBuildingTime
    {
        get { return remainingBuildingTime; }
        set { remainingBuildingTime = value; }
    }

    public Harbor getHarbor()
    {
        return this;
    }

    public void doAction(Ship selected, Map map, string select)
    {
        Node[,] graph = map.graph;
        //Shipyard - new ship
        if (select == "shipyard")
        {
            if (!building)
            {
                Admiral admiral = new Admiral();
                if (admiral.RecruitmentCost > selected.Gold)
                {
                    Debug.Log("Not enough money to build a new ship");
                    selected.displayFloatingInfo(Color.magenta, "Not enough money !", selected.transform.position);
                    admiral = null;
                }
                else
                {
                    Node nodeHarbor = graph[this.x, this.y];
                    List<Node> Neighbours = nodeHarbor.getSeaNodesNeighbours(graph);
                    //Check if we can place the new ship
                    bool placeable = false;
                    foreach (Node node in Neighbours)
                    {
                        if (node.isWalkable)
                        {
                            placeable = true;
                            GameObject ship_go = (GameObject)Instantiate(map.foodPrefab, node.worldPos, Quaternion.identity);
                            ship_go.name = "Ship_" + owner.Name + "_" + owner.NbTotalShip;
                            buildingName = owner.Name + "_Ship_" + owner.NbTotalShip;
                            ship_go.GetComponentInChildren<MeshRenderer>().material.color = owner.Color;
                            owner.NbTotalShip++;
                            node.isWalkable = false;
                            building = true;
                            remainingBuildingTime = buildingTime;
                            buildingShip = ship_go;
                            buildingShipX = node.x;
                            buildingShipY = node.y;
                            break;
                        }
                    }
                    if (!placeable)
                    {
                        selected.displayFloatingInfo(Color.magenta, "No place to build a ship !", selected.transform.position);
                    }
                }
            }
            else
            {
                selected.displayFloatingInfo(Color.magenta, "Already building a ship !", selected.transform.position);
            }
        }

        //Store - Selling food
        if (select == "store")
        {
            int price = (int)Mathf.Floor(selected.Food * changeFoodGold);
            selected.Gold += price;
            if (price / changeFoodGold > 0)
            {
                Debug.Log("Sold " + price / changeFoodGold + " of food for " + price + " gold");
                selected.Food = (int)(selected.Food - (price / changeFoodGold));
                selected.displayFloatingInfo(Color.black, "-" + price / changeFoodGold, selected.transform.position);
                selected.displayFloatingInfo(Color.yellow, "+" + price, selected.transform.position);
            }
            else
            {
                Debug.Log("No food to sell !");
                selected.displayFloatingInfo(Color.magenta, "No food to sell !", selected.transform.position);
            }
        }

        //Tavern - heal
        if (select == "tavern")
        {
            int lpMissing = 0;
            Debug.Log("gold amount : " + selected.Gold);
            int nbMemberNeeding = 0;
            foreach (CrewMember c in selected.Crew)
            {
                if (c.Lpmax - c.Lp != 0)
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
                selected.displayFloatingInfo(Color.yellow, "-" + lpMissing * lpCost, selected.transform.position);
                selected.displayFloatingInfo(Color.green, "+" + lpMissing, selected.transform.position);
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
            Debug.Log("Hp : " + selected.Hp);
        }

        //DarkArtAcademy - Conjurer
        if (select == "daa")
        {
            Debug.Log("Nb member : " + selected.Crew.Count);
            Debug.Log("Hp : " + selected.Hp);
            Debug.Log("Gold : " + selected.Gold);
            //8 members max by ship
            if (selected.Crew.Count < 8)
            {
                Conjurer conjurer = new Conjurer();
                if (conjurer.RecruitmentCost > selected.Gold)
                {
                    Debug.Log("Not enough money to recruit a conjurer");
                    selected.displayFloatingInfo(Color.magenta, "Not enough money !", selected.transform.position);
                    conjurer = null;
                }
                else
                {
                    selected.addCrewMember(conjurer);
                    selected.Gold -= conjurer.RecruitmentCost;
                    selected.displayFloatingInfo(Color.yellow, "-" + conjurer.RecruitmentCost, selected.transform.position);
                    Debug.Log("Conjurer recuited");
                }
            }
            else
            {
                Debug.Log("Too many members in the crew");
                selected.displayFloatingInfo(Color.magenta, "Too many members in the ship !", selected.transform.position);
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
                    selected.displayFloatingInfo(Color.magenta, "Not enough money !", selected.transform.position);
                    PM = null;
                }
                else
                {
                    selected.addCrewMember(PM);
                    selected.Gold -= PM.RecruitmentCost;
                    Debug.Log("PowderMonkey recuited");
                    selected.displayFloatingInfo(Color.yellow, "-" + PM.RecruitmentCost, selected.transform.position);
                }
            }
            else
            {
                Debug.Log("Too many members in the crew");
                selected.displayFloatingInfo(Color.magenta, "Too many members in the ship !", selected.transform.position);
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
                    selected.displayFloatingInfo(Color.magenta, "Not enough money !", selected.transform.position);
                    filibuster = null;
                }
                else
                {
                    selected.addCrewMember(filibuster);
                    selected.Gold -= filibuster.RecruitmentCost;
                    Debug.Log("filibuster recuited");
                    selected.displayFloatingInfo(Color.yellow, "-" + filibuster.RecruitmentCost, selected.transform.position);
                }
            }
            else
            {
                Debug.Log("Too many members in the crew");
                selected.displayFloatingInfo(Color.magenta, "Too many members in the ship !", selected.transform.position);
            }
            Debug.Log("Nb member : " + selected.Crew.Count);
            Debug.Log("Hp : " + selected.Hp);
            Debug.Log("Gold : " + selected.Gold);
        }
    }
}