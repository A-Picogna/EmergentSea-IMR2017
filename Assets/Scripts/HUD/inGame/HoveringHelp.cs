using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoveringHelp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public GameObject panelHelp;
	public int type;
	private HelpPanel helpPanel;
	private MouseFollower mouseFollower;
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
		mouseFollower = panelHelp.transform.GetComponent<MouseFollower> ();
	}

	void Update() {
		if (activated)
			mouseFollower.refreshPosition ();
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		activated = true;
		helpPanel.changeText (type);
		helpPanel.refresh ();
		panelHelp.SetActive(true);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		activated = false;
		panelHelp.SetActive(false);
	}
}