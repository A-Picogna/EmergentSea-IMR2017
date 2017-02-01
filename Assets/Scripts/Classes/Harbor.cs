//@author: R.Baronnet

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Harbor : Land
{
    public Player owner;
    public string ownerName;
    private float lpCost = 2f; //0.5 => 2pv = 1gold, 2 => 2pv = 4gold
    private float changeFoodGold = 0.1f; //0.5 => 100 food sold = 50 gold, 2 => 100 food = 200 gold
    private int buildingTime = 3;
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

	public void Load(HarborStruct h, Player p) {
		this.building = h.building;
		this.remainingBuildingTime = h.remainingBuildingTime;
		this.buildingShipX = h.buildingShipX;
		this.buildingShipY = h.buildingShipY;
		this.buildingName = h.buildingName;
		this.OwnerName = h.ownerName;
		this.owner = p;

		// Maj de la couleur du harbor
		this.GetComponentsInChildren<MeshRenderer>()[1].material.color = owner.Color;
	}


	public HarborStruct Save() {
		HarborStruct h = new HarborStruct ();

		h.building = this.building;
		h.remainingBuildingTime = this.remainingBuildingTime;
		h.buildingShipX = this.buildingShipX;
		h.buildingShipY = this.buildingShipY;
		h.buildingName = this.buildingName;
		h.ownerName = this.OwnerName;
		h.x = this.x;
		h.y = this.y;

		return h;
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
        if (CanUse(selected, map))
        {
            //Afficher panneau port
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanUse(Ship selected, Map map)
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
                    this.GetComponentsInChildren<MeshRenderer>()[1].material.color = owner.Color;
                    //Debug.Log(hex_go.GetComponentsInChildren<MeshRenderer>()[1].material.color = Color.red;);
                    return true;
                }
            }
            else
            {
                //Debug.Log("Harbor to far away from you!");
                return false;
            }
        }
        else
        {
            if (ownerName != null)
            {
                //Debug.Log("Owner : " + owner.Name);
            }
            else
            {
                //Debug.Log("No owner");
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
		ship.setRandomName ();
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

    public int doAction(Ship selected, Map map, string select)
	{
		int errorCode = 0;
		/*
		 * return error code on call
		 * 0 : OK
		 * 1 : Not Enough Money
		 * 2 : Ship is full
		 * 3 : No place to build a ship
		 * 4 : Already building a ship
		 * 5 : No food to sell
		 * 6 : crew already healed
		 */
        Node[,] graph = map.graph;
        //Shipyard - new ship
        if (select == "shipyard")
        {
            if (!building)
            {
                Admiral admiral = new Admiral();
                if (admiral.RecruitmentCost > selected.Gold)
                {
                    //Debug.Log("Not enough money to build a new ship");
					errorCode = 1;
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
                            selected.Gold -= admiral.RecruitmentCost;
                            break;
                        }
                    }
                    if (!placeable)
                    {
						errorCode = 3;
                    }
                }
            }
            else
            {
				errorCode = 4;
            }
        }

        //Store - Selling food
        if (select == "store")
        {
            int price = (int)Mathf.Floor(selected.Food * changeFoodGold);
            selected.Gold += price;
            if (price / changeFoodGold > 0)
            {
                //Debug.Log("Sold " + price / changeFoodGold + " of food for " + price + " gold");
                selected.Food = (int)(selected.Food - (price / changeFoodGold));
                selected.displayFloatingInfo(Color.black, "-" + price / changeFoodGold, selected.transform.position);
                selected.displayFloatingInfo(Color.yellow, "+" + price, selected.transform.position);
            }
            else
            {
                //Debug.Log("No food to sell !");
				errorCode = 5;
            }
        }

        //Tavern - heal
        /*if (select == "tavern")
        {
            int lpMissing = 0;
            //Debug.Log("gold amount : " + selected.Gold);
            int nbMemberNeeding = 0;
            foreach (CrewMember c in selected.Crew)
            {
                if (c.LpMax - c.Lp != 0)
                {
                    nbMemberNeeding += 1;
                }
                lpMissing += c.LpMax - c.Lp;
            }
            //Debug.Log("lp missing : " + lpMissing);
            int total = 0;
            if (lpMissing != 0)
            {
                int price = (int)Mathf.Ceil(lpMissing * lpCost);

                //if not enough money we heal the most, else we heal full
                if (selected.Gold < price)
                {
                    //Debug.Log("Not enough money to get a full heal");
                    lpMissing = (int)Mathf.Floor(selected.Gold / lpCost);
                }

                //Debug.Log("Healed : " + lpMissing);
                //Debug.Log("Cost : " + lpMissing * lpCost);
                selected.displayFloatingInfo(Color.yellow, "-" + lpMissing * lpCost, selected.transform.position);
                selected.displayFloatingInfo(Color.green, "+" + lpMissing, selected.transform.position);
                selected.Gold -= (int)Mathf.Ceil(lpMissing * lpCost);
                selected.Hp += lpMissing;

                foreach (CrewMember c in selected.Crew)
                {
                    if (c.LpMax - c.Lp != 0)
                    {
                        int heal = c.LpMax - c.Lp;
                        if (heal > lpMissing)
                        {
                            heal = lpMissing;
                        }
                        c.Lp += heal;
                        total += heal;
                        lpMissing -= heal;
                        //Debug.Log("Member HP end : " + c.Lp);
                    }
                }
            }
            else
			{
				errorCode = 6;
                //Debug.Log("No lp needed");
            }
            //Debug.Log("Realy Healed : " + total);
            //Debug.Log("Gold : " + selected.Gold);
            //Debug.Log("Hp : " + selected.Hp);
			// After healing, we update the canvas, and the value in the ship, and the displaying value upside the ship
			GameObject.Find ("HUDCanvas").GetComponent<PanelHandler> ().updateShip();
			selected.UpdateShipHp ();
			selected.DisplayHp (true);
        }*/

        //DarkArtAcademy - Conjurer
        if (select == "daa")
        {
            //Debug.Log("Nb member : " + selected.Crew.Count);
            //Debug.Log("Hp : " + selected.Hp);
            //Debug.Log("Gold : " + selected.Gold);
            //8 members max by ship
            if (selected.Crew.Count < 8)
            {
                Conjurer conjurer = new Conjurer();
                if (conjurer.RecruitmentCost > selected.Gold)
                {
					//Debug.Log("Not enough money to recruit a conjurer");
					errorCode = 1;
                    conjurer = null;
                }
                else
                {
                    selected.addCrewMember(conjurer);
                    selected.Gold -= conjurer.RecruitmentCost;
                    selected.displayFloatingInfo(Color.yellow, "-" + conjurer.RecruitmentCost, selected.transform.position);
                    //Debug.Log("Conjurer recuited");
                }
            }
            else
            {
                //Debug.Log("Too many members in the crew");
				errorCode = 2;
            }
            //Debug.Log("Nb member : " + selected.Crew.Count);
            //Debug.Log("Hp : " + selected.Hp);
            //Debug.Log("Gold : " + selected.Gold);
        }

        //Warehouse - PowderMonkey
        if (select == "warehouse")
        {
            //Debug.Log("Nb member : " + selected.Crew.Count);
            //Debug.Log("Hp : " + selected.Hp);
            //Debug.Log("Gold : " + selected.Gold);
            //8 members max by ship
            if (selected.Crew.Count < 8)
            {
                PowderMonkey PM = new PowderMonkey();
                if (PM.RecruitmentCost > selected.Gold)
                {
					//Debug.Log("Not enough money to recruit a PowderMonkey");
					errorCode = 1;
                    PM = null;
                }
                else
                {
                    selected.addCrewMember(PM);
                    selected.Gold -= PM.RecruitmentCost;
                    //Debug.Log("PowderMonkey recuited");
                    selected.displayFloatingInfo(Color.yellow, "-" + PM.RecruitmentCost, selected.transform.position);
                }
            }
            else
            {
				//Debug.Log("Too many members in the crew");
				errorCode = 2;
            }
            //Debug.Log("Nb member : " + selected.Crew.Count);
            //Debug.Log("Hp : " + selected.Hp);
            //Debug.Log("Gold : " + selected.Gold);
        }

        //Shallows - Filibuster
        if (select == "shallows")
        {
            //Debug.Log("Nb member : " + selected.Crew.Count);
            //Debug.Log("Hp : " + selected.Hp);
            //Debug.Log("Gold : " + selected.Gold);
            //8 members max by ship
            if (selected.Crew.Count < 8)
            {
                Filibuster filibuster = new Filibuster();
                if (filibuster.RecruitmentCost > selected.Gold)
                {
					//Debug.Log("Not enough money to recruit a filibuster");
					errorCode = 1;
                    filibuster = null;
                }
                else
                {
                    selected.addCrewMember(filibuster);
                    selected.Gold -= filibuster.RecruitmentCost;
                    //Debug.Log("filibuster recuited");
                    selected.displayFloatingInfo(Color.yellow, "-" + filibuster.RecruitmentCost, selected.transform.position);
                }
            }
            else
            {
				//Debug.Log("Too many members in the crew");
				errorCode = 2;
            }
            //Debug.Log("Nb member : " + selected.Crew.Count);
            //Debug.Log("Hp : " + selected.Hp);
            //Debug.Log("Gold : " + selected.Gold);
		}
        selected.UpdateShipHp();
        selected.DisplayHp(true);
        return errorCode;
    }
}