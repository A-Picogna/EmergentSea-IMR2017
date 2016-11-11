using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class MenuPanel : MonoBehaviour {
	public Text menuHeader;
	public Text resume;
	public Text save;
	public Text load;
	public Text options;
	public Text quit;
	private Lang lang;

	void Start () {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		menuHeader.text = lang.getString("menu");
		resume.text = lang.getString("resume");
		save.text = lang.getString("save");
		load.text = lang.getString("load");
		options.text = lang.getString("options");
		quit.text = lang.getString("quit");
	}
}
