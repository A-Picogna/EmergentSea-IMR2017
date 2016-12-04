using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {

	private List<Node> currentPath = null;
	private int food = 0;
	private int gold = 200;
	private int hp = 0;
	private int energyQuantity = 0;
	private int shipX = -1;
	private int shipY = -1;
	private int orientation = 0;
	private Player owner;
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

    //AI
    private bool used = false;
    private int nbFilibuster = 0;
    private int nbConjurer = 0;
    private int nbPowderMonkey = 0;
    private int directionX;
    private int directionY;
    private int directionLifeTime;

    // Use this for initialization
    void Awake () {
		//There is always an amiral when the ship is construct so we create one and add it to the ship
		Admiral admiral = new Admiral();
        addCrewMember(admiral);
		destination = transform.position;
		panelHandler = GameObject.Find ("HUDCanvas").GetComponent<PanelHandler> ();
        directionLifeTime = 0;
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
		}
	}

	public void MoveShipToNextHex(){
        Debug.Log("path " +currentPath.Count);
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
		if (owner.Type.Equals ("Humain")) {
			panelHandler.updateShip ();
		}
	}

	public void Die(){
		dead = true;
		Destroy (this.GetComponentInChildren<MeshCollider> ());
		StartCoroutine (Sink ());
		panelHandler.updateShip ();
	}

	public void RefuelEnergy(){
		int res = 0;
		foreach (CrewMember c in crew) {
			res += c.EnergyQuantity;
		}
		energyQuantity = res;
		panelHandler.updateShip ();
	}

	IEnumerator Sink (){
		Vector3 deathDestination = transform.position + new Vector3 (0, -1f, 0);
		while (Vector3.Distance (transform.position, deathDestination) >= 0.2f){
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, -30), Time.deltaTime);
			transform.position = Vector3.Lerp(transform.position, deathDestination, 0.1f * Time.deltaTime);
			yield return new WaitForEndOfFrame();
		}
		yield return null;
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

	public void Interact(Ship target){
		if (!this.playable) {
			return;
		}
		if (target == this) {
			fishing ();
		}
		if (target.owner.Name.Equals (owner.Name) && target != this) {
			Debug.Log ("It's a friend dammit! Don't Shoot!!!");
			if (AtTradeRange (target)) {
				Debug.Log ("Friendly ship at range, ready to trade !");
				Trade (target);
			}
		} else if (!target.owner.Name.Equals (owner.Name)) {
			if (energyQuantity >= 5) {
				int attackValue = 0;
				if (AtFilibusterRange (target)) {
					// 1 is the type code of Filibusters
					attackValue += Attack (1, target);
				}
				if (AtPowderMonkeyRange (target)) {
					// 2 is the type code of PowerMonkeys
					attackValue += Attack (2, target);
				}
				if (AtConjurerRange (target)) {
					// 3 is the type code of Conjurers
					attackValue += Attack (3, target);
				}
				if (attackValue > 0) {
					displayFloatingInfo (Color.red, "-" + attackValue + " HP", target.transform.position);
					energyQuantity -= 5;
				}
			}
		}
		panelHandler.updateShip ();
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
		float distance = Mathf.Abs (Vector3.Distance (transform.position, target.transform.position));
		// 1 Hex dist
		if (distance < 1f) {
			return true;
		} else {
			return false;
		}
	}

	public bool AtTradeRange(Ship target){
		float distance = Mathf.Abs (Vector3.Distance (transform.position, target.transform.position));
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
		float distance = Mathf.Abs (Vector3.Distance (transform.position, target.transform.position));
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

	public int Attack(int crewUsed, Ship target){
		int attackValue = 0;
		switch (crewUsed) {
		case 1:
			// Case Filibuster
			foreach (CrewMember c in crew) {
				if (c.Type == crewUsed) {
					attackValue += c.Atk;
				}
			}
			GiveXP (crewUsed);
			break;
		case 2:
			// Case PowderMonkey
			foreach (CrewMember c in crew) {
				if (c.Type == crewUsed) {
					attackValue += c.Atk;
				}
			}
			GiveXP (crewUsed);
			break;
		case 3:
			// Case Conjurer
			foreach (CrewMember c in crew) {
				if (c.Type == crewUsed) {
					attackValue += c.Atk;
				}
			}
			GiveXP (crewUsed);
			break;
		}
		target.TakeDamages (attackValue);
		return attackValue;
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
		panelHandler.updateShip ();
	}

	public void UpdateShipHp(){
		int res = 0;
		foreach (CrewMember c in crew){
			res += c.Lp;
		}
		hp = res;
	}

	public void displayFloatingInfo(Color color, string text, Vector3 pos){
		GameObject dmgBubble = GameObject.Find ("ShipFloatingInfo");
		dmgBubble.GetComponent<FloatingText> ().Reinit ();
		dmgBubble.transform.position = pos + new Vector3 (0, 0.7f, 0);
		dmgBubble.GetComponent<TextMesh> ().color = color;
		dmgBubble.GetComponent<TextMesh> ().text = text;
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
		if (distance < 1f && energyQuantity >= 3){
			displayFloatingInfo (Color.yellow, "+" + target.Treasure, transform.position);
			gold += target.Treasure;
			Destroy (target.Treasure_go);
			target.RemoveTreasure();
			energyQuantity -= 3;
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
		bool visibleByHumain = false;
		GameObject currentHex = (GameObject) GameObject.Find ("Hex_" + this.ShipX + "_" + this.ShipY);
		List<GameObject> firstNeighboursToReveal;
		List<GameObject> secondNeighboursToReveal;
		List<GameObject> thirdNeighboursToReveal;
		MeshRenderer[] meshRenderers;
		Node newNode;

        // Reveal Ship and ship Hex
        if (owner.Type != "IA") {
            currentHex.GetComponent<Hex>().setVisibility(2);
        }
		newNode = new Node(this.ShipX, this.ShipY, new Vector3(0,0,0), false, "ship");
		if (!owner.ExploredHex.Exists (e => e.x == newNode.x && e.y == newNode.y)) {
			owner.ExploredHex.Add(newNode);
		}

		// Reveal 1st Neighbours
		firstNeighboursToReveal = currentHex.GetComponent<Hex>().getNeighbours();
		foreach (GameObject n1 in firstNeighboursToReveal) {
			if (owner.Type.Equals ("IA")) {
				if (n1.GetComponent<Sea> () != null && n1.GetComponent<Sea> ().ShipContained != null && !n1.GetComponent<Sea> ().ShipContained.Owner.Name.Equals(this.owner.Name)) {
					visibleByHumain = true;
				}
			} else {
				n1.GetComponent<Hex> ().setVisibility (2);
			}
			newNode = new Node(n1.GetComponent<Hex>().x, n1.GetComponent<Hex>().y, new Vector3(0,0,0), false, "map");
			if (!owner.ExploredHex.Exists (e => e.x == newNode.x && e.y == newNode.y)) {
				owner.ExploredHex.Add(newNode);
			}
			// Reveal 2nd Neighbours
			secondNeighboursToReveal = n1.GetComponent<Hex> ().getNeighbours ();
			foreach (GameObject n2 in secondNeighboursToReveal) {
				if (owner.Type.Equals("IA")) {
					if (n2.GetComponent<Sea> () != null && n2.GetComponent<Sea> ().ShipContained != null && !(n2.GetComponent<Sea> ().ShipContained.Owner.Name.Equals(this.owner.Name)) ) {
						visibleByHumain = true;
					}
				} else {
					n2.GetComponent<Hex> ().setVisibility (2);
				}
				newNode = new Node(n2.GetComponent<Hex>().x, n2.GetComponent<Hex>().y, new Vector3(0,0,0), false, "map");
				if (!owner.ExploredHex.Exists (e => e.x == newNode.x && e.y == newNode.y)) {
					owner.ExploredHex.Add(newNode);
				}
				// Reveal 3rd Neighbours
				thirdNeighboursToReveal = n2.GetComponent<Hex> ().getNeighbours ();
				foreach (GameObject n3 in thirdNeighboursToReveal)
                {
					if (owner.Type.Equals("IA")) {
						if (n3.GetComponent<Sea> () != null && n3.GetComponent<Sea> ().ShipContained != null && !n3.GetComponent<Sea> ().ShipContained.Owner.Name.Equals(this.owner.Name)) {
							visibleByHumain = true;
						}
					} else {
						n3.GetComponent<Hex> ().setVisibility (2);
					}
					newNode = new Node(n3.GetComponent<Hex>().x, n3.GetComponent<Hex>().y, new Vector3(0,0,0), false, "map");
					if (!owner.ExploredHex.Exists (e => e.x == newNode.x && e.y == newNode.y)) {
						owner.ExploredHex.Add(newNode);
					}
				}
			}
		}
		if (owner.Type.Equals ("IA")) {
			if (visibleByHumain) {
				this.GetComponentInChildren<MeshRenderer> ().enabled = true;
			} else {
				this.GetComponentInChildren<MeshRenderer> ().enabled = false;
			}
		}
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

    public int DirectionLifeTime
    {
        get { return directionLifeTime; }
        set { directionLifeTime = value; }
    }

    // ====================
    // ====================

    public PanelHandler PanelHandler
	{
		get { return panelHandler; }
		set { panelHandler = value; }
	}
}
