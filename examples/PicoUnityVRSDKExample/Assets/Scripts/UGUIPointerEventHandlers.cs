using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UGUIPointerEventHandlers : MonoBehaviour {
	private string originalText;
	private Text label;

	private void Awake () {
		label = GetComponentInChildren<Text> ();
		originalText = label.text;
	}

	/* Pointer Events */

	public void HandlePointerEnter() {
		label.text = "Hovered";
	}

	public void HandlePointerExit() {
		label.text = originalText;
	}

	public void HandleClicked() {
		label.text = "Clicked!";
	}
}
