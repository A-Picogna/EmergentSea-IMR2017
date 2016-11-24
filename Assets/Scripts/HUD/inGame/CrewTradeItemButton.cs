// @author Yasmine Serot
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class CrewTradeItemButton : MonoBehaviour {
	public Button button;
	public Image type;
	public Text pvLabel;
	public Text textPercentagePV;
	public Slider percentagePV;
	public Text xpLabel;
	public Text textPercentageXP;
	public Slider percentageXP;

	private Lang lang;
	private CrewMember item;

	void Start() {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		button.onClick.AddListener (HandleClick);
	}

	public void Setup (CrewMember currentItem) {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		item = currentItem;
		if (currentItem.Type == 0)
			type.sprite =  Resources.Load<Sprite>("Images/admiral_icon");
		else if (currentItem.Type == 1)
			type.sprite =  Resources.Load<Sprite>("Images/sword_icon");
		else if (currentItem.Type == 2)
			type.sprite =  Resources.Load<Sprite>("Images/pistol_icon");
		else if (currentItem.Type == 3)
			type.sprite =  Resources.Load<Sprite>("Images/mage_icon");
		pvLabel.text = lang.getString("pv");
		textPercentagePV.text = item.Lp + "/" + item.LpMax;
		if (item.LpMax > 0)
			percentagePV.value = (item.Lp*100)/item.LpMax;
		else
			percentagePV.value = item.LpMax;
		xpLabel.text = lang.getString("xp");
		textPercentageXP.text = item.Xp + "/10000";
		percentageXP.value = (item.Xp*100) / 10000;
	}

	public void HandleClick() {

	}
}
