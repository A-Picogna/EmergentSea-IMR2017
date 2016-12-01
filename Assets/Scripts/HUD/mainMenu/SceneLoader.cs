using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour {
	private bool loadingAScene = false;
	private int count = 0;
	private int supercount = 0;

	[SerializeField]
	private Text loadingText;

	public void NewMap() {
		if (loadingAScene == false) {
			loadingAScene = true;

			gameObject.SetActive (true);

			StartCoroutine (LoadMapCoroutine ());
		}
	}

	public void Editor() {
		if (loadingAScene == false) {
			loadingAScene = true;

			gameObject.SetActive (true);

			StartCoroutine (EditorCoroutine ());
		}
	}

	void Update() {
		if (loadingAScene == true) {
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

	IEnumerator LoadMapCoroutine() {
		// We wait just 3 seconds to feel the loading
		yield return new WaitForSeconds (1);

		// Start an ansync operation to load the scene
		AsyncOperation async = SceneManager.LoadSceneAsync("map");

		while (!async.isDone) {
			yield return null;
		}

		loadingAScene = false;
	}

	IEnumerator EditorCoroutine() {
		// We wait just 3 seconds to feel the loading
		yield return new WaitForSeconds (1);

		// Start an ansync operation to load the scene
		AsyncOperation async = SceneManager.LoadSceneAsync("map_editor");

		while (!async.isDone) {
			yield return null;
		}

		loadingAScene = false;
	}
}