using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UGUIConcretePointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
	private string originalText;
	private Text label;

	private void Awake () {
		label = GetComponentInChildren<Text> ();
		originalText = label.text;
	}

	/* Pointer Events */

	public void OnPointerEnter(PointerEventData eventData) {
		label.text = "Hovered";
	}

	public void OnPointerExit(PointerEventData eventData) {
		label.text = originalText;
	}

	public void OnPointerClick(PointerEventData eventData) {
		label.text = "Clicked!";
	}
}
