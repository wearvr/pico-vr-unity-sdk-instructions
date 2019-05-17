///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_UnitySDKEyeManager
// Author: AiLi.Shang
// Date:  2017/01/18
// Discription:  Controller of cameras . Be fully careful of  Code modification
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using System;

public class Pvr_UnitySDKEyeManager : MonoBehaviour
{

    public bool isfirst = true;
	private int framenum = 0;
    /************************************    Properties  *************************************/
    #region Properties
    private Pvr_UnitySDKEye[] eyes = null;
    public Pvr_UnitySDKEye[] Eyes
    {
        get
        {
            if (eyes == null)
            {
                eyes = GetComponentsInChildren<Pvr_UnitySDKEye>(true).ToArray();
            }
            return eyes;
        }
    }

    public Camera ControllerCamera
    {
        get
        {
            return GetComponent<Camera>();
        }
    }

    private bool renderedStereo = false;

    private int ScreenHeight
    {
        get
        {
            return Screen.height - (Application.isEditor ? 36 : 0);
        }
    }
    #endregion

    /************************************ Process Interface  *********************************/
    #region  Process Interface
    public void AddStereoRig()
    {
        if (Eyes.Length > 0)
        {
            return;
        }
        CreateEye(Pvr_UnitySDKAPI.Eye.LeftEye);
        CreateEye(Pvr_UnitySDKAPI.Eye.RightEye);
    }

    private void CreateEye(Pvr_UnitySDKAPI.Eye eye)
    {
        string nm = name + (eye == Pvr_UnitySDKAPI.Eye.LeftEye ? " LeftEye" : " RightEye");
        GameObject go = new GameObject(nm);
        go.transform.parent = transform;
        go.AddComponent<Camera>().enabled = true;
#if !UNITY_5
    if (GetComponent<GUILayer>() != null) {
      go.AddComponent<GUILayer>();
    }
    if (GetComponent("FlareLayer") != null) {
      go.AddComponent<FlareLayer>();
    }
#endif
        var picovrEye = go.AddComponent<Pvr_UnitySDKEye>();
        picovrEye.eye = eye;
    }

    private void FillScreenRect(int width, int height, Color color)
    {
        int x = Screen.width / 2;
        int y = Screen.height / 2 - 15;
        width /= 2;
        height /= 2;
        Pvr_UnitySDKManager.SDK.Middlematerial.color = color;
        Pvr_UnitySDKManager.SDK.Middlematerial.SetPass(0);
        GL.PushMatrix();
        GL.LoadPixelMatrix();
        GL.Color(Color.white);
        GL.Begin(GL.QUADS);
        GL.Vertex3(x - width, y - height, 0);
        GL.Vertex3(x - width, y + height, 0);
        GL.Vertex3(x + width, y + height, 0);
        GL.Vertex3(x + width, y - height, 0);
        GL.End();
        GL.PopMatrix();

    }
    #endregion

    /*************************************  Unity API ****************************************/
    #region Unity API
    void Awake()
    {
        AddStereoRig();
    }

    void OnEnable()
    {
        StartCoroutine("EndOfFrame");
    }

    void Update()
    {

        ControllerCamera.enabled = !Pvr_UnitySDKManager.SDK.VRModeEnabled;
#if UNITY_EDITOR
        for (int i = 0; i < Eyes.Length; i++)
        {
            Eyes[i].eyecamera.enabled = Pvr_UnitySDKManager.SDK.VRModeEnabled;
        } 
#endif

        if (!Pvr_UnitySDKManager.SDK.IsViewerLogicFlow)
        {
            for (int i = 0; i < Eyes.Length; i++)
            {
                Eyes[i].EyeRender();
            }
        }
    }
    void OnDisable()
    {
        StopAllCoroutines();
    }

    #endregion

    /************************************    IEnumerator  *************************************/
    IEnumerator EndOfFrame()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
			if (isfirst&&framenum==3) {
				Pvr_UnitySDKAPI.System.UPvr_RemovePlatformLogo ();
				Pvr_UnitySDKAPI.System.UPvr_StartVRModel ();
				isfirst =false;
			}else if(isfirst&&framenum<3){
				Debug.Log("+++++++++++++++++++++++++++++++"+framenum);
				framenum++;
			}

#if UNITY_5_6
#else

            Pvr_UnitySDKPluginEvent.IssueWithData(RenderEventType.TimeWarp, Pvr_UnitySDKManager.SDK.RenderviewNumber);
            Pvr_UnitySDKManager.SDK.currEyeTextureIdx = Pvr_UnitySDKManager.SDK.nextEyeTextureIdx;
            Pvr_UnitySDKManager.SDK.nextEyeTextureIdx = (Pvr_UnitySDKManager.SDK.nextEyeTextureIdx + 1) % 3;
#endif


        }
    }

}