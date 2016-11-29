using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class HelpPanel : MonoBehaviour {
	public Text textHelp;
	public int type;

	private Lang lang;

	// Use this for initialization
	void Start () {
		refresh ();
	}

	public void refresh() {
		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		if (type == 0) {
			textHelp.text = lang.getString("help_trade");
		}
	}


}
