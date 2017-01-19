using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {

	private List<Node> currentPath = null;
	private int food = 0;
	private int gold = 200;
	private int energyQuantity = 0;
	private int shipX = -1;
	private int shipY = -1;
	private int orientation = 0;
	private Player owner;
	private int atkCost = 5;
	private int hp = 0;
	private string textHp = "";
	/*
	 * Orientation in degree
	 * 0 : right
	 * 60 : upper-right
	 * 120 : upper-left
	 * 180 : left
	 * 240 : lower-left
	 * 300 : lower-right
	 */
	bool playable;
	bool isMoving;
	bool dead = false;
	private string shipName;
	private List<CrewMember> crew = new List<CrewMember>();
	private PanelHandler panelHandler;
	public Vector3 destination;
	public AudioClip shipMovingSound;
	System.Random rand;

    //AI
    private bool used = false;
    private int nbFilibuster = 0;
    private int nbConjurer = 0;
    private int nbPowderMonkey = 0;
    private int directionX;
    private int directionY;
    private int directionLifeTime;
    private int targetX;
    private int targetY;
    private int targetDistance;

    // Use this for initialization
    void Awake () {
        targetDistance = -1;
		//There is always an amiral when the ship is construct so we create one and add it to the ship
		Admiral admiral = new Admiral();
        addCrewMember(admiral);
		destination = transform.position;
		panelHandler = GameObject.Find ("HUDCanvas").GetComponent<PanelHandler> ();
        directionLifeTime = 0;
	}

	public void LoadShip(ShipStruct s) {
		this.food = s.food;
		this.gold = s.gold;
		this.hp = s.hp;
		this.energyQuantity = s.energyQuantity;
		this.shipX = s.shipX;
		this.shipY = s.shipY;
		this.orientation = s.orientation;

		this.playable = s.playable;
		this.isMoving = s.isMoving;
		this.shipName = s.shipName;

		this.crew = s.crew;

		// Reinit
		destination = transform.position;
	}

	public ShipStruct SaveShip() {
		ShipStruct s = new ShipStruct();

		s.food = this.food;
		s.gold = this.gold;
		s.hp = this.hp;
		s.energyQuantity = this.energyQuantity;
		s.shipX = this.shipX;
		s.shipY = this.shipY;
		s.orientation = this.orientation;

		s.playable = this.playable;
		s.isMoving = this.isMoving;
		s.shipName = this.shipName;

		s.crew = this.crew;

		return s;
	}

	// Update is called once per frame
	void Update () {
		if (!dead) {
			if (currentPath != null && energyQuantity > 0) {
				if (Vector3.Distance (transform.position, destination) < 0.1f) {
					MoveShipToNextHex ();
				}
			} else {
				if (Vector3.Distance (transform.position, destination) < 0.1f) {
                    //currentPath = null;
					isMoving = false;
				}
			}
			transform.position = Vector3.Lerp(transform.position, destination, 5f * Time.deltaTime);
			transform.rotation = Quaternion.Euler(new Vector3(0, -orientation, 0));
			transform.FindChild ("health").transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		}
	}

	public void MoveShipToNextHex(){
        //Debug.Log("path " +currentPath.Count);
        // here we control the remaining energy quantity before moving
        if (currentPath == null)
        {
            return;
        }

        if (energyQuantity <= 0)
        {
            return;
        }
		
		if (currentPath.Count > 0) {
			isMoving = true;
			Vector3 prevPos = transform.position;
			energyQuantity -= 1;
			shipX = currentPath [0].x;
			shipY = currentPath [0].y;
			currentPath.RemoveAt (0);
			destination = GameObject.Find ("Hex_" + shipX + "_" + shipY).transform.position;
			orientation = Angle360RoundToNeareast60(prevPos, destination);
			// We reaveal newly discovered area
			UpdateFOW ();
		} else {
			currentPath = null;
		}
		if (owner.IsHuman) {
			panelHandler.updateShip ();
		}
	}

	public void Die(){
		dead = true;
		Destroy (this.GetComponentInChildren<MeshCollider> ());
		this.GetComponentInChildren<TextMesh> ().text = "";
		StartCoroutine (Sink ());
		if (!owner.IsHuman) {
			panelHandler.updateShip ();
		}
	}

	public void RefuelEnergy(){
		int res = 0;
		foreach (CrewMember c in crew) {
			res += c.EnergyQuantity;
		}
		energyQuantity = res;
		//panelHandler.updateShip ();
	}

	IEnumerator Sink (){
		int goldWreck = this.gold / 2;
		int x = shipX;
		int y = shipY;
		Vector3 deathDestination = transform.position + new Vector3 (0, -1f, 0);
		Map map = GameObject.Find ("Map").GetComponent<Map> ();
		GameObject caseTreasure = GameObject.Find ("Hex_" + x + "_" + y);
		while (Vector3.Distance (transform.position, deathDestination) >= 0.3f){
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, -30), Time.deltaTime);
			transform.position = Vector3.Lerp(transform.position, deathDestination, 0.2f * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		yield return null;
		GameObject tres = (GameObject) Instantiate (map.treasurePrefab, map.graph [x, y].worldPos, Quaternion.identity);
		tres.name = caseTreasure.name+"_Treasure";
		tres.transform.SetParent (caseTreasure.transform);
		caseTreasure.GetComponent<Sea> ().AddTreasure (goldWreck, tres);
		Destroy (this.gameObject);
	}

	int Angle360RoundToNeareast60(Vector3 prev, Vector3 dest){		
		Vector3 toOther = (prev - dest).normalized;
		int angle = Mathf.RoundToInt((Mathf.Atan2(toOther.z, toOther.x) * Mathf.Rad2Deg + 180) / 60) * 60;
		if (angle == 360) {
			angle = 0;
		}
		return angle;
	}

	int Angle360(Vector3 prev, Vector3 dest){		
		Vector3 toOther = (prev - dest).normalized;
		int angle = Mathf.RoundToInt((Mathf.Atan2(toOther.z, toOther.x) * Mathf.Rad2Deg + 180));
		return angle;
	}

	public int Interact(Ship target){
		int errorCode = 0;
		/*
		 * return error code on call
		 * 0 : OK
		 * 1 : Not Enough Energy
		 * 2 : Not in range - ally
		 * 3 : Not in range - enemy
		 * 4 : this ship is not playable
		 */
		if (!this.playable && this.owner.IsHuman ) {
			errorCode = 4;
			return errorCode;
		}
		if (target == this) {
			bool resFishing = fishing ();
			if (!resFishing) {
				errorCode = 1;
			}
		} else if (target.owner.Name.Equals (owner.Name)) {
			if (AtTradeRange (target)) {
				Trade (target);
			} else {
				errorCode = 2;
			}
		} else if (!target.owner.Name.Equals (owner.Name)) {
			if (energyQuantity >= atkCost) {
				int attackValue = 0;
				int retributionValue = 0;
				if (AtFilibusterRange (target)) {
					// 1 is the type code of Filibusters
					attackValue += Attack (1, target, 1f);
					// Retribution
					retributionValue += target.Attack (1, this, RetributionStrength);
				}
				if (AtPowderMonkeyRange (target)) {
					// 2 is the type code of PowerMonkeys
					attackValue += Attack (2, target, 1f);
					// Retribution
					retributionValue += target.Attack (2, this, RetributionStrength);
				}
				if (AtConjurerRange (target)) {
					// 3 is the type code of Conjurers
					attackValue += Attack (3, target, 1f);
					// Retribution
					retributionValue += target.Attack (3, this, RetributionStrength);
				}
				if (attackValue > 0) {
					target.displayFloatingInfo (Color.red, "-" + attackValue + " PV", target.transform.position);
					if (retributionValue > 0) {
						this.displayFloatingInfo (Color.red, "-" + retributionValue + " PV", this.transform.position);
					}
					energyQuantity -= atkCost;
				} else {
					errorCode = 3;
				}
			} else {
				errorCode = 1;
			}
		}
		if (owner.IsHuman) {
			panelHandler.updateShip ();
		}
		return errorCode;
	}

	public bool AtFilibusterRange(Ship target){
		bool found = false;
		foreach (CrewMember c in crew) {
			if (c is Filibuster) {
				found = true;
			}
		}
		if (!found) {
			return false;
		}
		float distance = Mathf.Abs (Vector3.Distance (destination, target.transform.position));
		// 1 Hex dist
		if (distance < 1f) {
			return true;
		} else {
			return false;
		}
	}

	public bool AtTradeRange(Ship target){
		float distance = Mathf.Abs (Vector3.Distance (destination, target.transform.position));
		// 1 Hex dist
		if (distance < 1f) {
			return true;
		} else {
			return false;
		}
	}

	public bool AtPowderMonkeyRange(Ship target){
		bool found = false;
		foreach (CrewMember c in crew) {
			if (c is PowderMonkey) {
				found = true;
			}
		}
		if (!found) {
			return false;
		}
		float distance = Mathf.Abs (Vector3.Distance (destination, target.transform.position));
		// 3 Hex dist
		if (distance > 2.9f) {
			return false;
		} else {
			bool result = false;
			Node[,] tmpGraph = GameObject.Find ("Map").GetComponent<Map> ().graph;
			Node targetNode = tmpGraph [target.ShipX, target.ShipY];
			int angle = Angle360 (targetNode.worldPos, destination);
			// oriented toward right or left
			if (orientation == 0 || orientation == 180) {
				if (angle >= 60 && angle <= 120 || angle >= 240 && angle <= 300) {
					result = true;
				}
			}
			// oriented toward upper right or lower left
			if (orientation == 60 || orientation == 240) {
				if (angle == 0)
					angle = 360;
				if (angle >= 120 && angle <= 180 || angle >= 300 && angle <= 360) {
					result = true;
				}
			}
			// oriented toward upper left or lower right
			if (orientation == 120 || orientation == 300) {
				if (angle == 360)
					angle = 0;
				if (angle >= 0 && angle <= 60 || angle >= 180 && angle <= 240) {
					result = true;
				}
			}
			return result;
		}
	}

	public bool AtConjurerRange(Ship target){
		bool found = false;
		foreach (CrewMember c in crew) {
			if (c is Conjurer) {
				found = true;
			}
		}
		if (!found) {
			return false;
		}
		float distance = Mathf.Abs (Vector3.Distance (transform.position, target.transform.position));
		// 2 Hex dist
		if (distance < 2f) {
			return true;
		} else {
			return false;
		}
	}
		
	public void Trade(Ship ship){
		panelHandler.hideAllModals ();
		panelHandler.initTrade (ship);
		panelHandler.showPanelTrade ();
	}

	public int Attack(int crewUsed, Ship target, float atkStrength){
		float attackValue = 0f;
		switch (crewUsed) {
		case 1:
			// Case Filibuster
			foreach (CrewMember c in crew) {
				if (c.Type == crewUsed) {
					attackValue += c.Atk * atkStrength;
				}
			}
			GiveXP (crewUsed);
			break;
		case 2:
			// Case PowderMonkey
			foreach (CrewMember c in crew) {
				if (c.Type == crewUsed) {
					attackValue += c.Atk * atkStrength;
				}
			}
			GiveXP (crewUsed);
			break;
		case 3:
			// Case Conjurer
			foreach (CrewMember c in crew) {
				if (c.Type == crewUsed) {
					attackValue += c.Atk * atkStrength;
				}
			}
			GiveXP (crewUsed);
			break;
		}
		target.TakeDamages (Mathf.RoundToInt (attackValue));
		return Mathf.RoundToInt (attackValue);
	}

	public void GiveXP(int crewType){
		foreach (CrewMember c in crew) {
			if (c.Type == crewType){
				c.gainXP (15);
			}
			// We also give xp to the admiral, code 0
			if (c.Type == 0) {
				c.gainXP (10);
			}
		}
	}

	public void TakeDamages(int damages){
		int victimIndex;
		bool takingDamages = true;
		do{
			if (crew.Count > 1){
				victimIndex = Random.Range(1,crew.Count);
				if (damages >= crew[victimIndex].Lp){
					damages -= crew[victimIndex].Lp;
					crew.RemoveAt(victimIndex);
				}
				else{
					crew[victimIndex].Lp -= damages;
					takingDamages = false;
				}
			}
			else{
				crew[0].Lp -= damages;
				takingDamages = false;
			}
		} while (takingDamages);
		UpdateShipHp ();
		DisplayHp (true);
		if (!owner.IsHuman) {
			panelHandler.updateShip ();
		}
	}

	public void UpdateShipHp(){
		int totalHp = 0;
		int currentHp = 0;
		foreach (CrewMember cm in crew){
			totalHp += cm.LpMax;
			currentHp += cm.Lp;
		}
		hp = currentHp;
		textHp = currentHp.ToString() + "/" + totalHp.ToString();
	}

	public void displayFloatingInfo(Color color, string text, Vector3 pos){
		transform.FindChild("floatingInfo").GetComponent<FloatingText>().SetText (color, text, pos);
	}

	public int calculateEQmax(){
		int EQMax = 0;
		foreach(CrewMember member in crew)
		{
			EQMax += member.EnergyQuantity;
		}
		return EQMax;
	}

	public bool HoistTreasure(Sea target){
		//if enough energy
		float distance = Mathf.Abs (Vector3.Distance (transform.position, target.transform.position));
		if (distance < 1f && energyQuantity >= 1){
			displayFloatingInfo (Color.yellow, "+" + target.Treasure, transform.position);
			gold += target.Treasure;
			Destroy (target.Treasure_go);
			target.RemoveTreasure();
			energyQuantity -= 1;
			panelHandler.updateShip ();
			return true;
		}
		else{
			return false; 
		}
	}

	public void harborDock()
	{
		//toCheck
	}

	public bool fishing(){
		//if enough energy
		if(energyQuantity >= 5){
			//debug test of fishing value of 100
			//food += 100;
			GameObject foodplace = GameObject.Find ("Hex_" + shipX + "_" + shipY);
			food += foodplace.GetComponent<Sea> ().FoodQuantity;
			//toImplement : modify foodQuantity of the hex
			//decrement energyQuantity, to check, debug value
			//Or set it to 0 to make the ship unable to move
			energyQuantity -= 5;
			panelHandler.updateShip ();
			return true;
		}
		else{ 
			return false; 
		}
	}

	public void addCrewMember(CrewMember member){
        if(member.Type == 1)
        {
            nbFilibuster += 1;
        }
        else if(member.Type == 2)
        {
            nbPowderMonkey += 1;
        }
        else if (member.Type == 3)
        {
            nbConjurer += 1;
        }
        crew.Add(member);
        hp += member.Lp;
        energyQuantity += member.EnergyQuantity;
        if (panelHandler)
        {
            panelHandler.updateShip();
        }
    }

	public void removeCrewMember(CrewMember member)
    {
        if (member.Type == 1)
        {
            nbFilibuster -= 1;
        }
        else if (member.Type == 2)
        {
            nbPowderMonkey -= 1;
        }
        else if (member.Type == 3)
        {
            nbConjurer -= 1;
        }
        crew.Remove(member);
        hp -= member.Lp;
        if (panelHandler)
        {
            panelHandler.updateShip();
        }
	}

	public void UpdateFOW (){
		bool visibleByOther = false;
		GameObject currentHex = (GameObject) GameObject.Find ("Hex_" + this.ShipX + "_" + this.ShipY);
		List<GameObject> neighbours;
		List<GameObject> shipsAtRange;
		MeshRenderer[] meshRenderers;
		Node newNode;
		// We get all the neighbours in a range of 3 tiles
		neighbours = currentHex.GetComponent<Hex> ().getNLevelOfNeighbours (0, 3);
		foreach (GameObject n in neighbours) {
			this.DisplayTargetHp (n);
			if (!owner.IsHuman) {
				if (n.GetComponent<Sea> () != null && n.GetComponent<Sea> ().ShipContained != null && !n.GetComponent<Sea> ().ShipContained.Owner.Name.Equals(this.owner.Name)) {
					visibleByOther = true;
				}
			} else {
				n.GetComponent<Hex> ().setVisibility (2);
			}
			newNode = new Node(n.GetComponent<Hex>().x, n.GetComponent<Hex>().y, new Vector3(0,0,0), false, "map");
			if (!used)
            {

                //Solution à verifier
                float distance = Mathf.Abs(Vector3.Distance(currentHex.transform.position, n.transform.position));
                getTarget(n, (int)Mathf.Floor(distance));
			}

			if (!owner.ExploredHex.Exists (e => e.x == newNode.x && e.y == newNode.y)) {
				owner.ExploredHex.Add(newNode);
			}
		}
		if (!owner.IsHuman) {
			if (visibleByOther) {
				this.GetComponentInChildren<MeshRenderer> ().enabled = true;
			} else {
				this.GetComponentInChildren<MeshRenderer> ().enabled = false;
			}
		}
	}

    public void getTarget(GameObject hex, int dist)
    {
        if (targetDistance == -1 || dist < targetDistance)
        {
            Node targetNode = GameObject.Find("Map").GetComponent<Map>().graph[hex.GetComponent<Hex>().x, hex.GetComponent<Hex>().y];
            if (targetNode.type == "sea" && !targetNode.isWalkable)
            {
                //Can be treasure or ship or destination
                if (hex.GetComponentInChildren<Sea>().Treasure > 0)
                { //It's a treasure
                    //Debug.Log("treasure near");
                    //doAction
                }
                else
                {
                    if(hex.GetComponent<Sea>().ShipContained.shipName != shipName)
                    { //if the targeted ship isn't this one
                        //Debug.Log("Ship near");
                        if (hex.GetComponent<Sea>().ShipContained.Owner.Name != owner.Name)
                        { //if the targeted ship is an ennemy
                            //Debug.Log("Setting dist");
                            targetDistance = dist;
                            targetX = targetNode.x;
                            targetY = targetNode.y;
                        }
                    }
                }
            }
        }
	}

	// ====================
	// UTILITIES
	// ====================

	public void DisplayTargetHp(GameObject go){
		if (go.GetComponent<Sea> () != null && go.GetComponent<Sea> ().ShipContained != null && !go.GetComponent<Sea> ().ShipContained.Owner.Name.Equals(this.owner.Name)) {
			Ship ship = go.GetComponent<Sea> ().ShipContained;
			if (this.AtConjurerRange (ship) || this.AtFilibusterRange (ship) || this.AtPowderMonkeyRange (ship)) {
				ship.UpdateShipHp ();
				ship.DisplayHp (true);
			} else {
				ship.DisplayHp (false);
			}
		}
	}

	public void DisplayHp(bool disp){
		if (disp) {
			this.GetComponentInChildren<TextMesh> ().text = this.GetComponent<Ship> ().textHp;
		} else {
			this.GetComponentInChildren<TextMesh> ().text = "";
		}
	}

	public void hideHp(){
	}

	// ====================
	// GETTER & SETTER 
	// ====================

	public int Food
	{
		get { return food; }
		set { food = value;panelHandler.updateShip (); }
	}

	public int Gold
	{
		get { return gold; }
		set { gold = value;panelHandler.updateShip (); }
	}

	public int Hp
	{
		get { return hp; }
		set { hp = value; }
	}

	public int EnergyQuantity
	{
		get { return energyQuantity; }
		set { energyQuantity = value; }
	}

	public List<CrewMember> Crew
	{
		get { return crew; }
		set { crew = value; }
	}

	public void moveShip(int x, int y)
	{
		//tocheck
	}

	public int ShipX
	{
		get { return shipX; }
		set { shipX = value; }
	}

	public int ShipY
	{
		get { return shipY; }
		set { shipY = value; }
	}

	public string ShipName
	{
		get { return shipName; }
		set { shipName = value; }
	}

	public List<Node> CurrentPath
	{
		get { return currentPath; }
		set { currentPath = value; }
	}

	public bool Playable
	{
		get { return playable; }
		set { playable = value; }
	}

	public int Orientation
	{
		get { return orientation; }
		set { orientation = value; }
	}

	public bool IsMoving
	{
		get { return isMoving; }
		set { isMoving = value; }
	}

	public Player Owner
	{
		get { return owner; }
		set { owner = value; }
	}

    public bool Used
    {
        get { return used; }
        set { used = value; }
    }

    public int NbConjurer
    {
        get { return nbConjurer; }
        set { nbConjurer = value; }
    }

    public int NbFilibuster
    {
        get { return nbFilibuster; }
        set { nbFilibuster = value; }
    }

    public int NbPowderMonkey
    {
        get { return nbPowderMonkey; }
        set { nbPowderMonkey = value; }
    }

    public int DirectionX
    {
        get { return directionX; }
        set { directionX = value; }
    }

    public int DirectionY
    {
        get { return directionY; }
        set { directionY = value; }
    }

    public int TargetX
    {
        get { return targetX; }
        set { targetX = value; }
    }

    public int TargetY
    {
        get { return targetY; }
        set { targetY = value; }
    }

    public int TargetDistance
    {
        get { return targetDistance; }
        set { targetDistance = value; }
    }

    public int DirectionLifeTime
    {
        get { return directionLifeTime; }
        set { directionLifeTime = value; }
	}

	public int AtkCost
	{
		get { return atkCost; }
		set { atkCost = value; }
	}

	public string TextHp
	{
		get { return textHp; }
		set { textHp = value; }
	}

	public PanelHandler PanelHandler
	{
		get { return panelHandler; }
		set { panelHandler = value; }
	}

	public float RetributionStrength
	{
		get { return (GameObject.Find("GameManager").GetComponent<GameManager>().retributionStrength / 100); }
	}
}
