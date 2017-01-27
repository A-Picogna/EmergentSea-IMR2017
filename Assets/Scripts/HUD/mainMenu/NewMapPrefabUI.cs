using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NewMapPrefabUI : MonoBehaviour
{
	public GameObject saveName;

	public void onSave() {

		// TODO add check for ships in map before saving, check CheckShipMapValidity() in MapEditor
		
		LoadManager.instance.savePrefabricatedMapEditor ((saveName.GetComponent<InputField> ()).text.ToString());

		gameObject.SetActive (false);
	}
}

