// @author Yasmine Serot
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ShipScrollList : MonoBehaviour {

	public GameObject shipItemButton;

	private List<Ship> itemList;
	public Transform contentPanel;
	public Text fleetHeader;
	public ShipObjectPool buttonObjectPool;
	private Lang lang;

	public MouseManager mouseManager;
	public PanelHandler panelHandler;
	public ShipInfoPanel shipInfoPanel;

	void Start () {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		fleetHeader.text = lang.getString("fleet");
		itemList = new List<Ship> ();
		RefreshDisplay ();
	}

	public void RefreshDisplay() {
		RemoveButtons ();
		AddButtons ();
	}
	
	private void AddButtons() {
		foreach (var item in itemList) {
			GameObject newItemButton = buttonObjectPool.GetObject();
			ShipItemButton shipButton = newItemButton.GetComponent<ShipItemButton> ();
			shipButton.Setup (item, mouseManager, panelHandler, shipInfoPanel);
			newItemButton.transform.SetParent (contentPanel, false);
			newItemButton.transform.localScale = Vector3.one;
		}
	}

	private void RemoveButtons() {
		while (contentPanel.childCount > 0) {
			GameObject toRemove = transform.GetChild (0).gameObject;
			buttonObjectPool.ReturnObject (toRemove);
		}
	}

	public void AddItem(Ship itemToAdd) {
		itemList.Add (itemToAdd);
	}

	public void RemoveItem(Ship itemToRemove) {
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList [i] == itemToRemove) {
				itemList.RemoveAt (i);
				break;
			}
		}
	}

	public void RemoveAll() {
		itemList = new List<Ship> ();
	}
}
