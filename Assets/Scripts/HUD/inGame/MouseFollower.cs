using UnityEngine;
using System.Collections;

public class MouseFollower : MonoBehaviour {
	private Canvas canvas;
	public int positionMode;

	// Use this for initialization
	void Start () {
		canvas = GetComponentInParent<Canvas>();
	}
		
	public void refreshPosition() {
		Vector2 pos;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out pos);
		if (positionMode == 0) {
			pos.x += 75;
		} else if (positionMode == 1) {
			pos.x -= 20;
			pos.y += 20;
		}
		transform.position = canvas.transform.TransformPoint(pos);
	}			
}