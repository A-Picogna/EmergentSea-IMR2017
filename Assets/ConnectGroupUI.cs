using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ConnectGroupUI : MonoBehaviour {

	public GameObject IPAdressInputFieldGameObject;
	private InputField IPAdressInputField;

	// Use this for initialization
	void Start () {
		IPAdressInputField = IPAdressInputFieldGameObject.GetComponent<InputField> ();
	}
	
	public void CallbackLaunchConnection() {

		LoadManager.instance.serverAdress = IPAdressInputField.text;
		LoadManager.instance.LoadManagerState = LoadManager.state.ConnectServer;

	}
}
