using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CrewHealthPanel : MonoBehaviour {
	public GameObject panel;
	public Text label;
	public Button cancel;
	public Text goldAmount;
	public Button fullHealth;
	public Button confirm;

	private List<CrewMember> itemList;
	private int cost;
	private int lpSum;
	public Transform panelLeft;
	public Transform panelRight;

	public CrewMemberHealthObjectPool buttonObjectPool;
	public EmptyObjectPool emptyObjectPool;

	public Ship item;
	public PanelHandler panelHandler;
	private Lang lang;

	// Use this for initialization
	void Start () {
		hidePanel ();
		cost = 0;
		lpSum = 0;
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		label.text = lang.getString ("inn");
		fullHealth.onClick.AddListener (setFullHealth);
		confirm.onClick.AddListener (pay);
		cancel.onClick.AddListener (hidePanel);
	}

	public void refreshPanel() {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		label.text = lang.getString ("inn");
		updateGold ();

		RemoveButtons ();
		AddButtons ();
	}

	public void Setup(Ship ship) {
		item = ship;
		cost = 0;
		lpSum = 0;
		itemList = ship.Crew;
		refreshPanel();
	}

	private void AddButtons() {
		int i;
		for (i = 0; i < itemList.Count; i++) {
			CrewMember item = itemList [i];
			GameObject newItemButton = buttonObjectPool.GetObject();
			CrewHealthButton crewButton = newItemButton.GetComponent<CrewHealthButton> ();
			crewButton.Setup (item, this);
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
	}

	private void RemoveButtons() {
		for (int i = 3; i >= 0; i--) {
			GameObject toRemove = panelLeft.GetChild (i).gameObject;
			if (toRemove.GetComponent<CrewHealthButton> ())
				buttonObjectPool.ReturnObject (toRemove);
			else
				emptyObjectPool.ReturnObject (toRemove);
			toRemove = panelRight.GetChild (i).gameObject;
			if (toRemove.GetComponent<CrewHealthButton> ())
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

	private void setFullHealth() {
		for (int i = 3; i >= 0; i--) {
			GameObject toUpdate = panelLeft.GetChild (i).gameObject;
			if (toUpdate.GetComponent<CrewHealthButton> ()) {
				CrewHealthButton chb = toUpdate.GetComponent<CrewHealthButton> ();
				chb.setMaxLP ();
			}
			toUpdate = panelRight.GetChild (i).gameObject;
			if (toUpdate.GetComponent<CrewHealthButton> ()) {
				CrewHealthButton chb = toUpdate.GetComponent<CrewHealthButton> ();
				buttonObjectPool.ReturnObject (toUpdate);
				chb.setMaxLP ();
			}
		}
	}
	private void pay() {
		if (cost <= item.Gold) {
			item.Gold -= cost;
			for (int i = 3; i >= 0; i--) {
				GameObject toUpdate = panelLeft.GetChild (i).gameObject;
				if (toUpdate.GetComponent<CrewHealthButton> ()) {
					CrewHealthButton chb = toUpdate.GetComponent<CrewHealthButton> ();
					chb.setHealth ();
				}
				toUpdate = panelRight.GetChild (i).gameObject;
				if (toUpdate.GetComponent<CrewHealthButton> ()) {
					CrewHealthButton chb = toUpdate.GetComponent<CrewHealthButton> ();
					buttonObjectPool.ReturnObject (toUpdate);
					chb.setHealth ();
				}
			}
			panel.SetActive (false);
			cost = 0;
			lpSum = 0;
			panelHandler.refreshCrewMemberDisplay ();
		}
	}
	public void updateGold(int sum = 0) {
		lpSum += sum;
		cost = (int)Mathf.Ceil(lpSum * GlobalVariables.healthCost);
		if (cost <= item.Gold) {
			goldAmount.color = new Color32( 0x32, 0x32, 0x32, 0xFF );
			goldAmount.text = lang.getString ("help_cost") + " " + cost + lang.getString ("gold");
		} else {
			goldAmount.color = Color.red;
			goldAmount.text = lang.getString ("help_cost") + " " + cost + lang.getString ("gold");
		}
	}

	public void showPanel() {
		panel.SetActive (true);
	}
	public void hidePanel() {
		cost = 0;
		lpSum = 0;
		panel.SetActive (false);
	}
}
