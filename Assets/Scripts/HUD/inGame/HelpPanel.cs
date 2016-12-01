using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HelpPanel : MonoBehaviour {
	public GameObject panelHelp;
	public Text textHelp;
	private int type;
	public int positionMode;
	private List<string> infoList;

	// Mouse follower
	private Canvas canvas;

	private Lang lang;

	// Use this for initialization
	void Start () {
		//canvas = GetComponentInParent<Canvas>();
		canvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();
		hidePanel ();
		refresh ();
	}

	public void refreshPosition() {
		Vector2 pos;
		if (canvas == null) {
			//canvas = GetComponentInParent<Canvas>();
			canvas = GameObject.Find("HUDCanvas").GetComponent<Canvas>();
		}
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
		if (positionMode == 0) {
			pos.x += 75;
		} else if (positionMode == 1) {
			pos.x -= 20;
			pos.y += 20;
		}
		transform.position = canvas.transform.TransformPoint(pos);
	}
	public void refresh() {
		refreshPosition ();
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		if (type == 0) { // Question mark in trade panel
			string text = formatString(lang.getString ("help_trade"));
			textHelp.text = text.Replace("//n", "\n");
		} else if (type == 1) { // Barre de vie
			//textHelp.text = lang.getString ("help_trade");
		} else if (type == 2) { // Barre d'expérience
			//textHelp.text = lang.getString ("help_trade");
		} else if (type == 3) { // Navire ennemi
			string text = lang.getString ("help_attack") + "//n" + lang.getString ("help_cost") + "<color=red> -" + GlobalVariables.energyAttack.ToString() + " " + lang.getString ("energy") + "</color> ";
			textHelp.text = text.Replace("//n", "\n");
		} else if (type == 4) { // Navire ennemi
			textHelp.text = lang.getString ("ennemy_ship");
		} else if (type == 5) { // Trésor
			textHelp.text = lang.getString ("treasure");
		} else if (type == 6) { // Trésor
			string text = lang.getString ("help_treasure") + "//n" + lang.getString ("help_cost") + "<color=red> -" + GlobalVariables.energyTreasure.ToString() + " " + lang.getString ("energy") + "</color> " +
				"//n" + lang.getString ("help_gain") + "<color=green> " + lang.getString ("gold") + "</color> ";
			textHelp.text = text.Replace("//n", "\n");
		} else if (type == 7) { // Harbor
			textHelp.text = lang.getString ("harbor");
		} else if (type == 8) { // Harbor
			textHelp.text = lang.getString ("help_harbor");
		} else if (type == 9) { // Navire allié
			textHelp.text = lang.getString ("friendly_ship");
		} else if (type == 10) { // Navire allié
			textHelp.text = lang.getString ("help_trading");
		} else if (type == 11) { // Pêcher
			string text = lang.getString ("help_fishing") + "//n" + lang.getString ("help_cost") + "<color=red> -" + GlobalVariables.energyFishing.ToString() + " " + lang.getString ("energy") + "</color> " +
				"//n" + lang.getString ("help_gain") + "<color=green> " + lang.getString ("food") + "</color> ";
			textHelp.text = text.Replace("//n", "\n");
		} else if (type == 12) { // Amiral
			string text = lang.getString ("admiral") + "//n" + formatString(lang.getString ("help_admiral"));
			textHelp.text = text.Replace("//n", "\n");
		} else if (type == 13) { // Flibustier
			string text = lang.getString ("filibuster") + "//n" + formatString(lang.getString ("help_filibuster"));
			textHelp.text = text.Replace("//n", "\n");
		} else if (type == 14) { // Artificier
			string text = lang.getString ("powder_monkey") + "//n" + formatString(lang.getString ("help_powder_monkey"));
			textHelp.text = text.Replace("//n", "\n");
		} else if (type == 15) { // Lanceur de maléfices
			string text = lang.getString ("conjurer") + "//n" + formatString(lang.getString ("help_conjurer"));
			textHelp.text = text.Replace("//n", "\n");
		} else if (type == 16) { // Taverne
			textHelp.text = lang.getString ("help_cost") + "<color=red> " + GlobalVariables.healthCost.ToString() + " " + lang.getString ("gold") + "</color>/" + lang.getString ("pv");
		} else if (type == 17) { // Vendre Nourriture
			textHelp.text = lang.getString ("help_gain") + "<color=green> " + GlobalVariables.changeFoodGold.ToString() + " " + lang.getString ("gold") + "</color>/" + lang.getString ("food");
		} else if (type == 18) { // Engager flibustier
			textHelp.text = lang.getString ("help_cost") + "<color=red> " + GlobalVariables.filibusterCost.ToString() + " " + lang.getString ("gold") + "</color>";
		} else if (type == 19) { // Engager artificier
			textHelp.text = lang.getString ("help_cost") + "<color=red> " + GlobalVariables.powderMonkeyCost.ToString() + " " + lang.getString ("gold") + "</color>";
		} else if (type == 20) { // Engager lanceur de maléfices
			textHelp.text = lang.getString ("help_cost") + "<color=red> " + GlobalVariables.conjurerCost.ToString() + " " + lang.getString ("gold") + "</color>";
		} else if (type == 21) { // Construction navire
			string text = lang.getString ("help_cost") + "<color=red> " + GlobalVariables.admiralCost.ToString() + " " + lang.getString ("gold") + "</color>//n" +
				lang.getString ("help_time") + " " + GlobalVariables.buildingTime + " " + lang.getString ("help_round") + "s";
			textHelp.text = text.Replace("//n", "\n");
		}
	}
	public void changeText(int tp) {
		type = tp;
	}
	public void addInfo(List<string> list) {
		infoList = list;
	}

	public void showPanel() {
		panelHelp.SetActive (true);
	}
	public void hidePanel() {
		panelHelp.SetActive (false);
	}

	private string formatString(string text) {
		int size = text.Length;
		int lineSize = 30;
		string[] words = text.Split(' ');
		string result = "";
		string tmp = "";

		for (int i = 0; i < words.Length; i++) {
			tmp += words [i] + " ";
			if (tmp.Length > lineSize) {
				tmp.Remove(tmp.Length - 1);
				result += tmp + "//n";
				tmp = "";
			}
		}
		if (tmp.Length > 0) {
			result += tmp;
		}
		return result;
	}
}
