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

    private string lark2state;
    private string lark2key;
    private float lark2w = 1.0f, lark2x = 0f, lark2y = 0f, lark2z = 0f;
    private int lark2touchx = 0, lark2touchy = 0, lark2home = 0, lark2app = 0, lark2click = 0, lark2volup = 0, lark2voldown = 0, lark2power = 0;
    private int touchXBegin = 0, touchXEnd = 0, touchYBegin = 0, touchYEnd = 0;
    private bool touchClock = false;
    public static bool longPressclock = false;
    public static Pvr_ControllerLink controllerlink;
    public bool ExtendedAPI;
    private float cTime = 1.0f;
    private int touchNum = 0;
    public int slipNum = 100;  //滑动值，0-255，滑动超过此值，则判定为成功，若感觉太灵敏则调高，反之调低
    private bool stopConnect = false;
    private SystemLanguage localanguage;
    public Text toast;
    private bool controllerServicestate = false;
    private float disConnectTime;
    private float longPressTime = 0.5f;
    private bool enableKeyEvent = true;
    #endregion


    public delegate void ControllerStatusChange(string isconnect);
    public static event ControllerStatusChange ControllerStatusChangeEvent;



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
        ControllerStatusChangeEvent += ControllerStatusChangeEventDebug;
    }
    // Use this for initialization
    void Start()
    {
        localanguage = Application.systemLanguage;
#if ANDROID_DEVICE
        if(!controllerlink.notPhone)
        {
            Invoke("CheckControllerService", 10.0f);
        }
		
		if (GetHBConnectionState() == 2)
        {
            controllerlink.isConnect = true;
        }
#endif

    }

    // Update is called once per frame
    void Update()
    {

        #region AndroidData
#if UNITY_ANDROID

        if (controllerlink.isConnect)
        {
            //手柄转动四元数
            lark2state = controllerlink.GetHBSensorState();
            JsonData Jstate = JsonMapper.ToObject(lark2state);
            lark2w = Convert.ToSingle(Jstate[0].ToString());
            lark2x = Convert.ToSingle(Jstate[1].ToString());
            lark2y = Convert.ToSingle(Jstate[2].ToString());
            lark2z = Convert.ToSingle(Jstate[3].ToString());
            Controller.ControllerQua = new Quaternion(lark2x, lark2y, lark2z, lark2w);

            //lark2手柄键值TouchpadX,TouchpadY,HomeKeyPress,AppKeyPress,ClickKeyPress,VolumeUpKeyPress,VolumeDownKeyPress,BatteryLevel
            lark2key = controllerlink.javaHummingbirdClass.CallStatic<string>("getHBKeyEvent");
            if (enableKeyEvent)
            {
                JsonData JKey = JsonMapper.ToObject(lark2key);
                if (Convert.ToInt16(JKey[0].ToString()) > 0 || Convert.ToInt16(JKey[1].ToString()) > 0)
                {
                    if (Convert.ToInt16(JKey[0].ToString()) == 0)
                    {
                        TouchPadPosition.x = 1;
                    }
                    if (Convert.ToInt16(JKey[1].ToString()) == 0)
                    {
                        TouchPadPosition.y = 1;
                    }
                    TouchPadPosition.x = Convert.ToInt16(JKey[0].ToString());
                    TouchPadPosition.y = Convert.ToInt16(JKey[1].ToString());
                }
                else
                {
                    touchNum++;
                    if (touchNum >= 1)
                    {
                        TouchPadPosition.x = 0;
                        TouchPadPosition.y = 0;
                        touchNum = 0;
                    }
                }
                Controller.BatteryLevel = Convert.ToInt16(JKey[7].ToString());

                #region base api 
                //键值状态
                //Home Key
                if (Convert.ToInt16(JKey[2].ToString()) == 1)
                {
                    if (!HomeKey.state)
                    {
                        HomeKey.pressedDown = true;
                        longPressclock = false;
                    }
                    else
                    {
                        HomeKey.pressedDown = false;
                    }
                    HomeKey.state = true;
                }
                else
                {
                    if (HomeKey.state)
                    {
                        HomeKey.pressedUp = true;
                    }
                    else
                    {
                        HomeKey.pressedUp = false;
                    }
                    HomeKey.state = false;
                    HomeKey.pressedDown = false;
                }
                //APP Key
                if (Convert.ToInt16(JKey[3].ToString()) == 1)
                {
                    if (!APPKey.state)
                    {
                        APPKey.pressedDown = true;
                        longPressclock = false;
                    }
                    else
                    {
                        APPKey.pressedDown = false;
                    }
                    APPKey.state = true;
                }
                else
                {
                    if (APPKey.state)
                    {
                        APPKey.pressedUp = true;
                    }
                    else
                    {
                        APPKey.pressedUp = false;
                    }
                    APPKey.state = false;
                    APPKey.pressedDown = false;
                }
                //Touchpad Key
                if (Convert.ToInt16(JKey[4].ToString()) == 1)
                {
                    if (!TouchPadKey.state)
                    {
                        TouchPadKey.pressedDown = true;
                        longPressclock = false;
                    }
                    else
                    {
                        TouchPadKey.pressedDown = false;
                    }
                    TouchPadKey.state = true;
                }
                else
                {
                    if (TouchPadKey.state)
                    {
                        TouchPadKey.pressedUp = true;
                    }
                    else
                    {
                        TouchPadKey.pressedUp = false;
                    }
                    TouchPadKey.state = false;
                    TouchPadKey.pressedDown = false;
                }
                //VolumeUP Key
                if (Convert.ToInt16(JKey[5].ToString()) == 1)
                {
                    if (!VolumeUpKey.state)
                    {
                        VolumeUpKey.pressedDown = true;
                        longPressclock = false;
                    }
                    else
                    {
                        VolumeUpKey.pressedDown = false;
                    }
                    VolumeUpKey.state = true;
                }
                else
                {
                    if (VolumeUpKey.state)
                    {
                        VolumeUpKey.pressedUp = true;
                    }
                    else
                    {
                        VolumeUpKey.pressedUp = false;
                    }
                    VolumeUpKey.state = false;
                    VolumeUpKey.pressedDown = false;
                }
                //VolumeDown Key
                if (Convert.ToInt16(JKey[6].ToString()) == 1)
                {
                    if (!VolumeDownKey.state)
                    {
                        VolumeDownKey.pressedDown = true;
                        longPressclock = false;
                    }
                    else
                    {
                        VolumeDownKey.pressedDown = false;
                    }
                    VolumeDownKey.state = true;
                }
                else
                {
                    if (VolumeDownKey.state)
                    {
                        VolumeDownKey.pressedUp = true;
                    }
                    else
                    {
                        VolumeDownKey.pressedUp = false;
                    }
                    VolumeDownKey.state = false;
                    VolumeDownKey.pressedDown = false;
                }
            }
            
            #endregion

            #region extended api
            //打开扩展API后，提供长按和滑动功能
            if (ExtendedAPI)
            {
                //slip
                if (TouchPadPosition.x > 0 || TouchPadPosition.y > 0)
                {
                    if (!touchClock)
                    {
                        touchXBegin = TouchPadPosition.x;
                        touchYBegin = TouchPadPosition.y;
                        touchClock = true;
                    }
                    touchXEnd = TouchPadPosition.x;
                    touchYEnd = TouchPadPosition.y;
                }
                else
                {
                    if (touchXEnd > touchXBegin)
                    {
                        if (touchYEnd > touchYBegin)
                        {
                            if (touchXEnd - touchXBegin > slipNum && ((touchXEnd - touchXBegin) > (touchYEnd - touchYBegin)))
                            {
                                //slide up
                                TouchPadKey.slideup = true;
                            }
                            if (touchYEnd - touchYBegin > slipNum && ((touchYEnd - touchYBegin) > (touchXEnd - touchXBegin)))
                            {
                                //slide right
                                TouchPadKey.slideright = true;
                            }
                        }
                        else if (touchYEnd < touchYBegin)
                        {
                            if (touchXEnd - touchXBegin > slipNum && ((touchXEnd - touchXBegin) > (touchYBegin - touchYEnd)))
                            {
                                //slide up
                                TouchPadKey.slideup = true;
                            }
                            if (touchYBegin - touchYEnd > slipNum && ((touchYBegin - touchYEnd) > (touchXEnd - touchXBegin)))
                            {
                                //slide left
                                TouchPadKey.slideleft = true;
                            }
                        }
                    }
                    else if (touchXEnd < touchXBegin)
                    {
                        if (touchYEnd > touchYBegin)
                        {
                            if (touchXBegin - touchXEnd > slipNum && ((touchXBegin - touchXEnd) > (touchYEnd - touchYBegin)))
                            {
                                //slide down
                                TouchPadKey.slidedown = true;
                            }
                            if (touchYEnd - touchYBegin > slipNum && ((touchYEnd - touchYBegin) > (touchXBegin - touchXEnd)))
                            {
                                //slide right
                                TouchPadKey.slideright = true;
                            }
                        }
                        else if (touchYEnd < touchYBegin)
                        {
                            if (touchXBegin - touchXEnd > slipNum && ((touchXBegin - touchXEnd) > (touchYBegin - touchYEnd)))
                            {
                                //slide down 
                                TouchPadKey.slidedown = true;
                            }
                            if (touchYBegin - touchYEnd > slipNum && ((touchYBegin - touchYEnd) > (touchXBegin - touchXEnd)))
                            {
                                //slide left
                                TouchPadKey.slideleft = true;
                            }
                        }
                    }
                    else
                    {
                        TouchPadKey.slideright = false;
                        TouchPadKey.slideleft = false;
                        TouchPadKey.slidedown = false;
                        TouchPadKey.slideup = false;
                    }
                    touchXBegin = 0;
                    touchXEnd = 0;
                    touchYBegin = 0;
                    touchYEnd = 0;
                    touchClock = false;
                }

                //longpress
                if (HomeKey.state)
                {
                    HomeKey.timecount += Time.deltaTime;
                    if (HomeKey.timecount >= longPressTime && !HomeKey.longPressedClock)
                    {
                        HomeKey.longPressed = true;
                        HomeKey.longPressedClock = true;
                        longPressclock = true;
                    }
                    else
                    {
                        HomeKey.longPressed = false;
                    }
                }
                else
                {
                    HomeKey.longPressedClock = false;
                    HomeKey.timecount = 0;
                    HomeKey.longPressed = false;
                }
                if (APPKey.state)
                {
                    APPKey.timecount += Time.deltaTime;
                    if (APPKey.timecount >= longPressTime && !APPKey.longPressedClock)
                    {
                        APPKey.longPressed = true;
                        APPKey.longPressedClock = true;
                        longPressclock = true;
                    }
                    else
                    {
                        APPKey.longPressed = false;
                    }
                }
                else
                {
                    APPKey.longPressedClock = false;
                    APPKey.timecount = 0;
                    APPKey.longPressed = false;
                }
                if (TouchPadKey.state)
                {
                    TouchPadKey.timecount += Time.deltaTime;
                    if (TouchPadKey.timecount >= longPressTime && !TouchPadKey.longPressedClock)
                    {
                        TouchPadKey.longPressed = true;
                        TouchPadKey.longPressedClock = true;
                        longPressclock = true;
                    }
                    else
                    {
                        TouchPadKey.longPressed = false;
                    }
                }
                else
                {
                    TouchPadKey.longPressedClock = false;
                    TouchPadKey.timecount = 0;
                    TouchPadKey.longPressed = false;
                }
                if (VolumeUpKey.state)
                {
                    VolumeUpKey.timecount += Time.deltaTime;
                    if (VolumeUpKey.timecount >= longPressTime && !VolumeUpKey.longPressedClock)
                    {
                        VolumeUpKey.longPressed = true;
                        VolumeUpKey.longPressedClock = true;
                        longPressclock = true;
                    }
                    else
                    {
                        VolumeUpKey.longPressed = false;
                    }
                }
                else
                {
                    VolumeUpKey.longPressedClock = false;
                    VolumeUpKey.timecount = 0;
                    VolumeUpKey.longPressed = false;
                }
                if (VolumeDownKey.state)
                {
                    VolumeDownKey.timecount += Time.deltaTime;
                    if (VolumeDownKey.timecount >= longPressTime && !VolumeDownKey.longPressedClock)
                    {
                        VolumeDownKey.longPressed = true;
                        VolumeDownKey.longPressedClock = true;
                        longPressclock = true;
                    }
                    else
                    {
                        VolumeDownKey.longPressed = false;
                    }
                }
                else
                {
                    VolumeDownKey.longPressedClock = false;
                    VolumeDownKey.timecount = 0;
                    VolumeDownKey.longPressed = false;
                }

            }
            #endregion


            if (controllerlink.notPhone)
            {
                if (!longPressclock && Controller.UPvr_GetKeyUp(Pvr_KeyCode.HOME) && !stopConnect)
                {
                    controllerlink.RebackToLauncher();
                }
                if (!longPressclock && Controller.UPvr_GetKeyUp(Pvr_KeyCode.VOLUMEUP))
                {
                    controllerlink.TurnUpVolume();
                }
                if (!longPressclock && Controller.UPvr_GetKeyUp(Pvr_KeyCode.VOLUMEDOWN))
                {
                    controllerlink.TurnDownVolume();
                }
                if (!Controller.UPvr_GetKey(Pvr_KeyCode.VOLUMEUP) && !Controller.UPvr_GetKey(Pvr_KeyCode.VOLUMEDOWN))
                {
                    cTime = 1.0f;
                }
                if (Controller.UPvr_GetKey(Pvr_KeyCode.VOLUMEUP))
                {
                    cTime -= Time.deltaTime;
                    if (cTime <= 0)
                    {
                        cTime = 0.2f;
                        controllerlink.TurnUpVolume();
                    }
                }
                if (Controller.UPvr_GetKey(Pvr_KeyCode.VOLUMEDOWN))
                {
                    cTime -= Time.deltaTime;
                    if (cTime <= 0)
                    {
                        cTime = 0.2f;
                        controllerlink.TurnDownVolume();
                    }
                }
            }

            if (Controller.UPvr_GetKey(Pvr_KeyCode.HOME) && Controller.UPvr_GetKey(Pvr_KeyCode.VOLUMEDOWN) && !stopConnect)
            {
                disConnectTime += Time.deltaTime;
                if (disConnectTime > 1.0)
                {
                    DisConnectBLE();
                    controllerlink.hummingBirdMac = "";
                    stopConnect = true;
                }
            }
        }
#endif
        #endregion
        #region IOSData
#if IOS_DEVICE
        Controller.PVR_GetLark2SensorMessage(ref lark2x, ref lark2y, ref lark2z, ref lark2w);
        Controller.ControllerQua = new Quaternion(lark2x, lark2y, lark2z, lark2w);

        Controller.PVR_GetLark2KeyValueMessage(ref lark2touchx, ref lark2touchy, ref lark2home, ref lark2app, ref lark2click, ref lark2volup, ref lark2voldown, ref lark2power);
        if (lark2touchx > 0 || lark2touchy > 0)
        {
            if (lark2touchx == 0)
            {
                TouchPadPosition.x = 1;
            }
            if (lark2touchy == 0)
            {
                TouchPadPosition.y = 1;
            }
            TouchPadPosition.x = lark2touchx;
            TouchPadPosition.y = lark2touchy;
        }
        else
        {
            touchNum++;
            if (touchNum >= 1)
            {
                TouchPadPosition.x = 0;
                TouchPadPosition.y = 0;
                touchNum = 0;
            }
        }
        Controller.BatteryLevel = lark2power;
        #region base api 
        //键值状态
        //Home Key
        if (lark2home == 1)
        {
            if (!HomeKey.state)
            {
                HomeKey.pressedDown = true;
                longPressclock = false;
            }
            else
            {
                HomeKey.pressedDown = false;
            }
            HomeKey.state = true;
        }
        else
        {
            if (HomeKey.state)
            {
                HomeKey.pressedUp = true;
            }
            else
            {
                HomeKey.pressedUp = false;
            }
            HomeKey.state = false;
            HomeKey.pressedDown = false;
        }
        //APP Key
        if (lark2app == 1)
        {
            if (!APPKey.state)
            {
                APPKey.pressedDown = true;
                longPressclock = false;
            }
            else
            {
                APPKey.pressedDown = false;
            }
            APPKey.state = true;
        }
        else
        {
            if (APPKey.state)
            {
                APPKey.pressedUp = true;
            }
            else
            {
                APPKey.pressedUp = false;
            }
            APPKey.state = false;
            APPKey.pressedDown = false;
        }
        //Touchpad Key
        if (lark2click == 1)
        {
            if (!TouchPadKey.state)
            {
                TouchPadKey.pressedDown = true;
                longPressclock = false;
            }
            else
            {
                TouchPadKey.pressedDown = false;
            }
            TouchPadKey.state = true;
        }
        else
        {
            if (TouchPadKey.state)
            {
                TouchPadKey.pressedUp = true;
            }
            else
            {
                TouchPadKey.pressedUp = false;
            }
            TouchPadKey.state = false;
            TouchPadKey.pressedDown = false;
        }
        //VolumeUP Key
        if (lark2volup == 1)
        {
            if (!VolumeUpKey.state)
            {
                VolumeUpKey.pressedDown = true;
                longPressclock = false;
            }
            else
            {
                VolumeUpKey.pressedDown = false;
            }
            VolumeUpKey.state = true;
        }
        else
        {
            if (VolumeUpKey.state)
            {
                VolumeUpKey.pressedUp = true;
            }
            else
            {
                VolumeUpKey.pressedUp = false;
            }
            VolumeUpKey.state = false;
            VolumeUpKey.pressedDown = false;
        }
        //VolumeDown Key
        if (lark2voldown == 1)
        {
            if (!VolumeDownKey.state)
            {
                VolumeDownKey.pressedDown = true;
                longPressclock = false;
            }
            else
            {
                VolumeDownKey.pressedDown = false;
            }
            VolumeDownKey.state = true;
        }
        else
        {
            if (VolumeDownKey.state)
            {
                VolumeDownKey.pressedUp = true;
            }
            else
            {
                VolumeDownKey.pressedUp = false;
            }
            VolumeDownKey.state = false;
            VolumeDownKey.pressedDown = false;
        }


        #endregion

        #region extended api
        //打开扩展API后，提供长按和滑动功能
        if (ExtendedAPI)
        {
            //slip
            if (TouchPadPosition.x > 0 || TouchPadPosition.y > 0)
            {
                if (!touchClock)
                {
                    touchXBegin = TouchPadPosition.x;
                    touchYBegin = TouchPadPosition.y;
                    touchClock = true;
                }
                touchXEnd = TouchPadPosition.x;
                touchYEnd = TouchPadPosition.y;
            }
            else
            {
                if (touchXEnd > touchXBegin)
                {
                    if (touchYEnd > touchYBegin)
                    {
                        if (touchXEnd - touchXBegin > slipNum && ((touchXEnd - touchXBegin) > (touchYEnd - touchYBegin)))
                        {
                            //slide up
                            TouchPadKey.slideup = true;
                        }
                        if (touchYEnd - touchYBegin > slipNum && ((touchYEnd - touchYBegin) > (touchXEnd - touchXBegin)))
                        {
                            //slide right
                            TouchPadKey.slideright = true;
                        }
                    }
                    else if (touchYEnd < touchYBegin)
                    {
                        if (touchXEnd - touchXBegin > slipNum && ((touchXEnd - touchXBegin) > (touchYBegin - touchYEnd)))
                        {
                            //slide up
                            TouchPadKey.slideup = true;
                        }
                        if (touchYBegin - touchYEnd > slipNum && ((touchYBegin - touchYEnd) > (touchXEnd - touchXBegin)))
                        {
                            //slide left
                            TouchPadKey.slideleft = true;
                        }
                    }

                }
                else if (touchXEnd < touchXBegin)
                {
                    if (touchYEnd > touchYBegin)
                    {
                        if (touchXBegin - touchXEnd > slipNum && ((touchXBegin - touchXEnd) > (touchYEnd - touchYBegin)))
                        {
                            //slide down
                            TouchPadKey.slidedown = true;
                        }
                        if (touchYEnd - touchYBegin > slipNum && ((touchYEnd - touchYBegin) > (touchXBegin - touchXEnd)))
                        {
                            //slide right
                            TouchPadKey.slideright = true;
                        }
                    }
                    else if (touchYEnd < touchYBegin)
                    {
                        if (touchXBegin - touchXEnd > slipNum && ((touchXBegin - touchXEnd) > (touchYBegin - touchYEnd)))
                        {
                            //slide down 
                            TouchPadKey.slidedown = true;
                        }
                        if (touchYBegin - touchYEnd > slipNum && ((touchYBegin - touchYEnd) > (touchXBegin - touchXEnd)))
                        {
                            //slide left
                            TouchPadKey.slideleft = true;
                        }
                    }
                }
                else
                {
                    TouchPadKey.slideright = false;
                    TouchPadKey.slideleft = false;
                    TouchPadKey.slidedown = false;
                    TouchPadKey.slideup = false;
                }
                touchXBegin = 0;
                touchXEnd = 0;
                touchYBegin = 0;
                touchYEnd = 0;
                touchClock = false;
            }

            //longpress
            if (HomeKey.state)
            {
                HomeKey.timecount += Time.deltaTime;
                if (HomeKey.timecount >= longPressTime && !HomeKey.longPressedClock)
                {
                    HomeKey.longPressed = true;
                    HomeKey.longPressedClock = true;
                    longPressclock = true;
                }
                else
                {
                    HomeKey.longPressed = false;
                }
            }
            else
            {
                HomeKey.longPressedClock = false;
                HomeKey.timecount = 0;
                HomeKey.longPressed = false;
            }
            if (APPKey.state)
            {
                APPKey.timecount += Time.deltaTime;
                if (APPKey.timecount >= longPressTime && !APPKey.longPressedClock)
                {
                    APPKey.longPressed = true;
                    APPKey.longPressedClock = true;
                    longPressclock = true;
                }
                else
                {
                    APPKey.longPressed = false;
                }
            }
            else
            {
                APPKey.longPressedClock = false;
                APPKey.timecount = 0;
                APPKey.longPressed = false;
            }
            if (TouchPadKey.state)
            {
                TouchPadKey.timecount += Time.deltaTime;
                if (TouchPadKey.timecount >= longPressTime && !TouchPadKey.longPressedClock)
                {
                    TouchPadKey.longPressed = true;
                    TouchPadKey.longPressedClock = true;
                    longPressclock = true;
                }
                else
                {
                    TouchPadKey.longPressed = false;
                }
            }
            else
            {
                TouchPadKey.longPressedClock = false;
                TouchPadKey.timecount = 0;
                TouchPadKey.longPressed = false;
            }
            if (VolumeUpKey.state)
            {
                VolumeUpKey.timecount += Time.deltaTime;
                if (VolumeUpKey.timecount >= longPressTime && !VolumeUpKey.longPressedClock)
                {
                    VolumeUpKey.longPressed = true;
                    VolumeUpKey.longPressedClock = true;
                    longPressclock = true;
                }
                else
                {
                    VolumeUpKey.longPressed = false;
                }
            }
            else
            {
                VolumeUpKey.longPressedClock = false;
                VolumeUpKey.timecount = 0;
                VolumeUpKey.longPressed = false;
            }
            if (VolumeDownKey.state)
            {
                VolumeDownKey.timecount += Time.deltaTime;
                if (VolumeDownKey.timecount >= longPressTime && !VolumeDownKey.longPressedClock)
                {
                    VolumeDownKey.longPressed = true;
                    VolumeDownKey.longPressedClock = true;
                    longPressclock = true;
                }
                else
                {
                    VolumeDownKey.longPressed = false;
                }
            }
            else
            {
                VolumeDownKey.longPressedClock = false;
                VolumeDownKey.timecount = 0;
                VolumeDownKey.longPressed = false;
            }

        }
        #endregion
#endif
        #endregion

        if (Controller.UPvr_GetKeyLongPressed(Pvr_KeyCode.HOME))
        {
            Pvr_UnitySDKManager.pvr_UnitySDKSensor.ResetUnitySDKSensor();
            ResetController();
        }
    }

    void OnDestroy()
    {
        ControllerStatusChangeEvent -= ControllerStatusChangeEventDebug;
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


    public Vector3 GetAngularVelocity()
    {
        if (controllerlink != null)
        {
            return controllerlink.GetAngularVelocity();
        }
        return new Vector3(0.0f, 0.0f, 0.0f);
    }

    public Vector3 GetAcceleration()
    {
        if (controllerlink != null)
        {
            return controllerlink.GetAcceleration();
        }
        return new Vector3(0.0f, 0.0f, 0.0f);
    }

    public void StartLark2Service()
    {
        if (controllerlink != null)
        {
            controllerlink.StartLark2Service();
        }
    }
    public void BindHBService()
    {
        if (controllerlink != null)
        {
            controllerlink.BindHBService();
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
    public void ResetController()
    {
        if (controllerlink != null)
        {
            controllerlink.ResetController();
        }
    }
    public static int GetHBConnectionState()
    {
        int sta;
        sta = controllerlink.GetHBConnectionState();
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
        string type;
        type = controllerlink.GetBLEImageType();
        return type;
    }
    public static long GetBLEVersion()
    {
        long version;
        version = controllerlink.GetBLEVersion();
        return version;
    }
    public static string GetFileImageType()
    {
        string type;
        type = controllerlink.GetFileImageType();
        return type;
    }
    public static long GetFileVersion()
    {
        long version;
        version = controllerlink.GetFileVersion();
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
        controllerlink.hummingBirdMac = mac.Substring(0, 17);
        controllerlink.hummingBirdRSSI = Convert.ToInt16(mac.Remove(0, 18));
    }
    public int GetControllerRSSI()
    {
        return controllerlink.hummingBirdRSSI;
    }

    public void setHbServiceBindState(string state)
    {
        controllerServicestate = true;
        //state：0-已解绑，1-已绑定，2-未知
        if (Convert.ToInt16(state) == 0)
        {
            Invoke("BindHBService", 0.5f);
        }
        else if (Convert.ToInt16(state) == 1)
        {

        }
    }

    public void setHbControllerConnectState(string isconnect)
    {
        if (ControllerStatusChangeEvent !=null)
        {
            ControllerStatusChangeEvent(isconnect);
        }
        controllerlink.controllerState = Convert.ToInt16(isconnect);
        //state：0-断开，1-已连接，2-未知
        stopConnect = false;
        if (int.Parse(isconnect) == 1)
        {
            controllerlink.isConnect = true;
        }
        if (int.Parse(isconnect) == 0)
        {
            controllerlink.isConnect = false;
            ResetAllKeyState();
        }
    }
    public void ControllerStatusChangeEventDebug (string isconnect)
    {
        Debug.Log("Waring : Controller Status Changed + " + isconnect);
    }

    public void setupdateFailed()
    {
        //回调方法
    }

    public void setupdateSuccess()
    {
        //回调方法
    }

    public void setupdateProgress(string progress)
    {
        //回调方法
        //升级进度 0-100 
    }

    public void setHbAutoConnectState(string state)
    {
        //UNKNOW = -1; //默认值
        //NO_DEVICE = 0;//没有扫描到HB手柄
        //ONLY_ONE = 1;//只扫描到一只HB手柄
        //MORE_THAN_ONE = 2;// 扫描到多只HB手柄
        //LAST_CONNECTED = 3;//扫描到上一次连接过的HB手柄
        //FACTORY_DEFAULT = 4;//扫描到工厂绑定的HB手柄（暂未启用）
        controllerServicestate = true;
        if (Convert.ToInt16(state) == 0)
        {
            if (controllerlink.GetHBConnectionState() == 0)
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
        //state = 0,非手机平台，服务没有启动
        //state = 1,手机平台，服务没有启动，但是系统会主动启动服务
        //state = 2,手机平台，服务apk没有安装，需要安装
        controllerServicestate = true;
        if (Convert.ToInt16(state) == 0)
        {
            ShowToast(0);
        }
        if (Convert.ToInt16(state) == 1)
        {
            BindHBService();
        }
        if (Convert.ToInt16(state) == 2)
        {
            ShowToast(1);
        }
    }

    private void ShowToast(int type)
    {
        switch (type)
        {
            case 0: //非手机平台，手柄服务没有启动
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
            case 1: //手机平台，服务apk没安装，提示安装
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
            case 2: //没有扫描到手柄
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
            case 3: //扫描到多个手柄
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
            case 4: //服务没有启动
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
    private void ResetAllKeyState()
    {
        HomeKey.longPressed = false;
        HomeKey.state = false;
        HomeKey.pressedDown = false;
        HomeKey.pressedUp = false;
        APPKey.longPressed = false;
        APPKey.state = false;
        APPKey.pressedDown = false;
        APPKey.pressedUp = false;
        TouchPadKey.longPressed = false;
        TouchPadKey.state = false;
        TouchPadKey.pressedDown = false;
        TouchPadKey.pressedUp = false;
        VolumeDownKey.longPressed = false;
        VolumeDownKey.state = false;
        VolumeDownKey.pressedDown = false;
        VolumeDownKey.pressedUp = false;
        VolumeUpKey.longPressed = false;
        VolumeUpKey.state = false;
        VolumeUpKey.pressedDown = false;
        VolumeUpKey.pressedUp = false;
    }
    public void EnableKeyEvent(bool state)
    {
        enableKeyEvent = state;
        ResetAllKeyState();
    }
    #endregion

}
