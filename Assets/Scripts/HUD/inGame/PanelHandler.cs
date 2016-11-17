using UnityEngine;
using System.Collections;

public class PanelHandler : MonoBehaviour {
	public GameObject panelHarbor;
	public GameObject panelTrade;
	public GameObject panelCrewMember;

	public GameObject panelShip;
	public GameObject panelUnkown;
	private UnkownPanel upScript;

	void Start () {
		upScript = panelUnkown.GetComponent<UnkownPanel>();
	}
	
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
	public void hideAllModals() {
		panelHarbor.SetActive (false);
		panelTrade.SetActive (false);
		panelCrewMember.SetActive (false);
	}

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
}
