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

    public Pvr_UnitySDKRender()
    {
        if (!canConnecttoActivity)
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
    public static AndroidJavaClass javaserviceClass;
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
    public void Init()
    {
        if (InitRenderAbility())
        {
            Debug.Log("Init Render Ability Success!");
            isInitrenderThread = false;
        }
        else
            Debug.LogError("Init Render Ability Failed!");
        //if (canConnecttoActivity && !isInitrenderThread)
        //{
        //    Pvr_UnitySDKPluginEvent.Issue(RenderEventType.InitRenderThread);
        //    isInitrenderThread = true;
        //    Debug.Log("InitRenderThread on");
        //}

    }


    public void IssueRenderThread()
    {
        if (canConnecttoActivity && !isInitrenderThread)
        {
            Pvr_UnitySDKPluginEvent.Issue(RenderEventType.InitRenderThread);
            isInitrenderThread = true;
            Debug.Log("IssueRenderThread end");
        }
        else
        {
            Debug.Log("IssueRenderThread  canConnecttoActivity = " + canConnecttoActivity);
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
    
    public Vector2 GetRendentextureSize()
    {
        Vector2 RendentextureWH;
        int w = 1024;
        int h = 1024;
        if (Pvr_UnitySDKManager.SDK.DefaultRenderTexture)
        {
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
        }
        else
        {
            if (Pvr_UnitySDKManager.SDK.RtLevel == Pvr_UnitySDKAPI.RenderTextureLevel.High)
            {
                int enumindex = (int)Pvr_UnitySDKAPI.GlobalIntConfigs.EYE_TEXTURE_RES_HIGH;
                Pvr_UnitySDKAPI.Render.UPvr_GetIntConfig(enumindex, ref w);
            }
            else
            {
                int enumindex = (int)Pvr_UnitySDKAPI.GlobalIntConfigs.EYE_TEXTURE_RES_NORMAL;
                Pvr_UnitySDKAPI.Render.UPvr_GetIntConfig(enumindex, ref w);
            }
            h = w;
            if (w == 0)
            {
                int enumindex = (int)Pvr_UnitySDKAPI.GlobalIntConfigs.EYE_TEXTURE_RESOLUTION0;
                Pvr_UnitySDKAPI.Render.UPvr_GetIntConfig(enumindex, ref w);
                enumindex = (int)Pvr_UnitySDKAPI.GlobalIntConfigs.EYE_TEXTURE_RESOLUTION1;
                Pvr_UnitySDKAPI.Render.UPvr_GetIntConfig(enumindex, ref h);
            }
        }
        RendentextureWH = new Vector2(w, h);
        return RendentextureWH;
    }

    
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

    
    public void ConnectToAndriod()
    {

#if ANDROID_DEVICE
        try
        {      
            Debug.Log("SDK Version :  " + Pvr_UnitySDKAPI.System.UPvr_GetSDKVersion().ToString() + "  Unity Script Version :" +  Pvr_UnitySDKAPI.System.UPvr_GetUnitySDKVersion().ToString());
            UnityEngine.AndroidJavaClass unityPlayer = new UnityEngine.AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<UnityEngine.AndroidJavaObject>("currentActivity");
            javaVrActivityClass = new UnityEngine.AndroidJavaClass("com.psmart.vrlib.VrActivity");
            javaserviceClass = new AndroidJavaClass("com.picovr.picovrlib.hummingbirdclient.UnityClient");
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

            string systemfps = "";
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref systemfps, javaserviceClass, "getSysproc", "persist.pvr.debug.appfps");
            if(systemfps != "")
                Pvr_UnitySDKManager.SDK.systemFPS = Convert.ToBoolean(Convert.ToInt16(systemfps));
        
            if (platformType == 0)
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

    private void ConfigureOverlayTexture(int index, Vector2 Size)
    {

        int x = (int)Size.x;
        int y = (int)Size.y;
        Pvr_UnitySDKManager.SDK.overlayTextures[index] = new RenderTexture(x, y, (int)Pvr_UnitySDKManager.SDK.RtBitDepth, Pvr_UnitySDKManager.SDK.RtFormat);
        Pvr_UnitySDKManager.SDK.overlayTextures[index].anisoLevel = 0;
        Pvr_UnitySDKManager.SDK.overlayTextures[index].antiAliasing = Mathf.Max(QualitySettings.antiAliasing, (int)Pvr_UnitySDKManager.SDK.RtAntiAlising);

        Pvr_UnitySDKManager.SDK.overlayTextures[index].Create();
        if (Pvr_UnitySDKManager.SDK.overlayTextures[index].IsCreated())
        {
            Pvr_UnitySDKManager.SDK.overlayTextureIds[index] = Pvr_UnitySDKManager.SDK.overlayTextures[index].GetNativeTexturePtr().ToInt32();
            Debug.LogFormat("overlayTextureIndex : {0:D}, id : {0:D}", index, Pvr_UnitySDKManager.SDK.overlayTextureIds[index]);
        }

    }


    private bool CreatRendertexture()
    {
        if (!Pvr_UnitySDKManager.SDK.IsViewerLogicFlow)
        {
            Vector2 renderTexSize = GetRendentextureSize();
            Pvr_UnitySDKManager.SDK.eyeTextures = new RenderTexture[Pvr_UnitySDKManager.eyeTextureCount];
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

            Pvr_UnitySDKManager.SDK.overlayTextures = new RenderTexture[Pvr_UnitySDKManager.eyeTextureCount];
            int overlayNum = Pvr_UnitySDKManager.SDK.overlayCamNum * (Pvr_UnitySDKManager.eyeTextureCount / 2);
            for (int i = 0; i < overlayNum; i++)
            {
                if (null == Pvr_UnitySDKManager.SDK.overlayTextures[i])
                {
                    try
                    {
                        ConfigureOverlayTexture(i, renderTexSize);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("ConfigureOverlayTexture ERROR " + e.Message);
                        throw;
                    }
                }
                if (!Pvr_UnitySDKManager.SDK.overlayTextures[i].IsCreated())
                {
                    Pvr_UnitySDKManager.SDK.overlayTextures[i].Create();
                    Pvr_UnitySDKManager.SDK.overlayTextureIds[i] = Pvr_UnitySDKManager.SDK.overlayTextures[i].GetNativeTexturePtr().ToInt32();
                }
                Pvr_UnitySDKManager.SDK.overlayTextureIds[i] = Pvr_UnitySDKManager.SDK.overlayTextures[i].GetNativeTexturePtr().ToInt32();
            }
        }
        return true;
    }
    #endregion

}
