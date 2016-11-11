//@author: R.Baronnet

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {
	
    private string type, name;
	private Color color;
	private List<Ship> fleet;
	private List<Harbor> harbors;
    
	public Player(string _type, Color _color, string _name){
		type = _type;
		color = _color;
		name = _name;
		fleet = new List<Ship> ();
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

	public string Type
	{
		get { return type; }
		set { type = value; }
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
}