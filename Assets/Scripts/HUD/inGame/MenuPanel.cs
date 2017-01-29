using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class MenuPanel : MonoBehaviour {
	public Text menuHeader;
	public Text resume;
	public Text save;
	public Text quit;
	private Lang lang;

	void Start () {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		menuHeader.text = lang.getString("menu");
		resume.text = lang.getString("resume");
		if (!SceneManager.GetActiveScene ().name.Equals ("map_tutorial")) {
			save.text = lang.getString("save");
		}
		quit.text = lang.getString("quit");
		GameObject.Find("btn_gameover_return").GetComponentInChildren<Text>().text = lang.getString("quit");
	}
}
