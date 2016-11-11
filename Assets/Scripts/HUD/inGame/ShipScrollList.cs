﻿// @author Yasmine Serot
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class ShipItem {
	public string shipName;
	public float percentageQE;
	public int goldAmount;
	public int foodAmount;
}

public class ShipScrollList : MonoBehaviour {

	public GameObject shipItemButton;

	public List<ShipItem> itemList;
	public Transform contentPanel;
	public Text fleetHeader;
	private Lang lang;

	void Start () {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		fleetHeader.text = lang.getString("fleet");
		RefreshDisplay ();
	}

	private void RefreshDisplay() {
		RemoveButtons ();
		AddButtons ();
	}
	
	private void AddButtons() {
		foreach (var item in itemList) {
			GameObject newItemButton = Instantiate (shipItemButton) as GameObject;
			ShipItemButton shipButton = newItemButton.GetComponent<ShipItemButton> ();
			shipButton.Setup (item);
			newItemButton.transform.SetParent (contentPanel, false);
		}
	}

	private void RemoveButtons() {
		while (contentPanel.childCount > 0) {
			GameObject toRemove = transform.GetChild (0).gameObject;
			Destroy(toRemove);
		}
	}

	private void AddItem(ShipItem itemToAdd) {
		itemList.Add (itemToAdd);
	}

	private void RemoveItem(ShipItem itemToRemove) {
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList [i] == itemToRemove) {
				itemList.RemoveAt (i);
				break;
			}
		}
	}
}
