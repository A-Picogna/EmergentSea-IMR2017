using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class at_testMapManager : MonoBehaviour {

	public Text textContainer;

	// Use this for initialization
	void Start () {
		//GameObject MapManager = GameObject.Find("MapManager");
		textContainer.text = "Taille de la carte :" + MapManager.instance.MapWidthParameter.ToString() ;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
