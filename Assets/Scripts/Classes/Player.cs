//@author: R.Baronnet

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {

	private bool humanPlayer;
	private string name;
	private Color color;
	private List<Ship> fleet;
	private List<Harbor> harbors;
    private int nbTotalShip;
	private List<Node> exploredHex;
    
	public Player(bool _type, Color _color, string _name){
		humanPlayer = _type;
		color = _color;
		name = _name;
		fleet = new List<Ship> ();
        harbors = new List<Harbor>();
		exploredHex = new List<Node> ();
        nbTotalShip = 0;
	}

	public Player(PlayerStruct p, List<Node> exploredHex) {
		this.humanPlayer = p.type;
		this.color = p.color;
		this.name = p.name;
		this.fleet = new List<Ship> ();
		//Added directly in the GameManager
		this.harbors = new List<Harbor>();
		//Added directly in the GameManager
		this.exploredHex = exploredHex;
		// Added directly in the GameManager

		this.nbTotalShip = 0;
	}


    // Use this for initialization
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {

	}

	public Color Color
	{
		get { return color; }
		set { color = value; }
	}

	public bool IsHuman
	{
		get { return humanPlayer; }
	}

	public void SetPlayerHuman()
	{
		humanPlayer = true;
	}

	public void SetPlayerAI()
	{
		humanPlayer = false;
	}

	public string Name
	{
		get { return name; }
		set { name = value; }
	}

	public List<Ship> Fleet
	{
		get { return fleet; }
		set { fleet = value; }
	}

	public List<Harbor> Harbors
	{
		get { return harbors; }
		set { harbors = value; }
	}

	public int NbTotalShip
	{
		get { return nbTotalShip; }
		set { nbTotalShip = value; }
	}

	public List<Node> ExploredHex
	{
		get { return exploredHex; }
		set { exploredHex = value; }
	}
}