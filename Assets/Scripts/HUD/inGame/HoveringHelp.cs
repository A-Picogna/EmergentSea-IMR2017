using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoveringHelp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public GameObject panelHelp;
	public int type;
	private HelpPanel helpPanel;
	private bool activated;

	// Use this for initialization
	void Start () {
		if (panelHelp == null) {
			PanelHandler pnlH = GetComponentInParent<Canvas>().GetComponent<PanelHandler>();
			panelHelp = pnlH.panelHelp2;
		}
		panelHelp.SetActive(false);
		activated = false;
		helpPanel = panelHelp.GetComponent<HelpPanel> ();
	}

	void Update() {
		if (activated) {
			helpPanel.refreshPosition ();
		}
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		print (helpPanel);
		print ("Change text");
		helpPanel.changeText (type);
		print ("Refresh");
		helpPanel.refresh ();
		print ("Active");
		activated = true;
		print ("Show");
		helpPanel.showPanel ();
		print (panelHelp.activeSelf);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		print ("Out");
		activated = false;
		helpPanel.hidePanel ();
	}
}