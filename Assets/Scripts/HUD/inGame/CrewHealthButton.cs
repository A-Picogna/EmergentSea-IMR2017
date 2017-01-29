using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrewHealthButton : MonoBehaviour {
	public GameObject panel;
	public Image type;
	public Text textPercentagePV;
	public Slider percentagePV;
	public Button max;
	public Button plus;
	public Button minus;

	private CrewHealthPanel scriptHealth;
	private CrewMember item;
	private int lp;
	private int missingLp;

	void Start () {
		max.onClick.AddListener (setMaxLP);
		plus.onClick.AddListener (addLP);
		minus.onClick.AddListener (removeLP);
	}
	
	public void Setup (CrewMember currentItem, CrewHealthPanel script) {
		scriptHealth = script;
		item = currentItem;
		if (item.Type == 0) {
			type.sprite = Resources.Load<Sprite> ("Images/admiral_icon");
		} else if (item.Type == 1) {
			type.sprite =  Resources.Load<Sprite>("Images/sword_icon");
		} else if (item.Type == 2) {
			type.sprite =  Resources.Load<Sprite>("Images/pistol_icon");
		} else if (item.Type == 3) {
			type.sprite =  Resources.Load<Sprite>("Images/mage_icon");
		}
		textPercentagePV.text = item.Lp + "/" + item.LpMax;
		if (item.LpMax > 0)
			percentagePV.value = (item.Lp*100)/item.LpMax;
		else
			percentagePV.value = item.LpMax;
		lp = item.Lp;
		missingLp = item.LpMax - item.Lp;
	}

	public void setMaxLP() {
		scriptHealth.updateGold (item.LpMax-lp);
		lp = item.LpMax;
		updateHealth ();
	}
	private void addLP() {
		if (lp < item.LpMax) {
			lp++;
			updateHealth ();
			scriptHealth.updateGold (1);
		}
	}
	private void removeLP() {
		if (lp > item.Lp) {
			lp--;
			updateHealth ();
			scriptHealth.updateGold (-1);
		}
	}
	private void updateHealth() {
		textPercentagePV.text = lp + "/" + item.LpMax;
		percentagePV.value = (lp*100)/item.LpMax;
	}
	public void setHealth() {
		item.Lp = lp;
	}
}
