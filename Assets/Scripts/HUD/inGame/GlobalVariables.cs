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

	// Gold
	public static float changeFoodGold = 0.1f;
	public static float healthCost = 0.5f;
	public static int admiralCost = 2000;
	public static int filibusterCost = 200;
	public static int powderMonkeyCost = 70;
	public static int conjurerCost = 150;

	public static int buildingTime = 2;

	//Stockage d'information
	public static string pathMaps = Application.persistentDataPath + "/PrefabricatedMaps/";
	public static string pathSaves = Application.persistentDataPath + "/Saves/";
}