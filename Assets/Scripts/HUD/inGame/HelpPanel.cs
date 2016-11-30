using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class HelpPanel : MonoBehaviour {
	public GameObject panelHelp;
	public Text textHelp;
	private int type;
	private List<string> infoList;

	// Mouse follower
	private Canvas canvas;

	private Lang lang;

	// Use this for initialization
	void Start () {
		canvas = GetComponentInParent<Canvas>();
		hidePanel ();
		refresh ();
	}

	public void refreshPosition() {
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
		pos.x += 75;
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
		/*
		for (int i = 0; i < size; i += lineSize) {
			if (i + lineSize <= size) {
				result += text.Substring (i, lineSize) + "//n";
			} else {
				result += text.Substring (i, size);
			}
		}*/
		return result;
	}
}
