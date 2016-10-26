using UnityEngine;
using System.Collections;

public class SaveUI : MonoBehaviour {

	public GameObject NewSaveUI;

	public void OnClickNewSave() {
		if (NewSaveUI.activeSelf)
			NewSaveUI.SetActive (false);
		else
			NewSaveUI.SetActive (true);
	}

}
