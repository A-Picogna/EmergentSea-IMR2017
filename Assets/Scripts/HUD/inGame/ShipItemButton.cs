// @author Yasmine Serot
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipItemButton : MonoBehaviour {
	public Button button;
	public Text shipName;
	public Slider percentageQE;
	public Text goldAmount;
	public Text foodAmount;

	private MouseManager mouseManager;
	private PanelHandler panelHandler;
	private ShipInfoPanel shipInfoPanel;
	private Ship item;

	void Start() {
		button.onClick.AddListener (HandleClick);
	}

	public void Setup (Ship currentItem, MouseManager mm, PanelHandler ph, ShipInfoPanel sip) {
		item = currentItem;
		shipName.text = item.ShipName;
		percentageQE.value = (item.EnergyQuantity*100)/item.calculateEQmax();
		goldAmount.text = amountformatter(item.Gold); // Utility function to form correct numbers
		foodAmount.text = amountformatter(item.Food);

		mouseManager = mm;
		panelHandler = ph;
		shipInfoPanel = sip;
	}

	private string amountformatter(int amount) {
		if (amount < Mathf.Pow(10, 3)) { 
			return amount.ToString();
		} else if (amount < Mathf.Pow(10, 6)) {
			int am = amount / (int)Mathf.Pow (10, 2);
			return (float)am / 10 + "k";
			//return amount / (int)Mathf.Pow(10, 3) + "k";
		} else if (amount < Mathf.Pow(10, 9)) {
			return amount / (int)Mathf.Pow(10, 6) + "M";
		} else if (amount < Mathf.Pow(10, 12)) {
			return amount / (int)Mathf.Pow(10, 9) + "G";
		} else {
			return amount / (int)Mathf.Pow(10, 12) + "T";
		}
	}

	public void HandleClick() {
		mouseManager.selectedUnit = item;
		GameObject cam = GameObject.Find ("Main Camera");
		cam.transform.position = new Vector3 (
			mouseManager.map.graph [item.ShipX, item.ShipY].worldPos.x, 
			cam.transform.position.y,
			mouseManager.map.graph [item.ShipX, item.ShipY].worldPos.z - 10
		);																		
		shipInfoPanel.updateShip (item);
		panelHandler.showPanelShip ();
		panelHandler.removeAllCrewMember ();
		foreach(CrewMember crewMember in item.Crew){
			panelHandler.addCrewMember (crewMember);
		}
		panelHandler.refreshCrewMemberDisplay ();
	}
}
