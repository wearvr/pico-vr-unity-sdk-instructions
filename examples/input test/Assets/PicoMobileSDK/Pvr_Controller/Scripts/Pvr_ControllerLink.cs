///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Pvr_Controller
// Author: Yangel.Yan
// Date:  2017/01/11
// Discription: The manager of using controller
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
using System.Runtime.Remoting.Messaging;

public class Pvr_ControllerLink
{

#if ANDROID_DEVICE
    public AndroidJavaClass javaHummingbirdClass;
    public AndroidJavaClass javaPico2ReceiverClass;
    public AndroidJavaClass javaserviceClass;
    public AndroidJavaClass javavractivityclass;
    public AndroidJavaClass javaCVClass;
    public AndroidJavaObject activity;
#endif
    public string gameobjname = "";
    public bool picoDevice = false;
    public string hummingBirdMac;
    public int hummingBirdRSSI;
    public string lark2key;
    public bool goblinserviceStarted = false;
    public bool neoserviceStarted = false;
    public bool controller0Connected = false;
    public bool controller1Connected = false;
    public ControllerHand Controller0;
    public ControllerHand Controller1;
    public int platFormType = -1; //0 phone，1 Pico Neo DK，2 Pico Goblin 3 Pico Neo
    public int trackingmode = -1; //ability 0:null,1:3dof,2:6dof 3:自定义
    public int systemProp = -1;   //0：goblin1 1：goblin1 2:neo 3:goblin2
    public int enablehand6dofbyhead = -1;
    public bool switchHomeKey = true;
    public Pvr_ControllerLink(string name)
    {
        gameobjname = name;
        hummingBirdMac = "";
        hummingBirdRSSI = 0;
        Debug.Log(gameobjname);
        StartHummingBirdService();
        Controller0 = new ControllerHand();
        Controller1 = new ControllerHand();
    }

    private void StartHummingBirdService()
    {
#if ANDROID_DEVICE
        try
        {
            UnityEngine.AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            javaHummingbirdClass = new AndroidJavaClass("com.picovr.picovrlib.hummingbirdclient.HbClientActivity");
            javaCVClass = new AndroidJavaClass("com.picovr.picovrlib.cvcontrollerclient.ControllerClient");
            javavractivityclass = new UnityEngine.AndroidJavaClass("com.psmart.vrlib.VrActivity");
            javaserviceClass = new AndroidJavaClass("com.picovr.picovrlib.hummingbirdclient.UnityClient");
            Pvr_UnitySDKAPI.System.Pvr_SetInitActivity(activity.GetRawObject(), javaHummingbirdClass.GetRawClass());
            int enumindex = (int)GlobalIntConfigs.PLATFORM_TYPE;
            Render.UPvr_GetIntConfig(enumindex, ref platFormType);
            PLOG.I("PvrLog platform" + platFormType);
            enumindex = (int)GlobalIntConfigs.TRACKING_MODE;
            Render.UPvr_GetIntConfig(enumindex, ref trackingmode);
            PLOG.I("PvrLog trackingmode" + trackingmode);
            systemProp = GetSysproc();
            PLOG.I("PvrLog systemProp" + systemProp);
            enumindex = (int) GlobalIntConfigs.ENBLE_HAND6DOF_BY_HEAD;
            Render.UPvr_GetIntConfig(enumindex, ref enablehand6dofbyhead);
            PLOG.I("PvrLog enablehand6dofbyhead" + enablehand6dofbyhead);
            if (trackingmode == 0 || trackingmode == 1 || (trackingmode == 3 && systemProp == 1) || (trackingmode == 3 && systemProp == 3))
            {
                picoDevice = platFormType != 0;
                javaPico2ReceiverClass = new UnityEngine.AndroidJavaClass("com.picovr.picovrlib.hummingbirdclient.HbClientReceiver");
                Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaPico2ReceiverClass, "startReceiver", activity, gameobjname);
                Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "setPlatformType", platFormType);
            }
            else
            {
                picoDevice = true;
                SetGameObjectToJar(gameobjname);
            }
            if (IsServiceExisted())
            {
                BindService();
            }
        }
        catch (AndroidJavaException e)
        {
            Debug.LogError("ConnectToAndriod------------------------catch" + e.Message);
        }
#endif
    }

    public bool IsServiceExisted()
    {

        bool service = false;
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<bool>(ref service, javaserviceClass, "isServiceExisted", activity,trackingmode);
#endif
        PLOG.I("PvrLog ServiceExisted ?" + service);
        return service;

    }
    public void SetGameObjectToJar(string name)
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "setGameObjectCallback", name);
#endif
    }

    public void BindService()
    {
        PLOG.I("PvrLog Start Bind");
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaserviceClass, "bindService", activity,trackingmode);
#endif
    }
    public void UnBindService()
    {
        PLOG.I("PvrLog Start UnBind");
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaserviceClass, "unbindService", activity,trackingmode);
#endif
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
    public int getHandness()
    {
        int handness = -1;
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref handness, javavractivityclass, "getPvrHandness", activity);
#endif
        PLOG.I("PvrLog HandNess =" + handness);
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

    public int GetSysproc()
    {
        int prop = -1;
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref prop, javaserviceClass, "getSysproc");
#endif
        return prop;
    }

    public void ResetController(int num)
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "resetControllerSensorState",num);
        }
        if(goblinserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "resetHbSensorState");
		}
#elif IOS_DEVICE
        Controller.Pvr_ResetSensor(3);

#endif
        PLOG.I("PvrLog ResetController" + num);
    }

    public void ConnectBLE()
    {
        if (hummingBirdMac != "")
        {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "connectHbController", hummingBirdMac);
#endif
        }
        PLOG.I("PvrLog ConnectHBController" + hummingBirdMac);
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
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "setBinPath",path,isasset);
#endif
    }

    public string GetBLEImageType()
    {
        string type = "";
#if ANDROID_DEVICE
        if (goblinserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref type, javaHummingbirdClass, "getBLEImageType");
        }
#endif
        return type;
    }

    public long GetBLEVersion()
    {
        long version = 0L;
#if ANDROID_DEVICE
        if (goblinserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<long>(ref version, javaHummingbirdClass, "getBLEVersion");
        }
#endif
        return version;
    }

    public string GetFileImageType()
    {
        string type = "";
#if ANDROID_DEVICE
        if (goblinserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref type, javaHummingbirdClass, "getFileImageType");
        }
#endif
        return type;
    }

    public long GetFileVersion()
    {
        long version = 0L;
#if ANDROID_DEVICE
        if (goblinserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<long>(ref version, javaHummingbirdClass, "getFileVersion");
        }
#endif
        return version;
    }

    public int GetControllerConnectionState(int num)
    {
        int state = -1;
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref state, javaCVClass, "getControllerConnectionState",num);
        }
        if (goblinserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref state, javaHummingbirdClass, "getHbConnectionState");
        }
#endif
        PLOG.D("PvrLog GetControllerState:" + num + "state:" + state);
        return state;
    }

    public void RebackToLauncher()
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "startLauncher");
        }
        if (goblinserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "startLauncher");
        }
#endif
    }

    public void TurnUpVolume()
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "turnUpVolume", activity);
        }
        if (goblinserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "turnUpVolume", activity);
        }
#endif
        PLOG.I("PvrLog TurnUpVolume");
    }

    public void TurnDownVolume()
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "turnDownVolume", activity);
        }
        if (goblinserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "turnDownVolume", activity);
        }
#endif
        PLOG.I("PvrLog TurnDownVolume");
    }
    
    public string GetHBControllerPoseData()
    {
        string data = "";
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref data, javaHummingbirdClass, "getHBSensorState");
#endif
        PLOG.D("PvrLog HBControllerData" + data);
        return data;
    }

    public float[] GetCvControllerPoseData(int hand)
    {
        var data = new float[7] { 0, 0, 0, 0, 0, 0, 0 };
#if ANDROID_DEVICE
        if (enablehand6dofbyhead == 1)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(ref data, javaCVClass, "getControllerSensorState", hand,Pvr_UnitySDKManager.SDK.headData);
        }
        else
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(ref data, javaCVClass, "getControllerSensorState", hand);
        }

#endif
        Quaternion pose = new Quaternion(data[0], data[1], data[2], data[3]);
        Vector3 pos = new Vector3(data[4], data[5], data[6]);
        PLOG.D("PvrLog CVControllerData " + hand + "Rotation:" + data[0] + data[1] + data[2] + data[3] + "Position:" +
               data[4] + data[5] + data[6] + "eulerAngles:" + pose.eulerAngles);

        if (float.IsNaN(pose.x) || float.IsNaN(pose.y) || float.IsNaN(pose.z) || float.IsNaN(pose.w))
        {
            pose = Quaternion.identity;
        } 
        if (float.IsNaN(pos.x) || float.IsNaN(pos.y) || float.IsNaN(pos.z))
        {
            pos = Vector3.zero;
        }
        return new float[7] { pose.x, pose.y, pose.z, pose.w, pos.x, pos.y, pos.z };
    }
   
    public string GetHBControllerKeyData()
    {
        string data = "";
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref data, javaHummingbirdClass, "getHBKeyEvent");
#endif
        PLOG.D("PvrLog ControllerKey" + data);
        return data;
    }

    public int[] GetCvControllerKeyData(int hand)
    {
        var data = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(ref data, javaCVClass, "getControllerKeyEvent", hand);
#endif
        PLOG.D("PvrLog ControllerKey" + data[0] + data[1] + data[2] + data[3] + data[4] + data[5] + data[6] + data[7] +
               data[8]);
        return data;
    }
   
    public void AutoConnectHbController(int scanTimeMs)
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "autoConnectHbController",scanTimeMs,gameobjname);
#endif
    }

    public void StartControllerThread(int headSensorState, int handSensorState)
    {

#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "startControllerThread",headSensorState,handSensorState);
#endif
        PLOG.I("PvrLog StartControllerThread" + headSensorState + handSensorState);
    }
    public void StopControllerThread(int headSensorState, int handSensorState)
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "stopControllerThread",headSensorState,handSensorState);
#endif
        PLOG.I("PvrLog StopControllerThread" + headSensorState + handSensorState);
    }

    
    public Vector3 GetAngularVelocity(int num)
    {
        var angulae = new float[3] { 0, 0, 0 };
        try
        {
#if ANDROID_DEVICE

            if (neoserviceStarted)
            {
                Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(ref angulae, javaCVClass, "getControllerAngularVelocity", num);
            }
            if (goblinserviceStarted)
            {
                Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(ref angulae, javaHummingbirdClass, "getHbAngularVelocity");
            }
#endif
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
        PLOG.D("PvrLog Gyro:" + angulae[0] + angulae[1] + angulae[2]);
        if (!float.IsNaN(angulae[0]) && !float.IsNaN(angulae[1]) && !float.IsNaN(angulae[2]))
        {
            return new Vector3(angulae[0], angulae[1], angulae[2]);
        }
        return new Vector3(0, 0, 0);
    }

    
    public Vector3 GetAcceleration(int num)
    {
        var accel = new float[3] { 0, 0, 0 };
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(ref accel, javaCVClass, "getControllerAcceleration", num);
        }
        if(goblinserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(ref accel, javaHummingbirdClass, "getHbAcceleration");
        }

#endif
        PLOG.D("PvrLog Acce:" + accel[0] + accel[1] + accel[2]);
        if (!float.IsNaN(accel[0]) && !float.IsNaN(accel[1]) && !float.IsNaN(accel[2]))
        {
            return new Vector3(accel[0], accel[1], accel[2]);
        }
        return new Vector3(0, 0, 0);
    }

    public string GetConnectedDeviceMac()
    {
        string mac = "";
#if ANDROID_DEVICE
        if (goblinserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref mac, javaHummingbirdClass, "getConnectedDeviceMac");
        }
#endif
        PLOG.I("PvrLog ConnectedDeviceMac:" + mac);
        return mac;
    }
  
    public void VibateController(int hand, int strength)
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "vibrateControllerStrength", hand, strength);
        }
#endif
    }

    public int GetMainControllerIndex()
    {
        int index = 0;
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref index, javaCVClass, "getMainControllerIndex");
        }
#endif
        PLOG.I("PvrLog MainControllerIndex:" + index);
        return index;
    }

    public void SetMainController(int index)
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "setMainController",index); 
        }
#endif
    }
    public void ResetHeadSensorForController()
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "resetHeadSensorForController");
        }
#endif
    }

    public void GetDeviceVersion(int deviceType)
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "getDeviceVersion",deviceType); 
        }
#endif
    }
 
    public void GetControllerSnCode(int controllerSerialNum)
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "getControllerSnCode",controllerSerialNum); 
        }
#endif
    }
 
    public void SetControllerUnbind(int controllerSerialNum)
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "setControllerUnbind",controllerSerialNum); 
        }
#endif
    }

    public void SetStationRestart()
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "setStationRestart"); 
        }
#endif
    }

    public void StartStationOtaUpdate()
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "startStationOtaUpdate"); 
        }
#endif
    }
  
    public void StartControllerOtaUpdate(int mode, int controllerSerialNum)
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "startControllerOtaUpdate",mode,controllerSerialNum); 
        }
#endif
    }
    
    public void EnterPairMode(int controllerSerialNum)
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "enterPairMode",controllerSerialNum); 
        }
#endif
    }
   
    public void SetControllerShutdown(int controllerSerialNum)
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "setControllerShutdown",controllerSerialNum); 
        }
#endif
    }
    
    public int GetStationPairState()
    {
        int index = -1;
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref index,javaCVClass, "getStationPairState"); 
        }
#endif
        PLOG.I("PvrLog StationPairState" + index);
        return index;
    }
   
    public int GetStationOtaUpdateProgress()
    {
        int index = -1;
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref index,javaCVClass, "getStationOtaUpdateProgress"); 
        }
#endif
        PLOG.I("PvrLog StationOtaUpdateProgress" + index);
        return index;
    }
    
    public int GetControllerOtaUpdateProgress()
    {
        int index = -1;
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref index,javaCVClass, "getControllerOtaUpdateProgress"); 
        }
#endif
        PLOG.I("PvrLog ControllerOtaUpdateProgress" + index);
        return index;
    }

    public void GetControllerVersionAndSN(int controllerSerialNum)
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "getControllerVersionAndSN",controllerSerialNum); 
        }
#endif
    }
    
    public void GetControllerUniqueID()
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "getControllerUniqueID"); 
        }
#endif
    }
    
    public void InterruptStationPairMode()
    {
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaCVClass, "interruptStationPairMode"); 
        }
#endif
    }

    public int GetControllerAbility(int controllerSerialNum)
    {
        int index = -1;
#if ANDROID_DEVICE
        if (neoserviceStarted)
        {
           Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref index,javaCVClass, "getControllerAbility",controllerSerialNum);
        }
#endif
        PLOG.I("PvrLog ControllerAbility:" + index);
        return index;
    }

    public void SwitchHomeKey(bool state)
    {
        switchHomeKey = state;
    }

    public void SetBootReconnect()
    {
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(javaHummingbirdClass, "setBootReconnect");
#endif
    }

    public int GetTriggerKeyEvent()
    {
        int key = -1;
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref key,javaHummingbirdClass, "getTriggerKeyEvent");
#endif
        PLOG.D("PvrLog GoblinControllerTriggerKey:" + key);
        return key;
    }
    //Acquisition of equipment temperature
    public int GetTemperature()
    {
        int value = -1;
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref value,javaHummingbirdClass, "getTemperature");
#endif
        PLOG.I("PvrLog Temperature:" + value);
        return value;
    }
    //Get the device type
    public int GetDeviceType()
    {
        int type = -1;
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>(ref type,javaHummingbirdClass, "getDeviceType");
#endif
        PLOG.I("PvrLog DeviceType:" + type);
        return type;
    }
    public string GetHummingBird2SN()
    {
        string type = "";
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref type,javaHummingbirdClass, "getHummingBird2SN");
#endif
        PLOG.I("PvrLog HummingBird2SN:" + type);
        return type;
    }

    public string GetControllerVersion()
    {
        string type = "";
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref type,javaHummingbirdClass, "getControllerVersion");
#endif
        PLOG.I("PvrLog ControllerVersion:" + type);
        return type;
    }

    public bool IsEnbleTrigger()
    {
        bool state = false;
#if ANDROID_DEVICE
        Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<bool>(ref state,javaHummingbirdClass, "isEnbleTrigger");
#endif
        PLOG.I("PvrLog IsEnbleTrigger:" + state);
        return state;
    }

}
