// @author Yasmine Serot
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CrewItemButton : MonoBehaviour {
	public Button button;
	public Image type;
	public Slider percentagePV;
	public Text level;
	public Slider percentageXP;

	GameObject helpPanel;
	private CrewMember item;

	void Start() {
		button.onClick.AddListener (HandleClick);
		helpPanel = GetComponentInParent<Canvas>().GetComponent<PanelHandler>().panelHelp;
	}

	public void Setup (CrewMember currentItem) {
		HoveringHelp hovHelp = transform.GetChild (0).GetComponent<HoveringHelp>();
		item = currentItem;/*
		List<string> infos = new List<string> ();
		infos.Add (item.Atk.ToString());
		infos.Add (item.Level.ToString());
		infos.Add (item.Lp.ToString());
		infos.Add (item.LpMax.ToString());
		infos.Add (item.Xp.ToString());
		infos.Add (item.XpMax.ToString());
		if (hovHelp.getHelpPanel () == null) {
			hovHelp.setHelpPanel (GetComponentInParent<Canvas> ().GetComponent<PanelHandler> ().panelHelp2.GetComponent<HelpPanel>());
		}
		hovHelp.getHelpPanel ().addInfo (infos);*/
		if (item.Type == 0) {
			type.sprite = Resources.Load<Sprite> ("Images/admiral_icon");
			hovHelp.type = 12;
		} else if (item.Type == 1) {
			type.sprite = Resources.Load<Sprite> ("Images/sword_icon");
			hovHelp.type = 13;
		} else if (item.Type == 2) {
			type.sprite = Resources.Load<Sprite> ("Images/pistol_icon");
			hovHelp.type = 14;
		} else if (item.Type == 3) {
			type.sprite = Resources.Load<Sprite> ("Images/mage_icon");
			hovHelp.type = 15;
		}
		if (item.LpMax > 0)
			percentagePV.value = (item.Lp*100)/item.LpMax;
		else
			percentagePV.value = item.LpMax;
		level.text = item.Level.ToString();
		percentageXP.value = item.Xp;


	}

	public void HandleClick() {

	}
}
