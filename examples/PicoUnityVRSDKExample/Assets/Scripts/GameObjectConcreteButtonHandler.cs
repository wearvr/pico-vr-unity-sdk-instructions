using System.Runtime.CompilerServices;
using UnityEngine;
using Pvr_UnitySDKAPI;

[RequireComponent(typeof(MeshRenderer))]
public class GameObjectConcreteButtonHandler : MonoBehaviour {
	private Color originalColor;
	private Color currentColor;
	
	private MeshRenderer meshRenderer;
    
    private void Awake() {
		meshRenderer = GetComponent<MeshRenderer> ();
		originalColor = meshRenderer.material.color;
	    currentColor = originalColor;
    }

	private void Update() {
	    if (Controller.UPvr_GetKey(Pvr_KeyCode.TOUCHPAD))
	    {
		    currentColor = Color.cyan;
	    }
	    else if (Controller.UPvr_GetKey(Pvr_KeyCode.APP))
	    {
		    currentColor = Color.red;
	    }
	    else if (Controller.UPvr_GetKey(Pvr_KeyCode.VOLUMEUP))
	    {
		    currentColor = Color.grey;
	    }
	    else if (Controller.UPvr_GetKey(Pvr_KeyCode.VOLUMEDOWN))
	    {
		    currentColor = Color.white;
	    }

		meshRenderer.material.color = currentColor;
	}

}

