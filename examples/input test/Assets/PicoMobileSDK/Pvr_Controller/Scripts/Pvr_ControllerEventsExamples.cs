using UnityEngine;
using System.Collections;

public class Pvr_ControllerEventsExamples : MonoBehaviour {

	// Use this for initialization
	void Start () {

	    if (GetComponent<Pvr_UIPointer>() == null)
	    {
	        return;
	    }

	    GetComponent<Pvr_UIPointer>().UIPointerElementEnter += UIPointerElementEnter;
	    GetComponent<Pvr_UIPointer>().UIPointerElementExit += UIPointerElementExit;
	    GetComponent<Pvr_UIPointer>().UIPointerElementClick += UIPointerElementClick;
	    GetComponent<Pvr_UIPointer>().UIPointerElementDragStart += UIPointerElementDragStart;
	    GetComponent<Pvr_UIPointer>().UIPointerElementDragEnd += UIPointerElementDragEnd;
    }

    private void UIPointerElementEnter(object sender,UIPointerEventArgs e)
    {
        Debug.Log("UI Pointer entered" + e.currentTarget.name);
    }
    private void UIPointerElementExit(object sender, UIPointerEventArgs e)
    {
        Debug.Log("UI Pointer exited" + e.currentTarget.name);
    }
    private void UIPointerElementClick(object sender, UIPointerEventArgs e)
    {
        Debug.Log("UI Pointer clicked" + e.currentTarget.name);
    }
    private void UIPointerElementDragStart(object sender, UIPointerEventArgs e)
    {
        Debug.Log("UI Pointer started dragging" + e.currentTarget.name);
    }
    private void UIPointerElementDragEnd(object sender, UIPointerEventArgs e)
    {
        Debug.Log("UI Pointer stopped dragging" + e.currentTarget.name);
    }
}
