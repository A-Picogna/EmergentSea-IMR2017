//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Current : Sea
{
    private int intensity, direction;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

	public int Intensity
    {
        get { return intensity; }
        set { intensity = value; }
    }

	public int Direction
    {
        get { return direction; }
        set { direction = value; }
    }
}