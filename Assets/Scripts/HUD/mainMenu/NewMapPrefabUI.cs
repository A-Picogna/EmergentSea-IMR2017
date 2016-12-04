using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewMapPrefabUI : MonoBehaviour
{
	public GameObject saveName;

	public void onSave() {
		
		LoadManager.instance.savePrefabricatedMapEditor ((saveName.GetComponent<InputField> ()).text.ToString());

		gameObject.SetActive (false);
	}
}

