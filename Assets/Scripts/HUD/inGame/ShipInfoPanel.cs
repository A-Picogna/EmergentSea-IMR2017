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

	public GameManager gameManager;
	public PanelHandler panelHandler;

	private Lang lang;
	private Ship item;

	void Start () {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
	}

	public void updateShip(Ship currentShip) {
		item = currentShip;
		shipName.text = currentShip.ShipName;
		percentageQE.value = (currentShip.EnergyQuantity*100)/currentShip.calculateEQmax();
		percentageQEText.text = currentShip.EnergyQuantity + "/" + currentShip.calculateEQmax();
		goldAmount.text = currentShip.Gold + " " + lang.getString ("gold");
		foodAmount.text = currentShip.Food + " " + lang.getString ("food");
	}
}
