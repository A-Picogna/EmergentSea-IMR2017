using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialPanelHandler : MonoBehaviour {
	public TutorialManager gameManager;
	public TutorialMouseManager mouseManager;
	public GameObject panelHarbor;
	public GameObject panelTrade;
	public GameObject panelTradeAmount;
	public GameObject panelCrewMember;
	public GameObject panelHelp;
	public GameObject panelHelp2;
	public GameObject panelHelp3;

	public GameObject panelShip;
	public GameObject panelUnkown;
	private UnkownPanel upScript;

	public GameObject objectScrollListShip;
	private ShipScrollList scrollScript;

	public GameObject panelShipInfo;
	private ShipInfoPanel shipInfoScript;
	public GameObject panelShipCrew;
	private CrewMemberList shipCrewScript;

	private TradePanel tradePanelScript;

	private HelpPanel helpPanelScript;
	private HelpPanel helpPanelScript2;
	private HelpPanel helpPanelScript3;

	void Start () {
		upScript = panelUnkown.GetComponent<UnkownPanel>();
		scrollScript = objectScrollListShip.GetComponent<ShipScrollList>();
		shipInfoScript = panelShipInfo.GetComponent<ShipInfoPanel>();
		shipCrewScript = panelShipCrew.GetComponent<CrewMemberList>();
		tradePanelScript = panelTrade.GetComponent<TradePanel> ();
		helpPanelScript = panelHelp.GetComponent<HelpPanel> ();
		helpPanelScript2 = panelHelp2.GetComponent<HelpPanel> ();
		helpPanelScript3 = panelHelp3.GetComponent<HelpPanel> ();
	}

	// Show/hide modals
	public void showPanelHarbor() {
		panelHarbor.SetActive (true);
	}
	public void hidePanelHarbor() {
		panelHarbor.SetActive (false);
	}
	public void showPanelTrade() {
		tradePanelScript.showPanel ();
	}
	public void hidePanelTrade() {
		tradePanelScript.hidePanel ();
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
		tradePanelScript.hidePanel ();
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
		tradePanelScript.Setup (mouseManager.selectedUnit, toShip);
	}

	// Helper utilities
	public void refreshHelper() {
		helpPanelScript.refresh ();
	}
	public void changeTextHelper(int tp) {
		helpPanelScript.changeText (tp);
	}
	public void addInfoHelper(List<string> list) {
		helpPanelScript.addInfo (list);
	}

	public void showPanelHelper() {
		helpPanelScript.showPanel ();
	}
	public void hidePanelHelper() {
		helpPanelScript.hidePanel ();
	}

	public void refreshHelper2() {
		helpPanelScript2.refresh ();
	}
	public void changeTextHelper2(int tp) {
		helpPanelScript2.changeText (tp);
	}
	public void addInfoHelper2(List<string> list) {
		helpPanelScript2.addInfo (list);
	}

	public void showPanelHelper2() {
		helpPanelScript2.showPanel ();
	}
	public void hidePanelHelper2() {
		helpPanelScript2.hidePanel ();
	}

	public void hidePanelHelper3() {
		helpPanelScript3.hidePanel ();
	}
}
