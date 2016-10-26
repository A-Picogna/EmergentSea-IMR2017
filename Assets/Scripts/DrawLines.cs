using UnityEngine;
using System.Collections;

// Put this script on a Camera
public class DrawLines : MonoBehaviour {

	// Fill/drag these in from the editor

	// Choose the Unlit/Color shader in the Material Settings
	// You can change that color, to change the color of the connecting lines
	public Material lineMat;
	public Vector3 mainPoint;
	public Vector3[] points;

	// Connect all of the `points` to the `mainPoint`


	// To show the lines in the game window whne it is running
	void OnPostRender() {
		GL.Begin(GL.LINES);
		lineMat.SetPass(0);
		GL.Color(new Color(50f, 50f, 50f, 1f));
		GL.Vertex3(0f, 0f, 0f);
		GL.Vertex3(1f, 1f, 1f);
		GL.End();
	}
	// To show the lines in the editor
	void OnDrawGizmos() {
		GL.Begin(GL.LINES);
		lineMat.SetPass(0);
		GL.Color(new Color(50f, 50f, 50f, 1f));
		GL.Vertex3(0f, 0f, 0f);
		GL.Vertex3(1f, 1f, 1f);
		GL.End();
	}
}