// @author Yasmine Serot
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Item {
	public string shipName;
	public float percentageQE;
	public int goldAmount;
	public int foodAmount;
}

public class ShipScrollList : MonoBehaviour {

	public List<Item> itemList;
	public Transform contentPanel;
	public Text fleetHeader;
	public ShipObjectPool objectPool;

	void Start () {
		RefreshDisplay ();
	}

	private void RefreshDisplay() {
		fleetHeader.text = "Flotte";
		RemoveButtons ();
		AddButtons ();
	}
	
	private void AddButtons() {
		foreach (var item in itemList) {
			GameObject newItemButton = objectPool.GetObject();
			ShipItemButton shipButton = newItemButton.GetComponent<ShipItemButton> ();
			shipButton.Setup (item, this);
			newItemButton.transform.SetParent (contentPanel, false);
		}
	}

	private void RemoveButtons() {
		while (contentPanel.childCount > 0) {
			GameObject toRemove = transform.GetChild (0).gameObject;
			objectPool.ReturnObject (toRemove);
		}
	}

	private void AddItem(Item itemToAdd) {
		itemList.Add (itemToAdd);
	}

	private void RemoveItem(Item itemToRemove) {
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList [i] == itemToRemove) {
				itemList.RemoveAt (i);
				break;
			}
		}
	}
}
