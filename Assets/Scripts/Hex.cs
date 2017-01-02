using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Hex : MonoBehaviour {

	// Const

	public int x;
	public int y;
	protected bool isWalkable;
	protected string type;

    // Coordinates in the grid (not unity unit)
	public float movementCost;

    public GameObject[] getNeighboursOld(){
        GameObject[] Neighbours = new GameObject[6];
        GameObject leftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + y);
        GameObject rightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + y);
        Neighbours[0] = leftNeighbour;
        Neighbours[1] = rightNeighbour;

        if (y % 2 == 0){
            GameObject upperLeftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + (y + 1));
            GameObject upperRightNeighbour = GameObject.Find("Hex_" + x + "_" + (y + 1));
            GameObject lowerLeftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + (y - 1));
            GameObject lowerRightNeighbour = GameObject.Find("Hex_" + x + "_" + (y - 1));
            Neighbours[2] = upperLeftNeighbour;
            Neighbours[3] = upperRightNeighbour;
            Neighbours[4] = lowerLeftNeighbour;
            Neighbours[5] = lowerRightNeighbour;
        }
        else{
            GameObject upperLeftNeighbour = GameObject.Find("Hex_" + x + "_" + (y + 1));
            GameObject upperRightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + (y + 1));
            GameObject lowerLeftNeighbour = GameObject.Find("Hex_" + x + "_" + (y - 1));
            GameObject lowerRightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + (y - 1));
            Neighbours[2] = upperLeftNeighbour;
            Neighbours[3] = upperRightNeighbour;
            Neighbours[4] = lowerLeftNeighbour;
            Neighbours[5] = lowerRightNeighbour;
        }

        return Neighbours;
    }

    public List<GameObject> getNeighbours(){
        List<GameObject> Neighbours = new List<GameObject>();
        GameObject leftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + y);
        GameObject rightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + y);
        if (leftNeighbour != null) { Neighbours.Add(leftNeighbour); }
        if (rightNeighbour != null) { Neighbours.Add(rightNeighbour); }

        if (y % 2 == 0)
        {
            GameObject upperLeftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + (y + 1));
            GameObject upperRightNeighbour = GameObject.Find("Hex_" + x + "_" + (y + 1));
            GameObject lowerLeftNeighbour = GameObject.Find("Hex_" + (x - 1) + "_" + (y - 1));
            GameObject lowerRightNeighbour = GameObject.Find("Hex_" + x + "_" + (y - 1));
            if (upperLeftNeighbour != null) { Neighbours.Add(upperLeftNeighbour); }
            if (upperRightNeighbour != null) { Neighbours.Add(upperRightNeighbour); }
            if (lowerLeftNeighbour != null) { Neighbours.Add(lowerLeftNeighbour); }
            if (lowerRightNeighbour != null) { Neighbours.Add(lowerRightNeighbour); }
        }
        else
        {
            GameObject upperLeftNeighbour = GameObject.Find("Hex_" + x + "_" + (y + 1));
            GameObject upperRightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + (y + 1));
            GameObject lowerLeftNeighbour = GameObject.Find("Hex_" + x + "_" + (y - 1));
            GameObject lowerRightNeighbour = GameObject.Find("Hex_" + (x + 1) + "_" + (y - 1));
            if (upperLeftNeighbour != null) { Neighbours.Add(upperLeftNeighbour); }
            if (upperRightNeighbour != null) { Neighbours.Add(upperRightNeighbour); }
            if (lowerLeftNeighbour != null) { Neighbours.Add(lowerLeftNeighbour); }
            if (lowerRightNeighbour != null) { Neighbours.Add(lowerRightNeighbour); }
        }

        return Neighbours;
    }


	/**
	 * Always set attribute depth to 0
	 * maxDepth is the number of level of neighbours you want
	 */
	public List<GameObject> getNLevelOfNeighbours(int depth, int maxDepth){
		List<GameObject> neighbours = new List<GameObject> ();
		List<GameObject> tmpNeighbours = new List<GameObject> ();
		if (depth >= maxDepth) {
			return neighbours;
		} else {			
			neighbours = this.getNeighbours();
			foreach (GameObject go in neighbours) {
				tmpNeighbours.AddRange(go.GetComponent<Hex> ().getNLevelOfNeighbours (depth + 1, maxDepth));
			}
			neighbours.AddRange(tmpNeighbours);
			neighbours = neighbours.Distinct ().ToList ();
			return neighbours;
		}
	}

	public void setVisibility(int visibilityLevel){
		MeshRenderer[] meshRenderers;
		MeshCollider[] meshColliders;
		BoxCollider[] boxColliders;
		FOWManager fowm = GameObject.Find ("FOWManager").GetComponent<FOWManager> ();;
		// Visibility Level
		// 0 for non-explored
		// 1 for explored
		// 2 for visible
		switch (visibilityLevel) {
		case 0:
			if (this.GetComponent<Sea> () != null && this.GetComponent<Sea> ().ShipContained != null) {
				this.GetComponent<Sea> ().ShipContained.GetComponentInChildren<MeshRenderer> ().enabled = false;
				this.GetComponent<Sea> ().ShipContained.DisplayHp (false);
			}
			this.GetComponent<LineRenderer> ().enabled = false;
			this.GetComponentInChildren<MeshRenderer> ().material = fowm.notVisibleMat;
			meshRenderers = this.GetComponentsInChildren<MeshRenderer> ();
			foreach (MeshRenderer mr in meshRenderers) {
				if (!mr.name.Equals("FlatHexagon")){
					mr.enabled = false;
				}
			}
			meshColliders = this.GetComponentsInChildren<MeshCollider> ();
			foreach (MeshCollider mc in meshColliders) {
				mc.enabled = false;
			}
			boxColliders = this.GetComponentsInChildren<BoxCollider> ();
			foreach (BoxCollider bc in boxColliders) {
				bc.enabled = false;
			}
			break;
		case 1:
			if (this.GetComponent<Sea> () != null && this.GetComponent<Sea> ().ShipContained != null) {
				this.GetComponent<Sea> ().ShipContained.GetComponentInChildren<MeshRenderer> ().enabled = false;
				this.GetComponent<Sea> ().ShipContained.DisplayHp (false);
			}
			this.GetComponent<LineRenderer> ().enabled = true;
			meshRenderers = this.GetComponentsInChildren<MeshRenderer> ();
			foreach (MeshRenderer mr in meshRenderers) {
				mr.enabled = true;
			}
			meshColliders = this.GetComponentsInChildren<MeshCollider> ();
			foreach (MeshCollider mc in meshColliders) {
				mc.enabled = false;
			}
			boxColliders = this.GetComponentsInChildren<BoxCollider> ();
			foreach (BoxCollider bc in boxColliders) {
				bc.enabled = false;
			}
			if (this.GetComponent<Sea> () != null) {
				this.GetComponentInChildren<MeshRenderer> ().material = fowm.exploredWaterMat;
			}
			if (this.GetComponent<Harbor> () != null) {
				this.GetComponentInChildren<MeshRenderer> ().material = fowm.exploredHarborMat;
			} else if (this.GetComponent<Land> () != null) {
				if (this.GetComponent<Land> ().IsCoast) {
					this.GetComponentInChildren<MeshRenderer> ().material = fowm.exploredCoastMat;
				} else {
					this.GetComponentInChildren<MeshRenderer> ().material = fowm.exploredLandMat;
				}
			}
			break;
		case 2:
			if (this.GetComponent<Sea> () != null && this.GetComponent<Sea> ().ShipContained != null) {
				this.GetComponent<Sea> ().ShipContained.GetComponentInChildren<MeshRenderer> ().enabled = true;
			}
			this.GetComponent<LineRenderer> ().enabled = true;
			meshRenderers = this.GetComponentsInChildren<MeshRenderer> ();
			foreach (MeshRenderer mr in meshRenderers) {
				mr.enabled = true;
			}
			meshColliders = this.GetComponentsInChildren<MeshCollider> ();
			foreach (MeshCollider mc in meshColliders) {
				mc.enabled = true;
			}
			boxColliders = this.GetComponentsInChildren<BoxCollider> ();
			foreach (BoxCollider bc in boxColliders) {
				bc.enabled = true;
			}
			if (this.GetComponent<Sea> () != null) {
				this.GetComponentInChildren<MeshRenderer> ().material = fowm.waterMat;
			}
			if (this.GetComponent<Harbor> () != null) {
				this.GetComponentInChildren<MeshRenderer> ().material = fowm.harborMat;
			} else if (this.GetComponent<Land> () != null) {
				if (this.GetComponent<Land> ().IsCoast) {
					this.GetComponentInChildren<MeshRenderer> ().material = fowm.coastMat;
				} else {
					this.GetComponentInChildren<MeshRenderer> ().material = fowm.landMat;
				}
			}
			break;
		}
	}

    public string Type{
		get { return type; }
		set { type = value; }
    }
		

	public bool IsWalkable{
		get { return isWalkable; }
		set { isWalkable = value; }
	}



}
