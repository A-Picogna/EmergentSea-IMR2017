﻿using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {
	
	public int Boundary = 50; // distance from edge scrolling starts
	public int speed = 5;
	private int theScreenWidth;
	private int theScreenHeight; 
	float cameraDistanceMax = 40f;
	float cameraDistanceMin = 5f;
	float scrollSpeed = 3f;     
	float DragSpeed = 10.0f;
	float cameraDistance;

	// Use this for initialization
	void Start () {
		theScreenWidth = Screen.width;
		theScreenHeight = Screen.height;
		cameraDistance = this.transform.position.y;
	}
	
	// Update is called once per frame
	void LateUpdate () {     

		cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
		cameraDistance = Mathf.Clamp(cameraDistance, cameraDistanceMin, cameraDistanceMax);
		this.transform.position = new Vector3 (this.transform.position.x, cameraDistance, this.transform.position.z);

		/*

		if (Input.mousePosition.x > theScreenWidth - Boundary){
			this.transform.position += new Vector3(speed * Time.deltaTime, 0, 0); // move on +X axis
		}
		if (Input.mousePosition.x < 0 + Boundary){
			this.transform.position -= new Vector3(speed * Time.deltaTime, 0, 0); // move on -X axis
		}
		if (Input.mousePosition.y > theScreenHeight - Boundary){
			this.transform.position += new Vector3(0, 0, speed * Time.deltaTime); // move on +Z axis
		}
		if (Input.mousePosition.y < 0 + Boundary){
			this.transform.position -= new Vector3(0, 0, speed * Time.deltaTime); // move on -Z axis
		}
		*/

		if (Input.GetMouseButton (0)) {
			if (Input.GetAxis ("Mouse X") > 0) {
				transform.position -= new Vector3 (Input.GetAxisRaw ("Mouse X") * Time.deltaTime * DragSpeed, 
					0.0f, Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * DragSpeed);
			}

			else if (Input.GetAxis ("Mouse X") < 0) {
				transform.position -= new Vector3 (Input.GetAxisRaw ("Mouse X") * Time.deltaTime * DragSpeed, 
					0.0f, Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * DragSpeed);
			}
		}

	}

}