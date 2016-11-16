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
	private Lang lang;

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
}
