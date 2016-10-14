//@author: R.Baronnet

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private string type, color;
    
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

	public string Color
    {
        get { return color; }
        set { color = value; }
    }
}