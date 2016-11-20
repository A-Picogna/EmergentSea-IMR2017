// @author Yasmine Serot
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class CrewItemButton : MonoBehaviour {
	public Button button;
	public Image type;
	public Text pvLabel;
	public Slider percentagePV;
	public Text xpLabel;
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
		print (item.Type);
		print (item.Lpmax);
		if (currentItem.Type == 0)
			type.sprite =  Resources.Load<Sprite>("Images/admiral_icon");
		else if (currentItem.Type == 1)
			type.sprite =  Resources.Load<Sprite>("Images/sword_icon");
		else if (currentItem.Type == 2)
			type.sprite =  Resources.Load<Sprite>("Images/pistol_icon");
		else if (currentItem.Type == 3)
			type.sprite =  Resources.Load<Sprite>("Images/mage_icon");
		pvLabel.text = lang.getString("pv");
		if (item.Lpmax > 0)
			percentagePV.value = (item.Lp*100)/item.Lpmax;
		else
			percentagePV.value = item.Lpmax;
		xpLabel.text = lang.getString("xp");
		percentageXP.value = item.Xp;
	}

	public void HandleClick() {

	}
}
