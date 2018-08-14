// The MIT License (MIT)
//
// Copyright (c) 2014, Unity Technologies & Google, Inc.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
//   The above copyright notice and this permission notice shall be included in
//   all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//   THE SOFTWARE.
using UnityEngine;
using UnityEngine.EventSystems;

public class Pvr_UnitySDKSightInputModule : BaseInputModule
{


    [Tooltip("Optional object to place at raycast intersections as a 3D cursor. " +
             "Be sure it is on a layer that raycasts will ignore.")]
    public GameObject cursor;
    public int trigger = 0;
    [HideInInspector]
    public float clickTime = 0.1f;  // Based on default time for a button to animate to Pressed.


    [HideInInspector]
    public Vector2 hotspot = new Vector2(0.5f, 0.5f);

    private PointerEventData pointerData;

 
    public override bool ShouldActivateModule()
    {

        if (!base.ShouldActivateModule())
        {
            return false;
        }
        return Pvr_UnitySDKManager.SDK.VRModeEnabled;
    }

 
    public override void DeactivateModule()
    {
        base.DeactivateModule();
        if (pointerData != null)
        {
            HandlePendingClick();
            HandlePointerExitAndEnter(pointerData, null);
            pointerData = null;
        }
        eventSystem.SetSelectedGameObject(null, GetBaseEventData());
        if (cursor != null)
        {
            cursor.SetActive(false);
        }
    }

    public override bool IsPointerOverGameObject(int pointerId)
    {
        return pointerData != null && pointerData.pointerEnter != null;
    }
    
    public override void Process()
    {
        CastRayFromGaze();
        UpdateCurrentObject();
        PlaceCursor();
        HandlePendingClick();
        HandleTrigger();
    }
    
    private void CastRayFromGaze()
    {
        if (pointerData == null)
        {
            pointerData = new PointerEventData(eventSystem);
        }
        pointerData.Reset();
        pointerData.position = new Vector2(hotspot.x * Screen.width, hotspot.y * Screen.height);
        eventSystem.RaycastAll(pointerData, m_RaycastResultCache);
        pointerData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_RaycastResultCache.Clear();
    }
    
    private void UpdateCurrentObject()
    {
        // Send enter events and update the highlight.
        var go = pointerData.pointerCurrentRaycast.gameObject;
        HandlePointerExitAndEnter(pointerData, go);
        // Update the current selection, or clear if it is no longer the current object.
        var selected = ExecuteEvents.GetEventHandler<ISelectHandler>(go);
        if (selected == eventSystem.currentSelectedGameObject)
        {
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, GetBaseEventData(),
                                  ExecuteEvents.updateSelectedHandler);
        }
        else {
            eventSystem.SetSelectedGameObject(null, pointerData);
        }
    }
    
    private void PlaceCursor()
    {
        if (cursor == null)
            return;
        var go = pointerData.pointerCurrentRaycast.gameObject;
        cursor.SetActive(go != null);
        if (cursor.activeInHierarchy)
        {
            Camera cam = pointerData.enterEventCamera;
            // Note: rays through screen start at near clipping plane.
            float dist = pointerData.pointerCurrentRaycast.distance + cam.nearClipPlane - 0.1f;
            

            //float dist = pointerData.pointerCurrentRaycast.distance;

            cursor.transform.position = cam.transform.position + cam.transform.forward * dist;

        }
    }
    
    private void HandlePendingClick()
    {
        if (!pointerData.eligibleForClick)
        {
            return;
        }
        if (!Pvr_UnitySDKManager.SDK.picovrTriggered
            && Time.unscaledTime - pointerData.clickTime < clickTime)
        {
            return;
        }

        // Send pointer up and click events.
        ExecuteEvents.Execute(pointerData.pointerPress, pointerData, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(pointerData.pointerPress, pointerData, ExecuteEvents.pointerClickHandler);

        // Clear the click state.
        pointerData.pointerPress = null;
        pointerData.rawPointerPress = null;
        pointerData.eligibleForClick = false;
        pointerData.clickCount = 0;
    }

    private void HandleTrigger()
    {
        if (!Pvr_UnitySDKManager.SDK.picovrTriggered)
        {
            return;
        }
        var go = pointerData.pointerCurrentRaycast.gameObject;
        //---------------------------------------------------------------

        if (go == null || ExecuteEvents.GetEventHandler<IPointerClickHandler>(go) == null)
        {
            return;
        }
        pointerData.pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(go);
#if PicoInputMethod
        GameObject target = pointerData.pointerPress;
#endif
        //------------------------------------------
        pointerData.pressPosition = pointerData.position;
        pointerData.pointerPressRaycast = pointerData.pointerCurrentRaycast;
        pointerData.pointerPress =
            ExecuteEvents.ExecuteHierarchy(go, pointerData, ExecuteEvents.pointerDownHandler)
            ?? ExecuteEvents.GetEventHandler<IPointerClickHandler>(go);

        pointerData.rawPointerPress = go;
        pointerData.eligibleForClick = true;
        pointerData.clickCount = 1;
        pointerData.clickTime = Time.unscaledTime;
    }

}

