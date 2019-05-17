///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Pvr_Audio3D
// Author: AiLi.Shang
// Date:  2017/01/11
// Discription: the demo shows how to use the  audio3dapi 
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

public class Pvr_Audio3D : MonoBehaviour {
    /************************************    Properties  *************************************/
    #region Properties
    public string MusicPath ;
    #endregion

    /************************************ Public Interfaces  *********************************/
    #region Public Interfaces

    public void OpenEffects()
    {
        Pvr_UnitySDKAPI.Audio3D.UPvr_OpenEffects();
    }
    public void CloseEffects()
    {
        Pvr_UnitySDKAPI.Audio3D.UPvr_CloseEffects();
    }
    public void SetSurroundroomType(int type)
    {
        Pvr_UnitySDKAPI.Audio3D.UPvr_SetSurroundroomType(type);
    }
    public void OpenRoomcharacteristics()
    {
        Pvr_UnitySDKAPI.Audio3D.UPvr_OpenRoomcharacteristics();
    }
    public void CloseRoomcharacteristics()
    {
        Pvr_UnitySDKAPI.Audio3D.UPvr_CloseRoomcharacteristics();
    }
    public void EnableSurround()
    {
        Pvr_UnitySDKAPI.Audio3D.UPvr_EnableSurround();
    }
    public void EnableReverb()
    {
        Pvr_UnitySDKAPI.Audio3D.UPvr_EnableReverb();
    }
    public void StartAudioEffect(string audioFile, bool isSdcard)
    {
        Pvr_UnitySDKAPI.Audio3D.UPvr_StartAudioEffect(audioFile, isSdcard);
    }
    public void StartMusic()
    {
        StartAudioEffect(MusicPath, true);
    }
    public void StopAudioEffect()
    {
        Pvr_UnitySDKAPI.Audio3D.UPvr_StopAudioEffect();
    }
    #endregion

    /*************************************  Unity API ****************************************/
    #region Unity API
    void Start()
    {
         bool enable = Pvr_UnitySDKAPI.Audio3D.UPvr_SpatializerUnlock();
         if (enable)
        {
            Pvr_UnitySDKAPI.Audio3D.UPvr_InitAm3d();
            Debug.Log("The AM3d ability is enabled!");
        }
        /*
        else
        {
            Debug.Log("Cannot enable this AM3d ability!!!");
        }
        */
        //MusicPath = "/sdcard/DCIM/GuitarLoop.wav";
        //StartAudioEffect(MusicPath, true);
    }
    void OnDisable()
    {
        Pvr_UnitySDKAPI.Audio3D.UPvr_CloseEffects();
        Pvr_UnitySDKAPI.Audio3D.UPvr_ReleaseAudioEffect();
    }
    #endregion


}
