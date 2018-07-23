///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_UnitySDKSensor
// Author: AiLi.Shang
// Date:  2017/01/13
// Discription: Be fully careful of  Code modification
///////////////////////////////////////////////////////////////////////////////
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
using UnityEngine;

public class Pvr_UnitySDKRender
{

    public  Pvr_UnitySDKRender()
    {
         if(!canConnecttoActivity)
        {

            ConnectToAndriod();
            Debug.Log("Init Render Ability Success!");
            isInitrenderThread = false;
        }
        Init();
    }
    /************************************    Properties  *************************************/
    #region Properties
#if ANDROID_DEVICE
    public AndroidJavaObject activity;
    public static AndroidJavaClass javaVrActivityClass;
    public static AndroidJavaClass javaSysActivityClass;   
	public static AndroidJavaClass javaVrActivityLongReceiver;
#endif

    private bool canConnecttoActivity = false;
    public bool CanConnecttoActivity
    {
        get { return canConnecttoActivity; }
        set
        {
            if (value != canConnecttoActivity)
                canConnecttoActivity = value;
        }
    }

    private bool isInitrenderThread = true;
    private string model;
    int Headweartype;
    private Vector2 prefinger1 = new Vector2(0.0f, 0.0f);
    private Vector2 prefinger2 = new Vector2(0.0f, 0.0f);
    #endregion

    /************************************   Public Interfaces **********************************/
    #region       PublicInterfaces
    
     public   void Init()
    {
        if (InitRenderAbility()) {
            Debug.Log("Init Render Ability Success!");
            isInitrenderThread = false;
        }
        else
            Debug.LogError("Init Render Ability Failed!");
        if (canConnecttoActivity && !isInitrenderThread)
        {
            Pvr_UnitySDKPluginEvent.Issue(RenderEventType.InitRenderThread);
            isInitrenderThread = true;
            Debug.Log("InitRenderThread on");
        }

    }

    private void AutoAdpatForPico1s()
    {
        Vector2 finger1 = Input.touches[0].position;
        Vector2 finger2 = Input.touches[1].position;
        if (Vector2.Distance(prefinger1, finger1) > 2.0f && Vector2.Distance(prefinger2, finger2) > 2.0f)
        {
            float x = (Input.touches[0].position.x + Input.touches[1].position.x) / Screen.width - 1.0f;
            float y = (Input.touches[0].position.y + Input.touches[1].position.y) / Screen.height - 1.0f;
            Pvr_UnitySDKAPI.Render.UPvr_SetRatio(x, y);
        }
        prefinger1 = finger1;
        prefinger2 = finger2;
    }
    // 获取texture 大小
    public Vector2 GetRendentextureSize()
    {
        Vector2 RendentextureWH;
        int w = 1024;
        int h = 1024;
        try
        {
            int enumindex = (int)Pvr_UnitySDKAPI.GlobalIntConfigs.EYE_TEXTURE_RESOLUTION0;
            Pvr_UnitySDKAPI.Render.UPvr_GetIntConfig(enumindex, ref w);
            enumindex = (int)Pvr_UnitySDKAPI.GlobalIntConfigs.EYE_TEXTURE_RESOLUTION1;
            Pvr_UnitySDKAPI.Render.UPvr_GetIntConfig(enumindex, ref h);
        }
        catch (System.Exception e)
        {
            Debug.LogError("GetRendentextureSize ERROR! " + e.Message);
            throw;
        }
        RendentextureWH = new Vector2(w, h);
        return RendentextureWH;
    }

    // 获取FOV
    public float GetEyeFOV()
    {
        float fov = 102;
        try
        {
            int enumindex = (int)Pvr_UnitySDKAPI.GlobalFloatConfigs.FOV;
            Pvr_UnitySDKAPI.Render.UPvr_GetFloatConfig(enumindex, ref fov);
            if (fov <= 0)
            {
                fov = 102;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("GetEyeFOV ERROR! " + e.Message);
            throw;
        }

        return fov;
    }

    public bool GetUsePredictedMatrix()
    {
        return true;
    }

    /// <summary>
    /// 连接安卓
    /// </summary>
    /// 

    public void ConnectToAndriod()
    {

#if ANDROID_DEVICE
        try
        {      
            Debug.Log("SDK Version :  " + Pvr_UnitySDKAPI.System.UPvr_GetSDKVersion().ToString() + "  Unity Script Version :" +  Pvr_UnitySDKAPI.System.UPvr_GetUnitySDKVersion().ToString());
            UnityEngine.AndroidJavaClass unityPlayer = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity");
            javaVrActivityClass = new UnityEngine.AndroidJavaClass("com.psmart.vrlib.VrActivity");
			javaVrActivityLongReceiver = new UnityEngine.AndroidJavaClass("com.psmart.vrlib.HomeKeyReceiver");
            javaSysActivityClass = new UnityEngine.AndroidJavaClass("com.psmart.aosoperation.SysActivity");
			Pvr_UnitySDKAPI.System.Pvr_SetInitActivity(activity.GetRawObject(), javaVrActivityClass.GetRawClass());
            model = javaVrActivityClass.CallStatic<string>("Pvr_GetBuildModel");
/*
            if (model == "Falcon")
            {                
                Headweartype = (int)Pvr_UnitySDKConfigProfile.DeviceTypes.PicoNeo;
                Debug.Log("Falcon : " + Headweartype.ToString());
                Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaVrActivityClass, "initFalconDevice", activity);
            }
*/
            double[] parameters = new double[5];
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(ref parameters, javaVrActivityClass, "getDPIParameters", activity);
            int platformType = -1 ;
            int enumindex = (int)Pvr_UnitySDKAPI.GlobalIntConfigs.PLATFORM_TYPE;
            Pvr_UnitySDKAPI.Render.UPvr_GetIntConfig(enumindex,ref platformType);
            
            if(platformType == 0)
            {
                 Pvr_UnitySDKAPI.Render.UPvr_ChangeScreenParameters(model, (int)parameters[0], (int)parameters[1], parameters[2], parameters[3], parameters[4]);				 
				Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }
            //Pvr_UnitySDKAPI.Render.UPvr_ChangeHeadwear(Headweartype);
        
          
        
        }
        catch (AndroidJavaException e)
        {
            Debug.LogError("ConnectToAndriod------------------------catch" + e.Message);
        }
#endif
        canConnecttoActivity = true;
    }

    #endregion

    /************************************  Private Interfaces **********************************/
    #region     PrivateInterfaces
    private bool UpdateRenderParaFrame()
    {
        Pvr_UnitySDKManager.SDK.EyeFov = GetEyeFOV();
        return true;
    }

    private bool InitRenderAbility()
    {
        if (UpdateRenderParaFrame())
        {
            if (CreatRendertexture())
            {
                float separation = 0.0625f;
                int enumindex = (int)Pvr_UnitySDKAPI.GlobalFloatConfigs.IPD;
                if (0 != Pvr_UnitySDKAPI.Render.UPvr_GetFloatConfig(enumindex, ref separation))
                {
                    Debug.LogError("Cannot get ipd");
                    separation = 0.0625f;
                }
                Pvr_UnitySDKManager.SDK.leftEyeOffset = new Vector3(-separation / 2, 0, 0);
                Pvr_UnitySDKManager.SDK.rightEyeOffset = new Vector3(separation / 2, 0, 0);
                return true;
            }
        }
        return false;
    }

    private void ConfigureEyeTexture(int eyeTextureIndex, Vector2 Size)
    {

        int x = (int)Size.x;
        int y = (int)Size.y;
        Pvr_UnitySDKManager.SDK.eyeTextures[eyeTextureIndex] = new RenderTexture(x, y, (int)Pvr_UnitySDKManager.SDK.RtBitDepth, Pvr_UnitySDKManager.SDK.RtFormat);
        Pvr_UnitySDKManager.SDK.eyeTextures[eyeTextureIndex].anisoLevel = 0;
        Pvr_UnitySDKManager.SDK.eyeTextures[eyeTextureIndex].antiAliasing = Mathf.Max(QualitySettings.antiAliasing, (int)Pvr_UnitySDKManager.SDK.RtAntiAlising);

        Pvr_UnitySDKManager.SDK.eyeTextures[eyeTextureIndex].Create();
        if (Pvr_UnitySDKManager.SDK.eyeTextures[eyeTextureIndex].IsCreated())
        {
            Pvr_UnitySDKManager.SDK.eyeTextureIds[eyeTextureIndex] = Pvr_UnitySDKManager.SDK.eyeTextures[eyeTextureIndex].GetNativeTexturePtr().ToInt32();
            Debug.Log("eyeTextureIndex : " + eyeTextureIndex.ToString());
        }

    }

    private bool CreatRendertexture()
    {
        Vector2 renderTexSize = GetRendentextureSize();

        // change   renderTexSize  if need   
        // renderTexSize =new Vector2( Pvr_UnitySDKManager.SDK.RtSizeWH, Pvr_UnitySDKManager.SDK.RtSizeWH);

        Debug.Log("renderTexSize " + renderTexSize.x + "    " + renderTexSize.y);

        for (int i = 0; i < Pvr_UnitySDKManager.eyeTextureCount; i++)
        {
            if (null == Pvr_UnitySDKManager.SDK.eyeTextures[i])
            {
                try
                {
                    ConfigureEyeTexture(i, renderTexSize);
                }
                catch (Exception e)
                {
                    Debug.LogError("ConfigureEyeTexture ERROR " + e.Message);
                    throw;
                }
            }
            if (!Pvr_UnitySDKManager.SDK.eyeTextures[i].IsCreated())
            {
                Pvr_UnitySDKManager.SDK.eyeTextures[i].Create();
                Pvr_UnitySDKManager.SDK.eyeTextureIds[i] = Pvr_UnitySDKManager.SDK.eyeTextures[i].GetNativeTexturePtr().ToInt32();
            }
            Pvr_UnitySDKManager.SDK.eyeTextureIds[i] = Pvr_UnitySDKManager.SDK.eyeTextures[i].GetNativeTexturePtr().ToInt32();
        }
        return true;
    }
    #endregion

}
