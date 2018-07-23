using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class GameObjectPointerEventHandlers : MonoBehaviour {

	private bool isGazedAt;
	private bool isClicked;
	private Color originalColor;

	private MeshRenderer meshRenderer;

	private void Awake() {
		meshRenderer = GetComponent<MeshRenderer> ();
		originalColor = meshRenderer.material.color;
	}

	private void Update() {
		if (isGazedAt) {
			if (isClicked) {
				meshRenderer.material.color = Color.red;
			} else {
				meshRenderer.material.color = Color.green;
			}
		} else {
			meshRenderer.material.color = originalColor;
		}
	}

	/* Pointer Events */

	public void HandlePointerEnter() {
		isGazedAt = true;
	}

	public void HandlePointerExit() {
		isGazedAt = false;
		isClicked = false;
	}

	public void HandlePointerClick() {
		isClicked = true;
	}
}
