///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Pvr_Audio3DAPI
// Author: AiLi.Shang
// Date:  2017/01/11
// Discription: The 3D audio API
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
    public struct Audio3D
    {
        #region Android
#if ANDROID_DEVICE
        public static AndroidJavaClass Pvr_Audio3DActivityClass;
        public static UnityEngine.AndroidJavaObject am3d;
        public const string LibFileName = "Pvr_UnitySDK";
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
	    public static extern void Pvr_SpatializerUnlock(string appPath);
#endif
        #endregion

        /**************************** Private Static Funcations *******************************************/
        #region Private Static Funcation

        private static void openEffects()
        {
#if ANDROID_DEVICE
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_Audio3DActivityClass, "Pvr_OpenEffects");
#endif
        }
        private static void init()
        {
#if ANDROID_DEVICE
            Pvr_Audio3DActivityClass = new UnityEngine.AndroidJavaClass("com.psmart.am3d.Am3d");
			Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_Audio3DActivityClass, "init",Pvr_UnitySDKManager.pvr_UnitySDKRender.activity);
#endif
        }
        private static void closeEffects()
        {
#if ANDROID_DEVICE
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_Audio3DActivityClass, "Pvr_CloseEffects");
#endif
        }
        private static void setSurroundroomType(int type)
        {
#if ANDROID_DEVICE
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_Audio3DActivityClass, "Pvr_SetSurroundroomType", type);
#endif
        }
        private static void openRoomcharacteristics()
        {
#if ANDROID_DEVICE
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_Audio3DActivityClass, "Pvr_OpenRoomcharacteristics");
#endif
        }
        private static void closeRoomcharacteristics()
        {
#if ANDROID_DEVICE
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_Audio3DActivityClass, "Pvr_CloseRoomcharacteristics");
#endif
        }
        private static void enableSurround()
        {
#if ANDROID_DEVICE
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_Audio3DActivityClass, "Pvr_EnableSurround");
#endif
        }
        private static void enableReverb()
        {
#if ANDROID_DEVICE
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_Audio3DActivityClass, "Pvr_EnableReverb");
#endif
        }
        private static void startAudioEffect(string audioFile, bool isSdcard)
        {
#if ANDROID_DEVICE
            isSdcard = true;
            Debug.Log("Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod startAudioEffect    + path  " + audioFile.ToString());
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_Audio3DActivityClass, "Pvr_StartAudioEffect", Pvr_UnitySDKManager.pvr_UnitySDKRender.activity, audioFile, isSdcard);
#endif
        }
        private static void stopAudioEffect()
        {
#if ANDROID_DEVICE
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_Audio3DActivityClass, "Pvr_StopAudioEffect");
#endif
        }
        private static void releaseAudioEffect()
        {
#if ANDROID_DEVICE
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_Audio3DActivityClass, "Pvr_ReleaseAudio");
#endif
        }

        #endregion

        /**************************** Public Static Funcations *******************************************/
        #region Public Static Funcation  

        public static void UPvr_InitAm3d()
        {
            //	am3d = new UnityEngine.AndroidJavaClass("com.psmart.jarload.Am3dActivity");  
            init();

        }
        public static void UPvr_OpenEffects()
        {
            openEffects();
        }
        public static void UPvr_CloseEffects()
        {
            closeEffects();
        }
        public static void UPvr_SetSurroundroomType(int type)
        {
            setSurroundroomType(type);
        }
        public static void UPvr_OpenRoomcharacteristics()
        {
            openRoomcharacteristics();
        }
        public static void UPvr_CloseRoomcharacteristics()
        {
            closeRoomcharacteristics();
        }
        public static void UPvr_EnableSurround()
        {
            enableSurround();
        }
        public static void UPvr_EnableReverb()
        {
            enableReverb();
        }
        public static void UPvr_StartAudioEffect(string audioFile, bool isSdcard)
        {
            startAudioEffect(audioFile, isSdcard);
        }
        public static void UPvr_StopAudioEffect()
        {
            stopAudioEffect();
        }
        public static void UPvr_ReleaseAudioEffect()
        {
            releaseAudioEffect();
        }

        public static bool UPvr_SpatializerUnlock()
        {

#if ANDROID_DEVICE
           string appPath = "";
		try 
		{
			IntPtr obj_context = AndroidJNI.FindClass("android/content/ContextWrapper");
			IntPtr method_getFilesDir = AndroidJNIHelper.GetMethodID(obj_context, "getFilesDir", "()Ljava/io/File;");
			using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
			{
				using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
				{
					IntPtr file = AndroidJNI.CallObjectMethod(obj_Activity.GetRawObject(), method_getFilesDir, new jvalue[0]);
					IntPtr obj_file = AndroidJNI.FindClass("java/io/File");
					IntPtr method_getAbsolutePath = AndroidJNIHelper.GetMethodID(obj_file, "getAbsolutePath", "()Ljava/lang/String;");   
					appPath = AndroidJNI.CallStringMethod(file, method_getAbsolutePath, new jvalue[0]);                    
					if(appPath == null) 
						return false;
					Debug.Log("Android app path: " + appPath);
				}
			}
			appPath = appPath.Replace("/files","/lib/");
		}
		catch(Exception e) 
		{
			Debug.Log(e.ToString());
		}

            Pvr_SpatializerUnlock(appPath);
            return true;
#else
            return false;
#endif
        }
        
        #endregion

    }

}