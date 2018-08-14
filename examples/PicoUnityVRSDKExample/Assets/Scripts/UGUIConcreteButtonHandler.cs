using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Pvr_UnitySDKAPI;

public class UGUIConcreteButtonHandler : MonoBehaviour {
    private string originalText;
	private Text label;
	private float startTime;
    
	private void Awake () {
		label = GetComponentInChildren<Text> ();
		originalText = label.text;
	}
    
    private void Update() {

        if (Controller.UPvr_GetKey(0, Pvr_KeyCode.TOUCHPAD))
        {
            label.text = "Pressed Touchpad";
        }
        else if (Controller.UPvr_GetKey(0, Pvr_KeyCode.HOME))
        {
            label.text = "Pressed Home Button";
        }
        else if (Controller.UPvr_GetKey(0, Pvr_KeyCode.APP))
        {
            label.text = "Pressed App";
        }
        else if (Controller.UPvr_GetKey(0, Pvr_KeyCode.VOLUMEUP))
        {
            label.text = "Pressed Volume+";
        }
        else if (Controller.UPvr_GetKey(0, Pvr_KeyCode.VOLUMEDOWN))
        {
            label.text = "Pressed Volume-";
        }
    }
}
