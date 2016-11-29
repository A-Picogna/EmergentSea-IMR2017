using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoveringHelp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public GameObject panelHelp;
	private MouseFollower mouseFollower;
	private bool activated;

	// Use this for initialization
	void Start () {
		panelHelp.SetActive(false);
		activated = false;
		mouseFollower = panelHelp.transform.GetComponent<MouseFollower> ();
	}

	void Update() {
		if (activated)
			mouseFollower.refreshPosition ();
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		activated = true;
		panelHelp.SetActive(true);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		activated = false;
		panelHelp.SetActive(false);
	}
}