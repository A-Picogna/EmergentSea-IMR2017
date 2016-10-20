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
	public int MapWidthParameter = 2;
	public int DifficultyParameter = 2;
	public int GroundFrequencyParameter = 2;
	public int PortFrequencyParameter = 2;
	public int TreasureFrequencyParameter = 2;
	//public int Parameter =;
	//public int Parameter =;




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
		
	////
	// Interface pour l'UI
	////
	public void MapWidthDropdownCallback(int WidthType) {
		// 0: Très petit
		// 1: Petit
		// 2: Moyen
		// 3: Grand
		// 4: Très grand
		// 5: Gigantesque
		// 6: Personnalisé
		MapWidthParameter = WidthType;
	}

	public void DifficultyDropdownCallback(int DifficultyType) {
		// 0: Par défaut
		DifficultyParameter = DifficultyType;
	}

	public void GroundFrequencyDropdownCallback(int GroundFrequencyType) {
		// 0: Très basse
		// 1: Basse
		// 2: Normale
		// 3: Elevée
		// 4: Très élevée
		GroundFrequencyParameter = GroundFrequencyType;
	}

	public void PortFrequencyDropdownCallback(int PortFrequencyType) {
		// 0: Très basse
		// 1: Basse
		// 2: Normale
		// 3: Elevée
		// 4: Très élevée
		PortFrequencyParameter = PortFrequencyType;
	}

	public void TreasureFrequencyDropdownCallback(int TreasureFrequencyType) {
		// 0: Très basse
		// 1: Basse
		// 2: Normale
		// 3: Elevée
		// 4: Très élevée
		TreasureFrequencyParameter = TreasureFrequencyType;
	}
}
