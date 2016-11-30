using UnityEngine;
using System.Collections;

public class MouseFollower : MonoBehaviour {
	private Canvas canvas;
	public int type;

	// Use this for initialization
	void Start () {
		canvas = GetComponentInParent<Canvas>();
	}
		
	public void refreshPosition() {
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
		if (type == 0) {
			pos.x += 75;
		} else if (type == 1) {
			pos.x += 100;
			pos.y += 100;
		}
		transform.position = canvas.transform.TransformPoint(pos);
	}			
}