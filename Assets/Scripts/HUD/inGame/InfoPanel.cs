using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisplayInfo(string message, float duration){
		StartCoroutine (MessageForPlayer(message, duration));
	}

	IEnumerator MessageForPlayer (string message, float duration){
		this.GetComponent<Text>().text = message;    
		yield return new WaitForSeconds(duration/2);
		this.GetComponent<Text>().CrossFadeAlpha(0.0f, duration/2, false);
		yield return new WaitForSeconds(duration/2);
		this.GetComponent<Text>().text = "";
		this.GetComponent<Text>().CrossFadeAlpha(1.0f, 0, false);
	}
}
