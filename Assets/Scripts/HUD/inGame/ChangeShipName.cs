using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeShipName : MonoBehaviour {
	public MouseManager mouseManager;
	public PanelHandler panelHandler;

	public GameObject panel;
	public Button buttonChange;
	public Button buttonValidate;
	public Button buttonCancel;
	public InputField newShipName;

	// Use this for initialization
	void Start () {
		hidePanel ();
		buttonChange.onClick.AddListener (showPanel);
		buttonValidate.onClick.AddListener (changeName);
		buttonCancel.onClick.AddListener (hidePanel);
	}

	public void changeName() {
		mouseManager.selectedUnit.ShipName = newShipName.text;
		panelHandler.updateShipInfo ();
		panelHandler.refreshListShipDisplay ();
		hidePanel ();
	}
	public void showPanel() {
		newShipName.text = mouseManager.selectedUnit.ShipName;
		panel.SetActive (true);
	}
	public void hidePanel() {
		panel.SetActive (false);
	}
}
