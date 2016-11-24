using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class TradePanel : MonoBehaviour {
	public GameObject panel;
	public Button button;
	public Text header;

	private Lang lang;

	// Use this for initialization
	void Start () {
		panel.SetActive (false);
		button.onClick.AddListener (hidePanel);
		refreshPanel ();
	}

	public void refreshPanel() {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		header.text = lang.getString("trade");
	}
	
	public void showPanel() {
		panel.SetActive (true);
	}

	public void hidePanel() {
		panel.SetActive (false);
	}
}
