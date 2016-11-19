using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ParameterUI : MonoBehaviour {

	public GameObject BrightnessSlider;

	public Color ambientDarkest;
	public Color ambientLightest;

	// Use this for initialization
	void Start () {
	
	}

	public void OnBrightnessSliderValueChanged(float number) {
		Debug.Log ("Number " + number.ToString ());
		RenderSettings.ambientLight = Color.Lerp(ambientDarkest, ambientLightest, number);
	}
}
