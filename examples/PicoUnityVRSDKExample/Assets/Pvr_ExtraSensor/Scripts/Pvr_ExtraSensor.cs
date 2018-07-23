///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Pvr_ExtraSensor
// Author: AiLi.Shang
// Date:  2017/01/13
// Discription:The using of extra senssor
///////////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine;



public class Pvr_ExtraSensor : MonoBehaviour
{
    [Range(1, 2)]
    public int SensorIndex = 1;
#if UNITY_ANDROID && !UNITY_EDITOR
    /************************************    Properties  *************************************/
    #region Properties


    int SensorCount = 1;

    bool SensorStart = false;

    bool SensorInit = false;

    bool SensorCanUse = false;

    Quaternion UnityQutation = Quaternion.identity;

    Vector3 EulerAngles = Vector3.zero;

    Vector3 UnityPoasition = Vector3.zero;
 
    Quaternion RotateX  = Quaternion.identity;
    #endregion

    /************************************   Public Interfaces **********************************/
    #region Public Interfaces

    public bool UnitySDKSensorCount()
    {
        bool enable = false;
        try
        {
            int index = (int)Pvr_UnitySDKAPI.GlobalIntConfigs.SEENSOR_COUNT;
            int temp = Pvr_UnitySDKAPI.Render.UPvr_GetIntConfig(index, ref SensorCount);
            Debug.Log("Sesnor Count = " + SensorCount.ToString());
            if (temp == 0)
                if (SensorCount > 1)
                {
                    enable = true;
                }

        }
        catch (System.Exception e)
        {
            Debug.LogError("UnitySDKSensorCount Get ERROR! " + e.Message);
            throw;
        }
        return enable;
    }

    public bool StartUnitySDKSensor()
    {
        bool enable = false;
        try
        {
            if (Pvr_UnitySDKAPI.Sensor.UPvr_StartSensor(SensorIndex) == 0)
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
            if (Pvr_UnitySDKAPI.Sensor.UPvr_StopSensor(SensorIndex) == 0)
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
            if (Pvr_UnitySDKAPI.Sensor.UPvr_ResetSensor(SensorIndex) == 0)
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
        if (SensorStart)
        {
            float w = 0, x = 0, y = 0, z = 0, px = 0, py = 0, pz = 0;
            try
            {
                int returns = Pvr_UnitySDKAPI.Sensor.UPvr_GetSensorState(SensorIndex, ref x, ref y, ref z, ref w, ref px, ref py, ref pz);
                if (returns == 0)
                {
                    UnityQutation = new Quaternion(-x, -y, z, w);
				   enable = true;

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
            if (Pvr_UnitySDKAPI.Sensor.UPvr_ResetSensor(SensorIndex) == 0)
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

    /*************************************  Unity API ****************************************/
    #region Unity API
    void Awake()
    {
        RotateX = Quaternion.Euler(new Vector3(-90.0f, 0.0f, 0.0f));
        SensorCanUse = UnitySDKSensorCount();
        if (SensorCanUse)
        {
            SensorStart = StartUnitySDKSensor();

        }
        else
            Debug.LogError("There is not extra Sensor!");
        UnityPoasition = this.gameObject.transform.position;
    }

    void Update()
    {
        if (SensorCanUse)
        {
            if (GetUnitySDKSensorState())
            {
                this.gameObject.transform.position = UnityPoasition;
                this.gameObject.transform.rotation = UnityQutation * RotateX ;
 			}
              if (Input.GetKeyDown(KeyCode.JoystickButton2))
             {
                  ResetUnitySDKSensor();
                  Debug.Log("Extra sensor Reset sensor");
              }
           
        }
    }

    void OnDisable()
    {
        if (SensorStart)
        {
            StopUnitySDKSensor();
        }
    }
     private void OnApplicationPause(bool pause)
    {
        Debug.Log("OnApplicationPause 2 -------------------------" + (pause ? "true" : "false"));

        if (pause)
        {
            if (SensorStart)
            {
                StopUnitySDKSensor();
                SensorStart = false;
            }
        }
        else
        {
            if (SensorCanUse && !SensorStart)
            {
                SensorStart = StartUnitySDKSensor();
            }
            else
                Debug.LogError("There is not extra Sensor!");
        }

    }
    #endregion
#endif


}


