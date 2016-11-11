//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Harbor : Land
{
    private Player owner; //or is it only a string?

    // Use this for initialization
    void Start()
    {
        type = "harbor";
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
}