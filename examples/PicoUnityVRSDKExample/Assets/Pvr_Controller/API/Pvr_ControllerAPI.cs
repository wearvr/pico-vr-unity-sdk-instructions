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
using System.Runtime.InteropServices;


namespace Pvr_UnitySDKAPI
{
    public enum ControllerState
    {
        ERROR = -1,
        Connected = 0,
        Connecting = 1,
        DisConnected = 2,
    }
    
    /// <summary>
    /// 手柄按键值
    /// </summary>
    public enum Pvr_KeyCode
    {
        APP = 1,
        TOUCHPAD = 2,
        HOME = 3,
        VOLUMEUP = 4,
        VOLUMEDOWN = 5,
    }
    /// <summary>
    /// 手柄Touchpad滑动方向
    /// </summary>
    public enum Pvr_SlipDirection
    {
        SlideUp = 1,
        SlideDown = 2,
        SlideRight = 3,
        SlideLeft = 4,
    }
    /// <summary>
    /// 手柄触摸板的XY坐标，范围为0-255
    /// </summary>
    public struct TouchPadPosition
    {
        public static int x;  
        public static int y;

    }
    public struct APPKey
    {
        public static bool state;
        public static bool pressedDown;
        public static bool pressedUp;
        public static bool longPressed;
        public static float timecount;
        public static bool longPressedClock;
    }
    public struct HomeKey
    {
        public static bool state;
        public static bool pressedDown;
        public static bool pressedUp;
        public static bool longPressed;
        public static bool longPressedClock;
        public static float timecount;
    }
    public struct TouchPadKey
    {
        public static bool state;
        public static bool pressedDown;
        public static bool pressedUp;
        public static bool longPressed;
        public static float timecount;
        public static bool slideup;
        public static bool slidedown;
        public static bool slideright;
        public static bool slideleft;
        public static bool longPressedClock;
    }
    public struct VolumeUpKey
    {
        public static bool state;
        public static bool pressedDown;
        public static bool pressedUp;
        public static bool longPressed;
        public static float timecount;
        public static bool longPressedClock;
    }
    public struct VolumeDownKey
    {
        public static bool state;
        public static bool pressedDown;
        public static bool pressedUp;
        public static bool longPressed;
        public static float timecount;
        public static bool longPressedClock;
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

        public static Quaternion ControllerQua = new Quaternion(0, 0, 0, 1);

        public static int BatteryLevel;

        /// <summary>
        /// 获取Touchpad的触摸值，传0，返回X，传1返回Y
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        public static int UPvr_GetTouchPadPosition(int tp)
        {
            if(tp == 0)
            {
                return TouchPadPosition.x;
            }
            else if(tp == 1)
            {
                return TouchPadPosition.y;
            }
            else
            {
                return 0;
            }
            
        }

        public static ControllerState UPvr_GetControllerState()
        {
            if (Pvr_ControllerManager.GetHBConnectionState() == 0)
            {
                return ControllerState.DisConnected;
            }
            else if (Pvr_ControllerManager.GetHBConnectionState() == 1)
            {
                return ControllerState.Connecting;
            }
            else if (Pvr_ControllerManager.GetHBConnectionState() == 2)
            {
                return ControllerState.Connected;
            }
            return ControllerState.ERROR;
        }

        /// <summary>
        /// 获取手柄转动四元数
        /// </summary>
        public static Quaternion UPvr_GetControllerQUA()
        {
            return ControllerQua;
        }

        /// <summary>
        /// 获取手柄的电量，目前为1234，四档
        /// </summary>
        public static int UPvr_GetControllerPower()
        {
            return BatteryLevel;
        }
        public static bool UPvr_GetSlipDirection(Pvr_SlipDirection dir)
        {
            switch(dir)
            {
                case Pvr_SlipDirection.SlideUp:
                    return TouchPadKey.slideup;
                case Pvr_SlipDirection.SlideDown:
                    return TouchPadKey.slidedown;
                case Pvr_SlipDirection.SlideRight:
                    return TouchPadKey.slideright;
                case Pvr_SlipDirection.SlideLeft:
                    return TouchPadKey.slideleft;

            }
            return false;
        }

        /// <summary>
        /// 获取Key的状态
        /// </summary>
        public static bool UPvr_GetKey(Pvr_KeyCode key)
        {
            switch (key)
            {
                case Pvr_KeyCode.APP:
                    return APPKey.state;
                case Pvr_KeyCode.HOME:
                    return HomeKey.state;
                case Pvr_KeyCode.TOUCHPAD:
                    return TouchPadKey.state;
                case Pvr_KeyCode.VOLUMEUP:
                    return VolumeUpKey.state;
                case Pvr_KeyCode.VOLUMEDOWN:
                    return VolumeDownKey.state;
            }
            return false;
        }

        /// <summary>
        /// 获取Key的状态，仅当按下时为true，一次性事件
        /// </summary>
        public static bool UPvr_GetKeyDown(Pvr_KeyCode key)
        {
            switch (key)
            {
                case Pvr_KeyCode.APP:
                    return APPKey.pressedDown;
                case Pvr_KeyCode.HOME:
                    return HomeKey.pressedDown;
                case Pvr_KeyCode.TOUCHPAD:
                    return TouchPadKey.pressedDown;
                case Pvr_KeyCode.VOLUMEUP:
                    return VolumeUpKey.pressedDown;
                case Pvr_KeyCode.VOLUMEDOWN:
                    return VolumeDownKey.pressedDown;
            }
            return false;
        }

        /// <summary>
        /// 获取Key的状态，仅当抬起时为true，一次性事件
        /// </summary>
        public static bool UPvr_GetKeyUp(Pvr_KeyCode key)
        {
            switch (key)
            {
                case Pvr_KeyCode.APP:
                    return APPKey.pressedUp;
                case Pvr_KeyCode.HOME:
                    return HomeKey.pressedUp;
                case Pvr_KeyCode.TOUCHPAD:
                    return TouchPadKey.pressedUp;
                case Pvr_KeyCode.VOLUMEUP:
                    return VolumeUpKey.pressedUp;
                case Pvr_KeyCode.VOLUMEDOWN:
                    return VolumeDownKey.pressedUp;
            }
            return false;
        }

        /// <summary>
        /// 获取Key的状态，仅当长按2s时为true，一次性事件
        /// </summary>
        public static bool UPvr_GetKeyLongPressed(Pvr_KeyCode key)
        {
            switch (key)
            {
                case Pvr_KeyCode.APP:
                    return APPKey.longPressed;
                case Pvr_KeyCode.HOME:
                    return HomeKey.longPressed;
                case Pvr_KeyCode.TOUCHPAD:
                    return TouchPadKey.longPressed;
                case Pvr_KeyCode.VOLUMEUP:
                    return VolumeUpKey.longPressed;
                case Pvr_KeyCode.VOLUMEDOWN:
                    return VolumeDownKey.longPressed;
            }
            return false;
        }

        public static bool UPvr_IsTouching()
        {
            return (TouchPadPosition.x != 0 || TouchPadPosition.y != 0) ? true : false;
        }    

        public static Vector3 Upvr_GetAngularVelocity()
        {
            Vector3 Aglr = new Vector3(0.0f, 0.0f, 0.0f);
#if ANDROID_DEVICE
            Aglr = Pvr_ControllerManager.Instance.GetAngularVelocity();
#elif IOS_DEVICE
            float[] Angulae = new float[3] { 0, 0, 0 };
            getHbAngularVelocity(Angulae);
            Aglr = new Vector3(Angulae[0], Angulae[1], Angulae[2]);
#endif
            return Aglr;
        }       

        public static Vector3 Upvr_GetAcceleration()
        {
            Vector3 Acc = new Vector3(0.0f, 0.0f, 0.0f);
#if ANDROID_DEVICE
            Acc = Pvr_ControllerManager.Instance.GetAcceleration();
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
