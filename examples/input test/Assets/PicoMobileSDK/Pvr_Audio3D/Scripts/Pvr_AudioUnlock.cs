///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Pvr_AudioUnlock
// Author: AiLi.Shang
// Date:  2017/01/13
// Discription:  the unlock of using 3dAudio's some funcation
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
using System.Runtime.InteropServices;

public class Pvr_AudioUnlock : MonoBehaviour {

#if ANDROID_DEVICE
    public const string LibFileName = "Pvr_UnitySDK";
    [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
	public static extern void Pvr_SpatializerUnlock(string appPath);
#endif
    void Start () {

#if ANDROID_DEVICE && !UNITY_EDITOR
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
						return;
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
#endif

                  }
}
