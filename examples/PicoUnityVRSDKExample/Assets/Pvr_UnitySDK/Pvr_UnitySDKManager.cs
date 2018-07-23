///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Pvr_UnitySDKManager
// Author: AiLi.Shang
// Date:  2017/01/13
// Discription:  Be fully careful of  Code modification!
/////////////////////////////////////////////////////////////////////////////// 
using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Collections.Generic;
using Pvr_UnitySDKAPI;

public class Pvr_UnitySDKManager : MonoBehaviour
{
    /************************************    Properties  *************************************/
    #region Properties
    public static PlatForm platform;

    public bool Enable6Dof = false;
    //signtal                   
    private static Pvr_UnitySDKManager sdk = null;
    public static Pvr_UnitySDKManager SDK
    {
        get
        {
            if (sdk == null)
            {
                sdk = UnityEngine.Object.FindObjectOfType<Pvr_UnitySDKManager>();
            }
            if (sdk == null)
            {
                var go = new GameObject("Pvr_UnitySDKManager");
                sdk = go.AddComponent<Pvr_UnitySDKManager>();
                go.transform.localPosition = Vector3.zero;
            }
            return sdk;
        }
    }

    // Sensor
    [HideInInspector]
    public static Pvr_UnitySDKSensor pvr_UnitySDKSensor;
    [HideInInspector]
    public Pvr_UnitySDKPose HeadPose;
    [HideInInspector]
    public bool reStartHead = false;
    //render
    [HideInInspector]
    public static Pvr_UnitySDKRender pvr_UnitySDKRender;
    [SerializeField]
    private float eyeFov = 90.0f;
    [HideInInspector]
    public float EyeFov
    {
        get
        {
            return eyeFov;
        }
        set
        {
            if (value != eyeFov)
            {
                eyeFov = value;
            }
        }
    }
    [HideInInspector]
    public const int eyeTextureCount = 6;
    [HideInInspector]
    public RenderTexture[] eyeTextures = new RenderTexture[eyeTextureCount];
    [HideInInspector]
    public int[] eyeTextureIds = new int[eyeTextureCount];
    [HideInInspector]
    public int currEyeTextureIdx = 0;
    [HideInInspector]
    public int nextEyeTextureIdx = 1;
    [HideInInspector]
    public int resetRot = 0;
    [HideInInspector]
    public int resetPos = 0;


    [SerializeField]
    private RenderTextureAntiAliasing rtAntiAlising = RenderTextureAntiAliasing.X_2;
    public RenderTextureAntiAliasing RtAntiAlising
    {
        get
        {
            return rtAntiAlising;
        }
        set
        {
            if (value != rtAntiAlising)
            {
                rtAntiAlising = value;

            }
        }
    }
    [SerializeField]
    private RenderTextureDepth rtBitDepth = RenderTextureDepth.BD_24;
    public RenderTextureDepth RtBitDepth
    {
        get
        {
            return rtBitDepth;
        }
        set
        {
            if (value != rtBitDepth)
                rtBitDepth = value;

        }
    }
    [SerializeField]
    private RenderTextureFormat rtFormat = RenderTextureFormat.Default;
    public RenderTextureFormat RtFormat
    {
        get
        {
            return rtFormat;
        }
        set
        {
            if (value != rtFormat)
                rtFormat = value;

        }
    }


    [SerializeField]
     public float RtSizeWH = 1280.0f;

  

    [HideInInspector]
    public int RenderviewNumber = 0;
    public Vector3 EyeOffset(Eye eye)
    {
        return eye == Eye.LeftEye ? leftEyeOffset : rightEyeOffset;
    }
    [HideInInspector]
    public Vector3 leftEyeOffset;
    [HideInInspector]
    public Vector3 rightEyeOffset;
    public Rect EyeRect(Eye eye)
    {
        return eye == Eye.LeftEye ? leftEyeRect : rightEyeRect;
    }
    [HideInInspector]
    public Rect leftEyeRect;
    [HideInInspector]
    public Rect rightEyeRect;
    [HideInInspector]
    public Matrix4x4 leftEyeView;
    [HideInInspector]
    public Matrix4x4 rightEyeView;

    // unity editor
    [HideInInspector]
    public Pvr_UnitySDKEditor pvr_UnitySDKEditor;
    [SerializeField]
    private bool vrModeEnabled = true;
    [HideInInspector]
    public bool VRModeEnabled
    {

        get
        {
            return vrModeEnabled;
        }
        set
        {
            if (value != vrModeEnabled)
                vrModeEnabled = value;

        }
    }
    [HideInInspector]
    public Material Eyematerial;
    [HideInInspector]
    public Material Middlematerial;
    [HideInInspector]
    public bool picovrTriggered { get; set; }
    [HideInInspector]
    public bool newPicovrTriggered = false;

    // FPS
    [SerializeField]
    private bool showFPS = false;
    public bool ShowFPS
    {
        get
        {
            return showFPS;
        }
        set
        {
            if (value != showFPS)
            {
                showFPS = value;
            }
        }
    }

    // screenFade
    [SerializeField] 
    private bool screenFade = false;
    public bool ScreenFade
    {
        get
        {
            return screenFade;
        }
        set
        {
            if (value != screenFade)
            {
                screenFade = value;
            }
        }
    }
    //Neck model
    [HideInInspector]
    public Vector3 neckOffset = new Vector3(0, 0.075f, 0.0805f);
    [SerializeField]
    private static bool pvrNeck = true;
    [HideInInspector]
    public static bool PVRNeck
    {
        get
        {
            return pvrNeck;
        }
        set
        {
            if (value != pvrNeck)
            {
                pvrNeck = value;
            }
        }
    }

    // life
    [HideInInspector]
    public bool onResume = false;


    public Pvr_UnitySDKConfigProfile pvr_UnitySDKConfig;
    #endregion

    /************************************ Public Interfaces  *********************************/
    #region Public Interfaces

    #endregion

    /************************************ Private Interfaces  *********************************/
    #region Private Interfaces

    private void AddPrePostRenderStages()
    {
        var preRender = UnityEngine.Object.FindObjectOfType<Pvr_UnitySDKPreRender>();
        if (preRender == null)
        {
            var go = new GameObject("PreRender", typeof(Pvr_UnitySDKPreRender));
            go.SendMessage("Reset");
            go.transform.parent = transform;
        }

        var postRender = UnityEngine.Object.FindObjectOfType<Pvr_UnitySDKPostRender>();
        if (postRender == null)
        {
            var go = new GameObject("PostRender", typeof(Pvr_UnitySDKPostRender));
            go.SendMessage("Reset");
            go.transform.parent = transform;
        }
    }


    private bool SDKManagerInit()
    {
        if ((ShowFPS && SDKManagerInitFPS()) || !ShowFPS)
        {
            if (SDKManagerInitConfigProfile())
            {
#if UNITY_EDITOR
                if (SDKManagerInitEditor())
                    return true;
                else
                    return false;
#else
                
                if (SDKManagerInitCoreAbility())
                   
                    return true;
                else
                    return false;
#endif
            }
            else
                return false;
        }
        return false;

    }

    private bool SDKManagerInitCoreAbility()
    {
        AddPrePostRenderStages();

        if (pvr_UnitySDKRender == null)
        {
            Debug.Log("pvr_UnitySDKRender  init");
           // pvr_UnitySDKRender = this.gameObject.AddComponent<Pvr_UnitySDKRender>();
            pvr_UnitySDKRender = new Pvr_UnitySDKRender();
          

        }else
            pvr_UnitySDKRender.Init();
        if (pvr_UnitySDKSensor == null)
        {
            Debug.Log("pvr_UnitySDKSensor init");
            HeadPose = new Pvr_UnitySDKPose(Vector3.forward, Quaternion.identity);
            // pvr_UnitySDKSensor = this.gameObject.AddComponent<Pvr_UnitySDKSensor>();
             pvr_UnitySDKSensor = new Pvr_UnitySDKSensor();
           // pvr_UnitySDKSensor.Init();
        }
        Pvr_UnitySDKAPI.System.UPvr_StartHomeKeyReceiver(this.gameObject.name);
        return true;
    }

    private bool SDKManagerInitFPS()
    {
        Transform[] father;
        father = GetComponentsInChildren<Transform>(true);
        GameObject FPS = null;
        foreach (Transform child in father)
        {
            if (child.gameObject.name == "FPS")
            {
                FPS = child.gameObject;
            }
        }
        if (FPS != null && ShowFPS)
        {
            FPS.SetActive(showFPS);
            return true;
        }
        else
            return false;

    }

    private bool SDKManagerInitConfigProfile()
    {
        pvr_UnitySDKConfig = Pvr_UnitySDKConfigProfile.Default;
        return true;
    }

    private bool SDKManagerInitEditor()
    {
        if (pvr_UnitySDKEditor == null)
        {
            HeadPose = new Pvr_UnitySDKPose(Vector3.forward, Quaternion.identity);
            pvr_UnitySDKEditor = this.gameObject.AddComponent<Pvr_UnitySDKEditor>();
        }
        return true;
    }

    private bool SDKManagerInitPara()
    {
        return true;
    }

    private void SDKManagerLongHomeKey()
    {
        if (pvr_UnitySDKSensor != null)
        {
            if (pvr_UnitySDKSensor.ResetUnitySDKSensor())
            {
                Debug.Log("Long Home Key to Reset Sensor Success!");
            }
            else{
                Debug.Log("Long Home Key to Reset Sensor Failed!");
            }
        } 
    }

    private void setLongHomeKey()
    {
        SDKManagerLongHomeKey();
    }
    #endregion

    /*************************************  Unity API ****************************************/
    #region Unity API

    void Awake()
    {
        Application.targetFrameRate = 60;
        if (sdk == null)
        {
            sdk = this;
        }
        if (sdk != this)
        {
            Debug.LogError("SDK object should be a singleton.");
            enabled = false;
            return;
        }
        if (SDKManagerInit())
        {
            Debug.Log("SDK Init success.");
        }
        else
        {
            Debug.LogError("SDK Init Failed.");
            Application.Quit();
        }   
    }
   

    void Update()
    {
        if (Input.touchCount == 1)//一个手指触摸屏幕
        {
            if (Input.touches[0].phase == TouchPhase.Began)//开始触屏
            {
                newPicovrTriggered = true;
            }
        }
        else
         if (Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            newPicovrTriggered = true;
        }

        if (pvr_UnitySDKSensor != null)
        {
            pvr_UnitySDKSensor.SensorUpdate();
        }
       

        picovrTriggered = newPicovrTriggered;
        newPicovrTriggered = false;
    }

    void OnDestroy()
    {
        if (sdk == this)
        {
            sdk = null;
        }
        RenderTexture.active = null;
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public void OnApplicationQuit()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        /*
               if (pvr_UnitySDKSensor != null)
                 {
                pvr_UnitySDKSensor.StopUnitySDKSensor();
                  }
                try{
                    Debug.Log("OnApplicationQuit  1  -------------------------");
                    Pvr_UnitySDKPluginEvent.Issue( RenderEventType.ShutdownRenderThread );
                }
                catch (Exception e)
                {
                    Debug.Log("ShutdownRenderThread Error" + e.Message);
                }
        */
#endif
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnPause()
    {
         LeaveVRMode();
		if (pvr_UnitySDKSensor != null)
        {
            pvr_UnitySDKSensor.StopUnitySDKSensor();
        }
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("OnApplicationPause-------------------------" + (pause ? "true" : "false"));
#if UNITY_ANDROID && !UNITY_EDITOR
        if (pause)
        {
            OnPause();
        }
        else
        {
            onResume = true;    
            GL.InvalidateState();
            StartCoroutine(OnResume());
        }
#endif
    }

    void OnApplicationFocus(bool focus)
    {
        Debug.Log("OnApplicationFocus-------------------------" + (focus ? "true" : "false"));
    }

    public static void EnterVRMode()
    {
        Pvr_UnitySDKPluginEvent.Issue(RenderEventType.Resume);
    }

    public static void LeaveVRMode()
    {
        Pvr_UnitySDKPluginEvent.Issue(RenderEventType.Pause);
    }

    #endregion

    /************************************    IEnumerator  *************************************/
    private IEnumerator OnResume()
    {
        for (int i = 0; i < 20; i++)
        {
            yield return null;
        }
        EnterVRMode();
        if (pvr_UnitySDKSensor != null)
        {
            pvr_UnitySDKSensor.StartUnitySDKSensor();
            pvr_UnitySDKSensor.ResetUnitySDKSensor();
        }
    }

}
