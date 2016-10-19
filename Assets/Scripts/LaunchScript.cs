using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LaunchScript : MonoBehaviour {

	public void LaunchGame() {
		
		SceneManager.LoadScene("Scenes/map.unity");
	
	}
}
