///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Pvr_ControllerManager
// Author: Yangel.Yan
// Date:  2017/01/11
// Discription: Be Sure Your controller demo has this script
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
using System;
using System.Collections;
using LitJson;
using Pvr_UnitySDKAPI;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Pvr_ControllerManager : MonoBehaviour
{

    /************************************    Properties  *************************************/
    private static Pvr_ControllerManager instance = null;

    public static Pvr_ControllerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = UnityEngine.Object.FindObjectOfType<Pvr_ControllerManager>();
            }
            if (instance == null)
            {
                var go = new GameObject("GameObject");
                instance = go.AddComponent<Pvr_ControllerManager>();
                go.transform.localPosition = Vector3.zero;
            }
            return instance;
        }
    }
    #region Properties

    public static bool longPressclock;
    public static Pvr_ControllerLink controllerlink;
    private float cTime = 1.0f;
    private float longpresstime = 0.5f; 
    private bool stopConnect; 
    private SystemLanguage localanguage;
    public Text toast;
    private bool controllerServicestate;
    private float disConnectTime;
    private int triggernum;
    private DateTime beginDT;
    private DateTime endDT;
    #endregion

    //Service Start Success
    //The service startup is successful and the API interface can be used normally.
    public delegate void PvrServiceStartSuccess();
    public static event PvrServiceStartSuccess PvrServiceStartSuccessEvent;
    //Controller State event
    //1.Goblin controller，"int a"，0：Disconnect 1：Connect
    //2.Neo controller，"int a,int b"，a(0:controller0,1：controller1)，b(0:Disconnect，1：Connect)  
    public delegate void PvrControllerStateChanged(string data);
    public static event PvrControllerStateChanged PvrControllerStateChangedEvent;
    //Master control hand change
    public delegate void ChangeMainControllerCallBack(string index);
    public static event ChangeMainControllerCallBack ChangeMainControllerCallBackEvent;


    //The following is the separation of platform events, suggesting the use of the above events.
    //goblin service bind success
    public delegate void SetHbServiceBindState();
    public static event SetHbServiceBindState SetHbServiceBindStateEvent;
    //neo ControllerThread start-up success
    public delegate void ControllerThreadStartedCallback();
    public static event ControllerThreadStartedCallback ControllerThreadStartedCallbackEvent;
    //neo service Bind success
    public delegate void SetControllerServiceBindState();
    public static event SetControllerServiceBindState SetControllerServiceBindStateEvent;
    //goblin Controller connection status change
    public delegate void ControllerStatusChange(string isconnect);
    public static event ControllerStatusChange ControllerStatusChangeEvent;
    //neo Controller connection status change
    public delegate void SetControllerAbility(string data);
    public static event SetControllerAbility SetControllerAbilityEvent;
    //neo Controller connection status change
    public delegate void SetControllerStateChanged(string data);
    public static event SetControllerStateChanged SetControllerStateChangedEvent;
    //goblin Mac
    public delegate void SetHbControllerMac(string mac);
    public static event SetHbControllerMac SetHbControllerMacEvent;
    //Get the version
    public delegate void ControllerDeviceVersionCallback(string data);
    public static event ControllerDeviceVersionCallback ControllerDeviceVersionCallbackEvent;
    //Acquisition controller SN
    public delegate void ControllerSnCodeCallback(string data);
    public static event ControllerSnCodeCallback ControllerSnCodeCallbackEvent;
    //controller unbundling
    public delegate void ControllerUnbindCallback(string status);
    public static event ControllerUnbindCallback ControllerUnbindCallbackEvent;
    //Station working status.
    public delegate void ControllerStationStatusCallback(string status);
    public static event ControllerStationStatusCallback ControllerStationStatusCallbackEvent;
    //Station is busy.
    public delegate void ControllerStationBusyCallback(string status);
    public static event ControllerStationBusyCallback ControllerStationBusyCallbackEvent;
    //OTA upgrade error
    public delegate void ControllerOtaStartCodeCallback(string data);
    public static event ControllerOtaStartCodeCallback ControllerOtaStartCodeCallbackEvent;
    //controller version and SN 
    public delegate void ControllerDeviceVersionAndSNCallback(string data);
    public static event ControllerDeviceVersionAndSNCallback ControllerDeviceVersionAndSNCallbackEvent;
    //The controller's unique identification code
    public delegate void ControllerUniqueIDCallback(string data);
    public static event ControllerUniqueIDCallback ControllerUniqueIDCallbackEvent;
    //The combined to controller
    public delegate void ControllerCombinedKeyUnbindCallback(string data);
    public static event ControllerCombinedKeyUnbindCallback ControllerCombinedKeyUnbindCallbackEvent;
    /*************************************  Unity API ****************************************/
    #region Unity API
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        if (instance != this)
        {
            Debug.LogError("instance object should be a singleton.");
            return;
        }
        if (controllerlink == null)
        {
            controllerlink = new Pvr_ControllerLink(this.gameObject.name);
        }
        else
        {
            BindService();
        }
    }
    // Use this for initialization
    void Start()
    {
        localanguage = Application.systemLanguage;

        if (controllerlink.trackingmode != 2 && controllerlink.trackingmode != 3)
        {
            Invoke("CheckControllerService", 10.0f);
        }
    }
    private void OnDestroy()
    {
        controllerlink.UnBindService();
    }
    // Update is called once per frame
    void Update()
    {
#if UNITY_ANDROID
        if (controllerlink.neoserviceStarted)
        {
            if (controllerlink.controller0Connected)
            {
                var pose0 = controllerlink.GetCvControllerPoseData(0);
                controllerlink.Controller0.Rotation = new Quaternion(pose0[0], pose0[1], pose0[2], pose0[3]);
                controllerlink.Controller0.Position = new Vector3(pose0[4] / 1000.0f, pose0[5] / 1000.0f, -pose0[6] / 1000.0f);
                
                var key0 = controllerlink.GetCvControllerKeyData(0);
                controllerlink.Controller0.TouchPadPosition = new Vector2(key0[0], key0[1]);
                
                SetSwipeData(controllerlink.Controller0);
                
                SetTouchPadClick(controllerlink.Controller0);
               
                TransformData(controllerlink.Controller0.HomeKey, key0[2]);
                
                TransformData(controllerlink.Controller0.AppKey, key0[3]);
                
                TransformData(controllerlink.Controller0.TouchKey, key0[4]);
              
                TransformData(controllerlink.Controller0.VolumeUpKey, key0[5]);
              
                TransformData(controllerlink.Controller0.VolumeDownKey, key0[6]);
               
                controllerlink.Controller0.TriggerNum = key0[7];
                
                SetTriggerClick(controllerlink.Controller0.TriggerKey, key0[7]);
                
                controllerlink.Controller0.Battery = key0[8];
            }
            if (controllerlink.controller1Connected)
            {
                var pose1 = controllerlink.GetCvControllerPoseData(1);
                controllerlink.Controller1.Rotation = new Quaternion(pose1[0], pose1[1], pose1[2], pose1[3]);
                controllerlink.Controller1.Position = new Vector3(pose1[4] / 1000.0f, pose1[5] / 1000.0f, -pose1[6] / 1000.0f);
                
                var key1 = controllerlink.GetCvControllerKeyData(1);
                controllerlink.Controller1.TouchPadPosition = new Vector2(key1[0], key1[1]);
                
                SetSwipeData(controllerlink.Controller1);
             
                SetTouchPadClick(controllerlink.Controller1);
             
                TransformData(controllerlink.Controller1.HomeKey, key1[2]);
              
                TransformData(controllerlink.Controller1.AppKey, key1[3]);
              
                TransformData(controllerlink.Controller1.TouchKey, key1[4]);
                
                TransformData(controllerlink.Controller1.VolumeUpKey, key1[5]);
                
                TransformData(controllerlink.Controller1.VolumeDownKey, key1[6]);
                
                controllerlink.Controller1.TriggerNum = key1[7];
                
                SetTriggerClick(controllerlink.Controller1.TriggerKey, key1[7]);
               
                controllerlink.Controller1.Battery = key1[8];
            }
            
        }
        //Goblin controller
        if (controllerlink.goblinserviceStarted && controllerlink.controller0Connected)
        {
            var pose0 = controllerlink.GetHBControllerPoseData();
            var jpose = JsonMapper.ToObject(pose0);
            controllerlink.Controller0.Rotation = new Quaternion(Convert.ToSingle(jpose[1].ToString()), Convert.ToSingle(jpose[2].ToString()), Convert.ToSingle(jpose[3].ToString()), Convert.ToSingle(jpose[0].ToString()));
            PLOG.D("PvrLog GoblinController X:" + controllerlink.Controller0.Rotation.eulerAngles.x + " Y:" +
                      controllerlink.Controller0.Rotation.eulerAngles.y + " Z:" +
                      controllerlink.Controller0.Rotation.eulerAngles.z);
           
            var key0 = controllerlink.GetHBControllerKeyData();
            var jkey = JsonMapper.ToObject(key0);
            controllerlink.Controller0.TouchPadPosition = new Vector2(Convert.ToInt16(jkey[0].ToString()), Convert.ToInt16(jkey[1].ToString()));
            
            SetSwipeData(controllerlink.Controller0);
            
            SetTouchPadClick(controllerlink.Controller0);
            
            TransformData(controllerlink.Controller0.HomeKey, Convert.ToInt16(jkey[2].ToString()));
            
            TransformData(controllerlink.Controller0.AppKey, Convert.ToInt16(jkey[3].ToString()));
            
            TransformData(controllerlink.Controller0.TouchKey, Convert.ToInt16(jkey[4].ToString()));
           
            TransformData(controllerlink.Controller0.VolumeUpKey, Convert.ToInt16(jkey[5].ToString()));
            
            TransformData(controllerlink.Controller0.VolumeDownKey, Convert.ToInt16(jkey[6].ToString()));
            
            controllerlink.Controller0.Battery = Convert.ToInt16(jkey[7].ToString());
            
            TransformData(controllerlink.Controller0.TriggerKey, controllerlink.GetTriggerKeyEvent());
            
        }
        
        SetSystemKey();
#endif
    }
    void OnApplicationQuit()
    {
        var headdof = Pvr_UnitySDKManager.SDK.HeadDofNum == HeadDofNum.SixDof ? 1 : 0;
        var handdof = Pvr_UnitySDKManager.SDK.HandDofNum == HandDofNum.SixDof ? 1 : 0;
      
        if (controllerlink.neoserviceStarted)
        {
            controllerlink.StopControllerThread(headdof, handdof);
        }
            
    }
    private void OnApplicationPause(bool pause)
    {
        var headdof = Pvr_UnitySDKManager.SDK.HeadDofNum == HeadDofNum.SixDof ? 1 : 0;
        var handdof = Pvr_UnitySDKManager.SDK.HandDofNum == HandDofNum.SixDof ? 1 : 0;
        if (pause)
        {
          
            if (controllerlink.neoserviceStarted)
            {
                controllerlink.SetGameObjectToJar("");
                controllerlink.StopControllerThread(headdof, handdof);
            }
            if(controllerlink.goblinserviceStarted)
            {
                controllerlink.StopLark2Receiver();
            }
        }
        else
        {
            controllerlink.Controller0 = new ControllerHand();
            controllerlink.Controller1 = new ControllerHand();
            if (controllerlink.neoserviceStarted)
            {
                controllerlink.SetGameObjectToJar(controllerlink.gameobjname);
                controllerlink.StartControllerThread(headdof, handdof);
            }
            if (controllerlink.goblinserviceStarted)
            {
                controllerlink.StartLark2Receiver();
                controllerlink.controller0Connected = GetControllerConnectionState(0) == 1;
                if (PvrServiceStartSuccessEvent != null)
                    PvrServiceStartSuccessEvent();
            }
        }
    }
    #endregion

    /************************************ Public Interfaces  *********************************/
    #region Public Interfaces

   
    public void StopLark2Service()
    {
        if (controllerlink != null)
        {
            controllerlink.StopLark2Service();
        }
    }
  
    public Vector3 GetAngularVelocity(int num)
    {
        if (controllerlink != null)
        {
            return controllerlink.GetAngularVelocity(num);
        }
        return new Vector3(0.0f, 0.0f, 0.0f);
    }
   
    public Vector3 GetAcceleration(int num)
    {
        if (controllerlink != null)
        {
            return controllerlink.GetAcceleration(num);
        }
        return new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void BindService()
    {
        if (controllerlink != null)
        {
            controllerlink.BindService();
        }
    }
    public void StartScan()
    {
#if ANDROID_DEVICE
        if (controllerlink != null)
        {
            controllerlink.StartScan();
        }
#elif IOS_DEVICE
        TouchPad.ScanIOSBLEDevice(2);
#endif
    }
    public void StopScan()
    {
        if (controllerlink != null)
        {
            controllerlink.StopScan();
        }
    }
   
    public void ResetController(int num)
    {
        if (controllerlink != null)
        {
            controllerlink.ResetController(num);
        }
    }
    public static int GetControllerConnectionState(int num)
    {
        var sta = controllerlink.GetControllerConnectionState(num);
        return sta;
    }
    public void ConnectBLE()
    {
#if ANDROID_DEVICE
        if (controllerlink != null)
        {
            controllerlink.ConnectBLE();
        }
#elif IOS_DEVICE
        TouchPad.ConnectIOSBLEDevice(controllerlink.hummingBirdMac);
#endif

    }
    public void DisConnectBLE()
    {
        if (controllerlink != null)
        {
            controllerlink.DisConnectBLE();
        }
    }

    public void SetBinPath(string path, bool isAsset)
    {
        if (controllerlink != null)
        {
            controllerlink.setBinPath(path, isAsset);
        }
    }
    public void StartUpgrade()
    {
        if (controllerlink != null)
        {
            controllerlink.StartUpgrade();
        }
    }
    public static string GetBLEImageType()
    {
        var type = controllerlink.GetBLEImageType();
        return type;
    }
    public static long GetBLEVersion()
    {
        var version = controllerlink.GetBLEVersion();
        return version;
    }
    public static string GetFileImageType()
    {
        var type = controllerlink.GetFileImageType();
        return type;
    }
    public static long GetFileVersion()
    {
        var version = controllerlink.GetFileVersion();
        return version;
    }
    public static void AutoConnectHbController(int scans)
    {
        if (controllerlink != null)
        {
            controllerlink.AutoConnectHbController(scans);
        }
    }
    public static string GetConnectedDeviceMac()
    {
        string mac = "";
        if (controllerlink != null)
        {
            mac = controllerlink.GetConnectedDeviceMac();
        }
        return mac;
    }
    //--------------
    public void setHbControllerMac(string mac)
    {
        PLOG.I("PvrLog HBMacRSSI" + mac);
        controllerlink.hummingBirdMac = mac.Substring(0, 17);
        controllerlink.hummingBirdRSSI = Convert.ToInt16(mac.Remove(0, 18));
        if (SetHbControllerMacEvent != null)
            SetHbControllerMacEvent(mac.Substring(0, 17));
    }
    public int GetControllerRSSI()
    {
        return controllerlink.hummingBirdRSSI;
    }

    public void setHbServiceBindState(string state)
    {
        PLOG.I("PvrLog HBBindCallBack" + state);
        controllerServicestate = true;
        //State: 0- unbound, 1- bound, 2- timed.
        if (Convert.ToInt16(state) == 0)
        {
            Invoke("BindService", 0.5f);
            controllerlink.goblinserviceStarted = false;
        }
        else if (Convert.ToInt16(state) == 1)
        {
            controllerlink.goblinserviceStarted = true;
            controllerlink.controller0Connected = GetControllerConnectionState(0) == 1;
            if (SetHbServiceBindStateEvent != null)
            {
                SetHbServiceBindStateEvent();
            }
            if (PvrServiceStartSuccessEvent != null)
            {
                PvrServiceStartSuccessEvent();
            }
        }
    }
    public void setControllerServiceBindState(string state)
    {
        PLOG.I("PvrLog CVBindCallBack" + state);
        //state:0 unbind,1:bind
        if (Convert.ToInt16(state) == 0)
        {
            Invoke("BindService", 0.5f);
            controllerlink.neoserviceStarted = false;
        }
        else if (Convert.ToInt16(state) == 1)
        {
            controllerlink.neoserviceStarted = true;
            var headdof = Pvr_UnitySDKManager.SDK.HeadDofNum == HeadDofNum.SixDof ? 1 : 0;
            var handdof = Pvr_UnitySDKManager.SDK.HandDofNum == HandDofNum.SixDof ? 1 : 0;
            controllerlink.StartControllerThread(headdof, handdof);
            if (SetControllerServiceBindStateEvent != null)
                SetControllerServiceBindStateEvent();
        }
    }
    public void setHbControllerConnectState(string isconnect)
    {
        PLOG.I("PvrLog HBControllerConnect" + isconnect);
        controllerlink.controller0Connected = Convert.ToInt16(isconnect) == 1;
        if (!controllerlink.controller0Connected)
        {
            controllerlink.Controller0 = new ControllerHand();
        }
        else
        {
            ResetController(0);
        }
        //State: 0- disconnect, 1- connected, 2- unknown.
        stopConnect = false;
        if (ControllerStatusChangeEvent != null)
            ControllerStatusChangeEvent(isconnect);
        if (PvrControllerStateChangedEvent != null)
            PvrControllerStateChangedEvent(isconnect);
    }
    
    public void setControllerStateChanged(string state)
    {
        PLOG.I("PvrLog CVControllerStateChanged" + state);
        if (SetControllerStateChangedEvent != null)
            SetControllerStateChangedEvent(state);
        if (PvrControllerStateChangedEvent != null)
            PvrControllerStateChangedEvent(state);
        
        int controller = Convert.ToInt16(state.Substring(0, 1));
        if (controller == 0)
        {
            controllerlink.controller0Connected = Convert.ToBoolean(Convert.ToInt16(state.Substring(2, 1)));
            if (!controllerlink.controller0Connected)
                controllerlink.Controller0 = new ControllerHand();
        }
        else
        {
            controllerlink.controller1Connected = Convert.ToBoolean(Convert.ToInt16(state.Substring(2, 1)));
            if (!controllerlink.controller1Connected)
                controllerlink.Controller1 = new ControllerHand();
        }
        if (Convert.ToBoolean(Convert.ToInt16(state.Substring(2, 1))))
        {
            controllerlink.ResetController(controller);
        }
    }
 
    public void setControllerAbility(string data)
    {
        //data format is ID,ability,state.
        //ID 0/1 represents two handles.
        //ability 1/2 1:3dof controller 2. 6dof controller.
        //state 0/1 0: disconnect 1: connection.
        //this callback for setControllerStateChanged extended edition, on the basis of this callback to increase the ability of controller
        PLOG.I("PvrLog setControllerAbility" + data);
        if (SetControllerAbilityEvent != null)
            SetControllerAbilityEvent(data);
    }

    public void controllerThreadStartedCallback()
    {
        PLOG.I("PvrLog ThreadStartSuccess");
        GetCVControllerState();
        if (ControllerThreadStartedCallbackEvent != null)
            ControllerThreadStartedCallbackEvent();
        if (PvrServiceStartSuccessEvent != null)
            PvrServiceStartSuccessEvent();
    }


    public void controllerDeviceVersionCallback(string data)
    {
        PLOG.I("PvrLog VersionCallBack" + data);
        //data format device, deviceVersion
        //device: 0-station 1- controller 0 2- controller 1 deviceVersion: version number.
        if (ControllerDeviceVersionCallbackEvent != null)
            ControllerDeviceVersionCallbackEvent(data);
    }

    public void controllerSnCodeCallback(string data)
    {
        PLOG.I("PvrLog SNCodeCallBack" + data);
        //data formats: controllerSerialNum, controllerSn
        //controllerSerialNum: 0- controller 1 controllerSn: the unique identification of the controller of Sn.
        if (ControllerSnCodeCallbackEvent != null)
            ControllerSnCodeCallbackEvent(data);
    }
   
    public void controllerUnbindCallback(string status)
    {
        PLOG.I("PvrLog ControllerUnBindCallBack" + status);
        // status: 0- failure 1- success 
        if (ControllerUnbindCallbackEvent != null)
            ControllerUnbindCallbackEvent(status);
    }
    
    public void controllerStationStatusCallback(string status)
    {
        PLOG.I("PvrLog StationStatusCallBack" + status);
        //STATION_STATUS{NORMAL = 0, QUERYING = 1, PAIRING = 2, OTA = 3, RESTARTING = 4, CTRLR_UNBINDING = 5, CTRLR_SHUTTING_DOWN = 6};
        if (ControllerStationStatusCallbackEvent != null)
            ControllerStationStatusCallbackEvent(status);
    }
    
    public void controllerStationBusyCallback(string status)
    {
        PLOG.I("PvrLog StationBusyCallBack" + status);
        //STATION_STATUS{NORMAL = 0, QUERYING, PAIRING, OTA, RESTARTING, CTRLR_UNBINDING, CTRLR_SHUTTING_DOWN};
        if (ControllerStationBusyCallbackEvent != null)
            ControllerStationBusyCallbackEvent(status);
    }
   
    public void controllerOTAStartCodeCallback(string data)
    {
        PLOG.I("PvrLog OTAUpdateCallBack" + data);
        //data:deviceType,statusCode
        // deviceType:0-station 1-controller statusCode: 0- upgrade launch success 1- upgrade file not found 2- upgrade file failed to open.
        if (ControllerOtaStartCodeCallbackEvent != null)
            ControllerOtaStartCodeCallbackEvent(data);
    }
 
    public void controllerDeviceVersionAndSNCallback(string data)
    {
        PLOG.I("PvrLog DeviceVersionAndSNCallback" + data);
        //data controllerSerialNum,deviceVersion
        //controllerSerialNum : 0- controller 0 1- controller 1 deviceVersion: version and SN 
        if (ControllerDeviceVersionAndSNCallbackEvent != null)
            ControllerDeviceVersionAndSNCallbackEvent(data);
    }
    
    public void controllerUniqueIDCallback(string data)
    {
        PLOG.I("PvrLog controllerUniqueIDCallback" + data);
        //data controller0ID，controller1ID
        //controller0ID ：ID of controller 0;Controller1ID: ID of controller 1 (if the current controller is not connected, the ID will return to 0)
        if (ControllerUniqueIDCallbackEvent != null)
            ControllerUniqueIDCallbackEvent(data);
    }
    
    public void controllerCombinedKeyUnbindCallback(string controllerSerialNum)
    {
        //controllerSerialNum 0：controller0 1：controller1
        if (ControllerCombinedKeyUnbindCallbackEvent != null)
            ControllerCombinedKeyUnbindCallbackEvent(controllerSerialNum);
    }
    public void setupdateFailed()
    {
        
    }

    public void setupdateSuccess()
    {
        
    }

    public void setupdateProgress(string progress)
    {
        //The upgrade progress 0-100 
    }

    public void setHbAutoConnectState(string state)
    {
        PLOG.I("PvrLog HBAutoConnectState" + state);
        // UNKNOW = 1; the default value
        // NO_DEVICE = 0; No scan to HB controller.
        // ONLY_ONE = 1; Scan only one HB controller.
        // MORE_THAN_ONE = 2; Scan to multiple HB handles.
        // LAST_CONNECTED = 3; Scan the HB controller that was last connected.
        // FACTORY_DEFAULT = 4; Scan the HB controller of the factory binding(temporarily not enabled)
        controllerServicestate = true;
        if (Convert.ToInt16(state) == 0)
        {
            if (GetControllerConnectionState(0) == 0)
            {
                ShowToast(2);
            }
        }
        if (Convert.ToInt16(state) == 2)
        {
            ShowToast(3);
        }
    }

    public void callbackControllerServiceState(string state)
    {
        PLOG.I("PvrLog HBServiceState" + state);
        //state = 0,Non-mobile platform, service is not started.
        //state = 1,The mobile platform, the service is not started, but the system will initiate the service.
        //state = 2,Mobile platform, service apk is not installed, need to install.
        controllerServicestate = true;
        if (Convert.ToInt16(state) == 0)
        {
            ShowToast(0);
        }
        if (Convert.ToInt16(state) == 1)
        {
            BindService();
        }
        if (Convert.ToInt16(state) == 2)
        {
            ShowToast(1);
        }
    }
  
    public void changeMainControllerCallback(string index)
    {
        PLOG.I("PvrLog MainControllerCallBack" + index);
        //index = 0/1
        if (ChangeMainControllerCallBackEvent != null)
            ChangeMainControllerCallBackEvent(index);
    }

    private void ShowToast(int type)
    {
        switch (type)
        {
            case 0: 
                if (toast != null)
                {
                    if (localanguage == SystemLanguage.Chinese || localanguage == SystemLanguage.ChineseSimplified)
                    {
                        toast.text = "手柄服务未启动，请先启动手柄服务";
                    }
                    else
                    {
                        toast.text = "No handle service found, please turnon the handle service first";
                    }
                    Invoke("HideToast", 5.0f);
                }
                break;
            case 1: 
                if (toast != null)
                {
                    if (localanguage == SystemLanguage.Chinese || localanguage == SystemLanguage.ChineseSimplified)
                    {
                        toast.text = "未发现手柄服务，请使用PicoVR下载并安装";
                    }
                    else
                    {
                        toast.text = "No handle service found, please use PicoVR to download and install";
                    }
                    Invoke("HideToast", 5.0f);
                }
                break;
            case 2: 
                if (toast != null)
                {
                    if (localanguage == SystemLanguage.Chinese || localanguage == SystemLanguage.ChineseSimplified)
                    {
                        toast.text = "没有扫描到手柄，请确保手机蓝牙开启，并短按手柄Home键";
                    }
                    else
                    {
                        toast.text = "Can not find any handle, please turn on bluetooth and press handle home key";
                    }
                    AutoConnectHbController(6000);
                    Invoke("HideToast", 5.0f);
                }

                break;
            case 3: 
                if (toast != null)
                {
                    if (localanguage == SystemLanguage.Chinese || localanguage == SystemLanguage.ChineseSimplified)
                    {
                        toast.text = "扫描到多个手柄，请保持周围只有一个开启状态的手柄";
                    }
                    else
                    {
                        toast.text = "Find more than one handle, turn off the unused handle";
                    }
                    AutoConnectHbController(6000);
                    Invoke("HideToast", 5.0f);
                }
                break;
            case 4: 
                if (toast != null)
                {
                    if (localanguage == SystemLanguage.Chinese || localanguage == SystemLanguage.ChineseSimplified)
                    {
                        toast.text = "手柄服务启动异常，请检查后台权限及安全设置";
                    }
                    else
                    {
                        toast.text = "The handle service starts abnormally. Please check the background permissions and security settings";
                    }
                }
                break;
            default:
                return;
        }
    }
    private void HideToast()
    {
        if (toast != null)
        {
            toast.text = "";
        }
    }

    private void CheckControllerService()
    {
        if (!controllerServicestate)
        {
            ShowToast(4);
        }
    }

    private void GetCVControllerState()
    {
        var state0 = GetControllerConnectionState(0);
        var state1 = GetControllerConnectionState(1);
        PLOG.I("PvrLog CVconnect" + state0 + state1);
        if (state0 == -1 && state1 == -1)
        {
            Invoke("GetCVControllerState", 0.02f);
        }
        if (state0 != -1 && state1 != -1)
        {
            controllerlink.controller0Connected = state0 == 1;
            controllerlink.controller1Connected = state1 == 1;
            if (!controllerlink.controller0Connected && controllerlink.controller1Connected)
            {
                if (Controller.UPvr_GetMainHandNess() == 0)
                {
                    Controller.UPvr_SetMainHandNess(1);
                }
            }
        }
    }

    private void SetSystemKey()
    {
        if (controllerlink.switchHomeKey)
        {
            if (Pvr_UnitySDKManager.SDK.HeadDofNum == HeadDofNum.ThreeDof || Pvr_UnitySDKManager.SDK.SixDofRecenter)
            {
                if (Controller.UPvr_GetKeyLongPressed(0, Pvr_KeyCode.HOME))
                {
                    if (Pvr_UnitySDKManager.SDK.IsViewerLogicFlow)
                    {
                        Pvr_UnitySDKManager.pvr_UnitySDKSensor.ResetUnitySDKSensorAll();
                    }
                    else
                    {
                        if (Pvr_UnitySDKManager.SDK.safeToast.activeSelf)
                        {
                            Pvr_UnitySDKManager.SDK.safeToast.SetActive(false);
                            Pvr_UnitySDKManager.pvr_UnitySDKSensor.OptionalResetUnitySDKSensor(1, 1);
                        }
                        else
                        {
                            Pvr_UnitySDKManager.pvr_UnitySDKSensor.OptionalResetUnitySDKSensor(1, 0);
                        }
                        if (Pvr_UnitySDKManager.SDK.HeadDofNum == HeadDofNum.SixDof && Pvr_UnitySDKManager.SDK.SixDofRecenter)
                        {
                            controllerlink.ResetHeadSensorForController();
                        }
                    }
                    ResetController(0);
                }
                if (Controller.UPvr_GetKeyLongPressed(1, Pvr_KeyCode.HOME))
                {
                    if (Pvr_UnitySDKManager.SDK.IsViewerLogicFlow)
                    {
                        Pvr_UnitySDKManager.pvr_UnitySDKSensor.ResetUnitySDKSensorAll();
                    }
                    else
                    {
                        if (Pvr_UnitySDKManager.SDK.safeToast.activeSelf)
                        {
                            Pvr_UnitySDKManager.SDK.safeToast.SetActive(false);
                            Pvr_UnitySDKManager.pvr_UnitySDKSensor.OptionalResetUnitySDKSensor(1, 1);
                        }
                        else
                        {
                            Pvr_UnitySDKManager.pvr_UnitySDKSensor.OptionalResetUnitySDKSensor(1, 0);
                        }
                        if (Pvr_UnitySDKManager.SDK.HeadDofNum == HeadDofNum.SixDof && Pvr_UnitySDKManager.SDK.SixDofRecenter)
                        {
                            controllerlink.ResetHeadSensorForController();
                        }
                    }
                    ResetController(1);
                }
            }
            else
            {
                if (Controller.UPvr_GetKeyLongPressed(0, Pvr_KeyCode.HOME))
                {
                    ResetController(0);
                }
                if (Controller.UPvr_GetKeyLongPressed(1, Pvr_KeyCode.HOME))
                {
                    ResetController(1);
                }
            }
        }
        
        
        if (controllerlink.picoDevice)
        {
            if (controllerlink.switchHomeKey)
            {
                if (!controllerlink.Controller0.HomeKey.LongPressedClock && Controller.UPvr_GetKeyUp(0, Pvr_KeyCode.HOME) || !controllerlink.Controller1.HomeKey.LongPressedClock && Controller.UPvr_GetKeyUp(1, Pvr_KeyCode.HOME) && !stopConnect)
                {
                    controllerlink.RebackToLauncher();
                }
            }
            if (!controllerlink.Controller0.VolumeUpKey.LongPressedClock && Controller.UPvr_GetKeyUp(0, Pvr_KeyCode.VOLUMEUP) || !controllerlink.Controller1.VolumeUpKey.LongPressedClock && Controller.UPvr_GetKeyUp(1, Pvr_KeyCode.VOLUMEUP))
            {
                controllerlink.TurnUpVolume();
            }
            if (!controllerlink.Controller0.VolumeDownKey.LongPressedClock && Controller.UPvr_GetKeyUp(0, Pvr_KeyCode.VOLUMEDOWN) || !controllerlink.Controller1.VolumeDownKey.LongPressedClock && Controller.UPvr_GetKeyUp(1, Pvr_KeyCode.VOLUMEDOWN))
            {
                controllerlink.TurnDownVolume();
            }
            if (!Controller.UPvr_GetKey(0, Pvr_KeyCode.VOLUMEUP) && !Controller.UPvr_GetKey(0, Pvr_KeyCode.VOLUMEDOWN) && !Controller.UPvr_GetKey(1, Pvr_KeyCode.VOLUMEUP) && !Controller.UPvr_GetKey(1, Pvr_KeyCode.VOLUMEDOWN))
            {
                cTime = 1.0f;
            }
            if (Controller.UPvr_GetKey(0, Pvr_KeyCode.VOLUMEUP) || Controller.UPvr_GetKey(1, Pvr_KeyCode.VOLUMEUP))
            {
                cTime -= Time.deltaTime;
                if (cTime <= 0)
                {
                    cTime = 0.2f;
                    controllerlink.TurnUpVolume();
                }
            }
            if (!Controller.UPvr_GetKey(0, Pvr_KeyCode.HOME) && !Controller.UPvr_GetKey(1, Pvr_KeyCode.HOME) && (Controller.UPvr_GetKey(0, Pvr_KeyCode.VOLUMEDOWN) || Controller.UPvr_GetKey(1, Pvr_KeyCode.VOLUMEDOWN)))
            {
                cTime -= Time.deltaTime;
                if (cTime <= 0)
                {
                    cTime = 0.2f;
                    controllerlink.TurnDownVolume();
                }
            }
        }
        if (controllerlink.goblinserviceStarted)
        {
            if (Controller.UPvr_GetKey(0, Pvr_KeyCode.HOME) && Controller.UPvr_GetKey(0, Pvr_KeyCode.VOLUMEDOWN) && !stopConnect)
            {
                disConnectTime += Time.deltaTime;
                if (disConnectTime > 1.0)
                {
                    DisConnectBLE();
                    controllerlink.hummingBirdMac = "";
                    stopConnect = true;
                    disConnectTime = 0;
                }
            }
        }
    }

    private void SetSwipeData(ControllerHand hand)
    {
        if (hand.TouchPadPosition != Vector2.zero)
        {
            if (!hand.touchClock)
            {
                beginDT = DateTime.UtcNow;
                
                hand.touchDownPosition = hand.TouchPadPosition;
                hand.touchClock = true;
            }
            hand.touchUpPosition = hand.TouchPadPosition;
        }
        else
        {
            endDT = DateTime.UtcNow;
            TimeSpan ts = endDT - beginDT;
            if (Convert.ToInt64(ts.TotalMilliseconds) <= 270)
            {
                hand.swipeData = hand.touchUpPosition - hand.touchDownPosition;

                if (Mathf.Abs(hand.swipeData.x) > 10 && Mathf.Abs(hand.swipeData.y) > 10)
                {
                    if (hand.swipeData.y / hand.swipeData.x >= 1.732)
                    {
                        hand.isVertical = false;
                        hand.isHorizontal = true;
                    }
                    else
                    {
                        hand.isVertical = true;
                        hand.isHorizontal = false;
                    }
                }
                if (hand.swipeData.x > 0f && hand.isVertical)
                {
                    hand.SwipeDirection = SwipeDirection.SwipeUp;
                }
                else if (hand.swipeData.x < 0f && hand.isVertical)
                {
                    hand.SwipeDirection = SwipeDirection.SwipeDown;
                }
                else if (hand.swipeData.y > 0f && hand.isHorizontal)
                {
                    hand.SwipeDirection = SwipeDirection.SwipeRight;
                }
                else if (hand.swipeData.y < 0f && hand.isHorizontal)
                {
                    hand.SwipeDirection = SwipeDirection.SwipeLeft;
                }
                else
                {
                    hand.SwipeDirection = SwipeDirection.No;
                }
            }
            beginDT = DateTime.UtcNow;
            hand.touchDownPosition = Vector2.zero;
            hand.touchUpPosition = Vector2.zero;
            hand.touchClock = false;
        }
    }

    private void SetTriggerClick(PvrControllerKey trigger,int value)
    {
        if (value >= 170)
        {
            trigger.PressedUp = false;
            if (!trigger.State)
            {
                trigger.PressedDown = true;
                trigger.LongPressedClock = false;
            }
            else
            {
                trigger.PressedDown = false;
            }
            trigger.State = true;
        }
        else
        {
            trigger.PressedUp = trigger.State;
            trigger.State = false;
            trigger.PressedDown = false;
        }
        if (trigger.State)
        {
            trigger.TimeCount += Time.deltaTime;
            if (trigger.TimeCount >= longpresstime &&
                !trigger.LongPressedClock)
            {
                trigger.LongPressed = true;
                trigger.LongPressedClock = true;
            }
            else
            {
                trigger.LongPressed = false;
            }
        }
        else
        {
            trigger.LongPressedClock = false;
            trigger.TimeCount = 0;
            trigger.LongPressed = false;
        } 
    }

    private void SetTouchPadClick(ControllerHand hand)
    {
        if (hand.TouchKey.State)
        {
            if (hand.TouchPadPosition.x <= 255 && hand.TouchPadPosition.x >= 127f + 63.5f * Mathf.Sin(45) &&
                hand.TouchPadPosition.y <= 127f + 63.5f * Mathf.Sin(45) &&
                hand.TouchPadPosition.y >= 127f - 63.5f * Mathf.Sin(45))
            {
                hand.TouchPadClick = TouchPadClick.ClickUp;
            }
            else if (hand.TouchPadPosition.x > 0 && hand.TouchPadPosition.x <= 127f - 63.5f * Mathf.Sin(45) &&
                     hand.TouchPadPosition.y <= 127f + 63.5f * Mathf.Sin(45) &&
                     hand.TouchPadPosition.y >= 127f - 63.5f * Mathf.Sin(45))
            {
                hand.TouchPadClick = TouchPadClick.ClickDown;
            }
            else if (hand.TouchPadPosition.y > 0 && hand.TouchPadPosition.y <= 127f - 63.5f * Mathf.Sin(45) &&
                     hand.TouchPadPosition.x <= 127f + 63.5f * Mathf.Sin(45) &&
                     hand.TouchPadPosition.x >= 127f - 63.5f * Mathf.Sin(45))
            {
                hand.TouchPadClick = TouchPadClick.ClickLeft;
            }
            else if (hand.TouchPadPosition.y <= 255 && hand.TouchPadPosition.y >= 127f + 63.5f * Mathf.Sin(45) &&
                     hand.TouchPadPosition.x <= 127f + 63.5f * Mathf.Sin(45) &&
                     hand.TouchPadPosition.x >= 127f - 63.5f * Mathf.Sin(45))
            {
                hand.TouchPadClick = TouchPadClick.ClickRight;
            }
            else
            {
                hand.TouchPadClick = TouchPadClick.No;
            }
        }
        else
        {
            hand.TouchPadClick = TouchPadClick.No;
        }
    }

    private void TransformData(PvrControllerKey key, int keystate)
    {
        if (keystate == 1)
        {
            key.PressedUp = false;
            if (!key.State)
            {
                key.PressedDown = true;
                key.LongPressedClock = false;
            }
            else
            {
                key.PressedDown = false;
            }
            key.State = true;
        }
        else
        {
            key.PressedUp = key.State;
            key.State = false;
            key.PressedDown = false;
        }
        if (key.State)
        {
            key.TimeCount += Time.deltaTime;
            if (key.TimeCount >= longpresstime &&
                !key.LongPressedClock)
            {
                key.LongPressed = true;
                key.LongPressedClock = true;
            }
            else
            {
                key.LongPressed = false;
            }
        }
        else
        {
            key.TimeCount = 0;
            key.LongPressed = false;
        }
    }

    #endregion

}
