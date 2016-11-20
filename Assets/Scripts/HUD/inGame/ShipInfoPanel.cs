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
	}

	public void updateShip(Ship currentShip) {
		shipName.text = currentShip.ShipName;
		percentageQE.value = (currentShip.EnergyQuantity*100)/currentShip.calculateEQmax();
		percentageQEText.text = currentShip.EnergyQuantity + "/" + currentShip.calculateEQmax();
		goldAmount.text = currentShip.Gold + " " + lang.getString ("gold");
		foodAmount.text = currentShip.Food + " " + lang.getString ("food");
		fishingButtonText.text = lang.getString("fishing");
	}
}
