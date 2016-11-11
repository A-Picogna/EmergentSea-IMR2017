using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CrewItem {
	public Sprite type;
	public string pvLabel;
	public float percentagePV;
	public string xpLabel;
	public float percentageXP;
}

public class CrewMemberList : MonoBehaviour {

	public GameObject crewItemButton;

	public List<CrewItem> itemList;
	public Transform contentPanel;

	// Use this for initialization
	void Start () {
		RefreshDisplay ();
	}

	private void RefreshDisplay() {
		RemoveButtons ();
		AddButtons ();
	}

	private void AddButtons() {
		foreach (var item in itemList) {
			GameObject newItemButton = Instantiate (crewItemButton) as GameObject;
			CrewItemButton shipButton = newItemButton.GetComponent<CrewItemButton> ();
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

	private void AddItem(CrewItem itemToAdd) {
		itemList.Add (itemToAdd);
	}

	private void RemoveItem(CrewItem itemToRemove) {
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList [i] == itemToRemove) {
				itemList.RemoveAt (i);
				break;
			}
		}
	}

}
