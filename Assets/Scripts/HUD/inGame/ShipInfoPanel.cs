using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class ShipInfoPanel : MonoBehaviour {
	public Text shipName;
	public Slider percentageQE;
	public Text percentageQEText;
	public Text goldAmount;
	public Text foodAmount;
	public Text fishingButtonText;
	private Lang lang;

	void Start () {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		fishingButtonText.text = lang.getString("fishing");
	}
}
