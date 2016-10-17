using UnityEngine;
using System.Collections;

public class MapManager : MonoBehaviour {
	/* The MapManager is a class which will be used to pass Map informations between Scenes
	 * For example, it can be called to the Map scene at anytime to know :
	 * * How many opponents do i have to place in the map
	 * * How easy/difficult this party will be
	 * * Etc...
	 * */
	// TODO have a loader in the Main Menu to create the MapManager ( see ://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/ )

	public static MapManager instance = null;
	// The magic works here : You can use the singleton just by indicating MapManager.instance

	// MAP INFORMATION
	public int nbOfSharks = 0; // For testing purposes


	// Use this before initialization
	void Awake() {
		//Check if instance already exists
		if (instance == null)

			//if not, set instance to this
			instance = this;

		//If instance already exists and it's not this:
		else if (instance != this)

			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a MapManager.
			Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		Debug.Log ("MapManager loaded");
	}
}
