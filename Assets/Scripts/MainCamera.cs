using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MainCamera : MonoBehaviour {
	
	public int Boundary = 50;
	// distance from edge scrolling starts
	public int speed = 5;
	private int theScreenWidth;
	private int theScreenHeight;
	float DragSpeed = 20.0f;
	float cameraDistance;
	float minFov = 10f;
	float maxFov = 40f;
	float sensitivity = 20f;

	// Use this for initialization
	void Start () {
		cameraDistance = this.transform.position.y;
	}
	
	// Update is called once per frame
	void LateUpdate () {     


		if (!EventSystem.current.IsPointerOverGameObject ()) {   
			float fov = Camera.main.fieldOfView;
			fov -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
			fov = Mathf.Clamp(fov, minFov, maxFov);
			Camera.main.fieldOfView = fov;

			if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
				this.transform.position += new Vector3 (speed * Time.deltaTime, 0, 0); // move on +X axis
			}
			if (Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.LeftArrow)) {
				this.transform.position -= new Vector3 (speed * Time.deltaTime, 0, 0); // move on -X axis
			}
			if (Input.GetKey (KeyCode.Z) || Input.GetKey (KeyCode.UpArrow)) {
				this.transform.position += new Vector3 (0, 0, speed * Time.deltaTime); // move on +Z axis
			}
			if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
				this.transform.position -= new Vector3 (0, 0, speed * Time.deltaTime); // move on -Z axis
			}

			if (Input.GetMouseButton (0)) {
				if (Input.GetAxis ("Mouse X") > 0) {
					transform.position -= new Vector3 (Input.GetAxisRaw ("Mouse X") * Time.deltaTime * DragSpeed, 
						0.0f, Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * DragSpeed);
				} else if (Input.GetAxis ("Mouse X") < 0) {
					transform.position -= new Vector3 (Input.GetAxisRaw ("Mouse X") * Time.deltaTime * DragSpeed, 
						0.0f, Input.GetAxisRaw ("Mouse Y") * Time.deltaTime * DragSpeed);
				}
			}
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
		}

	}

}