using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {
	// Languages
	public static string pathLang = "XML/Languages.xml";
	public static string currentLang = "French";

	// Energy Cost
	public static int energyAttack = 5;
	public static int energyTreasure = 3;
	public static int energyFishing = 5;

	//Stockage d'information
	public static string pathMaps = Application.persistentDataPath + "/PrefabricatedMaps/";
	public static string pathSaves = Application.persistentDataPath + "/Saves/";
}