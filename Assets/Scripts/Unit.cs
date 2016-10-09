using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour {

	public Vector3 destination;
	float speed = 2;

	// Use this for initialization
	void Start () {
		destination = transform.position;
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 direction = destination - transform.position;
		Vector3 velocity = direction.normalized * speed * Time.deltaTime;
		velocity = Vector3.ClampMagnitude (velocity, direction.magnitude);
		transform.Translate (velocity);
	
	}
}
