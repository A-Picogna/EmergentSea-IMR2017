using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelHandler : MonoBehaviour {
	public GameManager gameManager;
	public MouseManager mouseManager;
	public GameObject panelHarbor;
	public GameObject panelTrade;
	public GameObject panelTradeAmount;
	public GameObject panelCrewMember;

	public GameObject panelShip;
	public GameObject panelUnkown;
	private UnkownPanel upScript;

	public GameObject objectScrollListShip;
	private ShipScrollList scrollScript;

	public GameObject panelShipInfo;
	private ShipInfoPanel shipInfoScript;
	public GameObject panelShipCrew;
	private CrewMemberList shipCrewScript;

	void Start () {
		upScript = panelUnkown.GetComponent<UnkownPanel>();
		scrollScript = objectScrollListShip.GetComponent<ShipScrollList>();
		shipInfoScript = panelShipInfo.GetComponent<ShipInfoPanel>();
		shipCrewScript = panelShipCrew.GetComponent<CrewMemberList>();
	}

	// Show/hide modals
	public void showPanelHarbor() {
		panelHarbor.SetActive (true);
	}
	public void hidePanelHarbor() {
		panelHarbor.SetActive (false);
	}
	public void showPanelTrade() {
		panelTrade.SetActive (true);
	}
	public void hidePanelTrade() {
		panelTrade.SetActive (false);
	}
	public void showPanelCrewMember() {
		panelCrewMember.SetActive (true);
	}
	public void hidePanelCrewMember() {
		panelCrewMember.SetActive (false);
	}
	public void hidePanelTradeamount() {
		panelTradeAmount.SetActive (false);
	}
	public void hideAllModals() {
		panelHarbor.SetActive (false);
		panelTrade.SetActive (false);
		panelCrewMember.SetActive (false);
		panelTradeAmount.SetActive (false);
	}

	// Show/hide bottom
	public void showPanelShip() {
		panelShip.SetActive (true);
	}
	public void hidePanelShip() {
		panelShip.SetActive (false);
	}
	public void showPanelUnkown() {
		panelUnkown.SetActive (true);
	}
	public void hidePanelUnkown() {
		panelUnkown.SetActive (false);
	}
	public void hideAllBottom() {
		hidePanelShip ();
		hidePanelUnkown ();
	}

	// Chose what to show bottom
	public void initPanelEnnemyShip() {
		panelUnkown.SetActive (true);
		upScript.InitEnnemyShip ();
	}
	public void initPanelTreasure() {
		panelUnkown.SetActive (true);
		upScript.InitTreasure ();
	}
	public void initPanelHarbor() {
		panelUnkown.SetActive (true);
		upScript.InitHarbor ();
	}

	// Update fleet list
	public void addShip(Ship ship) {
		scrollScript.AddItem(ship);
	}
	public void removeAllShip() {
		scrollScript.RemoveAll();
	}
	public void refreshListShipDisplay() {
		scrollScript.RefreshDisplay();
	}

	// Update Ship bottom
	public void updateShipInfo() {
		shipInfoScript.updateShip (mouseManager.selectedUnit);
	}

	// Update crew member bottom
	public void addCrewMember(CrewMember crewMember) {
		shipCrewScript.AddItem(crewMember);
	}
	public void removeAllCrewMember() {
		shipCrewScript.RemoveAll();
	}
	public void refreshCrewMemberDisplay() {
		shipCrewScript.RefreshDisplay();
	}

	// Update Ship
	public void updateShip() {
		List<Ship> currentPlayerFleet = gameManager.currentPlayer.Fleet;
		if (mouseManager.selectedUnit != null && mouseManager.selectedUnit.Crew != null) {
			List<CrewMember> currentShipCrew = mouseManager.selectedUnit.Crew;
			removeAllShip ();
			foreach (Ship shipItem in currentPlayerFleet) {
				addShip (shipItem);
			}
			refreshListShipDisplay ();

			updateShipInfo ();
			removeAllCrewMember ();
			foreach (CrewMember crewMember in currentShipCrew) {
				addCrewMember (crewMember);
			}
			refreshCrewMemberDisplay ();
		}
		showPanelShip ();
	}

	// Prepare trade
	public void initTrade(Ship toShip) {
		TradePanel tradePanelScript = panelTrade.GetComponent<TradePanel> ();
		tradePanelScript.Setup (mouseManager.selectedUnit, toShip);
	}
}
