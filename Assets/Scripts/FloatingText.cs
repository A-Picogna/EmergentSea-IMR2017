using UnityEngine;
using System.Collections;

public class FloatingText : MonoBehaviour {

	bool move = false;
	Coroutine co;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<TextMesh> ().text != "" && !move) {
			move = true;
			co = StartCoroutine (FadingPanel(transform.position, transform.position+ new Vector3(0, 0.5f, 0), 2f));
		}
	}

	public void Reinit(){
		if (co != null) {
			GetComponent<TextMesh> ().text = "";
			move = false;
			StopCoroutine (co);
			co = null;
		}
	}

	IEnumerator FadingPanel(Vector3 source, Vector3 target, float overTime){
		float startTime = Time.time;
		while(Time.time < startTime + overTime){
			transform.position = Vector3.Lerp(source, target, (Time.time - startTime)/overTime);
			yield return null;
		}
		GetComponent<TextMesh> ().text = "";
		move = false;
	}
}
