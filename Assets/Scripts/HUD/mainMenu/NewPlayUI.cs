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

	public GameObject MapGenerationDescGroup;
	public GameObject MapGenerationInputGroup;
	public GameObject MapListDescGroup;
	public GameObject MapListInputGroup;

	public GameObject MapListInputDropdown;
	private Dropdown MapListDropdownComponent;

	public GameObject LaunchGameButton;

	private string[] mapList;

	void Start(){
		// Initilialize Components
		MapXFeedbackTextComponent = MapXFeedbackText.gameObject.GetComponent<Text> ();
		MapYFeedbackTextComponent = MapYFeedbackText.gameObject.GetComponent<Text> ();

		MapXSliderComponent = MapXSlider.gameObject.GetComponent<Slider> ();
		MapYSliderComponent = MapYSlider.gameObject.GetComponent<Slider> ();

		populateMapList ();

		MapTypeInputDropdownCallback(0);
		updateMapListChoiceCallback (0);
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

	public void MapWidthDropdownCallback(int WidthType) {
		if (WidthType == 6) {
		
			MapXDescGroup.gameObject.SetActive (true);
			MapXInputGroup.gameObject.SetActive (true);
			MapYDescGroup.gameObject.SetActive (true);
			MapYInputGroup.gameObject.SetActive (true);

			MapXFeedbackTextComponent.text = LoadManager.instance.MapX.ToString ();
			MapYFeedbackTextComponent.text = LoadManager.instance.MapY.ToString ();

			MapXSliderComponent.value = LoadManager.instance.MapX;
			MapYSliderComponent.value = LoadManager.instance.MapY;
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
		switch (MapType) {
		case 0: //Préfabriquée
			MapGenerationIsActive = false;
			LoadManager.instance.LoadManagerState = LoadManager.state.StartLoadedMap;
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

	private void populateMapList() {
		mapList = Directory.GetFiles (Application.persistentDataPath + "/PrefabricatedMaps/");
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

	private void updateMapListChoiceCallback(int number) {
		LoadManager.instance.MapPrefabToLoad = mapList [number];
	}
}
