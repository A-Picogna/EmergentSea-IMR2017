﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
//	private bool loadingAScene = false;
	private int count = 0;
	private int supercount = 0;
//	AsyncOperation async;

	[SerializeField]
	private Text loadingText;

	public void NewMap() {
//		Resources.UnloadUnusedAssets ();
		if (LoadManager.instance.loadingAScene == false) {
			LoadManager.instance.loadingAScene = true;

			gameObject.SetActive (true);

			LoadManager.instance.LoadSceneRoutine("map");
		}
	}

	public void Editor() {
//		Resources.UnloadUnusedAssets ();
		if (LoadManager.instance.loadingAScene == false) {
			LoadManager.instance.loadingAScene = true;

			gameObject.SetActive (true);

			LoadManager.instance.LoadSceneRoutine("map_editor");
		}
	}

    public void Tutorial()
    {
        //		Resources.UnloadUnusedAssets ();
        if (LoadManager.instance.loadingAScene == false)
        {
            LoadManager.instance.LoadManagerState = LoadManager.state.LaunchTutorial;
            LoadManager.instance.loadingAScene = true;

            gameObject.SetActive(true);

            LoadManager.instance.LoadSceneRoutine("map_tutorial");
        }
    }

    void Update() {
		if (LoadManager.instance.loadingAScene = true) {
			if (supercount == 19) {
				switch (count) 
				{
				case 0:
					loadingText.text = "Loading.";
					break;
				case 1:
					loadingText.text = "Loading..";
					break;
				case 2:
					loadingText.text = "Loading...";
					break;
				}
				count = (count + 1) % 3;
			}
			supercount = (supercount + 1) % 20;
		}
	}

//	IEnumerator LoadMapCoroutine() {
//		// We wait just 3 seconds to feel the loading
//		yield return new WaitForSeconds (1);
//
//		//Debug.Log ("LoadSceneAsync");
//
//		// Start an ansync operation to load the scene
//		async = SceneManager.LoadSceneAsync("map", LoadSceneMode.Single);
//
//		while (!async.isDone) {
//			yield return null;
//		}
//
//		loadingAScene = false;
//	}
//
//	IEnumerator EditorCoroutine() {
//		// We wait just 3 seconds to feel the loading
//		yield return new WaitForSeconds (1);
//
//		// Start an ansync operation to load the scene
//		async = SceneManager.LoadSceneAsync("map_editor", LoadSceneMode.Single);
//
//		while (!async.isDone) {
//			yield return null;
//		}
//
//		loadingAScene = false;
//	}
}