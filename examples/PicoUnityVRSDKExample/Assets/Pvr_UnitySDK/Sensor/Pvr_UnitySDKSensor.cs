///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_UnitySDKSensor
// Author: AiLi.Shang
// Date:  2017/01/13
// Discription:The using of main sensor.
///////////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine;

public class Pvr_UnitySDKSensor 
{

    public  Pvr_UnitySDKSensor()
    {
        Init();
    }

    /************************************    Properties  *************************************/
    #region Properties
   
    public Pvr_UnitySDKPose SensorValvue;

    public bool HMDUsing = true;

    bool SensorStart = false;

    bool SensorInit = false;

    bool Sensor6dofInit = false;

    Quaternion UnityQutation = Quaternion.identity;

    Vector3 EulerAngles = Vector3.zero;

    Vector3 UnityPoasition = Vector3.zero;

    Pvr_UnitySDKAPI.Sensorindex sensorIndex = Pvr_UnitySDKAPI.Sensorindex.Default;

    #endregion

    /************************************   Public Interfaces **********************************/
    #region Public Interfaces


    public void Init()
    {
        Sensor6dofInit = InitUnitySDK6DofSensor();
        SensorInit = InitUnitySDKSensor();
        SensorStart = StartUnitySDKSensor();
    }

    public void SensorUpdate()
    {
        if (GetUnitySDKSensorState())
        {
            Pvr_UnitySDKManager.SDK.HeadPose = new Pvr_UnitySDKPose(UnityPoasition, UnityQutation);
        }
    }
    public bool InitUnitySDKSensor()
    {
       
        bool enable = false;
        try
        {            
            if (Pvr_UnitySDKAPI.Sensor.UPvr_Init((int)sensorIndex) == 0)
                enable = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("InitUnitySDKSensor ERROR! " + e.Message);
            throw;
        }
        return enable;
    }
    public bool InitUnitySDK6DofSensor()
    {
        bool enable = false;
        try
        {
            int ability6dof = 0;
            int enumindex = (int)Pvr_UnitySDKAPI.GlobalIntConfigs.ABILITY6DOF;
            Pvr_UnitySDKAPI.Render.UPvr_GetIntConfig(enumindex, ref ability6dof);
            if (ability6dof == 1)
            {

                if (Pvr_UnitySDKAPI.Sensor.UPvr_Enable6DofModule(Pvr_UnitySDKManager.SDK.Enable6Dof) == 0)
                {
                    enable = true;
                    Pvr_UnitySDKManager.PVRNeck = false;
                } 
            }
            else
            {
                Debug.LogWarning("This platform does NOT support 6 Dof ! " );
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("InitUnity6DofSDKSensor ERROR! " + e.Message);
            throw;
        }
        return enable;
    }
  
    public bool StartUnitySDKSensor()
    {
        bool enable = false;
        try
        {
            if (Pvr_UnitySDKAPI.Sensor.UPvr_StartSensor((int)sensorIndex) == 0)
                enable = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("StartUnitySDKSensor ERROR! " + e.Message);
            throw;
        }
        return enable;
    }

    public bool StopUnitySDKSensor()
    {
        bool enable = false;
        try
        {
            if (Pvr_UnitySDKAPI.Sensor.UPvr_StopSensor((int)sensorIndex) == 0)
                enable = true;

        }
        catch (System.Exception e)
        {
            Debug.LogError("StopUnitySDKSensor ERROR! " + e.Message);
            throw;
        }
        return enable;
    }

    public bool ResetUnitySDKSensor()
    {
        bool enable = false;
        try
        {
            if (Pvr_UnitySDKAPI.Sensor.UPvr_ResetSensor((int)sensorIndex) == 0)
            {
                enable = true;
                Debug.LogError("ResetUnitySDKSensor OK! ");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("ResetUnitySDKSensor ERROR! " + e.Message);
            throw;
        }
        return enable;
    }

    public bool GetUnitySDKSensorState()
    {
        bool enable = false;
        if (SensorInit && SensorStart)
        {
            float fov = 102;
            float w = 0, x = 0, y = 0, z = 0, px = 0, py = 0, pz = 0;
            try
            {
                int returns = Pvr_UnitySDKAPI.Sensor.UPvr_GetMainSensorState(ref x, ref y, ref z, ref w,  ref px, ref py, ref pz, ref fov, ref Pvr_UnitySDKManager.SDK.RenderviewNumber);
                if (returns == 0)
                {
                    UnityQutation = new Quaternion(-x, -y, z, w);
                    Pvr_UnitySDKManager.SDK.EyeFov = fov;
                    enable = true;
                    
                    if (Pvr_UnitySDKManager.PVRNeck)
                    {
                        UnityPoasition = UnityQutation * Pvr_UnitySDKManager.SDK.neckOffset - Pvr_UnitySDKManager.SDK.neckOffset;
                    }
                    else
                    {
                        UnityPoasition = new Vector3(-px, -py, pz);
                    }
                }
                if (returns == -1)
                    Debug.Log("sesnor update --- GetUnitySDKSensorState     -1    ");
            }
            catch (System.Exception e)
            {
                Debug.LogError("GetUnitySDKSensorState ERROR! " + e.Message);
                throw;
            }

        }
        return enable;
    }
   
    public bool GetUnitySDKPSensorState()
    {
        bool enable = false;
        try
        {
            if (Pvr_UnitySDKAPI.Sensor.UPvr_ResetSensor((int)sensorIndex) == 0)
                enable = true;

        }
        catch (System.Exception e)
        {
            Debug.LogError("GetUnitySDKPSensorState ERROR! " + e.Message);
            throw;
        }
        return enable;
    }
    
    #endregion


}
