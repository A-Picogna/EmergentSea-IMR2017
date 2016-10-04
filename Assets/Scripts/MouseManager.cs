using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		// only work in orthographic mode :
		// Vector3 worldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		// WITH 3D OBJECTS :
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hitInfo;
		if (Physics.Raycast (ray, out hitInfo)) {
			Debug.Log (hitInfo.collider.transform.parent.name);
		}

	}
}
