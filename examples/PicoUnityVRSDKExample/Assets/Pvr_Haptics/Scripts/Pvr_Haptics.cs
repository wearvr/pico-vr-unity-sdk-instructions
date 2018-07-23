///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_Haptics
// Author: AiLi.Shang
// Date:  2017/04/05
// Discription:The using of Haptics
///////////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine;

public class Pvr_Haptics : MonoBehaviour
{
    /************************************    Properties  *************************************/
    #region Properties
    public bool UseHaptic = true;
    public int vibrationDuration = 500;
    public int silienceDuration = 200;
    public int repeat_times = 2;
    public int vibrationStrength = 100;
    public int whichHaptic = 2;

    private bool usingHaptic = false;
    #endregion

    /************************************   Public Interfaces **********************************/
    #region Public Interfaces

    #endregion

    /*************************************  Unity API ****************************************/
    #region Unity API
    void Awake()
    {
        if (Pvr_UnitySDKAPI.Haptics.UPvr_HasControllerVibrator())
        {
            if (UseHaptic)
            {
                usingHaptic = true;
                Debug.Log("This platform  supports Haptics");
            }
        }
        else
        {
            UseHaptic = false; 
            Debug.Log("This platform didnot support Haptics");
        }
    }

    void Update()
    {  
        if (usingHaptic)
        {           
        }
    }
    public void Pvr_HasControllerVibrator()
    {
        Pvr_UnitySDKAPI.Haptics.UPvr_HasControllerVibrator();
    }
    public void Pvr_SetControllerVibrateMode()
    {
        int[] pattern = new int[5] { vibrationDuration, silienceDuration, repeat_times, vibrationStrength, whichHaptic };
        Pvr_UnitySDKAPI.Haptics.UPvr_SetControllerVibrateMode(pattern, 5, 1);
    }

    public void Pvr_SetControllerVibrateTime()
    {
        int milliseconds = 2000;
        Pvr_UnitySDKAPI.Haptics.UPvr_SetControllerVibrateTime(milliseconds);
    }

    public void Pvr_CancelControllerVibrate()
    {
        Pvr_UnitySDKAPI.Haptics.UPvr_CancelControllerVibrate();
    }
    #endregion
   


}
