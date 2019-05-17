///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_UnitySDKPostRender
// Author: AiLi.Shang
// Date:  2017/01/18
// Discription: just for developers do somthing on poste render
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class Pvr_UnitySDKPostRender : MonoBehaviour
{ 

    public Camera cam { get; private set; }

    void Awake()
    {
        cam = GetComponent<Camera>();
        Reset();
    } 

    void Reset()
    {
#if UNITY_EDITOR
        var cam = GetComponent<Camera>();
#endif

        cam.clearFlags = CameraClearFlags.Depth;
        cam.backgroundColor = Color.magenta;

        cam.orthographic = true;
        cam.orthographicSize = 0.5f;
        cam.cullingMask = 0;
        cam.useOcclusionCulling = false;
        cam.depth = 100;
    }


#if UNITY_5_6
    void OnRenderObject()
    {
        if (Camera.current != cam)
            return;

        //Debug.Log("LLLL Pvr_UnitySDKPostRender OnRenderObject TimeWarp");
        Pvr_UnitySDKPluginEvent.IssueWithData(RenderEventType.TimeWarp, Pvr_UnitySDKManager.SDK.RenderviewNumber);
        Pvr_UnitySDKManager.SDK.currEyeTextureIdx = Pvr_UnitySDKManager.SDK.nextEyeTextureIdx;
        Pvr_UnitySDKManager.SDK.nextEyeTextureIdx = (Pvr_UnitySDKManager.SDK.nextEyeTextureIdx + 1) % 3;
    }
#endif
}
