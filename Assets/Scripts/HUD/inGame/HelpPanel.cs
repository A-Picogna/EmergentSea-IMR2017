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
			textHelp.text = lang.getString ("help_trade");
		} else if (type == 1) { // Barre de vie
			textHelp.text = lang.getString ("help_trade");
		} else if (type == 2) { // Barre d'expérience
			textHelp.text = lang.getString ("help_trade");
		} else if (type == 3) { // Navire ennemi
			string text = lang.getString ("help_attack") + "//n" + lang.getString ("help_cost") + "<color=red> -" + GlobalVariables.energyAttack.ToString() + "</color> " + lang.getString ("energy");
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
}
