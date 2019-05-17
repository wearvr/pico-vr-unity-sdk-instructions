///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_UnitySDKPostRender
// Author: AiLi.Shang
// Date:  2017/01/18
// Discription: just for developers do somthing on pre render
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;


[RequireComponent(typeof(Camera))]
public class Pvr_UnitySDKPreRender : MonoBehaviour
{  
    public Camera cam { get; private set; }

    void Awake()
    {
        cam = GetComponent<Camera>();
    }  

    void Reset()
    {
#if UNITY_EDITOR
        var cam = GetComponent<Camera>();
#endif

        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.cullingMask = 0;
        cam.useOcclusionCulling = false;
        cam.depth = -100;
    }
}
