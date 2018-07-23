///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights Reserved.
// File: Pvr_Controller
// Author: Yangel.Yan
// Date:  2017/01/11
// Discription: The demo of using controller
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
using UnityEngine.UI;
using Pvr_UnitySDKAPI;

public class Pvr_Controller : MonoBehaviour
{

    /************************************    Properties  *************************************/
    #region Properties

    public Transform direction;
    public Transform m_dot;
    public Ray ray;

    public UserHandNess handness;   //左右手区分，开发者调用设置左右手。
    public enum UserHandNess
    {
        Right,
        Left
    }

    public enum GazeType
    {
        Never,
        DuringMotion,
        Always
    }
    public ControllerAxis Axis;
    public GazeType Gazetype;
    public enum ControllerAxis
    {
        Controller,
        Wrist,
        Elbow,
        Shoulder
    }
    [Range(0.0f, 0.2f)]
    public float ElbowHeight = 0.0f;
    [Range(0.0f, 0.2f)]
    public float ElbowDepth = 0.0f;
    [Range(0.0f, 30.0f)]
    public float PointerTiltAngle = 15.0f;
    #endregion

    /*************************************  Unity API ****************************************/
    #region Unity API
    void Awake()
    {
        var controller = transform.Find("controller").gameObject;
        controller.SetActive(false);
        var dot = transform.Find("dot").gameObject;
        dot.SetActive(false);
        var ray_alpha = transform.Find("ray_alpha").gameObject;
        ray_alpha.SetActive(false);
    }
    void Start()
    {
        Invoke("DelayShowController", 0.5f);
        handness = (UserHandNess)Pvr_ControllerManager.controllerlink.getHandness();
        SetArmParaToSo((int)handness, (int)Gazetype, ElbowHeight, ElbowDepth, PointerTiltAngle);
        ray = new Ray();
        ray.origin = transform.position;
    }
    void Update()
    {
        DoUpdate();
        ray.direction = direction.position - transform.position;
    }

    public void DelayShowController()
    {
        var controller = transform.Find("controller").gameObject;
        controller.SetActive(true);
        var dot = transform.Find("dot").gameObject;
        dot.SetActive(true);
        var ray_alpha = transform.Find("ray_alpha").gameObject;
        ray_alpha.SetActive(true);
    }
    #endregion

    public void DoUpdate()
    {
        CalcArmModelfromSo();
        UpdateControllerDataSO();
    }

    private float mouseX = 0;
    private float mouseY = 0;
    private float mouseZ = 0;
    private Quaternion UpdateSimulatedFrameParams()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            mouseX += Input.GetAxis("Mouse X") * 5;
            if (mouseX <= -180)
            {
                mouseX += 360;
            }
            else if (mouseX > 180)
            {
                mouseX -= 360;
            }
            mouseY -= Input.GetAxis("Mouse Y") * 2.4f;
            mouseY = Mathf.Clamp(mouseY, -80, 80);
        }
        else if (Input.GetKey(KeyCode.RightShift))
        {
            mouseZ += Input.GetAxis("Mouse X") * 5;
            mouseZ = Mathf.Clamp(mouseZ, -80, 80);
        }

        return Quaternion.Euler(mouseY, mouseX, mouseZ);
    }
    public void ChangeHandNess()
    {
        handness = handness == UserHandNess.Right ? UserHandNess.Left : UserHandNess.Right;
        SetArmParaToSo((int)handness, (int)Gazetype, ElbowHeight, ElbowDepth, PointerTiltAngle);
    }

    private Vector3 inputDirection;

    private void SetArmParaToSo(int hand, int gazeType, float elbowHeight, float elbowDepth, float pointerTiltAngle)
    {
        Pvr_UnitySDKAPI.Controller.UPvr_SetArmModelParameters(hand, gazeType, elbowHeight, elbowDepth, pointerTiltAngle);
    }

    private void CalcArmModelfromSo()
    {
        float[] Headrot = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };
        float[] Handrot = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };
        float[] AgeeAngularVelocity = new float[3] { 0.0f, 0.0f, 0.0f };
        UserHandNess ness = handness;
        Quaternion controllerData = new Quaternion();
#if UNITY_EDITOR
        controllerData = UpdateSimulatedFrameParams();
        transform.localRotation = controllerData;
#else
        controllerData = Controller.UPvr_GetControllerQUA();  

        Handrot[0] = controllerData.x;
        Handrot[1] = controllerData.y;
        Handrot[2] = controllerData.z;
        Handrot[3] = controllerData.w;

        Vector3 AngVelocity = Pvr_UnitySDKAPI.Controller.Upvr_GetAngularVelocity();
        AgeeAngularVelocity[0] = AngVelocity.x;
        AgeeAngularVelocity[1] = AngVelocity.y;
        AgeeAngularVelocity[2] = AngVelocity.z;
        if (Gazetype == GazeType.DuringMotion)
        {
            Vector3 gazeDirection = Pvr_UnitySDKManager.SDK.HeadPose.Orientation * Vector3.forward;
            gazeDirection.y = 0.0f;
            gazeDirection.Normalize();
            float angular = AngVelocity.magnitude;
            float gazeFilter = Mathf.Clamp((angular - 0.2f) / 45.0f, 0.0f, 0.1f);
            inputDirection = Vector3.Slerp(inputDirection, gazeDirection, gazeFilter);
            Quaternion gazeRotation = Quaternion.FromToRotation(Vector3.forward, inputDirection);
            Headrot[0] = gazeRotation.x;
            Headrot[1] = gazeRotation.y;
            Headrot[2] = gazeRotation.z;
            Headrot[3] = gazeRotation.w;
        }
        else
        {
            Headrot[0] = Pvr_UnitySDKManager.SDK.HeadPose.Orientation.x;
            Headrot[1] = Pvr_UnitySDKManager.SDK.HeadPose.Orientation.y;
            Headrot[2] = Pvr_UnitySDKManager.SDK.HeadPose.Orientation.z;
            Headrot[3] = Pvr_UnitySDKManager.SDK.HeadPose.Orientation.w;
        }
        Pvr_UnitySDKAPI.Controller.UPvr_CalcArmModelParameters( Headrot, Handrot, AgeeAngularVelocity);
#endif
    }

    public void UpdateControllerDataSO()
    {
#if !UNITY_EDITOR
        float[] rot = new float[4] { 0, 0, 0, 0 };
        float[] pos = new float[3] { 0, 0, 0 };
        Vector3 finalyPosition;
        Quaternion finalyRotation;
        switch (Axis)
        {
            case ControllerAxis.Controller:
                Controller.UPvr_GetPointerPose(rot, pos);
                pointerPosition = new Vector3(pos[0], pos[1], pos[2]);
                pointerRotation = new Quaternion(rot[0], rot[1], rot[2], rot[3]);
                finalyPosition = pointerPosition;
                finalyRotation = pointerRotation;
                break;
            case ControllerAxis.Wrist:
                Controller.UPvr_GetWristPose(rot, pos);
                wristPosition = new Vector3(pos[0], pos[1], pos[2]);
                wristRotation = new Quaternion(rot[0], rot[1], rot[2], rot[3]);
                finalyPosition = wristPosition;
                finalyRotation = wristRotation;
                break;
            case ControllerAxis.Elbow:
                Controller.UPvr_GetElbowPose(rot, pos);
                elbowPosition = new Vector3(pos[0], pos[1], pos[2]);
                elbowRotation = new Quaternion(rot[0], rot[1], rot[2], rot[3]);   
                finalyPosition = elbowPosition;
                finalyRotation = elbowRotation;
                break;
            case ControllerAxis.Shoulder:
                Controller.UPvr_GetShoulderPose(rot, pos);
                shoulderPosition = new Vector3(pos[0], pos[1], pos[2]);
                shoulderRotation = new Quaternion(rot[0], rot[1], rot[2], rot[3]);  
                finalyPosition = shoulderPosition;
                finalyRotation = shoulderRotation;
                break;
            default:
                throw new System.Exception("Invalid FromJoint.");
        }



        transform.localPosition = finalyPosition;
        transform.localRotation = finalyRotation;
#endif
    }

    public static Vector3 pointerPosition { get; set; }
    public static Quaternion pointerRotation { get; set; }
    public static Vector3 elbowPosition { get; set; }
    public static Quaternion elbowRotation { get; set; }
    public static Vector3 wristPosition { get; set; }
    public static Quaternion wristRotation { get; set; }
    public static Vector3 shoulderPosition { get; set; }
    public static Quaternion shoulderRotation { get; set; }

}
