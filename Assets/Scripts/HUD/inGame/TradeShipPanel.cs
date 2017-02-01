using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TradeShipPanel : MonoBehaviour {
	public Text shipName;
	public Text goldAmount;
	public Text foodAmount;
	public Button tradeGold;
	public Text tradeGoldText;
	public Button tradeFood;
	public Text tradeFoodText;

	private List<CrewMember> itemList;
	public Transform panelLeft;
	public Transform panelRight;

	public GameObject panelGiveAmount;
	public Image arrowWay;
	public Text tradeType;
	public InputField tradeAmount;
	public Button sendButton;
	public Text sendText;

	public CrewMemberTradeObjectPool buttonObjectPool;
	public EmptyObjectPool emptyObjectPool;

	public Ship item;
	public TradeShipPanel otherShipPanel;
	public PanelHandler panelHandler;
	private int index;
	private bool selectedForTrade;
	private CrewMember toBeTransfered;
	private TradeShipPanel toBeTradedWith;
	private Button highlightedButton;
	private int indexHighlightedButton;
	private int indexParentHighlightedButton;
	private Lang lang;

	// Use this for initialization
	void Start () {
		selectedForTrade = false;
		panelGiveAmount.SetActive (false);
		index = transform.GetSiblingIndex();
		tradeAmount.keyboardType = TouchScreenKeyboardType.NumberPad;
		tradeAmount.characterValidation = InputField.CharacterValidation.Integer;
		tradeGold.onClick.AddListener (handleClickGold);
		tradeFood.onClick.AddListener (handleClickFood);
	}

	public void refreshPanel() {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		shipName.text = item.ShipName;
		goldAmount.text = item.Gold.ToString () + " " + lang.getString ("gold");
		foodAmount.text = item.Food.ToString () + " " + lang.getString ("food");
		tradeGoldText.text = lang.getString ("give") + " " + lang.getString ("gold");
		tradeFoodText.text = lang.getString ("give") + " " + lang.getString ("food");

		RemoveButtons ();
		AddButtons ();
	}

	public void Setup(Ship ship) {
		item = ship;
		itemList = ship.Crew;
		refreshPanel();
	}
	
	private void AddButtons() {
		int i;
		for (i = 0; i < itemList.Count; i++) {
			CrewMember item = itemList [i];
			GameObject newItemButton = buttonObjectPool.GetObject();
			CrewTradeItemButton crewButton = newItemButton.GetComponent<CrewTradeItemButton> ();
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

	private void handleClickGold() {
		sendText.text = lang.getString ("give");
		tradeAmount.text = "0";
		if (index == 1)
			arrowWay.sprite =  Resources.Load<Sprite>("Images/arrow_lr");
		else
			arrowWay.sprite =  Resources.Load<Sprite>("Images/arrow_rl");
		tradeType.text = lang.getString ("gold");
		sendButton.onClick.RemoveAllListeners ();
		sendButton.onClick.AddListener (sendGold);
		panelGiveAmount.SetActive (true);
	}
	private void handleClickFood() {
		sendText.text = lang.getString ("give");
		tradeAmount.text = "0";
		if (index == 1)
			arrowWay.sprite =  Resources.Load<Sprite>("Images/arrow_lr");
		else
			arrowWay.sprite =  Resources.Load<Sprite>("Images/arrow_rl");
		tradeType.text = lang.getString ("food");
		sendButton.onClick.RemoveAllListeners ();
		sendButton.onClick.AddListener (sendFood);
		panelGiveAmount.SetActive (true);
	}

	public void sendGold() {
		int amount = int.Parse(tradeAmount.text);
		if (amount > item.Gold)
			amount = item.Gold;
		else if (amount < 0)
			amount = 0;
		otherShipPanel.item.Gold += amount;
		item.Gold -= amount;
		refreshPanel ();
		otherShipPanel.refreshPanel ();
		panelHandler.updateShip ();
		panelGiveAmount.SetActive(false);
	}
	public void sendFood() {
		int amount = int.Parse(tradeAmount.text);
		if (amount > item.Food)
			amount = item.Food;
		else if (amount < 0)
			amount = 0;
		otherShipPanel.item.Food += amount;
		item.Food -= amount;
		refreshPanel ();
		otherShipPanel.refreshPanel ();
		panelHandler.updateShip ();
		panelGiveAmount.SetActive(false);
	}

	public void transferCrewMember(CrewMember toGive, Button button) {
		if (selectedForTrade) {
			var colors = highlightedButton.colors;
			colors.normalColor = Color.white;
			colors.highlightedColor = new Color32( 0xF5, 0xF5, 0xF5, 0xFF );
			highlightedButton.colors = colors;

			if (toBeTradedWith != this) {
				otherShipPanel.item.Crew.Remove (toBeTransfered);
				otherShipPanel.item.Crew.Add (toGive);
				item.Crew.Remove (toGive);
				item.Crew.Add (toBeTransfered);
				otherShipPanel.refreshPanel ();
				panelHandler.updateShip ();
			}
			refreshPanel ();

			readyForTrade (false, null, null, null);
			otherShipPanel.readyForTrade (false, null, null, null);
		} else if (otherShipPanel.item.Crew.Count < 8) {
			otherShipPanel.item.Crew.Add (toGive);
			item.Crew.Remove (toGive);

			refreshPanel ();
			otherShipPanel.refreshPanel ();
			panelHandler.updateShip ();
		} else if (!selectedForTrade) {
			readyForTrade (true, toGive, this, button);
			otherShipPanel.readyForTrade (true, toGive, this, button);
			refreshPanel ();
			for (int i = 0; i < itemList.Count; i++) {
				if (i < 4) {
					CrewTradeItemButton ctib = panelLeft.transform.GetChild (i).GetComponent<CrewTradeItemButton> ();
					if (ctib.checkItem (toGive)) {
						Button but = panelLeft.transform.GetChild (i).GetComponent<Button> ();
						var colors = but.colors;
						colors.normalColor = Color.grey;
						colors.highlightedColor = Color.grey;
						but.colors = colors;
						readyForTrade (true, toGive, this, but);
						otherShipPanel.readyForTrade (true, toGive, this, but);
						break;
					}
				} else {
					CrewTradeItemButton ctib = panelRight.transform.GetChild (i-4).GetComponent<CrewTradeItemButton> ();
					if (ctib.checkItem (toGive)) {
						Button but = panelRight.transform.GetChild (i-4).GetComponent<Button> ();
						var colors = but.colors;
						colors.normalColor = Color.grey;
						colors.highlightedColor = Color.grey;
						but.colors = colors;
						readyForTrade (true, toGive, this, but);
						otherShipPanel.readyForTrade (true, toGive, this, but);
						break;
					}
				}
			}
		}
        item.UpdateShipHp();
        item.DisplayHp(true);
        otherShipPanel.item.UpdateShipHp();
        otherShipPanel.item.DisplayHp(true);
    }

	public void readyForTrade(bool selected, CrewMember cm, TradeShipPanel tsp, Button button) {
		selectedForTrade = selected;
		toBeTransfered = cm;
		toBeTradedWith = tsp;
		highlightedButton = button;
		if (button != null) {
			indexHighlightedButton = button.transform.GetSiblingIndex ();
			indexParentHighlightedButton = button.transform.parent.transform.GetSiblingIndex ();
		}
	}
	public void reinitialiseButtonColor() {
		var colors = highlightedButton.colors;
		colors.normalColor = Color.white;
		colors.highlightedColor = new Color32( 0xF5, 0xF5, 0xF5, 0xFF );
		highlightedButton.colors = colors;
	}
	public bool duringTransfer() {
		return selectedForTrade;
	}
}
