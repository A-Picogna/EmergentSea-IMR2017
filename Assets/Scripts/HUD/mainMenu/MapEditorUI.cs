using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class MapEditorUI : MonoBehaviour
{
	public GameObject MapXFeedbackText;
	public GameObject MapXSlider;
	public GameObject MapYFeedbackText;
	public GameObject MapYSlider;

	private Text MapXFeedbackTextComponent;
	private Slider MapXSliderComponent;
	private Text MapYFeedbackTextComponent;
	private Slider MapYSliderComponent;

	public GameObject MapGenerationDescGroup;
	public GameObject MapGenerationInputGroup;

	public GameObject MapListInputDropdown;
	private Dropdown MapListDropdownComponent;

	public GameObject MapTypeInputDropdown;
	public GameObject MapListDescGroup;
	public GameObject MapListInputGroup;

	private string[] mapList =  new string[] {};
	private bool isInit = false;

	void Start() {
		// Initilialize Components
		//Debug.Log("Test");
		MapXFeedbackTextComponent = MapXFeedbackText.gameObject.GetComponent<Text> (); //
		MapYFeedbackTextComponent = MapYFeedbackText.gameObject.GetComponent<Text> ();

		MapXSliderComponent = MapXSlider.gameObject.GetComponent<Slider> ();
		MapYSliderComponent = MapYSlider.gameObject.GetComponent<Slider> ();

		populateMapList ();
		updateValuesMap ();

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
			LoadManager.instance.LoadManagerState = LoadManager.state.LoadMapEditor;
			updateMapListChoiceCallback (0);
			break;
		case 1: //Générée
			MapGenerationIsActive = true;
			LoadManager.instance.LoadManagerState = LoadManager.state.StartEditor;
			break;
		}
		MapGenerationDescGroup.SetActive(MapGenerationIsActive);
		MapGenerationInputGroup.SetActive(MapGenerationIsActive);
		MapListInputGroup.SetActive(!MapGenerationIsActive);
		MapListDescGroup.SetActive(!MapGenerationIsActive);
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
	}
}

