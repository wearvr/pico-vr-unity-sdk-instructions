#if !UNITY_EDITOR
#if UNITY_ANDROID
#define ANDROID_DEVICE
#elif UNITY_IPHONE
#define IOS_DEVICE
#elif UNITY_STANDALONE_WIN
#define WIN_DEVICE
#endif
#endif



using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Pvr_UnitySDKAPI;

[RequireComponent(typeof(Camera))]
public class Pvr_UnitySDKOverlay : MonoBehaviour
{


    public enum OverlaySide
    {
        OverlayLeft,
        OverlayRight,
        OverlayBoth
    }

    public OverlaySide overlaySide;
    public Camera eyecamera { get; private set; }

    private const int bufferSize = 3;
    private int IDIndex = 0;


#if ANDROID_DEVICE
    [DllImport("Pvr_UnitySDK")]
    private static extern int PvrSetOverlayTextureIDs(int side, int id);
#endif

    public void EyeRender()
    {
        SetupUpdate();
        if (Pvr_UnitySDKManager.SDK.overlayTextures[IDIndex] != null)
        {
            Pvr_UnitySDKManager.SDK.overlayTextures[IDIndex].DiscardContents();
            eyecamera.targetTexture = Pvr_UnitySDKManager.SDK.overlayTextures[IDIndex];
        }
    }



    private void Setup(OverlaySide overlaySide)
    {
        eyecamera = GetComponent<Camera>();

        eyecamera.aspect = 1.0f;
        eyecamera.rect = new Rect(0, 0, 1, 1);
        if (overlaySide != OverlaySide.OverlayBoth)
        {
            Eye eyeParam = (overlaySide == OverlaySide.OverlayLeft ? Eye.LeftEye : Eye.RightEye);
            transform.localPosition = Pvr_UnitySDKManager.SDK.EyeOffset(eyeParam);

#if UNITY_EDITOR
            eyecamera.rect = Pvr_UnitySDKManager.SDK.EyeRect(eyeParam);
#endif

        }
    }

    private void SetupUpdate()
    {
        eyecamera.fieldOfView = Pvr_UnitySDKManager.SDK.EyeFov;
        if (overlaySide != OverlaySide.OverlayBoth)
        {
            IDIndex = Pvr_UnitySDKManager.SDK.currEyeTextureIdx + (int)overlaySide * bufferSize;
        }
        else
        {
            IDIndex = Pvr_UnitySDKManager.SDK.currEyeTextureIdx;
        }

        //eyecamera.enabled = true;
    }


    void Awake()
    {
        eyecamera = this.GetComponent<Camera>();
    }


    // Use this for initialization
    void Start()
    {
        Setup(overlaySide);

        SetupUpdate();
        if (Pvr_UnitySDKManager.SDK.overlayTextures[IDIndex] != null)
        {
            eyecamera.enabled = true;
        }
        else
        {
            eyecamera.enabled = false;
#if ANDROID_DEVICE
            Debug.LogError( "overlayTextures have not created! overlaySide = " + overlaySide );
#endif
        }
    }

    // Update is called once per frame
    void Update()
    {
        EyeRender();
    }


    void OnPreCull()
    { }

    void OnPostRender()
    {
        int id = Pvr_UnitySDKManager.SDK.overlayTextureIds[IDIndex];
        
#if ANDROID_DEVICE
        if (overlaySide != OverlaySide.OverlayBoth)
        {
            PvrSetOverlayTextureIDs((int)overlaySide, id);
        }
        else
        {
            PvrSetOverlayTextureIDs(0, id);
            PvrSetOverlayTextureIDs(1, id);
        }       
#endif
    }
}
