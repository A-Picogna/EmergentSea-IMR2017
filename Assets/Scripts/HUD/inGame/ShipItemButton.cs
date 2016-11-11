// @author Yasmine Serot
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShipItemButton : MonoBehaviour {
	public Button button;
	public Text shipName;
	public Slider percentageQE;
	public Text goldAmount;
	public Text foodAmount;

	private Item item;
	private ShipScrollList scrollList;

	void Start() {
		button.onClick.AddListener (HandleClick);
	}

	public void Setup (Item currentItem, ShipScrollList currentScrollList) {
		item = currentItem;
		shipName.text = item.shipName;
		percentageQE.value = item.percentageQE;
		goldAmount.text = item.goldAmount.ToString(); // Utility function to form correct numbers
		foodAmount.text = item.foodAmount.ToString();

		scrollList = currentScrollList;
	}

	public void HandleClick() {
		
	}
}
