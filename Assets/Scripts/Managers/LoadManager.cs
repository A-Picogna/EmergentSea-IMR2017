using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine.SceneManagement;


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
	public enum state { Inactive, StartNewMap, StartLoadedMap, LoadSave, StartEditor }

	// LoadManager Info
	public state LoadManagerState;

	// MAP INFORMATION
	public int nbOfSharks = 0; // For testing purposes
	public int MapWidthParameter = 2;
	public int DifficultyParameter = 0;
	public int GroundFrequencyParameter = 1;
	public int PortFrequencyParameter = 1;
	public int TreasureFrequencyParameter = 1;
	public int MapX = 0;
	public int MapY = 0;
	public int FleetSize = 2;
	public int goldAmountPerFleet = 200;

	//public int Parameter =;
	//public int Parameter =;
	public GameObject hexPrefab;
	public GameObject landPrefab;
	public GameObject seaPrefab;
	public GameObject mapPrefab;
    public GameObject harborPrefab;
    public GameObject coastPrefab;
	public GameObject gameManagerPrefab;
	public GameObject mapEditorPrefab;
	public GameObject saveLoadMenuPrefab;
	public GameObject loadingScreenPrefab;
	public GameObject CanvasPrefab;
	public GameObject NewMapPrefab;

	// Private variables
	private Map MapLoaded;
	private MapEditor MapLoadedEditor;
	public string MapPrefabToLoad;
	public string SaveToLoad;
	private int nbCasesTerre;
	private int nbCasesTresors;
	private MapFile MapAsStarted;
	private AsyncOperation async;
	public bool loadingAScene;
	private Coroutine cor;


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
			MapX = 20;
			MapY = 12;
			break;
		case 1:
			MapX = 28;
			MapY = 18;
			break;
		case 2:
			MapX = 33;
			MapY = 21;
			break;
		case 3:
			MapX = 40;
			MapY = 26;
			break;
		case 4:
			MapX = 52;
			MapY = 32;
			break;
		case 5:
			MapX = 64;
			MapY = 40;
			break;
		case 6:
			//Géré par NewPlayUI
			break;
		}
		// On recalcule le nombre de terre
		GroundFrequencyDropdownCallback(GroundFrequencyParameter);
		TreasureFrequencyDropdownCallback(GroundFrequencyParameter);
	}

	public void DifficultyDropdownCallback(int DifficultyType) {
		// 0: Par défaut
		DifficultyParameter = DifficultyType;
	}

	public void GroundFrequencyDropdownCallback(int GroundFrequencyType) {
		// 0 : Peu
		// 1 : Normale
		// 2 : Beaucoup
		GroundFrequencyParameter = GroundFrequencyType;

		// Calcul du nbcaseinit
		this.nbCasesTerre = (int)((Math.Sqrt(this.MapX*this.MapY))/2);
		// Application facteur
		switch (GroundFrequencyType) {
			case 0:
			this.nbCasesTerre = Mathf.RoundToInt(this.nbCasesTerre * 0.5f);
				break;
			case 1:
				break;
			case 2:
			this.nbCasesTerre = Mathf.RoundToInt(this.nbCasesTerre * 1.5f);
				break;
		}
		Debug.Log ("nbCasesTerre calculé = " + this.nbCasesTerre.ToString ());
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
		// 0 : Peu
		// 1 : Normale
		// 2 : Beaucoup
		TreasureFrequencyParameter = TreasureFrequencyType;

		// Calcul du nbcaseinit
		this.nbCasesTresors = Mathf.RoundToInt( (this.MapX*this.MapY) * 0.015f);
		// Application facteur
		switch (TreasureFrequencyType) {
		case 0:
			this.nbCasesTresors = Mathf.RoundToInt(this.nbCasesTresors * 0.5f);
			break;
		case 1:
			break;
		case 2:
			this.nbCasesTresors = Mathf.RoundToInt(this.nbCasesTresors * 1.5f);
			break;
		}
		Debug.Log ("nbCasesTresors calculé = " + this.nbCasesTresors.ToString ());
	}

	public void MapXSliderCallBack(float MapX) {
		this.MapX = Mathf.FloorToInt (MapX);
	}

	public void MapYSliderCallBack(float MapY) {
		this.MapY = Mathf.FloorToInt (MapY);
	}

	public void ShipNumberSliderCallBack(float number) {
		this.FleetSize = Mathf.FloorToInt (number);
	}

	public void GoldQuantitySliderCallBack(float number) {
		this.goldAmountPerFleet = Mathf.FloorToInt (number);
	}

	private Map initMap() {
		GameObject newObject = Instantiate(mapPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		newObject.name = newObject.name.Replace ("(Clone)", "").Trim ();

		Map mapSettings = newObject.GetComponent<Map>();
		//do additional initialization steps here

		mapSettings.hexPrefab = hexPrefab;
		mapSettings.landPrefab = landPrefab;
		mapSettings.seaPrefab = seaPrefab;
        mapSettings.harborPrefab = harborPrefab;
        mapSettings.coastPrefab = coastPrefab;

        mapSettings.height = MapY;
		mapSettings.width = MapX;

		mapSettings.nbCasesRemplinit = this.nbCasesTerre;
		mapSettings.nombreCasesTreasure = this.nbCasesTresors;

		mapSettings.LaunchMapGeneration ();

		return mapSettings;
	}

	private MapEditor initEditorMap() {
		//////////// POUR L'EDITEUR
		GameObject newObject = Instantiate(mapEditorPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		newObject.name = newObject.name.Replace ("(Clone)", "").Trim ();

		MapEditor mapSettings = newObject.GetComponent<MapEditor>();

		mapSettings.hexPrefab = hexPrefab;
		mapSettings.landPrefab = landPrefab;
		mapSettings.seaPrefab = seaPrefab;
		mapSettings.harborPrefab = harborPrefab;
		mapSettings.coastPrefab = coastPrefab;

		Debug.Log (MapX.ToString()+ " " + MapY.ToString ());
		mapSettings.height = MapY;
		mapSettings.width = MapX;


		return mapSettings;
	}

	private Map loadMap(MapFile saveMap) {
		GameObject newObject = Instantiate(mapPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		newObject.name = newObject.name.Replace ("(Clone)", "").Trim ();

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
		GameManager gameManager = initGame (MapLoaded);
		linkObjects (MapLoaded, gameManager);
	}

	private void loadWorld(MapFile saveMap) {
		MapLoaded = loadMap (saveMap);
		GameManager gameManager = initGame (MapLoaded);
		linkObjects (MapLoaded, gameManager);
		MapAsStarted = MapLoaded.SaveMap ();
	}

	private void loadSave(SaveFile save) {
		MapLoaded = loadMap (save.map);
		// Pour l'instant on refait un GameManager
		GameManager gameManager = initGame (MapLoaded);
		linkObjects (MapLoaded, gameManager);
		MapAsStarted = MapLoaded.SaveMap ();
	}

	private void initEditor() {
		//////////// POUR L'EDITEUR
		MapLoadedEditor = initEditorMap ();
		linkObjectsEditor ();
	}

	private void linkObjects(Map MapLoaded, GameManager gameManager) {
		GameObject mouseSettings = GameObject.Find ("MouseManager");
		((MouseManager)mouseSettings.GetComponent<MouseManager> ()).map = MapLoaded;

		// Connection des objets
		GameObject pathfinder = GameObject.Find ("Pathfinder");
		((Pathfinder)pathfinder.GetComponent<Pathfinder> ()).map = MapLoaded;

		GameObject btn_save = GameObject.Find ("btn_save");

		GameObject HUDCanvas = GameObject.Find ("HUDCanvas");
		PanelHandler panelHandler = HUDCanvas.GetComponent<PanelHandler> ();
		panelHandler.gameManager = gameManager;

		// Création d'un GameObject avec Canvas
		GameObject canvas = Instantiate(CanvasPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		canvas.transform.SetAsLastSibling	 ();

		// Instanciation de l'outil pour charger une carte
		GameObject saveLoadMenu = Instantiate(saveLoadMenuPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		saveLoadMenu.transform.SetParent(canvas.transform, false);
		//saveLoadMenu.transform.localScale = new Vector3 (2.0f, 2.0f, 2.0f);

		Debug.Log ("Linking save button...");
		((UnityEngine.UI.Button)btn_save.GetComponent<UnityEngine.UI.Button> ()).onClick.AddListener (() => {
			saveLoadMenu.SetActive (true);
		});

		Debug.Log ("Linking return button...");
		GameObject btn_return = GameObject.Find ("btn_load");
		((UnityEngine.UI.Button)btn_return.GetComponent<UnityEngine.UI.Button> ()).onClick.RemoveAllListeners ();
		((UnityEngine.UI.Button)btn_return.GetComponent<UnityEngine.UI.Button> ()).onClick.AddListener (() => {
			LoadManager.instance.BackToMenu();
		});
		// Instanciation du loading Screen
		//GameObject loadingScreen = Instantiate(loadingScreenPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		//loadingScreen.transform.SetParent(canvas.transform, false);
		//loadingScreen.SetActive (true);

		// Reconfiguration du loading Screen
		GameObject LoadButton = GameObject.Find ("LoadButton");
		//((UnityEngine.UI.Button)LoadButton.GetComponent<UnityEngine.UI.Button> ()).onClick.AddListener (() => {
		//	Debug.Log("Lancement nouvelle map");
		//	((SceneLoader)loadingScreen.GetComponent<SceneLoader> ()).NewMap();
		//});

		// Cacher le menu de chargement
		saveLoadMenu.SetActive (false);
		//loadingScreen.SetActive (false);
	}

	private void linkObjectsEditor() {
		GameObject btn_save = GameObject.Find ("btn_save");

		// Création d'un GameObject avec Canvas
		GameObject canvas = Instantiate(CanvasPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		canvas.transform.SetAsLastSibling();

		// Instanciation du menu pour nommer la sauvegarde
		GameObject NewMapMenu = Instantiate(NewMapPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		NewMapMenu.transform.SetParent(canvas.transform, false);
		//saveLoadMenu.transform.localScale = new Vector3 (2.0f, 2.0f, 2.0f);

		Debug.Log ("Linking save button...");
		((UnityEngine.UI.Button)btn_save.GetComponent<UnityEngine.UI.Button> ()).onClick.AddListener (() => {
			NewMapMenu.SetActive (true);
		});

		NewMapMenu.SetActive (false);
	}

	void OnLevelWasLoaded() {
		
		this.MapLoaded = null;
		this.MapLoadedEditor = null;
		this.MapAsStarted = null;

		switch (LoadManagerState) {
		case state.Inactive:
			Debug.Log ("Normalement je devrais revenir au menu, je fais rien.");
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
		case state.LoadSave:
			Debug.Log ("Chargement d'une sauvegarde ! :o");
			Debug.Log ("Sauvegarde à charger: " + SaveToLoad.ToString ());
			loadSave (SaveToLoad);
			break;
		case state.StartEditor:
			Debug.Log ("Chargement de l'éditeur !");
			initEditor ();
			break;
		default:
			Debug.LogError ("Ca ne devrait pas arriver.");
			break;
		}
	}

	private GameManager initGame(Map worldMap) {
		GameObject newObject = Instantiate(gameManagerPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
		newObject.name = newObject.name.Replace ("(Clone)", "").Trim ();

		GameManager gameSettings = newObject.GetComponent<GameManager> ();

		gameSettings.mouseManager = (GameObject.Find ("MouseManager")).GetComponent<MouseManager> ();
		gameSettings.endTurnButton = (GameObject.Find ("btn_roundEnd")).GetComponent<UnityEngine.UI.Button> ();
		gameSettings.textEndTurnNumber = (GameObject.Find ("txt_roundNumber")).GetComponent<UnityEngine.UI.Text> ();
		gameSettings.panelHandler = (GameObject.Find ("HUDCanvas")).GetComponent<PanelHandler> ();
	
		gameSettings.map = worldMap;

		gameSettings.FleetSize = this.FleetSize;
		gameSettings.GoldAmount = this.goldAmountPerFleet;


		return gameSettings;
	}

	public void savePrefabricatedMap(string name) {
		if (!Directory.Exists (GlobalVariables.pathMaps)) {
			Directory.CreateDirectory (GlobalVariables.pathMaps);
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream saveFile = File.Open(GlobalVariables.pathMaps + name + ".map", FileMode.OpenOrCreate);

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
		Debug.Log (GlobalVariables.pathMaps + name + ".map");
		saveFile.Close ();
	}

	public void savePrefabricatedMapEditor(string name) {
		//////////////// POUR L'EDITEUR
		if (!Directory.Exists (GlobalVariables.pathMaps)) {
			Directory.CreateDirectory (GlobalVariables.pathMaps);
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream saveFile = File.Open(GlobalVariables.pathMaps + name + ".map", FileMode.OpenOrCreate);

		// 2. Construct a SurrogateSelector object
		SurrogateSelector ss = new SurrogateSelector();

		Vector3SerializationSurrogate v3ss = new Vector3SerializationSurrogate();
		ss.AddSurrogate(typeof(Vector3),
			new StreamingContext(StreamingContextStates.All),
			v3ss);

		// 5. Have the formatter use our surrogate selector
		bf.SurrogateSelector = ss;

		Debug.Log ("Saving...");
		bf.Serialize (saveFile, MapLoadedEditor.SaveMap ());
		Debug.Log (GlobalVariables.pathMaps + name + ".map");
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

	public void save(string name) {
		if (!Directory.Exists (GlobalVariables.pathSaves)) {
			Directory.CreateDirectory (GlobalVariables.pathSaves);
		}
		BinaryFormatter bf = new BinaryFormatter();
		FileStream saveFile = File.Open(GlobalVariables.pathSaves + name + ".save", FileMode.OpenOrCreate);

		// 2. Construct a SurrogateSelector object
		SurrogateSelector ss = new SurrogateSelector();

		Vector3SerializationSurrogate v3ss = new Vector3SerializationSurrogate();
		ss.AddSurrogate(typeof(Vector3),
			new StreamingContext(StreamingContextStates.All),
			v3ss);

		// 5. Have the formatter use our surrogate selector
		bf.SurrogateSelector = ss;

		Debug.Log ("Saving save...");

		SaveFile saveObject = new SaveFile (MapLoaded.SaveMap ());

		bf.Serialize (saveFile, saveObject);
		Debug.Log (GlobalVariables.pathSaves + name + ".map");
		saveFile.Close ();
	}

	public void loadSave(string path) {
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
		SaveFile saveObject = (SaveFile)bf.Deserialize (saveFile);

		loadSave (saveObject);
	}

	public void BackToMenu(){
		this.LoadManagerState = LoadManager.state.Inactive;
		Time.timeScale = 1;
		SceneManager.LoadScene("main", LoadSceneMode.Single);
	}

	public void LoadSceneRoutine(string map) {
		StopAllCoroutines ();
		Debug.Log ("StartCoroutine");
		cor = StartCoroutine (LoadManager.instance.LoadSceneCoroutine(map));
		Debug.Log ("StartCoroutineFinished");
	}

	public IEnumerator LoadSceneCoroutine(string map) {
		// We wait just 3 seconds to feel the loading
		Debug.Log ("Start wait");
		yield return new WaitForSeconds (1);
		Debug.Log ("Wait is over");
		Debug.Log ("LoadSceneAsync");

		// Start an ansync operation to load the scene
		async = SceneManager.LoadSceneAsync(map, LoadSceneMode.Single);
		Debug.Log ("LoadSceneAsync launched");
		while (!async.isDone) {
			Debug.Log (async.progress.ToString());
			yield return null;
		}
		Debug.Log ("LoadSceneAsync done");
		loadingAScene = false;
		Debug.Log ("LoadingAScene false !");
		yield break;
		Debug.Log ("yield break !");
	}
}
