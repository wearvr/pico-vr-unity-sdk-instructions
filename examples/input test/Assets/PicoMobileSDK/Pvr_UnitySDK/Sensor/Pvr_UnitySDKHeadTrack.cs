///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_UnitySDKHeadTrack
// Author: AiLi.Shang
// Date:  2017/01/13
// Discription:Main tracking,manage the rotation of cameras.Be fully careful of  Code modification
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class Pvr_UnitySDKHeadTrack : MonoBehaviour
{

    public bool trackRotation = true;

    public bool trackPosition = true;

    public Transform target;  

    private bool updated = false;  

    private Vector3 startPosition;

    private Quaternion startQuaternion;
    private bool dataClock;

    public void Awake()
    {
        if (target == null)
        {
            startPosition = transform.localPosition;
            startQuaternion = transform.localRotation;
        }
        else
        {
            startPosition = transform.position;
            startQuaternion = transform.rotation;
        }
        //Pvr_UnitySDKAPI.Sensor.UPvr_InitPsensor();

    }

    public Ray Gaze
    {
        get
        {
            UpdateHead();
            return new Ray(transform.position, transform.forward);
        }
    }

    void Update()
    {
        updated = false;  // OK to recompute head pose.
//# if!UNITY_EDITOR && UNITY_ANDROID
//        if (Pvr_UnitySDKManager.SDK.posStatus == 1 || Pvr_UnitySDKAPI.Sensor.UPvr_GetPsensorState() == 1)
//        {
//            trackPosition = false;
//            if (!dataClock)
//            {
//                Pvr_UnitySDKManager.SDK.resetBasePos = Pvr_UnitySDKManager.SDK.HeadPose.Position;
//                dataClock = true;
//            }
//        }
//        else
//        {
//            if (dataClock)
//            {
//                Pvr_UnitySDKAPI.Sensor.UPvr_OptionalResetSensor((int)Pvr_UnitySDKAPI.Sensorindex.Default, 0, 1);
//                Pvr_ControllerManager.controllerlink.ResetHeadSensorForController();
//                Pvr_ControllerManager.controllerlink.ResetController(0);
//                Pvr_ControllerManager.controllerlink.ResetController(1);
//            }
//            trackPosition = true;
//            dataClock = false;
//        }
//#endif
        UpdateHead();
    }

    private void UpdateHead()
    {
        if (updated)
        {
            return;
        }
        updated = true;
        if (Pvr_UnitySDKManager.SDK == null)
        {
            return;
        }
        if (trackRotation)
        {
            var rot = Pvr_UnitySDKManager.SDK.HeadPose.Orientation;
            if (target == null)
            {
                transform.localRotation = rot;
            }
            else
            {
                transform.rotation = rot * target.rotation;
            }
        }

        else
        {
            var rot = Pvr_UnitySDKManager.SDK.HeadPose.Orientation;
            if (target == null)
            {
                transform.localRotation = Quaternion.identity;
            }
            else
            {
                transform.rotation = rot * target.rotation;
            }
        }
        if (trackPosition)
        {
            Vector3 pos = Pvr_UnitySDKManager.SDK.HeadPose.Position;
            if (target == null)
            {
                transform.localPosition = pos;
            }
            else
            {
                transform.position = target.position + target.rotation * pos;
            }
        }
    }

}
