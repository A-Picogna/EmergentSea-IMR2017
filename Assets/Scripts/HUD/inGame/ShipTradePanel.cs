using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ShipTradePanel : MonoBehaviour {
	public GameObject crewItemButton;

	private List<CrewMember> itemList;
	public Transform panelLeft;
	public Transform panelRight;

	public CrewMemberTradeObjectPool buttonObjectPool;
	public EmptyObjectPool emptyObjectPool;

	public Button tradeGold;
	public Button tradeFood;

	public Text shipName;
	public Text goldAmount;
	public Text foodAmount;
	public Text tradeGoldText;
	public Text tradeFoodText;
	private Lang lang;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
