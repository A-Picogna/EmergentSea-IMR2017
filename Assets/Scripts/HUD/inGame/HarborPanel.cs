using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class HarborPanel : MonoBehaviour {
	public GameObject panel;
	public Button button;
	public Button innButton;
	public Button storehouseButton;
	public Button warehouseButton;
	public Button slumsButton;
	public Button darkArtsAcademyButton;
	public Button shipyardButton;

	public Text harborHeader;
	public Text harborDescription;
	public Text inn;
	public Text storehouse;
	public Text warehouse;
	public Text slums;
	public Text darkArtsAcademy;
	public Text shipyard;
	private Lang lang;

    public string buttonClicked;

    public bool selected;

	void Start () {
		panel.SetActive (false);
		button.onClick.AddListener (hidePanel);

		innButton.onClick.AddListener (handleClickInn);
		storehouseButton.onClick.AddListener (handleClickStorehouse);
		warehouseButton.onClick.AddListener (handleClickWarehouse);
		slumsButton.onClick.AddListener (handleClickSlums);
		darkArtsAcademyButton.onClick.AddListener (handleClickDarkArtsAcademy);
		shipyardButton.onClick.AddListener (handleClickShipyard);

		lang = new Lang(Path.Combine(Application.dataPath, GlobalVariables.pathLang), GlobalVariables.currentLang);
		harborHeader.text = lang.getString("harbor");
		harborDescription.text = lang.getString("harbor_description");
		inn.text = lang.getString("inn");
		storehouse.text = lang.getString("storehouse");
		warehouse.text = lang.getString("warehouse");
		slums.text = lang.getString("slums");
		darkArtsAcademy.text = lang.getString("dark_arts_academy");
		shipyard.text = lang.getString("shipyard");

        selected = false;
	}

	public void showPanel() {
		panel.SetActive (true);
	}

	public void hidePanel() {
		panel.SetActive (false);
        selected = false;
    }

	private void handleClickInn() {

        buttonClicked = "tavern";
        selected = true;
    }

	private void handleClickStorehouse() {

        buttonClicked = "store";
        selected = true;
    }
	private void handleClickWarehouse() {

        buttonClicked = "warehouse";
        selected = true;
    }
	private void handleClickSlums() {

        buttonClicked = "shallows";
        selected = true;
    }
	private void handleClickDarkArtsAcademy() {

        buttonClicked = "daa";
        selected = true;
    }
	private void handleClickShipyard() {

        buttonClicked = "shipyard";
        selected = true;
    }
}
