using UnityEngine;
using System.Collections;

public class MouseFollower : MonoBehaviour {
	private Canvas canvas;
	private int mode;

	// Use this for initialization
	void Start () {
		canvas = GetComponentInParent<Canvas>();
	}

	public void refreshPosition() {
		//transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		//transform.localPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		//transform.localPosition = Input.mousePosition - canvas.transform.localPosition;
		//transform.position = Input.mousePosition - canvas.transform.localPosition;
		//transform.anchoredPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
		pos.x += 75;
		transform.position = canvas.transform.TransformPoint(pos);
	}			
}