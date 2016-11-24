using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


public class LoadManager : MonoBehaviour {
	/* The LoadManager is a class which will be used to pass Map informations between Scenes
	 * For example, it can be called to the Map scene at anytime to know :
	 * * How many opponents do i have to place in the map
	 * * How easy/difficult this party will be
	 * * Etc...
	 * */
	// TODO have a loader in the Main Menu to create the LoadManager ( see ://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/ )



	public static LoadManager instance = null;
	// The magic works here : You can use the singleton just by indicating nager.instance

	// Object Enum
	public enum state { Inactive, StartNewMap, StartLoadedMap }

	// LoadManager Info
	public state LoadManagerState;

	// MAP INFORMATION
	public int nbOfSharks = 0; // For testing purposes
	public int MapWidthParameter = 2;
	public int DifficultyParameter = 2;
	public int GroundFrequencyParameter = 2;
	public int PortFrequencyParameter = 2;
	public int TreasureFrequencyParameter = 2;
	public int MapX = 0;
	public int MapY = 0;
	//public int Parameter =;
	//public int Parameter =;
	public GameObject hexPrefab;
	public GameObject landPrefab;
	public GameObject seaPrefab;
	public GameObject mapPrefab;
    public GameObject harborPrefab;
    public GameObject coastPrefab;
	public GameObject gameManagerPrefab;

	// Private variables
	private Map MapLoaded;
	public string MapPrefabToLoad;


	// Use this before initialization (and between loading Maps)
	void Awake() {
		//Check if instance already exists
		if (instance == null) {
			//if not, set instance to this
			instance = this;
			//And force the default parameters
			LoadManagerState = state.Inactive;
			MapWidthDropdownCallback(MapWidthParameter);
		}
			

		//If instance already exists and it's not this:
		else if (instance != this)
			
			//Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a LoadManager.
			Destroy(gameObject);    

		//Sets this to not be destroyed when reloading scene
		DontDestroyOnLoad(gameObject);

		Debug.Log ("LoadManager loaded");
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
		switch (WidthType) {
		case 0:
			MapX = 40;
			MapY = 24;
			break;
		case 1:
			MapX = 56;
			MapY = 36;
			break;
		case 2:
			MapX = 66;
			MapY = 42;
			break;
		case 3:
			MapX = 80;
			MapY = 52;
			break;
		case 4:
			MapX = 104;
			MapY = 64;
			break;
		case 5:
			MapX = 128;
			MapY = 80;
			break;
		case 6:
			//Géré par NewPlayUI
			break;
		}
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

	public void MapXSliderCallBack(float MapX) {
		if (MapWidthParameter == 6) {
			this.MapX = Mathf.FloorToInt (MapX);
		}
	}

	public void MapYSliderCallBack(float MapY) {
		if (MapWidthParameter == 6) {
			this.MapY = Mathf.FloorToInt (MapY);
		}
	}

	private Map initMap() {
		GameObject newObject = Instantiate(mapPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		Map mapSettings = newObject.GetComponent<Map>();
		//do additional initialization steps here

		mapSettings.hexPrefab = hexPrefab;
		mapSettings.landPrefab = landPrefab;
		mapSettings.seaPrefab = seaPrefab;
        mapSettings.harborPrefab = harborPrefab;
        mapSettings.coastPrefab = coastPrefab;

        mapSettings.height = MapX;
		mapSettings.width = MapY;

		mapSettings.LaunchMapGeneration ();

		return mapSettings;
	}

	private Map loadMap(MapFile saveMap) {
		GameObject newObject = Instantiate(mapPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		Map mapSettings = newObject.GetComponent<Map>();

		mapSettings.hexPrefab = hexPrefab;
		mapSettings.landPrefab = landPrefab;
		mapSettings.seaPrefab = seaPrefab;
		mapSettings.harborPrefab = harborPrefab;
		mapSettings.coastPrefab = coastPrefab;

		mapSettings.height = saveMap.height;
		mapSettings.width = saveMap.width;

		mapSettings.LaunchMapLoading (saveMap);

		return mapSettings;
	}

	private void initWorld() {
		MapLoaded = initMap ();
		GameObject mouseSettings = GameObject.Find ("MouseManager");
		((MouseManager)mouseSettings.GetComponent<MouseManager> ()).map = MapLoaded;

		GameManager gameManager = initGame (MapLoaded);

		// Connection des objets
		GameObject pathfinder = GameObject.Find ("Pathfinder");
		((Pathfinder)pathfinder.GetComponent<Pathfinder> ()).map = MapLoaded;

		GameObject btn_save = GameObject.Find ("btn_save");
		((UnityEngine.UI.Button)btn_save.GetComponent<UnityEngine.UI.Button> ()).onClick.AddListener (() => {
			LoadManager.instance.savePrefabricatedMap ("test");
		});

	}

	private void loadWorld(MapFile saveMap) {
		MapLoaded = loadMap (saveMap);
		GameObject mouseSettings = GameObject.Find ("MouseManager");
		((MouseManager)mouseSettings.GetComponent<MouseManager> ()).map = MapLoaded;

		GameManager gameManager = initGame (MapLoaded);

		// Connection des objets
		GameObject pathfinder = GameObject.Find ("Pathfinder");
		((Pathfinder)pathfinder.GetComponent<Pathfinder> ()).map = MapLoaded;

		GameObject btn_save = GameObject.Find ("btn_save");
		((UnityEngine.UI.Button)btn_save.GetComponent<UnityEngine.UI.Button> ()).onClick.AddListener (() => {
			LoadManager.instance.savePrefabricatedMap ("test");
		});
	}

	void OnLevelWasLoaded() {
		switch (LoadManagerState) {
		case state.Inactive:
			Debug.Log ("PAS BON, mais on laisse passer");
			initWorld ();
			break;
		case state.StartNewMap:
			Debug.Log ("Génération de la map ;)");
			Debug.Log ("Type : " + LoadManager.instance.MapWidthParameter.ToString() + ", Length : " + LoadManager.instance.MapX.ToString() + ", Width : " + LoadManager.instance.MapY.ToString());
			initWorld ();
			break;
		case state.StartLoadedMap:
			Debug.Log ("Chargement d'une map préfabriqué");
			Debug.Log ("Map à charger :" + MapPrefabToLoad.ToString());
			loadPrefabricatedMap (MapPrefabToLoad);
			break;
		default:
			Debug.LogError ("Ca ne devrait pas arriver.");
			break;
		}
	}

	private GameManager initGame(Map worldMap) {
		GameObject newObject = Instantiate(gameManagerPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		GameManager gameSettings = newObject.GetComponent<GameManager> ();

		gameSettings.mouseManager = (GameObject.Find ("MouseManager")).GetComponent<MouseManager> ();
		gameSettings.endTurnButton = (GameObject.Find ("btn_roundEnd")).GetComponent<UnityEngine.UI.Button> ();
		gameSettings.panelHandler = (GameObject.Find ("HUDCanvas")).GetComponent<PanelHandler> ();
	
		gameSettings.map = worldMap;

		return gameSettings;
	}

	public void savePrefabricatedMap(string name) {
		if (!Directory.Exists (Application.persistentDataPath + "/PrefabricatedMaps")) {
			Directory.CreateDirectory (Application.persistentDataPath + "/PrefabricatedMaps");
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream saveFile = File.Open(Application.persistentDataPath + "/PrefabricatedMaps/" + name + ".map", FileMode.OpenOrCreate);

		// 2. Construct a SurrogateSelector object
		SurrogateSelector ss = new SurrogateSelector();

		Vector3SerializationSurrogate v3ss = new Vector3SerializationSurrogate();
		ss.AddSurrogate(typeof(Vector3),
			new StreamingContext(StreamingContextStates.All),
			v3ss);

		// 5. Have the formatter use our surrogate selector
		bf.SurrogateSelector = ss;

		Debug.Log ("Saving...");
		bf.Serialize (saveFile, MapLoaded.SaveMap ());
		Debug.Log (Application.persistentDataPath + "/PrefabricatedMaps/" + name + ".map");
		saveFile.Close ();
	}

	public void loadPrefabricatedMap(string path) {
		// LOADING FILE
		///////
		FileStream saveFile = File.OpenRead (path);
		BinaryFormatter bf = new BinaryFormatter();

		// 2. Construct a SurrogateSelector object
		SurrogateSelector ss = new SurrogateSelector();

		Vector3SerializationSurrogate v3ss = new Vector3SerializationSurrogate();
		ss.AddSurrogate(typeof(Vector3),
			new StreamingContext(StreamingContextStates.All),
			v3ss);

		// 5. Have the formatter use our surrogate selector
		bf.SurrogateSelector = ss;

		Debug.Log ("Loading...");
		MapFile saveMap = (MapFile)bf.Deserialize (saveFile);

		loadWorld (saveMap);
	}
}
