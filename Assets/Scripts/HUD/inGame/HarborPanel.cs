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

    public bool tavernClicked;
    public bool storeClicked;
    public bool warehouseClicked;
    public bool shallowsClicked;
    public bool daaClicked;
    public bool shipyardClicked;

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

        tavernClicked = false;
        storeClicked = false;
        warehouseClicked = false;
        shallowsClicked = false;
        daaClicked = false;
        shipyardClicked = false;

        selected = false;
	}

	public void showPanel() {
		panel.SetActive (true);
	}

	public void hidePanel() {
		panel.SetActive (false);
        selected = true;
    }

	private void handleClickInn() {

        tavernClicked = true;
        storeClicked = false;
        warehouseClicked = false;
        shallowsClicked = false;
        daaClicked = false;
        shipyardClicked = false;
        selected = true;
    }

	private void handleClickStorehouse() {

        tavernClicked = false;
        storeClicked = true;
        warehouseClicked = false;
        shallowsClicked = false;
        daaClicked = false;
        shipyardClicked = false;
        selected = true;
    }
	private void handleClickWarehouse() {

        tavernClicked = false;
        storeClicked = false;
        warehouseClicked = true;
        shallowsClicked = false;
        daaClicked = false;
        shipyardClicked = false;
        selected = true;
    }
	private void handleClickSlums() {

        tavernClicked = false;
        storeClicked = false;
        warehouseClicked = false;
        shallowsClicked = true;
        daaClicked = false;
        shipyardClicked = false;
        selected = true;
    }
	private void handleClickDarkArtsAcademy() {

        tavernClicked = false;
        storeClicked = false;
        warehouseClicked = false;
        shallowsClicked = false;
        daaClicked = true;
        shipyardClicked = false;
        selected = true;
    }
	private void handleClickShipyard() {

        tavernClicked = false;
        storeClicked = false;
        warehouseClicked = false;
        shallowsClicked = false;
        daaClicked = false;
        shipyardClicked = true;
        selected = true;
    }
}
