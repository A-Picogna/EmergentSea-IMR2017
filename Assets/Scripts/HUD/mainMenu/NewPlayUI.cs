using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

	void Start(){
		// Initilialize Components
		MapXFeedbackTextComponent = MapXFeedbackText.gameObject.GetComponent<Text> ();
		MapYFeedbackTextComponent = MapYFeedbackText.gameObject.GetComponent<Text> ();

		MapXSliderComponent = MapXSlider.gameObject.GetComponent<Slider> ();
		MapYSliderComponent = MapYSlider.gameObject.GetComponent<Slider> ();

	}

	public void OnMapXSlideValueChanged(float number) {
		MapXFeedbackTextComponent.text = number.ToString ();
	}

	public void OnMapYSlideValueChanged(float number) {
		MapYFeedbackTextComponent.text = number.ToString ();
	}

	public void MapWidthDropdownCallback(int WidthType) {
		if (WidthType == 6) {
		
			MapXDescGroup.gameObject.SetActive (true);
			MapXInputGroup.gameObject.SetActive (true);
			MapYDescGroup.gameObject.SetActive (true);
			MapYInputGroup.gameObject.SetActive (true);

			MapXFeedbackTextComponent.text = MapManager.instance.MapX.ToString ();
			MapYFeedbackTextComponent.text = MapManager.instance.MapY.ToString ();

			MapXSliderComponent.value = MapManager.instance.MapX;
			MapYSliderComponent.value = MapManager.instance.MapY;
		} else {
			
			MapXDescGroup.gameObject.SetActive (false);
			MapXInputGroup.gameObject.SetActive (false);
			MapYDescGroup.gameObject.SetActive (false);
			MapYInputGroup.gameObject.SetActive (false);
		
		}
	}
}
