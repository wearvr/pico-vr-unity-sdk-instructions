///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Pvr_VolumePowerBrightness
// Author: AiLi.Shang
// Date:  2017/01/13
// Discription: The Common using of Android System
///////////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine;
using UnityEngine.UI;

public class Pvr_VolumePowerBrightness : MonoBehaviour
{
    /************************************    Properties  *************************************/
    #region Properties
    bool VolEnable = false;
    bool BattEnable = false;

    public Text showResult;
    public Text setVolumnum;
    public Text setBrightnum;

    public string MusicPath;
    #endregion

    /************************************   Public Interfaces **********************************/
    #region Public Interfaces

    public void GetMaxVolumeNumber()
    {
        int maxVolume = 0;
        maxVolume = Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_GetMaxVolumeNumber();
        showResult.text = "最大音量: " + maxVolume.ToString();
    }
    public void GetCurrentVolumeNumber()
    {
        int currVolume = 0;
        currVolume = Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_GetCurrentVolumeNumber();
        showResult.text = "当前音量：" + currVolume.ToString();
    }
    public void VolumeUp()
    {
        bool enable = false;
        enable = Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_VolumeUp();
        if (!enable)
        {
            Debug.LogError("VolumeUp Error");
        }
    }
    public void VolumeDown()
    {
        bool enable = false;
        enable = Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_VolumeDown();
        if (!enable)
        {
            Debug.LogError("VolumeDown Error");
        }
    }
    public void SetVolumeNum()
    {
        bool enable = false;
        System.Random rm = new System.Random();
        int volume = rm.Next(0, 15);
        setVolumnum.text = "随机数：" + volume.ToString();
        enable = Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_SetVolumeNum(volume);
        if (!enable)
        {
            Debug.LogError("SetVolumeNum Error");
        }
    }
    public void SetBrightness()
    {
        bool enable = false;
        System.Random rm = new System.Random();
        int brightness = rm.Next(0, 255);
        setBrightnum.text = "随机数：" + brightness.ToString();
        enable = Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_SetCommonBrightness(brightness);

        if (!enable)
        {
            Debug.LogError("SetBrightness Error");
        }
    }
    public void GetCurrentBrightness()
    {
        int lightness = 0;
        lightness = Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_GetCommonBrightness();

        showResult.text = "当前亮度：" + lightness.ToString();
    }

    public bool setAudio(string s)
    {
        Debug.Log(s.ToString());
        // do what you want !
        return true;
    }

    public bool setBattery(string s)
    {
        Debug.Log(s.ToString());
        // do what you want !
        return true;
    }

    #endregion

    /************************************  Private Interfaces **********************************/
    #region Private Interfaces

   
    private bool InitBatteryVolClass()
    {
        return Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_InitBatteryVolClass();
    }
    private bool StartBatteryReceiver(string startreceivre)
    {
        BattEnable = Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_StartBatteryReceiver(startreceivre);
        return BattEnable;
    }
    private bool StopBatteryReceiver()
    {
        return Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_StopBatteryReceiver();
    }
    private bool StartAudioReceiver(string startreceivre)
    {
        VolEnable = Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_StartAudioReceiver(startreceivre);
        return VolEnable;
    }
    private bool StopAudioReceiver()
    {
        return Pvr_UnitySDKAPI.VolumePowerBrightness.UPvr_StopAudioReceiver();
    }

    #endregion

    /************************************   MonoBehaviour **********************************/
    #region Unity API
    void Start()
    {
        Pvr_UnitySDKAPI.Audio3D.UPvr_StartAudioEffect(MusicPath, true);
    }

    void Awake()
    {

        InitBatteryVolClass();
        string gameobjName = this.gameObject.name;
        StartBatteryReceiver(gameobjName);
        StartAudioReceiver(gameobjName);

    }

    void OnDisable()
    {
        if (VolEnable)
        {
            StopAudioReceiver();
        }
        if (BattEnable)
        {
            StopBatteryReceiver();
        }

    }

  
    #endregion

}
