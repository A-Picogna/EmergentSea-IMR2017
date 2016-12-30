using UnityEngine;
using System.Collections;

public class GlobalVariables : MonoBehaviour {
	// Languages
	public static string pathLang = "XML/Languages.xml";
	public static string currentLang = "French";

	// Energy Cost
	public static int energyAttack = 5;
	public static int energyTreasure = 1;
	public static int energyFishing = 5;

	// Gold
	public static float changeFoodGold = 0.1f;
	public static float healthCost = 0.5f;
	public static int admiralCost = 2000;
	public static int filibusterCost = 200;
	public static int powderMonkeyCost = 70;
	public static int conjurerCost = 150;

	public static int buildingTime = 2;

	// CrewMember stats
	public static int filibusterAttack = 150;
	public static int powderMonkeyAttack = 100;
	public static int conjurerAttack = 80;
	public static int filibusterLife = 200;
	public static int powderMonkeyLife = 100;
	public static int conjurerLife = 50;

	//Stockage d'information
	public static string pathMaps = Application.persistentDataPath + "/PrefabricatedMaps/";
	public static string pathSaves = Application.persistentDataPath + "/Saves/";
}