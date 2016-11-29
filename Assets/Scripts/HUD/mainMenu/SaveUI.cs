using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class SaveUI : MonoBehaviour {

	public enum State {Saving, Loading, Nothing};
	public GameObject NewSaveUI;
	public GameObject SaveButton;
	public State SaveUIState;
	public GameObject SaveUIPrefab;
	public GameObject SaveList;
	public GameObject LaunchButton;
	public GameObject DeleteButton;
	public GameObject LoadedSaveUI;

	public string[] saveList;

	public void OnClickNewSave() {
		changeState (State.Saving);

		SaveUIState = State.Saving;
	}

	void Start() {
		changeState (State.Nothing);
		populateSaveList ();
	}

	private void populateSaveList() {
		try {
			saveList = Directory.GetFiles (GlobalVariables.pathSaves);
		} catch (DirectoryNotFoundException) {
			Directory.CreateDirectory (GlobalVariables.pathSaves);
			saveList = Directory.GetFiles (GlobalVariables.pathSaves);
		}
		int offset = 0;
		//foreach (string saveFile in saveList) {
			GameObject SaveFileLine = (GameObject)Instantiate(SaveUIPrefab);
			SaveFileLine.transform.SetParent(SaveList.transform, false);
			SaveFileLine.transform.Translate (new Vector3 (0, offset * -17, 0));
			(SaveFileLine.GetComponentInChildren <Text> ()).text = "Test_X";
			(SaveFileLine.GetComponent<Button>()).onClick.AddListener (() => {
				SelectSave("saveFile");
			});
		//}
	}

	private void SelectSave(string saveFile) {
		changeState (State.Loading);

		LoadManager.instance.SaveToLoad = saveFile;
		LoadManager.instance.LoadManagerState = LoadManager.state.LoadSave;

		(LoadedSaveUI.GetComponentInChildren<Text> ()).text = "Save_X";
	}

	private void changeState(State state) {
		this.SaveUIState = state;

		switch (state) {
			case State.Loading:
				NewSaveUI.SetActive (false);
				SaveButton.SetActive (false);

				DeleteButton.SetActive (true);
				LaunchButton.SetActive (true);
				LoadedSaveUI.SetActive (true);
				break;
			case State.Saving:
				NewSaveUI.SetActive (true);
				SaveButton.SetActive (true);

				DeleteButton.SetActive (false);
				LaunchButton.SetActive (false);
				LoadedSaveUI.SetActive (false);
				
				break;
			case State.Nothing:
				NewSaveUI.SetActive (false);
				SaveButton.SetActive (false);
				
				DeleteButton.SetActive (false);
				LaunchButton.SetActive (false);
				LoadedSaveUI.SetActive (false);	
				
				break;
		}
		
	}
}
