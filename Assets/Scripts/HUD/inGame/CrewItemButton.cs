// @author Yasmine Serot
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CrewItemButton : MonoBehaviour {
	public Button button;
	public Image type;
	public Text pvLabel;
	public Slider percentagePV;
	public Text xpLabel;
	public Slider percentageXP;

	private CrewItem item;

	void Start() {
		button.onClick.AddListener (HandleClick);
	}

	public void Setup (CrewItem currentItem) {
		item = currentItem;
		type.sprite = item.type;
		pvLabel.text = item.pvLabel;
		percentagePV.value = item.percentagePV;
		xpLabel.text = item.xpLabel;
		percentageXP.value = item.percentageXP;
	}

	public void HandleClick() {

	}
}
