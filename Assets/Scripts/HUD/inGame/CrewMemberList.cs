using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class CrewMemberList : MonoBehaviour {

	public GameObject crewItemButton;

	private List<CrewMember> itemList;
	public Transform panelLeft;
	public Transform panelRight;

	public CrewMemberObjectPool buttonObjectPool;
	public EmptyObjectPool emptyObjectPool;

	// Use this for initialization
	void Start () {
		itemList = new List<CrewMember> ();
		RefreshDisplay ();
	}

	public void RefreshDisplay() {
		RemoveButtons ();
		AddButtons ();
	}

	private void AddButtons() {
		int i;
		for (i = 0; i < itemList.Count; i++) {
			CrewMember item = itemList [i];
			GameObject newItemButton = buttonObjectPool.GetObject();
			CrewItemButton crewButton = newItemButton.GetComponent<CrewItemButton> ();
			crewButton.Setup (item);
			if (i < 4)
				newItemButton.transform.SetParent (panelLeft, false);
			else 
				newItemButton.transform.SetParent (panelRight, false);
			newItemButton.transform.localScale = Vector3.one;
		}
		for (int j = i; j < 8; j++) {
			GameObject newItemButton = emptyObjectPool.GetObject();
			if (j < 4)
				newItemButton.transform.SetParent (panelLeft, false);
			else 
				newItemButton.transform.SetParent (panelRight, false);
			newItemButton.transform.localScale = Vector3.one;
		}
		/*
		foreach (var item in itemList) {
			GameObject newItemButton = buttonObjectPool.GetObject();
			CrewItemButton shipButton = newItemButton.GetComponent<CrewItemButton> ();
			shipButton.Setup (item);
			newItemButton.transform.SetParent (contentPanel, false);

			//shipButton.Setup (item, mouseManager, panelHandler, shipInfoPanel);
		}*/
	}

	private void RemoveButtons() {
		for (int i = 3; i >= 0; i--) {
			GameObject toRemove = panelLeft.GetChild (i).gameObject;
			if (toRemove.GetComponent<Button> ())
				buttonObjectPool.ReturnObject (toRemove);
			else
				emptyObjectPool.ReturnObject (toRemove);
			toRemove = panelRight.GetChild (i).gameObject;
			if (toRemove.GetComponent<Button> ())
				buttonObjectPool.ReturnObject (toRemove);
			else
				emptyObjectPool.ReturnObject (toRemove);
		}
	}

	public void AddItem(CrewMember itemToAdd) {
		itemList.Add (itemToAdd);
	}

	public void RemoveItem(CrewMember itemToRemove) {
		for (int i = 0; i < itemList.Count; i++) {
			if (itemList [i] == itemToRemove) {
				itemList.RemoveAt (i);
				break;
			}
		}
	}

	public void RemoveAll() {
		itemList = new List<CrewMember> ();
	}

}
