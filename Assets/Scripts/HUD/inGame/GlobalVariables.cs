using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {
	// Languages
	public static string pathLang = "XML/Languages.xml";
	public static string currentLang = "French";

	// Energy Cost
	public static int energyAttack = 5;

	//Stockage d'information
	public static string pathMaps = Application.persistentDataPath + "/PrefabricatedMaps/";
	public static string pathSaves = Application.persistentDataPath + "/Saves/";
}