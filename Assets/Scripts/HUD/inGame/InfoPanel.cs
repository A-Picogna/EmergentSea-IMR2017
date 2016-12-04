using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InfoPanel : MonoBehaviour {

	Coroutine co;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DisplayInfo(string message, float duration){
		co = StartCoroutine (MessageForPlayer(message, duration));
	}

	IEnumerator MessageForPlayer (string message, float duration){
		if (duration < 3f) {
			duration = 3f;
		}
		this.GetComponent<Text>().text = message;    
		yield return new WaitForSeconds(duration-2f);
		this.GetComponent<Text>().CrossFadeAlpha(0.0f, 2f, false);
		yield return new WaitForSeconds(2f);
		this.GetComponent<Text>().text = "";
		this.GetComponent<Text>().CrossFadeAlpha(1.0f, 0, false);
	}
}
