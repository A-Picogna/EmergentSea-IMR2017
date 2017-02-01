 using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class NewPlayUI : MonoBehaviour {

	public GameObject MapXFeedbackText;
	public GameObject MapXSlider;
	public GameObject MapYFeedbackText;
	public GameObject MapYSlider;

	public GameObject MapXDescGroup;
	public GameObject MapXInputGroup;
	public GameObject MapYDescGroup;
	public GameObject MapYInputGroup;

	private Text MapXFeedbackTextComponent;
	private Slider MapXSliderComponent;
	private Text MapYFeedbackTextComponent;
	private Slider MapYSliderComponent;

	public GameObject MapTypeInputDropdown;

	public GameObject GroundFrequencyInputDropdown;
	public GameObject TreasureFrequencyInputDropdown;
	public GameObject PortFrequencyInputDropdown;

	public GameObject MapGenerationDescGroup;
	public GameObject MapGenerationInputGroup;
	public GameObject MapListDescGroup;
	public GameObject MapListInputGroup;

	public GameObject MapListInputDropdown;
	private Dropdown MapListDropdownComponent;

	public GameObject ShipNumberFeedbackText;
	public GameObject ShipNumberSlider;
	public GameObject GoldQuantityFeedbackText;
	public GameObject GoldQuantitySlider;
	public GameObject RetributionStrengthFeedbackText;
	public GameObject RetributionStrengthSlider;
    public GameObject LocalMultiplayerToggle;

	private Text ShipNumberTextComponent;
	private Slider ShipNumberSliderComponent;
	private Text GoldQuantityTextComponent;
	private Slider GoldQuantitySliderComponent;
	private Text RetributionStrengthTextComponent;
	private Slider RetributionStrengthSliderComponent;
    private Toggle LocalMultiplayerToggleComponent;

	public GameObject LaunchGameButton;

	private string[] mapList =  new string[] {};
	private bool isInit = false;

	void Start(){
		// Initilialize Components
		MapXFeedbackTextComponent = MapXFeedbackText.gameObject.GetComponent<Text> ();
		MapYFeedbackTextComponent = MapYFeedbackText.gameObject.GetComponent<Text> ();

		MapXSliderComponent = MapXSlider.gameObject.GetComponent<Slider> ();
		MapYSliderComponent = MapYSlider.gameObject.GetComponent<Slider> ();

		ShipNumberTextComponent = ShipNumberFeedbackText.gameObject.GetComponent<Text> ();
		GoldQuantityTextComponent = GoldQuantityFeedbackText.gameObject.GetComponent<Text> ();
		RetributionStrengthTextComponent = RetributionStrengthFeedbackText.gameObject.GetComponent<Text> ();

		ShipNumberSliderComponent = ShipNumberSlider.gameObject.GetComponent<Slider> ();
		GoldQuantitySliderComponent = GoldQuantitySlider.gameObject.GetComponent<Slider> ();
		RetributionStrengthSliderComponent = RetributionStrengthSlider.gameObject.GetComponent<Slider> ();
	    LocalMultiplayerToggleComponent = LocalMultiplayerToggle.gameObject.GetComponent<Toggle>();

		populateMapList ();
		updateValuesMap ();
		updateValuesGame ();

		MapTypeInputDropdownCallback(0);
		isInit = true;
	}

	void OnEnable() {
		if (isInit == true) {
			MapTypeInputDropdownCallback ((MapTypeInputDropdown.GetComponent<Dropdown> ()).value);
		}
	}

	public void OnMapXSlideValueChanged(float number) {
		if(MapXFeedbackTextComponent != null){
			MapXFeedbackTextComponent.text = number.ToString ();
		}
	}

	public void OnMapYSlideValueChanged(float number) {
		if (MapYFeedbackTextComponent != null) {
			MapYFeedbackTextComponent.text = number.ToString ();
		}
	}

	public void OnShipNumberSlideValueChanged(float number) {
		if (ShipNumberTextComponent != null) {
			ShipNumberTextComponent.text = number.ToString ();
		}
	}

	public void OnGoldQuantitySlideValueChanged(float number) {
		if (GoldQuantityTextComponent != null) {
			GoldQuantityTextComponent.text = number.ToString ();
		}
	}

	public void OnRetributionStrengthSlideValueChanged(float number) {
		if (RetributionStrengthTextComponent != null) {
			RetributionStrengthTextComponent.text = number.ToString ();
		}
	}

	public void MapWidthDropdownCallback(int WidthType) {
		LoadManager.instance.MapWidthDropdownCallback (WidthType);
		if (WidthType == 6) {
		
			MapXDescGroup.gameObject.SetActive (true);
			MapXInputGroup.gameObject.SetActive (true);
			MapYDescGroup.gameObject.SetActive (true);
			MapYInputGroup.gameObject.SetActive (true);


		} else {
			
			MapXDescGroup.gameObject.SetActive (false);
			MapXInputGroup.gameObject.SetActive (false);
			MapYDescGroup.gameObject.SetActive (false);
			MapYInputGroup.gameObject.SetActive (false);
		
		}
	}

	public void MapTypeInputDropdownCallback(int MapType) {
		//On change l'interface en fonction du paramêtre
		bool MapGenerationIsActive = false;
		if (mapList.Length <= 0) {
			(MapTypeInputDropdown.GetComponent<Dropdown> ()).value = 1;
			MapType = 1;
		}
		switch (MapType) {
		case 0: //Préfabriquée
			MapGenerationIsActive = false;
			LoadManager.instance.LoadManagerState = LoadManager.state.StartLoadedMap;
			updateMapListChoiceCallback (0);
			break;
		case 1: //Générée
			MapGenerationIsActive = true;
			LoadManager.instance.LoadManagerState = LoadManager.state.StartNewMap;
			break;
		}
		MapGenerationDescGroup.SetActive(MapGenerationIsActive);
		MapGenerationInputGroup.SetActive(MapGenerationIsActive);
		MapListDescGroup.SetActive(!MapGenerationIsActive);
		MapListInputGroup.SetActive(!MapGenerationIsActive);

	}

	public void DifficultyDropdownCallback(int DifficultyType) {
		LoadManager.instance.DifficultyDropdownCallback(DifficultyType);
	}

	public void GroundFrequencyDropdownCallback(int GroundFrequencyType) {
		LoadManager.instance.GroundFrequencyDropdownCallback(GroundFrequencyType);
	}

	public void PortFrequencyDropdownCallback(int PortFrequencyType) {
		LoadManager.instance.PortFrequencyDropdownCallback(PortFrequencyType);
	}

	public void TreasureFrequencyDropdownCallback(int TreasureFrequencyType) {
		LoadManager.instance.TreasureFrequencyDropdownCallback (TreasureFrequencyType);
	}

	public void MapXSliderCallBack(float MapX) {
		LoadManager.instance.MapXSliderCallBack (MapX);
	}

	public void MapYSliderCallBack(float MapY) {
		LoadManager.instance.MapYSliderCallBack (MapY);
	}

	public void ShipNumberSliderCallBack(float number) {
		LoadManager.instance.ShipNumberSliderCallBack(number);
	}

	public void GoldQuantitySliderCallBack(float number) {
		LoadManager.instance.GoldQuantitySliderCallBack (number);
	}

	public void RetributionStrengthSliderCallBack(float number) {
		LoadManager.instance.RetributionStrengthSliderCallBack (number); 
	}

	public void LocalMultiplayerCallback(bool LocalMultiplayer)
	{
		LoadManager.instance.LocalMultiplayerCallback (LocalMultiplayer);
	}

	private void populateMapList() {
		try {
			mapList = Directory.GetFiles (GlobalVariables.pathMaps);
		}
		catch (DirectoryNotFoundException) {
			Directory.CreateDirectory (GlobalVariables.pathMaps);
			mapList = Directory.GetFiles (GlobalVariables.pathMaps);
		}
		MapListDropdownComponent = MapListInputDropdown.GetComponent<Dropdown> ();

		MapListDropdownComponent.ClearOptions ();

		List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

		Dropdown.OptionData odbuffer;
		foreach (string mapfile in mapList) {
			odbuffer = new Dropdown.OptionData ();
			odbuffer.text = Path.GetFileName(mapfile);
			options.Add (odbuffer);
		}

		MapListDropdownComponent.AddOptions (options);
	}

	public void updateMapListChoiceCallback(int number) {
		if (mapList.Length != 0) {
			LoadManager.instance.MapPrefabToLoad = mapList [number];
		}
	}

	private void updateValuesMap() {
		MapXFeedbackTextComponent.text = LoadManager.instance.MapX.ToString ();
		MapYFeedbackTextComponent.text = LoadManager.instance.MapY.ToString ();

		MapXSliderComponent.value = LoadManager.instance.MapX;
		MapYSliderComponent.value = LoadManager.instance.MapY;


		(GroundFrequencyInputDropdown.GetComponent<Dropdown> ()).value = LoadManager.instance.GroundFrequencyParameter;
		(TreasureFrequencyInputDropdown.GetComponent<Dropdown> ()).value = LoadManager.instance.TreasureFrequencyParameter;
		(PortFrequencyInputDropdown.GetComponent<Dropdown> ()).value = LoadManager.instance.PortFrequencyParameter;
	    (LocalMultiplayerToggle.GetComponent<Toggle>()).isOn = LoadManager.instance.LocalMultiplayer;
	}

	private void updateValuesGame() {
		ShipNumberTextComponent.text = LoadManager.instance.FleetSize.ToString();
		GoldQuantityTextComponent.text = LoadManager.instance.goldAmountPerFleet.ToString();
		RetributionStrengthTextComponent.text = LoadManager.instance.retributionStrength.ToString ();

		ShipNumberSliderComponent.value = LoadManager.instance.FleetSize;
		GoldQuantitySliderComponent.value = LoadManager.instance.goldAmountPerFleet;
		RetributionStrengthSliderComponent.value = LoadManager.instance.retributionStrength;
	    LocalMultiplayerToggleComponent.isOn = LoadManager.instance.LocalMultiplayer;
	}
}
