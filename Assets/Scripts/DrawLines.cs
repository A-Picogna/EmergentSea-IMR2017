﻿using UnityEngine;
using System.Collections;

// Put this script on a Camera
using System.Linq;


public class DrawLines : MonoBehaviour {

	void Start(){
		this.GetComponent<MeshFilter>().mesh = new Mesh();
		Mesh mesh = this.GetComponent<MeshFilter>().mesh;

		Vector3[] hexVertices = new Vector3[8];

		float size = 0.5f;
		float x0 = 0;
		float y0 = 0;

		var SQRT32 = Mathf.Sqrt(3) / 2;

		/*
		//-- from above with front faceing up
		//center
		hexVertices[0] = new Vector3(x0, 0, y0);
		//Back Left
		hexVertices[1] = new Vector3(x0 - size / 2, 0, y0 - size * SQRT32);
		//Back Right
		hexVertices[2] = new Vector3(x0 + size / 2, 0, y0 - size * SQRT32);
		//Right
		hexVertices[3] = new Vector3(x0 + size, 0, y0);
		//Front right
		hexVertices[4] = new Vector3(x0 + size / 2, 0, y0 + size * SQRT32);
		//Front left
		hexVertices[5] = new Vector3(x0 - size / 2, 0, y0 + size * SQRT32);
		//Left
		hexVertices[6] = new Vector3(x0 - size, 0, y0);
		*/
		for (int i = 0; i < 7; i++) {
			hexVertices[i] = new Vector3(hex_corner(x0,y0,i)[0], 0f, hex_corner(x0,y0,i)[1]);
		}

		LineRenderer lineRenderer = this.GetComponent<LineRenderer> (); // Add or get LineRenderer component to game object
		lineRenderer.SetWidth(0.01f, 0.01f);
		lineRenderer.SetVertexCount(7);  // 6+1 since vertex 6 has to connect to vertex 1
		for (int i = 0; i < 7; i++) {
			lineRenderer.SetPosition(i, hexVertices[i]);
		}

		mesh.vertices = hexVertices;

		int[] hexTriangles = new int[] {1, 6, 0, 6, 5, 0, 5, 4, 0, 4, 3, 0, 3, 2, 0, 2, 1, 0 };

		//Haven't done the UVing yet..
		Vector2[] hexUV = new Vector2[] { new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0) };

		mesh.SetVertices(hexVertices.ToList());
		mesh.SetUVs(0, hexUV.ToList());
		mesh.SetTriangles(hexTriangles, 0);

		mesh.RecalculateBounds();
	}	

	float[] hex_corner(float x, float y, int i){
		int angle_deg = 60 * i   + 30;
		float angle_rad = Mathf.PI / 180 * angle_deg;
		float[] res = new float[2];
		res[0] = x + 0.5f * Mathf.Cos (angle_rad);
		res[1] = y + 0.5f * Mathf.Sin (angle_rad);
		return res;
	}
}