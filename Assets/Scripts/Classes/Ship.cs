using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ship : MonoBehaviour {

	private List<Node> currentPath = null;
	private int food;
	private int gold;
	private int hp;
	private int energyQuantity;
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
	bool dead;
	private string shipName;
	private List<CrewMember> crew = new List<CrewMember>();
	public Vector3 destination;

	// Use this for initialization
	void Start () {
		//There is always an amiral when the ship is construct so we create one and add it to the ship
		Admiral admiral = new Admiral();
		addCrewMember(admiral);
		dead = false;
		energyQuantity = 100000000;
		food = 100;
		gold = 100;
		hp = 2000;
		destination = transform.position;
	}

	// Update is called once per frame
	void Update () {
		if (!dead) {
			if (currentPath != null) {
				if (Vector3.Distance (transform.position, destination) < 0.1f) {
					MoveShipToNextHex ();
				}
			} else {
				if (Vector3.Distance (transform.position, destination) < 0.1f) {
					isMoving = false;
				}
			}
			transform.position = Vector3.Lerp(transform.position, destination, 5f * Time.deltaTime);
			transform.rotation = Quaternion.Euler(new Vector3(0, -orientation, 0));
		}
	}

	public void MoveShipToNextHex(){
		// here we control the remaining energy quantity before moving
		if(currentPath==null)
			return;

		if(energyQuantity <= 0)
			return;
		
		if (currentPath.Count > 0) {
			isMoving = true;
			Vector3 prevPos = transform.position;
			energyQuantity -= 1;
			shipX = currentPath [0].x;
			shipY = currentPath [0].y;
			currentPath.RemoveAt (0);
			destination = GameObject.Find ("Hex_" + shipX + "_" + shipY).transform.position;
			orientation = Angle360RoundToNeareast60(prevPos, destination);
		} else {
			currentPath = null;
		}
	}

	public void Die(){
		dead = true;
		Destroy (this.GetComponentInChildren<MeshCollider> ());
		StartCoroutine (Sink ());
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
		if (target.owner.Name.Equals (owner.Name)) {
			Debug.Log ("It's a friend dammit! Don't Shoot!!!");
			if (AtFilibusterRange (target)) {
				Debug.Log ("Friendly ship at range, ready to trade !");
				Trade ();
			}
		} else {
			int attackValue = 0;
			if (AtFilibusterRange(target)){
				Debug.Log ("Filibusters at range, ready to aboard !");
				attackValue += Attack ("Filibuster", target);
			}
			if (AtConjurerRange(target)){
				Debug.Log ("Conjurer at range, ready to cast !");
				attackValue += Attack ("Conjurer", target);
			}
			if (AtPowderMonkeyRange(target)){
				Debug.Log ("Canon at range, ready to fire !");
				attackValue += Attack ("PowderMonkey", target);
			}
			if (attackValue > 0) {
				displayFloatingInfo (Color.red, "-" + attackValue + " HP", target.transform.position);
			}
		}
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
			Node originNode = tmpGraph [shipX, shipY];
			Node targetNode = tmpGraph [target.ShipX, target.ShipY];
			List<Node> neighbours = new List<Node> ();
			List<Node> neighbours2 = new List<Node> ();
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
		
	public void Trade(){

	}

	public int Attack(string crewUsed, Ship target){
		int attackValue = 0;
		switch (crewUsed) {
		case "Filibuster":
			foreach (CrewMember c in crew) {
				if (c is Filibuster) {
					attackValue += c.Atk;
				}
			}
			break;
		case "Conjurer":
			foreach (CrewMember c in crew) {
				if (c is Conjurer) {
					attackValue += c.Atk;
				}
			}
			break;
		case "PowderMonkey":
			foreach (CrewMember c in crew) {
				if (c is PowderMonkey) {
					attackValue += c.Atk;
				}
			}
			break;
		}
		target.Hp -= attackValue;
		return attackValue;
		Debug.Log ("Ouch! I've lost " + attackValue + ", I have only " + target.Hp + " left!");
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
			food += 100;

			//toImplement : modify foodQuantity of the hex
			//decrement energyQuantity, to check, debug value
			//Or set it to 0 to make the ship unable to move
			energyQuantity -= 5;

			return true;
		}
		else{ 
			return false; 
		}
	}

	public void addCrewMember(CrewMember member){
		crew.Add(member);
	}

	public void removeCrewMember(CrewMember member){
		crew.Remove(member);
	}





	// ====================
	// GETTER & SETTER 
	// ====================

	public int Food
	{
		get { return food; }
		set { food = value; }
	}

	public int Gold
	{
		get { return gold; }
		set { gold = value; }
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

	// ====================
	// ====================



}
