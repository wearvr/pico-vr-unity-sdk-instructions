///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_UnitySDKAPI
// Author: AiLi.Shang
// Date:  2017/01/11
// Discription: The API Core funcation.Be fully careful of  Code modification
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

    public enum GlobalIntConfigs
    {

        EYE_TEXTURE_RESOLUTION0,
        EYE_TEXTURE_RESOLUTION1,
        SEENSOR_COUNT,
        ABILITY6DOF,
        PLATFORM_TYPE, //0 phone，1 Pico Neo，2 Goblin
        TRACKING_MODE, // 0-default 1-hb 2-cv 3-hb+cv
        LOG_LEVEL,
        ENBLE_HAND6DOF_BY_HEAD,
        ENBLE_6DOF_GLOBAL_TRACKING,
        TARGET_FRAME_RATE,
        iShowFPS,
        SensorMode,
        LOGICFLOW,// 0 ,1 Viewer	
        EYE_TEXTURE_RES_HIGH,    
        EYE_TEXTURE_RES_NORMAL,
        iCtrlModelLoadingPri
    };

    public enum GlobalFloatConfigs
    {
        IPD,
        FOV,
        NECK_MODEL_X,
        NECK_MODEL_Y,
        NECK_MODEL_Z,
        DISPLAY_REFRESH_RATE
    };
    public enum RenderTextureAntiAliasing
    {
        X_1 = 1,
        X_2 = 2,
        X_4 = 4,
        X_8 = 8,
    }
    public enum PlatForm
    {
        Android = 1,
        IOS = 2,
        Win = 3,
        Notsupport = 4,
    }


    public enum RenderTextureDepth
    {
        BD_0 = 0,
        BD_16 = 16,
        BD_24 = 24,
    }

    public enum RenderTextureLevel
    {
        Normal,
        High
    }
    public enum Sensorindex
    {
        Default = 0,
        FirstSensor = 1,
        SecondSensor = 2,
    }


    public enum Eye
    {
        LeftEye,
        RightEye
    }
    
    public enum HeadDofNum
    {
        ThreeDof,
        SixDof
    }

    public enum HandDofNum
    {
        ThreeDof,
        SixDof
    }

    public enum HandNum
    {
        One,
        Two
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct EyeSetting
    {
        public Transform eyelocalPosition;
        public Rect eyeRect;
        public float eyeFov;
        public float eyeAspect;
        public Matrix4x4 eyeProjectionMatrix;
        public Shader eyeShader;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Sensor
    {
        #region Android
#if ANDROID_DEVICE
        //---------------------------------------so------------------------------------------------
        public const string LibFileName = "Pvr_UnitySDK";      
              
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_Enable6DofModule(bool enable);
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_OptionalResetSensor(int index, int resetRot, int resetPos);
        //---------------------------------------so------------------------------------------------
#endif
        #endregion

        #region IOS
#if IOS_DEVICE
        //---------------------------------------so------------------------------------------------
		public const string LibFileName = "__Internal";
		//---------------------------------------so------------------------------------------------
#endif
        #endregion

        #region UNITY_EDITOR
#if UNITY_EDITOR
        public const string LibFileName = "Pvr_UnitySDK";
#endif
        #endregion

#if !UNITY_STANDALONE_WIN
        #region DllFuncation
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_Init(int index);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_StartSensor(int index);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_StopSensor(int index);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_ResetSensor(int index);
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_ResetSensorAll(int index);


        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_GetSensorState(int index, ref float x, ref float y, ref float z, ref float w, ref float px, ref float py, ref float pz);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_GetMainSensorState(ref float x, ref float y, ref float z, ref float w, ref float px, ref float py, ref float pz, ref float fov, ref int viewNumber);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_GetPsensorState();

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_GetSensorAcceleration(int index, ref float x, ref float y, ref float z);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_GetSensorGyroscope(int index, ref float x, ref float y, ref float z);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_GetSensorMagnet(int index, ref float x, ref float y, ref float z);
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_Get6DofSensorQualityStatus();
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool Pvr_Get6DofSafePanelFlag();
        #endregion
#endif
        #region Public Static Funcation

        public static bool UPvr_Pvr_Get6DofSafePanelFlag()
        {
#if ANDROID_DEVICE
            return Pvr_Get6DofSafePanelFlag();
#endif
            return false;

        }
        public static int UPvr_Init(int index)
        {
#if ANDROID_DEVICE
            return Pvr_Init(index);
#endif
            return 0;
        }
        public static void UPvr_InitPsensor()
        {
            Pvr_InitPsensor();
        }
        public static int UPvr_GetPsensorState()
        {
            int platformType = -1;
#if ANDROID_DEVICE
            int enumindex = (int)GlobalIntConfigs.PLATFORM_TYPE;
            Render.UPvr_GetIntConfig(enumindex, ref platformType);
#endif
            if (platformType == 1)
            {
#if UNITY_STANDALONE_WIN
                return 0;
#else
                return Pvr_GetPsensorState();
#endif
            }
            else
            {
                int state = Pvr_GetAndroidPsensorState();
                if (state != 0 && state != -1)
                {
                    state = 1;
                }
                return state;
            }

        }
        public static void UPvr_UnregisterPsensor()
        {
            Pvr_UnregisterPsensor();
        }
        public static int UPvr_StartSensor(int index)
        {
#if UNITY_STANDALONE_WIN
            return 0;
#else
            return Pvr_StartSensor(index);
#endif
        }
        public static int UPvr_StopSensor(int index)
        {
#if UNITY_STANDALONE_WIN
            return 0;
#else
            return Pvr_StopSensor(index);
#endif
        }
        public static int UPvr_ResetSensor(int index)
        {
            Pvr_UnitySDKManager.SDK.resetBasePos = new Vector3();
#if UNITY_STANDALONE_WIN
            return 0;
#else
            return Pvr_ResetSensor(index);
#endif
        }
        public static int UPvr_OptionalResetSensor(int index, int resetRot, int resetPos)
        {
#if ANDROID_DEVICE
            return Pvr_OptionalResetSensor(index, resetRot, resetPos);
#else
            return 0;
#endif
        }
        public static int UPvr_GetSensorState(int index, ref float x, ref float y, ref float z, ref float w, ref float px, ref float py, ref float pz)
        {
#if UNITY_STANDALONE_WIN
            return 0;
#else
            return Pvr_GetSensorState(index, ref x, ref y, ref z, ref w, ref px, ref py, ref pz);
#endif
        }
        public static int UPvr_GetMainSensorState(ref float x, ref float y, ref float z, ref float w, ref float px, ref float py, ref float pz, ref float fov, ref int viewNumber)
        {
#if UNITY_STANDALONE_WIN
            return 0;
#else
            return Pvr_GetMainSensorState(ref x, ref y, ref z, ref w, ref px, ref py, ref pz, ref fov, ref viewNumber);
#endif
        }

        public static int UPvr_GetSensorAcceleration(int index, ref float x, ref float y, ref float z)
        {
#if UNITY_STANDALONE_WIN
            return 0;
#else
            return Pvr_GetSensorAcceleration(index, ref x, ref y, ref z);
#endif
        }

        public static int UPvr_GetSensorGyroscope(int index, ref float x, ref float y, ref float z)
        {
#if UNITY_STANDALONE_WIN
            return 0;
#else
            return Pvr_GetSensorGyroscope(index, ref x, ref y, ref z);
#endif
        }

        public static int UPvr_GetSensorMagnet(int index, ref float x, ref float y, ref float z)
        {
#if UNITY_STANDALONE_WIN
            return 0;
#else
            return Pvr_GetSensorMagnet(index, ref x, ref y, ref z);
#endif
        }
        public static int UPvr_Get6DofSensorQualityStatus()
        {
#if UNITY_STANDALONE_WIN
            return 0;
#else
            return Pvr_Get6DofSensorQualityStatus();
#endif
        }
        public static int UPvr_Enable6DofModule(bool enable)
        {
#if ANDROID_DEVICE
            return    Pvr_Enable6DofModule(enable);
#endif
            return 0;
        }
        public static void Pvr_InitPsensor()
        {
#if ANDROID_DEVICE
            try
            {
                Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaSysActivityClass, "initPsensor",Pvr_UnitySDKManager.pvr_UnitySDKRender.activity);
            }
            catch (Exception e)
            {
                Debug.LogError(" Error :" + e.ToString());
            }
#endif
        }

        public static bool Pvr_IsHead6dofReset()
        {
            bool state = false;
#if ANDROID_DEVICE
            try
            {
                Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<bool>(ref state,Pvr_UnitySDKRender.javaSysActivityClass, "isHead6dofReset", Pvr_UnitySDKManager.pvr_UnitySDKRender.activity);
            }
            catch (Exception e)
            {
                Debug.LogError(" Error :" + e.ToString());
            }
#endif
            return state;
        }
        public static int Pvr_GetAndroidPsensorState()
        {
            int psensor = -1;
#if ANDROID_DEVICE
    
            try
            {
                Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<int>( ref psensor,Pvr_UnitySDKRender.javaSysActivityClass, "getPsensorState");
            }
            catch (Exception e)
            {
                Debug.LogError(" Error :" + e.ToString());
            }
#endif
            return psensor;
        }
        public static void Pvr_UnregisterPsensor()
        {
#if ANDROID_DEVICE
            try
            {
                Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaSysActivityClass, "unregisterListener");
            }
            catch (Exception e)
            {
                Debug.LogError(" Error :" + e.ToString());
            }
#endif
        }
        public static int UPvr_ResetSensorAll(int index)
        {
#if ANDROID_DEVICE               
                return Pvr_ResetSensorAll(index);   
#endif
            return 0;
        }  
        #endregion

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Render
    {
        #region Android
#if ANDROID_DEVICE
        public const string LibFileName = "Pvr_UnitySDK";
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
		private static extern void Pvr_ChangeScreenParameters(string model, int width, int height, double xppi, double yppi, double densityDpi );
		[DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
		private static extern int Pvr_SetRatio(float midH, float midV);

#endif
        #endregion

        #region IOS
#if IOS_DEVICE
		public const string LibFileName = "__Internal";
		[DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void UnityRenderEventIOS(int eventType,int eventData);

		[DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
		private static extern int Pvr_SetRatioIOS(float midH, float midV);

#endif
        #endregion

        #region UNITY_EDITOR
#if UNITY_EDITOR
        public const string LibFileName = "Pvr_UnitySDK";
#endif
        #endregion
#if !UNITY_STANDALONE_WIN
        #region DllFuncation
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_SetPupillaryPoint(bool enable);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Pvr_GetSupportHMDTypes();

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Pvr_SetCurrentHMDType([MarshalAs(UnmanagedType.LPStr)]string type);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_GetIntConfig(int configsenum, ref int res);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern int Pvr_GetFloatConfig(int configsenum, ref float res);

        #endregion
#endif

        #region Public Static Funcation
#if ANDROID_DEVICE
		public static void UPvr_ChangeScreenParameters(string model, int width, int height, double xppi, double yppi, double densityDpi)
		{
			Pvr_ChangeScreenParameters(model,  width,  height,  xppi,  yppi, densityDpi );
		}
#endif
        public static int UPvr_SetRatio(float midH, float midV)
        {
#if ANDROID_DEVICE
            return Pvr_SetRatio(midH, midV);
#endif
#if IOS_DEVICE
			return Pvr_SetRatioIOS(midH, midV);
#endif
            return 0;
        }
        public static int UPvr_GetIntConfig(int configsenum, ref int res)
        {
#if UNITY_STANDALONE_WIN
            return 0;
#else
            return Pvr_GetIntConfig(configsenum, ref res);
#endif
        }

        public static int UPvr_GetFloatConfig(int configsenum, ref float res)
        {
#if UNITY_STANDALONE_WIN
            return 0;
#else
            return Pvr_GetFloatConfig(configsenum, ref res);
#endif
        }
        public static string UPvr_GetSupportHMDTypes()
        {
#if !UNITY_STANDALONE_WIN
            IntPtr ptr = Pvr_GetSupportHMDTypes();
            if (ptr != IntPtr.Zero)
            {
                return Marshal.PtrToStringAnsi(ptr);
            }
#endif
            return null;

        }
        public static void UPvr_SetCurrentHMDType(string type)
        {
#if !UNITY_STANDALONE_WIN
            Pvr_SetCurrentHMDType(type);
#endif
        }
        #endregion

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct System
    {
        const string UnitySDKVersion = "2.7.6.1";

        #region Android

#if ANDROID_DEVICE
         //---------------------------------------so------------------------------------------------
        public const string LibFileName = "Pvr_UnitySDK";
		
		[DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void Pvr_SetInitActivity(IntPtr activity, IntPtr vrActivityClass);
       
        
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="jclass"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static bool UPvr_CallStaticMethod<T>(ref T result, UnityEngine.AndroidJavaClass jclass, string name, params object[] args)
        {
            try
            {
                result = jclass.CallStatic<T>(name, args);
                return true;
            }
            catch (AndroidJavaException e)
            {
                Debug.LogError("Exception calling static method " + name + ": " + e);
                return false;
            }
        }
        public  static bool UPvr_CallStaticMethod(UnityEngine.AndroidJavaObject jobj, string name, params object[] args)
        {
            try
            {
                jobj.CallStatic(name, args);
                return true;
            }
            catch (AndroidJavaException e)
            {
                Debug.LogError("CallStaticMethod  Exception calling activity method " + name + ": " + e);
                return false;
            }
        }

        
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="jobj"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public  static bool UPvr_CallMethod<T>(ref T result, UnityEngine.AndroidJavaObject jobj, string name, params object[] args)
        {
            try
            {
                result = jobj.Call<T>(name, args);
                return true;
            }
            catch (AndroidJavaException e)
            {
                Debug.LogError("Exception calling activity method " + name + ": " + e);
                return false;
            }
        }
       
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="jobj"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public  static bool UPvr_CallMethod(UnityEngine.AndroidJavaObject jobj, string name, params object[] args)
        {
            try
            {
                jobj.Call(name, args);
                return true;
            }
            catch (AndroidJavaException e)
            {
                Debug.LogError(" Exception calling activity method " + name + ": " + e);
                return false;
            }
        }  
#endif
        #endregion

        #region IOS
#if IOS_DEVICE
         //---------------------------------------so------------------------------------------------
		public const string LibFileName = "__Internal";
		
#endif
        #endregion

        #region UNITY_EDITOR
#if UNITY_EDITOR
        public const string LibFileName = "Pvr_UnitySDK";
#endif
        #endregion

#if !UNITY_STANDALONE_WIN
        #region DllFuncation
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr Pvr_GetSDKVersion();
        #endregion
#endif

        #region Public Static Funcation
        public static string UPvr_GetSDKVersion()
        {
#if ANDROID_DEVICE
            IntPtr ptr = Pvr_GetSDKVersion();
            if (ptr != IntPtr.Zero)
            {
                return Marshal.PtrToStringAnsi(ptr);
            }
#endif
            return "";
        }

        public static string UPvr_GetUnitySDKVersion()
        {
            return UnitySDKVersion;

        }
        public static string UPvr_GetDeviceMode()
        {
            string devicemode = "";
#if ANDROID_DEVICE
            devicemode = SystemInfo.deviceModel;
#endif
            return devicemode;
        }

        public static string UPvr_GetDeviceModel()
        {
            return SystemInfo.deviceModel;
        }
        public static string UPvr_GetDeviceSN()
        {
            string serialNum = "UNKONWN";
#if ANDROID_DEVICE
            System.UPvr_CallStaticMethod<string>(ref serialNum, Pvr_UnitySDKRender.javaSysActivityClass, "getDeviceSN");
#endif
            return serialNum;
        }

#if ANDROID_DEVICE
        public static AndroidJavaObject UPvr_GetCurrentActivity()
        {
            AndroidJavaObject currentActivity;
            UnityEngine.AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");          
            return currentActivity;
        }
#endif

        public static void UPvr_ShutDown()
        {
#if ANDROID_DEVICE
            System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaSysActivityClass, "Pvr_ShutDown");
#endif
        }

        public static void UPvr_Reboot()
        {
#if ANDROID_DEVICE
            System.UPvr_CallStaticMethod( Pvr_UnitySDKRender.javaSysActivityClass, "Pvr_Reboot",UPvr_GetCurrentActivity());
#endif
        }

        public static void UPvr_Sleep()
        {
#if ANDROID_DEVICE
            System.UPvr_CallStaticMethod( Pvr_UnitySDKRender.javaSysActivityClass, "Pvr_Sleep");
#endif
        }

        public static bool UPvr_StartHomeKeyReceiver(string startreceivre)
        {


#if ANDROID_DEVICE
            try
            {
                if (Pvr_UnitySDKManager.pvr_UnitySDKRender !=null)
                {
					Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityLongReceiver, "Pvr_StartReceiver", Pvr_UnitySDKManager.pvr_UnitySDKRender.activity, startreceivre);
                    Debug.Log("Start home key   Receiver");
                    return true;
                }
              
            }
            catch (Exception e)
            {
                Debug.LogError("Start home key  Receiver  Error :" + e.ToString());
                return false;
            }
#endif
            return true;
        }
        public static bool UPvr_StopHomeKeyReceiver()
        {
#if ANDROID_DEVICE
            try
            {
                if (Pvr_UnitySDKManager.pvr_UnitySDKRender !=null)
                {
					Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityLongReceiver, "Pvr_StopReceiver", Pvr_UnitySDKManager.pvr_UnitySDKRender.activity);
                    Debug.Log("Stop home key   Receiver");
                    return true;
                }
              
            }
            catch (Exception e)
            {
                Debug.LogError("Stop home key  Receiver  Error :" + e.ToString());
                return false;
            }
#endif
            return true;
        }
        public static void UPvr_StartVRModel()
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "startVRModel");
#endif
        }
        public static void UPvr_RemovePlatformLogo()
        {
#if ANDROID_DEVICE
			Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "removePlatformLogo");
#endif
        }
        public static void UPvr_ShowPlatformLogo()
        {
#if ANDROID_DEVICE
			Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "showPlatformLogo");
#endif
        }
        public static void UPvr_StopVRModel()
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "stopVRModel");
#endif
        }

        public static string UPvr_GetCountryCode()
        {
            string code = "";
#if ANDROID_DEVICE
             System.UPvr_CallStaticMethod<string>(ref code,Pvr_UnitySDKRender.javaVrActivityClass, "getCountryCode",UPvr_GetCurrentActivity());
#endif
            return code;
        }
        #endregion

        public static bool UPvr_checkDevice(string packagename)
        {
            bool value = false;
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<bool>(ref value,Pvr_UnitySDKRender.javaVrActivityClass, "checkDevice", packagename,UPvr_GetCurrentActivity());
#endif
            return value;
        }
    }


}
