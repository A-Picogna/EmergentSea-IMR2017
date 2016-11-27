using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HoveringHelp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	public GameObject panelHelp;

	// Use this for initialization
	void Start () {
		panelHelp.SetActive(false);
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		panelHelp.SetActive(true);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		panelHelp.SetActive(false);
	}
}
