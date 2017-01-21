using UnityEngine;
using System.Collections;

public class MultiplayerMenuUI : MonoBehaviour {

	// Use this for initialization
	void OnEnable () {
		LoadManager.instance.multiplayerMode = true;
	}

	public void CallbackDisableMenu() {
		LoadManager.instance.multiplayerMode = false;
		gameObject.SetActive (false);
	}
}
