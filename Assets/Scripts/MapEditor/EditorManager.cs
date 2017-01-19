// @Author Alexandre Picogna

using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System.Threading;

public class EditorManager : MonoBehaviour {

	public MouseManager mouseManager;
	public MapEditor map;
	public List<Player> EMplayers;


	void Start () {
		Init ();
	}


	// Update is called once per frame
	void Update () {
		
	}

	public void Init() {
		this.AddPlayer ("Player1", Color.red, true);
		this.AddPlayer ("Player2", Color.blue, false);
	}

	public Player AddPlayer(string name, Color color, bool type){
		Player newPlayer = new Player (type, color, name);
		EMplayers.Add (newPlayer);
		return newPlayer;
	}

	public Player GetPlayerByName (string name){
		Player result = null;
		for (int i = 0; i < EMplayers.Count; i++) {
			if (EMplayers [i].name.Equals (name)) {
				result = EMplayers [i];
			}
		}
		//Debug.Log (result);
		return result;
	}

	public int GetPlayerIndexByName (string name){
		int result = -1;
		for (int i = 0; i < EMplayers.Count; i++) {
			if (EMplayers [i].name.Equals (name)) {
				result = i;
			}
		}
		return result;
	}
}
