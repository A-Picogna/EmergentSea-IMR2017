using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class UnkownPanel : MonoBehaviour {
	public GameObject panel;
	public Image type;
	public Text label;
	public Text description;
	private Lang lang;

	void Start () {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		panel.SetActive (false);
	}

	public void InitEnnemyShip() {
		label.text = lang.getString("ennemy_ship");
		description.text = lang.getString("ennemy_ship_description");
	}

	public void InitTreasure() {
		label.text = lang.getString("treasure");
		description.text = lang.getString("treasure_description");
	}

	public void InitHarbor() {
		label.text = lang.getString("harbor");
		description.text = lang.getString("harbor_description");
	}
}
