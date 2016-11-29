using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {
	// Langage
	public static string pathLang = "XML/Languages.xml";
	public static string currentLang = "French";

	//Stockage d'information
	public static string pathMaps = Application.persistentDataPath + "/PrefabricatedMaps/";
	public static string pathSaves = Application.persistentDataPath + "/Saves/";
}