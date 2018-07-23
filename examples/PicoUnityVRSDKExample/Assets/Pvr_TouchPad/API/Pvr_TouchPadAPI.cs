///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_HapticsAPI
// Author: AiLi.Shang
// Date:  2017/03/22
// Discription: The Haptics API 
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
    [StructLayout(LayoutKind.Sequential)]
    public struct TouchPad
    {
#if IOS_DEVICE
        [DllImport("__Internal")]
        private static extern int PVR_OpenBLECentral();
        [DllImport("__Internal")]
        public static extern int PVR_ConnectBLEDevice(string mac);
        [DllImport("__Internal")]
        public static extern int PVR_ScanBLEPeripheral(int type); //type 0:unknown,1:pico 1s 2:pico u 3:all
#endif
        public static EventHandler FindBledeEvent;

        /**************************** Private Static Funcations *******************************************/
#region Private Static Funcation
        private static void startBLEConnectService(string name)
        {
#if ANDROID_DEVICE
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_TouchPad.javaVrActivityClass, "Pvr_StartLarkConnectService", Pvr_TouchPad.activity, name);
#elif IOS_DEVICE
            PVR_OpenBLECentral();
#endif
        }

        private static void stopBLEConnectService()
        {
#if ANDROID_DEVICE
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_TouchPad.javaVrActivityClass, "Pvr_StopLarkConnectService", Pvr_TouchPad.activity);
#endif
        }

#endregion

        /**************************** Public Static Funcations *******************************************/
#region Public Static Funcation  
        //  public static void  UPvr_

        public static void UPvr_StartBLEConnectService(string name)
        {
            startBLEConnectService(name);
        }

        public static void UPvr_StopBLEConnectService()
        {
            stopBLEConnectService();
        }
        public static void ScanIOSBLEDevice(int type)
        {
#if IOS_DEVICE
            PVR_ScanBLEPeripheral(type);
#endif
        }
        public static void ConnectIOSBLEDevice(string mac)
        {
#if IOS_DEVICE
            PVR_ConnectBLEDevice(mac);
#endif
        }
#endregion

    }

}