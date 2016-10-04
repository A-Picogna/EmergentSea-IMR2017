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
			GameObject ourHitObject = hitInfo.collider.transform.gameObject;
			Debug.Log (ourHitObject.name);

			if (Input.GetMouseButtonDown(0)) {
				MeshRenderer mr = ourHitObject.GetComponentInChildren<MeshRenderer> ();
				if (mr.material.color == Color.red) {
					mr.material.color = Color.white;
				} else {
					mr.material.color = Color.red;
				}
			}
		}

	}
}
