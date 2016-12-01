using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

	void Start() {
		// Initilialize Components
		Debug.Log("Start Routine");
		MapXFeedbackTextComponent = MapXFeedbackText.gameObject.GetComponent<Text> (); //
		MapYFeedbackTextComponent = MapYFeedbackText.gameObject.GetComponent<Text> ();

		MapXSliderComponent = MapXSlider.gameObject.GetComponent<Slider> ();
		MapYSliderComponent = MapYSlider.gameObject.GetComponent<Slider> ();
		Debug.Log("Start Routine");

		init ();
	}

	void OnEnable() {
		Start ();
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

	public void init() {
		// On préviens le loadmanager qu'on lance l'éditeur de carte
		LoadManager.instance.LoadManagerState = LoadManager.state.StartEditor;
		// On initialise le Formulaire
		initForm();
	}

	public void initForm() {
		MapXFeedbackTextComponent.text = LoadManager.instance.MapX.ToString ();
		MapYFeedbackTextComponent.text = LoadManager.instance.MapY.ToString ();

		MapXSliderComponent.value = LoadManager.instance.MapX;
		MapYSliderComponent.value = LoadManager.instance.MapY;
	}
}

