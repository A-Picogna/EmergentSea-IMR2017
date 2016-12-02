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
		helpPanel = panelHelp.GetComponent<HelpPanel> ();
		panelHelp.SetActive(false);
		activated = false;
	}

	void Update() {
		if (activated) {
			helpPanel.showPanel ();
			helpPanel.refreshPosition ();
		}
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		helpPanel.changeText (type);
		helpPanel.refresh ();
		activated = true;
		helpPanel.showPanel ();
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		activated = false;
		helpPanel.hidePanel ();
	}
	public HelpPanel getHelpPanel() {
		return helpPanel;
	}
	public void setHelpPanel(HelpPanel hp) {
		helpPanel = hp;
	}
}