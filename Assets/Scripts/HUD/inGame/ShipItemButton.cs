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

	private ShipItem item;

	void Start() {
		button.onClick.AddListener (HandleClick);
	}

	public void Setup (ShipItem currentItem) {
		item = currentItem;
		shipName.text = item.shipName;
		percentageQE.value = item.percentageQE;
		goldAmount.text = amountformatter(item.goldAmount); // Utility function to form correct numbers
		foodAmount.text = amountformatter(item.foodAmount);
	}

	private string amountformatter(int amount) {
		if (amount < Mathf.Pow(10, 3)) { 
			return amount.ToString();
		} else if (amount < Mathf.Pow(10, 6)) {
			return amount / (int)Mathf.Pow(10, 3) + "k";
		} else if (amount < Mathf.Pow(10, 9)) {
			return amount / (int)Mathf.Pow(10, 6) + "M";
		} else if (amount < Mathf.Pow(10, 12)) {
			return amount / (int)Mathf.Pow(10, 9) + "G";
		} else {
			return amount / (int)Mathf.Pow(10, 12) + "T";
		}
	}

	public void HandleClick() {
		
	}
}
