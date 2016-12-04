using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class MenuPanelEditor : MonoBehaviour {
	public Text menuHeader;
	public Text resume;
	public Text save;
	public Text quit;
	private Lang lang;

	void Start () {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		menuHeader.text = lang.getString("menu");
		resume.text = lang.getString("resume");
		save.text = lang.getString("save");
		quit.text = lang.getString("quit_editor");
	}
}

