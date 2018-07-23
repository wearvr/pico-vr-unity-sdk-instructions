///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Pvr_Controller
// Author: Yangel.Yan
// Date:  2017/01/11
// Discription: The demo of using controller
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

using UnityEngine;
using UnityEngine.UI;
using Pvr_UnitySDKAPI;
using System;

public class Pvr_ControllerLink
{
    public string gameobjname = "";
    public bool notPhone = false;
    //#if UNITY_ANDROID
    public AndroidJavaClass javaHummingbirdClass;
    public AndroidJavaClass javaPico2ReceiverClass;
    public AndroidJavaClass javavractivityclass;
    public AndroidJavaObject activity;
    public string hummingBirdMac;
    public int hummingBirdRSSI;
    public string lark2key;
    public bool isConnect= false;
    public int controllerState = 100;
    //#endif
    public Pvr_ControllerLink(string name)
    {
        gameobjname = name;
        hummingBirdMac = "";
        hummingBirdRSSI = 0;
        Debug.Log(gameobjname);
        StartHummingBirdService();
    }
    private void StartHummingBirdService()
    {
#if ANDROID_DEVICE
        try
        {              
            UnityEngine.AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            javaHummingbirdClass = new AndroidJavaClass("com.picovr.picovrlib.hummingbirdclient.HbClientActivity");
            javavractivityclass = new UnityEngine.AndroidJavaClass("com.psmart.vrlib.VrActivity");
            javaPico2ReceiverClass = new UnityEngine.AndroidJavaClass("com.picovr.picovrlib.hummingbirdclient.HbClientReceiver");
            Pvr_UnitySDKAPI.System.Pvr_SetInitActivity(activity.GetRawObject(), javaHummingbirdClass.GetRawClass());
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaPico2ReceiverClass, "startReceiver", activity, gameobjname);
            int platformType = -1;
            int enumindex = (int)GlobalIntConfigs.PLATFORM_TYPE;
            Render.UPvr_GetIntConfig(enumindex, ref platformType);
            if(platformType == 2 || platformType == 1)
            {
                notPhone = true;
            }
            else
            {
                notPhone = false;
            }

            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "setPlatformType", platformType);
            if (isHbServiceExisted())
            {
                Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "bindHbService", activity);
            }
            
        }
        catch (AndroidJavaException e)
        {
            Debug.LogError("ConnectToAndriod------------------------catch" + e.Message);
        }
#endif
    }
    public bool isHbServiceExisted()
    {
        bool isService = false;
#if ANDROID_DEVICE           
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<bool>(ref isService, javaHummingbirdClass, "isHbServiceExisted", activity);
#endif
        return isService;
    }
    public void StopLark2Receiver()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaPico2ReceiverClass, "stopReceiver",activity);
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaPico2ReceiverClass, "stopOnBootReceiver",activity);
#endif
    }
    public void StartLark2Receiver()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaPico2ReceiverClass, "startReceiver",activity, gameobjname);
#endif
    }
    public void StopLark2Service()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaPico2ReceiverClass, "stopReceiver", activity); 
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "unbindHbService", activity);
#endif
    }
    public void StartLark2Service()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaPico2ReceiverClass, "startReceiver",activity, gameobjname);
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "bindHbService", activity);
#endif
    }
    public void BindHBService()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "bindHbService", activity);
#endif
    }
    public int getHandness()
    {
        int handness = -1;
#if ANDROID_DEVICE           
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref handness, javavractivityclass, "getPvrHandness", activity);
#endif
        return handness;
    }
    public void StartScan()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "scanHbDevice", true);
#endif
    }
    public void StopScan()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "scanHbDevice", false);
#endif
    }
    public void ResetController()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "resetHbSensorState");
#elif IOS_DEVICE
        Controller.Pvr_ResetSensor(3);

#endif
    }

    public void ConnectBLE()
    {
        if (hummingBirdMac != "")
        {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "connectHbController", hummingBirdMac);
#endif
        }

    }

    public void DisConnectBLE()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "disconnectHbController");
#endif
    }
    public bool StartUpgrade()
    {
        bool start = false;
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<bool>(ref start, javaHummingbirdClass, "startUpgrade");
#endif
        return start;
    }
    public void setBinPath(string path, bool isasset)
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod( javaHummingbirdClass, "setBinPath",path,isasset);
#endif
    }

    public string GetBLEImageType()
    {
        string type = "";
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref type, javaHummingbirdClass, "getBLEImageType");
#endif
        return type;
    }

    public long GetBLEVersion()
    {
        long version = 0L;
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<long>(ref version, javaHummingbirdClass, "getBLEVersion");
#endif
        return version;
    }

    public string GetFileImageType()
    {
        string type = "";
#if ANDROID_DEVICE
      Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref type, javaHummingbirdClass, "getFileImageType");
#endif
        return type;
    }

    public long GetFileVersion()
    {
        long version = 0L;
#if ANDROID_DEVICE
      Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<long>(ref version, javaHummingbirdClass, "getFileVersion");
#endif
        return version;
    }

    public int GetHBConnectionState()
    {
        //0 未连接 1连接中 2连接成功
        int state = -1;
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref state, javaHummingbirdClass, "getHbConnectionState");
#endif
        return state;
    }

    public void RebackToLauncher()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "startLauncher");
#endif
    }

    public void TurnUpVolume()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "turnUpVolume",activity);
#endif
    }

    public void TurnDownVolume()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "turnDownVolume",activity);
#endif
    }
    public string GetHBSensorState()
    {
        string status = "";

#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref status,javaHummingbirdClass, "getHBSensorState");
#endif
        return status;
    }
    /// <summary>
    /// 自动连接HB手柄
    /// </summary>
    /// <param name="scanTimeMs">扫描时间，单位毫秒</param>
    public void AutoConnectHbController(int scanTimeMs)
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "autoConnectHbController",scanTimeMs,gameobjname);
#endif
    }

    // 获取陀螺仪数据
    public Vector3 GetAngularVelocity()
    {
        float[] Angulae = new float[3] { 0, 0, 0 };
        try
        {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(ref Angulae,javaHummingbirdClass, "getHbAngularVelocity");
#endif
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        Vector3 Aglr = new Vector3(Angulae[0], Angulae[1], Angulae[2]);
        return Aglr;
    }


    // 获取加速度
    public Vector3 GetAcceleration()
    {
        float[] Accel = new float[3] { 0, 0, 0 };
#if ANDROID_DEVICE
     Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(ref Accel,javaHummingbirdClass, "getHbAcceleration");
#endif
        Vector3 Accele = new Vector3(Accel[0], Accel[1], Accel[2]);
        return Accele;
    }

    public string GetConnectedDeviceMac()
    {
        string mac = "";
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref mac, javaHummingbirdClass, "getConnectedDeviceMac");
#endif
        return mac;
    }
    
}
