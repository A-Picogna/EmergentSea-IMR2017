using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class HarborPanel : MonoBehaviour {
	public GameObject panel;
	public Button button;
	public Button buttonTest;

	public Text harborHeader;
	public Text harborDescription;
	public Text inn;
	public Text storehouse;
	public Text warehouse;
	public Text slums;
	public Text darkArtsAcademy;
	public Text shipyard;
	private Lang lang;

	void Start () {
		panel.SetActive (false);
		button.onClick.AddListener (hidePanel);
		buttonTest.onClick.AddListener (showPanel);

		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		harborHeader.text = lang.getString("harbor");
		harborDescription.text = lang.getString("harbor_description");
		inn.text = lang.getString("inn");
		storehouse.text = lang.getString("storehouse");
		warehouse.text = lang.getString("warehouse");
		slums.text = lang.getString("slums");
		darkArtsAcademy.text = lang.getString("dark_arts_academy");
		shipyard.text = lang.getString("shipyard");
	}

	public void showPanel() {
		panel.SetActive (true);
	}

	public void hidePanel() {
		panel.SetActive (false);
	}
}
