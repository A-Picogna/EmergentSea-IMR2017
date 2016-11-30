using UnityEngine;
using System.Collections;

public class MouseFollower : MonoBehaviour {
	private Canvas canvas;

	// Use this for initialization
	void Start () {
		canvas = GetComponentInParent<Canvas>();
	}
		
	public void refreshPosition() {
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
		pos.x += 75;
		transform.position = canvas.transform.TransformPoint(pos);
	}			
}