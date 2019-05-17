///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Controller
// Author: Yangel.Yan
// Date:  2017/01/11
// Discription: The Controller API 
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
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;


namespace Pvr_UnitySDKAPI
{

    public class PvrControllerKey
    {
        public bool State;
        public bool PressedDown;
        public bool PressedUp;
        public bool LongPressed;
        public float TimeCount;
        public bool LongPressedClock;
        public PvrControllerKey()
        {
            State = false;
            PressedDown = false;
            PressedUp = false;
            LongPressed = false;
            TimeCount = 0;
            LongPressedClock = false;
        }
    }

    public class ControllerHand
    {
        public PvrControllerKey AppKey;
        public PvrControllerKey TouchKey;
        public PvrControllerKey HomeKey;
        public PvrControllerKey VolumeDownKey;
        public PvrControllerKey VolumeUpKey;
        public PvrControllerKey TriggerKey;
        public Vector2 TouchPadPosition;
        public int TriggerNum;
        public Quaternion Rotation;
        public Vector3 Position;
        public int Battery;
        public Vector2 touchDownPosition; 
        public Vector2 touchUpPosition;
        public Vector2 swipeData;
        public bool isVertical;
        public bool isHorizontal;
        public bool touchClock;
        public bool triggerClock;
        public ControllerState ConnectState;
        public SwipeDirection SwipeDirection;
        public TouchPadClick TouchPadClick;

        public ControllerHand()
        {
            AppKey = new PvrControllerKey();
            TouchKey = new PvrControllerKey();
            HomeKey = new PvrControllerKey();
            VolumeDownKey = new PvrControllerKey();
            VolumeUpKey = new PvrControllerKey();
            TriggerKey = new PvrControllerKey();
            TouchPadPosition = new Vector2();
            Rotation = new Quaternion();
            Position = new Vector3();
            touchDownPosition = new Vector2();
            touchUpPosition = new Vector2();
            swipeData = new Vector2();
            isVertical = false;
            isHorizontal = false;
            touchClock = false;
            triggerClock = false;
            Battery = 0;
            TriggerNum = 0;
            ConnectState = ControllerState.Error;
            SwipeDirection = SwipeDirection.No;
            TouchPadClick = TouchPadClick.No;
        }
    }

    public enum ControllerState
    {
        Error = -1,
        DisConnected = 0,
        Connected = 1,
    }
    /// <summary>
    /// controller key value
    /// </summary>
    public enum Pvr_KeyCode
    {
        APP = 1,
        TOUCHPAD = 2,
        HOME = 3,
        VOLUMEUP = 4,
        VOLUMEDOWN = 5,
        TRIGGER = 6,
    }
    /// <summary>
    /// The controller Touchpad slides in the direction.
    /// </summary>
    public enum SwipeDirection
    {
        No =0 ,
        SwipeUp = 1,
        SwipeDown = 2,
        SwipeRight = 3,
        SwipeLeft = 4,
    }

    /// <summary>
    /// The controller Touchpad click the direction.
    /// </summary>
    public enum TouchPadClick
    {
        No = 0,
        ClickUp = 1,
        ClickDown = 2,
        ClickRight = 3,
        ClickLeft = 4,
    }

    public struct Controller
    {
        /**************************** Private Static Funcations *******************************************/
#region Private Static Funcation

#if ANDROID_DEVICE
        public const string LibFileName = "Pvr_UnitySDK";
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Pvr_CalcArmModelParameters(float[] headOrientation,float[] controllerOrientation,float[] gyro);
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Pvr_GetPointerPose( float[] rotation,  float[] position);
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Pvr_GetElbowPose( float[] rotation,  float[] position);
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Pvr_GetWristPose( float[] rotation,  float[] position);
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Pvr_GetShoulderPose( float[] rotation,  float[] position);
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Pvr_SetArmModelParameters(int hand, int gazeType, float elbowHeight, float elbowDepth, float pointerTiltAngle);
#endif
#endregion


        /**************************** Public Static Funcations *******************************************/
#region Public Static Funcation  

        
        public static Vector2 UPvr_GetTouchPadPosition(int hand)
        {
            switch (hand)
            {
                case 0:
                {
                    var postion = Pvr_ControllerManager.controllerlink.Controller0.TouchPadPosition;
                    return postion;
                }
                case 1:
                {
                    var postion = Pvr_ControllerManager.controllerlink.Controller1.TouchPadPosition;
                    return postion;
                }
            }
            return new Vector2(0, 0);
        }

        public static ControllerState UPvr_GetControllerState(int hand)
        {
            switch (hand)
            {
                case 0:
                    Pvr_ControllerManager.controllerlink.Controller0.ConnectState = Pvr_ControllerManager.GetControllerConnectionState(0) == 1 ? ControllerState.Connected : ControllerState.DisConnected;
                    return Pvr_ControllerManager.controllerlink.Controller0.ConnectState;
                case 1:
                    if (Pvr_ControllerManager.controllerlink.neoserviceStarted)
                    {
                        Pvr_ControllerManager.controllerlink.Controller1.ConnectState = Pvr_ControllerManager.GetControllerConnectionState(1) == 1 ? ControllerState.Connected : ControllerState.DisConnected;
                    }
                    return Pvr_ControllerManager.controllerlink.Controller1.ConnectState;
                 
            }
            return ControllerState.Error;
        }
        /// <summary>
        /// Get the controller rotation data.
        /// </summary>
        /// <param name="hand">0,1</param>
        /// <returns></returns>
        public static Quaternion UPvr_GetControllerQUA(int hand)
        {
            switch (hand)
            {
                case 0:
                    return Pvr_ControllerManager.controllerlink.Controller0.Rotation;
                case 1:
                    return Pvr_ControllerManager.controllerlink.Controller1.Rotation;
            }
            return new Quaternion(0, 0, 0, 1);
        }
        /// <summary>
        /// Get the controller position data.
        /// </summary>
        /// <param name="hand">0,1</param>
        /// <returns></returns>
        public static Vector3 UPvr_GetControllerPOS(int hand)
        {
            switch (hand)
            {
                case 0:
                    return Pvr_ControllerManager.controllerlink.Controller0.Position;
                case 1:
                    return Pvr_ControllerManager.controllerlink.Controller1.Position;
            }
            return new Vector3(0, 0, 0);
        }
        /// <summary>
        /// Get the value of the trigger key 
        /// </summary>
        /// <param name="hand"></param>
        /// <returns>Neo:0-255,Goblin2:0/1</returns>
        public static int UPvr_GetControllerTriggerValue(int hand)
        {
            switch (hand)
            {
                case 0:
                    return Pvr_ControllerManager.controllerlink.Controller0.TriggerNum;
                case 1:
                    return Pvr_ControllerManager.controllerlink.Controller1.TriggerNum;
            }
            return 0;
        }
        /// <summary>
        /// Get the power of the controller, neo power is 1-10, goblin/goblin2 power is 1-4.
        /// </summary>
        public static int UPvr_GetControllerPower(int hand)
        {
            switch (hand)
            {
                case 0:
                    return Pvr_ControllerManager.controllerlink.Controller0.Battery;
                case 1:
                    return Pvr_ControllerManager.controllerlink.Controller1.Battery;
            }
            return 0;
        }
        /// <summary>
        /// Get the sliding direction of the touchpad.
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public static SwipeDirection UPvr_GetSwipeDirection(int hand)
        {
            switch (hand)
            {
                case 0:
                    return Pvr_ControllerManager.controllerlink.Controller0.SwipeDirection;
                case 1:
                    return Pvr_ControllerManager.controllerlink.Controller1.SwipeDirection;
            }
            return SwipeDirection.No;
        }
        /// <summary>
        /// Get the click direction of the touchpad.
        /// </summary>
        /// <param name="hand"></param>
        /// <returns></returns>
        public static TouchPadClick UPvr_GetTouchPadClick(int hand)
        {
            switch (hand)
            {
                case 0:
                    return Pvr_ControllerManager.controllerlink.Controller0.TouchPadClick;
                case 1:
                    return Pvr_ControllerManager.controllerlink.Controller1.TouchPadClick;
            }
            return TouchPadClick.No;
        }

        /// <summary>
        /// Get the key state
        /// </summary>
        /// <param name="hand">0,1</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool UPvr_GetKey(int hand, Pvr_KeyCode key)
        {
            if (hand == 0)
            {
                switch (key)
                {
                    case Pvr_KeyCode.APP:
                        return Pvr_ControllerManager.controllerlink.Controller0.AppKey.State;
                    case Pvr_KeyCode.HOME:
                        return Pvr_ControllerManager.controllerlink.Controller0.HomeKey.State;
                    case Pvr_KeyCode.TOUCHPAD:
                        return Pvr_ControllerManager.controllerlink.Controller0.TouchKey.State;
                    case Pvr_KeyCode.VOLUMEUP:
                        return Pvr_ControllerManager.controllerlink.Controller0.VolumeUpKey.State;
                    case Pvr_KeyCode.VOLUMEDOWN:
                        return Pvr_ControllerManager.controllerlink.Controller0.VolumeDownKey.State;
                    case Pvr_KeyCode.TRIGGER:
                        return Pvr_ControllerManager.controllerlink.Controller0.TriggerKey.State;
                    default:
                        return false;
                }
            }
            if (hand == 1)
            {
                switch (key)
                {
                    case Pvr_KeyCode.APP:
                        return Pvr_ControllerManager.controllerlink.Controller1.AppKey.State;
                    case Pvr_KeyCode.HOME:
                        return Pvr_ControllerManager.controllerlink.Controller1.HomeKey.State;
                    case Pvr_KeyCode.TOUCHPAD:
                        return Pvr_ControllerManager.controllerlink.Controller1.TouchKey.State;
                    case Pvr_KeyCode.VOLUMEUP:
                        return Pvr_ControllerManager.controllerlink.Controller1.VolumeUpKey.State;
                    case Pvr_KeyCode.VOLUMEDOWN:
                        return Pvr_ControllerManager.controllerlink.Controller1.VolumeDownKey.State;
                    case Pvr_KeyCode.TRIGGER:
                        return Pvr_ControllerManager.controllerlink.Controller1.TriggerKey.State;
                    default:
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Get the pressed state of the key
        /// </summary>
        /// <param name="hand">0,1</param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool UPvr_GetKeyDown(int hand, Pvr_KeyCode key)
        {
            if (hand == 0)
            {
                switch (key)
                {
                    case Pvr_KeyCode.APP:
                        return Pvr_ControllerManager.controllerlink.Controller0.AppKey.PressedDown;
                    case Pvr_KeyCode.HOME:
                        return Pvr_ControllerManager.controllerlink.Controller0.HomeKey.PressedDown;
                    case Pvr_KeyCode.TOUCHPAD:
                        return Pvr_ControllerManager.controllerlink.Controller0.TouchKey.PressedDown;
                    case Pvr_KeyCode.VOLUMEUP:
                        return Pvr_ControllerManager.controllerlink.Controller0.VolumeUpKey.PressedDown;
                    case Pvr_KeyCode.VOLUMEDOWN:
                        return Pvr_ControllerManager.controllerlink.Controller0.VolumeDownKey.PressedDown;
                    case Pvr_KeyCode.TRIGGER:
                        return Pvr_ControllerManager.controllerlink.Controller0.TriggerKey.PressedDown;
                    default:
                        return false;
                }
            }
            if(hand == 1)
            {
                switch (key)
                {
                    case Pvr_KeyCode.APP:
                        return Pvr_ControllerManager.controllerlink.Controller1.AppKey.PressedDown;
                    case Pvr_KeyCode.HOME:
                        return Pvr_ControllerManager.controllerlink.Controller1.HomeKey.PressedDown;
                    case Pvr_KeyCode.TOUCHPAD:
                        return Pvr_ControllerManager.controllerlink.Controller1.TouchKey.PressedDown;
                    case Pvr_KeyCode.VOLUMEUP:
                        return Pvr_ControllerManager.controllerlink.Controller1.VolumeUpKey.PressedDown;
                    case Pvr_KeyCode.VOLUMEDOWN:
                        return Pvr_ControllerManager.controllerlink.Controller1.VolumeDownKey.PressedDown;
                    case Pvr_KeyCode.TRIGGER:
                        return Pvr_ControllerManager.controllerlink.Controller1.TriggerKey.PressedDown;
                    default:
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the lift state of the key.
        /// </summary>
        /// <param name="hand"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool UPvr_GetKeyUp(int hand, Pvr_KeyCode key)
        {
            if (hand == 0)
            {
                switch (key)
                {
                    case Pvr_KeyCode.APP:
                        return Pvr_ControllerManager.controllerlink.Controller0.AppKey.PressedUp;
                    case Pvr_KeyCode.HOME:
                        return Pvr_ControllerManager.controllerlink.Controller0.HomeKey.PressedUp;
                    case Pvr_KeyCode.TOUCHPAD:
                        return Pvr_ControllerManager.controllerlink.Controller0.TouchKey.PressedUp;
                    case Pvr_KeyCode.VOLUMEUP:
                        return Pvr_ControllerManager.controllerlink.Controller0.VolumeUpKey.PressedUp;
                    case Pvr_KeyCode.VOLUMEDOWN:
                        return Pvr_ControllerManager.controllerlink.Controller0.VolumeDownKey.PressedUp;
                    case Pvr_KeyCode.TRIGGER:
                        return Pvr_ControllerManager.controllerlink.Controller0.TriggerKey.PressedUp;
                    default:
                        return false;
                }
            }
            if (hand == 1)
            {
                switch (key)
                {
                    case Pvr_KeyCode.APP:
                        return Pvr_ControllerManager.controllerlink.Controller1.AppKey.PressedUp;
                    case Pvr_KeyCode.HOME:
                        return Pvr_ControllerManager.controllerlink.Controller1.HomeKey.PressedUp;
                    case Pvr_KeyCode.TOUCHPAD:
                        return Pvr_ControllerManager.controllerlink.Controller1.TouchKey.PressedUp;
                    case Pvr_KeyCode.VOLUMEUP:
                        return Pvr_ControllerManager.controllerlink.Controller1.VolumeUpKey.PressedUp;
                    case Pvr_KeyCode.VOLUMEDOWN:
                        return Pvr_ControllerManager.controllerlink.Controller1.VolumeDownKey.PressedUp;
                    case Pvr_KeyCode.TRIGGER:
                        return Pvr_ControllerManager.controllerlink.Controller1.TriggerKey.PressedUp;
                    default:
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the long press state of the Key.
        /// </summary>
        public static bool UPvr_GetKeyLongPressed(int hand, Pvr_KeyCode key)
        {
            if (hand == 0)
            {
                switch (key)
                {
                    case Pvr_KeyCode.APP:
                        return Pvr_ControllerManager.controllerlink.Controller0.AppKey.LongPressed;
                    case Pvr_KeyCode.HOME:
                        return Pvr_ControllerManager.controllerlink.Controller0.HomeKey.LongPressed;
                    case Pvr_KeyCode.TOUCHPAD:
                        return Pvr_ControllerManager.controllerlink.Controller0.TouchKey.LongPressed;
                    case Pvr_KeyCode.VOLUMEUP:
                        return Pvr_ControllerManager.controllerlink.Controller0.VolumeUpKey.LongPressed;
                    case Pvr_KeyCode.VOLUMEDOWN:
                        return Pvr_ControllerManager.controllerlink.Controller0.VolumeDownKey.LongPressed;
                    case Pvr_KeyCode.TRIGGER:
                        return Pvr_ControllerManager.controllerlink.Controller0.TriggerKey.LongPressed;
                    default:
                        return false;
                }
            }
            if (hand == 1)
            {
                switch (key)
                {
                    case Pvr_KeyCode.APP:
                        return Pvr_ControllerManager.controllerlink.Controller1.AppKey.LongPressed;
                    case Pvr_KeyCode.HOME:
                        return Pvr_ControllerManager.controllerlink.Controller1.HomeKey.LongPressed;
                    case Pvr_KeyCode.TOUCHPAD:
                        return Pvr_ControllerManager.controllerlink.Controller1.TouchKey.LongPressed;
                    case Pvr_KeyCode.VOLUMEUP:
                        return Pvr_ControllerManager.controllerlink.Controller1.VolumeUpKey.LongPressed;
                    case Pvr_KeyCode.VOLUMEDOWN:
                        return Pvr_ControllerManager.controllerlink.Controller1.VolumeDownKey.LongPressed;
                    case Pvr_KeyCode.TRIGGER:
                        return Pvr_ControllerManager.controllerlink.Controller1.TriggerKey.LongPressed;
                    default:
                        return false;
                }
            }
            return false;
        }

        public static bool UPvr_IsTouching(int hand)
        {
            const float tolerance = 0;
            switch (hand)
            {
                case 0:
                {
                    return Math.Abs(Pvr_ControllerManager.controllerlink.Controller0.TouchPadPosition.x) > tolerance ||
                           Math.Abs(Pvr_ControllerManager.controllerlink.Controller0.TouchPadPosition.y) > tolerance;
                }
                case 1:
                {
                    return Math.Abs(Pvr_ControllerManager.controllerlink.Controller1.TouchPadPosition.x) > tolerance ||
                           Math.Abs(Pvr_ControllerManager.controllerlink.Controller1.TouchPadPosition.y) > tolerance;
                }
            }
            return false;
        }
        /// <summary>
        /// The service type that currently needs bind.
        /// </summary>
        /// <returns>1：Goblin service 2:Neo service </returns>
        public static int UPvr_GetPreferenceDevice()
        {
            var trackingmode = Pvr_ControllerManager.controllerlink.trackingmode;
            var systemproc = Pvr_ControllerManager.controllerlink.systemProp;
            if (trackingmode == 0 || trackingmode == 1)
            {
                return 1;
            }
            if (trackingmode == 2)
            {
                return 2;
            }
            if (trackingmode == 3)
            {
                if (systemproc == 0 || systemproc == 1)
                {
                    return 1;
                }
                if (systemproc == 2)
                {
                    return 2;
                }
                if (systemproc == 3)
                {
                    return 1;
                }
            }
            return 1;
        }

        public static bool UPvr_IsEnbleTrigger()
        {
            return Pvr_ControllerManager.controllerlink.IsEnbleTrigger();
        }
        /// <summary>
        ///Gets the controller type of the current connection.
        /// </summary>
        /// <returns>0: no connection 1：goblin1 2:Neo 3:goblin2 </returns>
        public static int UPvr_GetDeviceType()
        {
            return Pvr_ControllerManager.controllerlink.GetDeviceType();
        }
        /// <summary>
        /// Gets the current master hand for which 0/1.
        /// </summary>
        /// <returns></returns>
        public static int UPvr_GetMainHandNess()
        {
            return Pvr_ControllerManager.controllerlink.GetMainControllerIndex();
        }
        /// <summary>
        /// Set the current controller as the master controller.
        /// </summary>
        public static void UPvr_SetMainHandNess(int hand)
        {
            Pvr_ControllerManager.controllerlink.SetMainController(hand);
        }
        /// <summary>
        /// Ability to obtain the current controller (3dof/6dof)
        /// </summary>
        /// <param name="hand">0/1</param>
        /// <returns>-1:error 0：6dof  1：3dof 2:6dof </returns>
        public static int UPvr_GetControllerAbility(int hand)
        {
            return Pvr_ControllerManager.controllerlink.GetControllerAbility(hand);
        }
        //get controller version
        public static string UPvr_GetControllerVersion()
        {
            return Pvr_ControllerManager.controllerlink.GetControllerVersion();
        }
        //Get version number deviceType: 0-station 1- controller 0 2- controller 1.
        public static void UPvr_GetDeviceVersion(int deviceType)
        {
            Pvr_ControllerManager.controllerlink.GetDeviceVersion(deviceType);
        }
        //Get the controller Sn number controllerSerialNum: 0- controller 0 1- controller 1.
        public static void UPvr_GetControllerSnCode(int controllerSerialNum)
        {
            Pvr_ControllerManager.controllerlink.GetControllerSnCode(controllerSerialNum);
        }
        //Unlash the controller controllerSerialNum: 0- controller 0 1- controller 1.
        public static void UPvr_SetControllerUnbind(int controllerSerialNum)
        {
            Pvr_ControllerManager.controllerlink.SetControllerUnbind(controllerSerialNum);
        }
        //Restart the station
        public static void UPvr_SetStationRestart()
        {
            Pvr_ControllerManager.controllerlink.SetStationRestart();
        }
        //Launch station OTA upgrade.
        public static void UPvr_StartStationOtaUpdate()
        {
            Pvr_ControllerManager.controllerlink.StartStationOtaUpdate();
        }
        //Launch controller ota upgrade mode: 1-rf upgrade communication module 2- upgrade STM32 module;ControllerSerialNum: 0- controller 0 1- controller 1.
        public static void UPvr_StartControllerOtaUpdate(int mode, int controllerSerialNum)
        {
            Pvr_ControllerManager.controllerlink.StartControllerOtaUpdate(mode, controllerSerialNum);
        }
        //Enter the pairing mode controllerSerialNum: 0- controller 0 1- controller 1.
        public static void UPvr_EnterPairMode(int controllerSerialNum)
        {
            Pvr_ControllerManager.controllerlink.EnterPairMode(controllerSerialNum);
        }
        //controller shutdown controllerSerialNum: 0- controller 0 1- controller 1.
        public static void UPvr_SetControllerShutdown(int controllerSerialNum)
        {
            Pvr_ControllerManager.controllerlink.SetControllerShutdown(controllerSerialNum);
        }
        // Retrieves the pairing status of the current station with 0- unpaired state 1- pairing.
        public static int UPvr_GetStationPairState()
        {
            return Pvr_ControllerManager.controllerlink.GetStationPairState();
        }
        //Get the upgrade of station ota.
        public static int UPvr_GetStationOtaUpdateProgress()
        {
            return Pvr_ControllerManager.controllerlink.GetStationOtaUpdateProgress();
        }
        //Get the Controller ota upgrade progress.
        //Normal 0-100
        //Exception 101: failed to receive a successful upgrade of id 102: the controller did not enter the upgrade status 103: upgrade interrupt exception.
        public static int UPvr_GetControllerOtaUpdateProgress()
        {
            return Pvr_ControllerManager.controllerlink.GetControllerOtaUpdateProgress();
        }
        //Also get the controller version number and SN number controllerSerialNum: 0- controller 0 1- controller 1.
        public static void UPvr_GetControllerVersionAndSN(int controllerSerialNum)
        {
            Pvr_ControllerManager.controllerlink.GetControllerVersionAndSN(controllerSerialNum);
        }
        //Gets the unique identifier of the controller.
        public static void UPvr_GetControllerUniqueID()
        {
            Pvr_ControllerManager.controllerlink.GetControllerUniqueID();
        }
        //Disconnect the station from the current pairing mode.
        public void UPvr_InterruptStationPairMode()
        {
            Pvr_ControllerManager.controllerlink.InterruptStationPairMode();
        }
        
        // <summary>
        // Obtain the controller's gyroscope data.
        // </summary>
        public static Vector3 UPvr_GetAngularVelocity(int num)
        {
            Vector3 Aglr = new Vector3(0.0f, 0.0f, 0.0f);
#if ANDROID_DEVICE
            Aglr = Pvr_ControllerManager.Instance.GetAngularVelocity(num);
#elif IOS_DEVICE
            float[] Angulae = new float[3] { 0, 0, 0 };
            getHbAngularVelocity(Angulae);
            Aglr = new Vector3(Angulae[0], Angulae[1], Angulae[2]);
#endif
            return Aglr;
        }       

        public static Vector3 UPvr_GetAcceleration(int num)
        {
            Vector3 Acc = new Vector3(0.0f, 0.0f, 0.0f);
#if ANDROID_DEVICE
            Acc = Pvr_ControllerManager.Instance.GetAcceleration(num);
#elif IOS_DEVICE
            float[] Accel = new float[3] { 0, 0, 0 };
            getHbAcceleration(Accel);
            Acc = new Vector3(Accel[0], Accel[1], Accel[2]);
#endif
            return Acc;
        }
        public static void UPvr_SetArmModelParameters(int hand, int gazeType, float elbowHeight, float elbowDepth, float pointerTiltAngle)
        {
#if ANDROID_DEVICE || IOS_DEVICE
            Pvr_SetArmModelParameters( hand,  gazeType,  elbowHeight,  elbowDepth,  pointerTiltAngle);
#endif
        }

        public static void UPvr_CalcArmModelParameters(float[] headOrientation, float[] controllerOrientation, float[] controllerPrimary)
        {
#if ANDROID_DEVICE || IOS_DEVICE
            Pvr_CalcArmModelParameters( headOrientation,  controllerOrientation, controllerPrimary);
#endif
        }
        public static void UPvr_GetPointerPose( float[] rotation,  float[] position)
        {
#if ANDROID_DEVICE || IOS_DEVICE
            Pvr_GetPointerPose(  rotation,  position);
#endif
        }
        public static void UPvr_GetElbowPose( float[] rotation,  float[] position)
        {
#if ANDROID_DEVICE || IOS_DEVICE
            Pvr_GetElbowPose(  rotation,   position);
#endif
        }
        public static void UPvr_GetWristPose( float[] rotation,  float[] position)
        {
#if ANDROID_DEVICE || IOS_DEVICE
            Pvr_GetWristPose(  rotation,  position);
#endif
        }
        public static void UPvr_GetShoulderPose( float[] rotation,  float[] position)
        {
#if ANDROID_DEVICE || IOS_DEVICE
            Pvr_GetShoulderPose(  rotation,   position);
#endif
        }

#if IOS_DEVICE
        [DllImport("__Internal")]
        private static extern void Pvr_SetArmModelParameters(int hand, int gazeType, float elbowHeight, float elbowDepth, float pointerTiltAngle);
        [DllImport("__Internal")]
        private static extern void Pvr_CalcArmModelParameters(float[] headOrientation, float[] controllerOrientation, float[] gyro);
        [DllImport("__Internal")]
        private static extern void Pvr_GetPointerPose(float[] rotation, float[] position);
        [DllImport("__Internal")]
        private static extern void Pvr_GetElbowPose(float[] rotation, float[] position);
        [DllImport("__Internal")]
        private static extern void Pvr_GetWristPose(float[] rotation, float[] position);
        [DllImport("__Internal")]
        private static extern void Pvr_GetShoulderPose(float[] rotation, float[] position);
        [DllImport("__Internal")]
	    public static extern void PVR_GetLark2SensorMessage (ref float x, ref float y, ref float z, ref float w);
        [DllImport("__Internal")]
        public static extern void PVR_GetLark2KeyValueMessage(ref int touchpadx, ref int touchpady, ref int home, ref int app, ref int click, ref int volup, ref int voldown, ref int power);
        [DllImport("__Internal")]
        public static extern void getHbAngularVelocity(float[] gyro);
        [DllImport("__Internal")]
        public static extern void getHbAcceleration(float[] acce);
        [DllImport("__Internal")]
        public static extern int Pvr_ResetSensor(int index); //reset sensor index = 3
#endif
#endregion
    }

}
