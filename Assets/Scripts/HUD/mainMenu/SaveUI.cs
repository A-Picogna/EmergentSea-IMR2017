using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
	public GameObject NewSaveTextInput;
	public GameObject NewSaveButton;	
	private List<GameObject> SaveFileLine = new List<GameObject>();
	public string[] saveList;
	private bool amIInGame;
	Random rand;

	public void OnClickNewSave() {
		changeState (State.Saving);
	}

	void Start() {
		rand = new Random ();
		changeState (State.Nothing);
		checkGame ();
		populateSaveList ();
		if (amIInGame)
			gameObject.transform.parent.transform.SetAsLastSibling ();
	}

	private void checkGame() {
		if (SceneManager.GetActiveScene().name.Equals("main")) {
			amIInGame = false;
		} else {
			amIInGame = true;
		}
		NewSaveButton.SetActive (amIInGame);
	}

	private void populateSaveList() {
		try {
			saveList = Directory.GetFiles (GlobalVariables.pathSaves);
		} catch (DirectoryNotFoundException) {
			Directory.CreateDirectory (GlobalVariables.pathSaves);
			saveList = Directory.GetFiles (GlobalVariables.pathSaves);
		}

		for(int i=0; i<saveList.Length; i++){
			GameObject saveObject = (GameObject)Instantiate (SaveUIPrefab, SaveList.transform, false);
			//Debug.Log (saveObject.ToString ());
			SaveFileLine.Add(saveObject);
			SaveFileLine [i].name = "Save_" + i;
			///SaveFileLine [offset].transform.SetParent(SaveList.transform, false);
			//Debug.Log("lol");
			//Debug.Log (saveObject.ToString ());

			if(!amIInGame)
			{
				//SaveFileLine [i].transform.Translate (new Vector3 (0, 15, 0));
				SaveFileLine [i].transform.Translate (new Vector3 (0, i * (-SaveFileLine [i].GetComponent<RectTransform>().rect.height) + 15, 0));
				//SaveFileLine [i].transform.Translate (new Vector3 (0, i * (30-15), 0));
			}
			else {
				//SaveFileLine [i].transform.Translate (new Vector3 (0, i * -30, 0));
				SaveFileLine [i].transform.Translate (new Vector3 (0, i * (-SaveFileLine [i].GetComponent<RectTransform>().rect.height) - 5, 0));
			}
			setText (SaveFileLine [i].GetComponentInChildren <Text> (), Path.GetFileName (saveList [i]));
			setButtonOnClickListener ((SaveFileLine [i].GetComponent<Button> ()), saveList [i]);
			//(SaveFileLine [i].GetComponentInChildren <Text> ()).text = Path.GetFileName(saveList[i]);
			//(SaveFileLine [i].GetComponent<Button>()).onClick.AddListener (() => {
			//	//Debug.Log(saveList[i].ToString());
			//	SelectSave(saveList[i]);
			//});
		}
	}

	private void setButtonOnClickListener(Button button, string str) {
			button.onClick.AddListener (() => {
				SelectSave(str);
			});
	}

	private void setText(Text txt, string str) {
		txt.text = str;
	}

	private void resetSaveList() {
		foreach (GameObject saveFileObject in SaveFileLine) {
			saveFileObject.name = saveFileObject.name + "_trash";
			Destroy (saveFileObject);
		}
		SaveFileLine = new List<GameObject> ();
	}

	private void updateSaveList() {
		resetSaveList ();
		populateSaveList ();
	}

	private void SelectSave(string saveFile) {
		changeState (State.Loading);

		LoadManager.instance.SaveToLoad = saveFile;
		LoadManager.instance.LoadManagerState = LoadManager.state.LoadSave;

		(LoadedSaveUI.GetComponentInChildren<Text> ()).text = Path.GetFileName(saveFile);
	}

	public void DeleteSave() {
		File.Delete (LoadManager.instance.SaveToLoad);
		LoadManager.instance.SaveToLoad = "";
		changeState (State.Nothing);
		updateSaveList ();
	}

	public void Save() {
		string saveName = (NewSaveTextInput.GetComponent<InputField> ()).text;
		LoadManager.instance.save (saveName);
		updateSaveList ();	
	} 

	private void changeState(State state) {
		this.SaveUIState = state;

		switch (state) {
		case State.Loading:
			NewSaveUI.SetActive (false);
			SaveButton.SetActive (false);

			DeleteButton.SetActive (true);

			if (amIInGame) {
				LaunchButton.SetActive (false);
			} else { 
				LaunchButton.SetActive (true);
			}
			
			LoadedSaveUI.SetActive (true);
			break;
		case State.Saving:
			NewSaveUI.SetActive (true);
			if(LoadManager.instance.gameManager.currentPlayer.IsHuman){
				SaveButton.SetActive (true);
			} else {
				SaveButton.SetActive (false);
			}

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
