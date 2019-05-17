///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_UnitySDKEye
// Author: AiLi.Shang
// Date:  2017/02/09
// Discription: Be fully careful of  Code modification
///////////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine;
using Pvr_UnitySDKAPI;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class Pvr_UnitySDKEye : MonoBehaviour
{

    /************************************    Properties  *************************************/
    #region Properties
    public Eye eye;

    //new public Camera eyecamera { get; private set; }
    public Camera eyecamera { get; private set; }

    private Pvr_UnitySDKEyeManager controller;

    public Pvr_UnitySDKEyeManager Controller
    {
        get
        {
            if (transform.parent == null)
            {
                return null;
            }
            if ((Application.isEditor && !Application.isPlaying) || controller == null)
            {
                return transform.parent.GetComponentInParent<Pvr_UnitySDKEyeManager>();
            }
            return controller;
        }
    }

    Matrix4x4 realProj = Matrix4x4.identity; 

    private const int bufferSize = 3;

    private int IDIndex = 0;

    private RenderEventType eventType = 0;


    public bool isFadeUSing = false;

    public float fadeTime = 5.0f;
    private float blacklastTime = 0.5f;
    public Color fadeColor = new Color(0f, 0f, 0f, 1.0f); 
    private Material fadeMaterial = null;
    private bool isFading = false;
    private float initialBlackTime = 0.0f;
    private float totalBlackTime = 0.0f;


    #endregion

    /************************************ Public Interfaces  *********************************/
    #region Public Interfaces

    public void EyeRender()
    {
        SetupUpdate();
        if (Pvr_UnitySDKManager.SDK.eyeTextures[IDIndex] != null)
        {
            Pvr_UnitySDKManager.SDK.eyeTextures[IDIndex].DiscardContents();
            eyecamera.targetTexture = Pvr_UnitySDKManager.SDK.eyeTextures[IDIndex];
        }
    }

    #endregion

    /************************************ Private Interfaces  *********************************/
    #region Private Interfaces
    private void Setup(Eye eye)
    {
        eyecamera = GetComponent<Camera>();
        transform.localPosition = Pvr_UnitySDKManager.SDK.EyeOffset(eye);
        eyecamera.aspect = 1.0f;
        eyecamera.rect = new Rect(0, 0, 1, 1);
#if UNITY_EDITOR
        eyecamera.rect = Pvr_UnitySDKManager.SDK.EyeRect(eye);
#endif

        //  AW     

        if (Pvr_UnitySDKManager.SDK.IsViewerLogicFlow)
        {
            eyecamera.rect = Pvr_UnitySDKManager.SDK.EyeRect(eye);
            Rect left = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
            Rect right = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
            if (eye == Eye.LeftEye)
                eyecamera.rect = left;
            else
                eyecamera.rect = right;
            eyecamera.fieldOfView = Pvr_UnitySDKManager.SDK.EyeFov;
        }
        //  AW
        eventType = (eye == Pvr_UnitySDKAPI.Eye.LeftEye) ?
                        RenderEventType.LeftEyeEndFrame :
                        RenderEventType.RightEyeEndFrame;
    }

    private void SetupUpdate()
    {
        eyecamera.fieldOfView = Pvr_UnitySDKManager.SDK.EyeFov;
        IDIndex = Pvr_UnitySDKManager.SDK.currEyeTextureIdx + (int)eye * bufferSize;    
        eyecamera.enabled = true;   
    }

    #region  DrawVignetteLine

    private Material mat_Vignette;

    void DrawVignetteLine()
    {
        if (null == mat_Vignette)
        {
            mat_Vignette = new Material(Shader.Find("Diffuse"));//Mobile/
            if (null == mat_Vignette)
            {
                return;
            }
        }
        GL.PushMatrix();
        mat_Vignette.SetPass(0);
        GL.LoadOrtho();
        vignette();
        GL.PopMatrix();
        screenFade();
    }

    void screenFade()
    {
        if (isFading)
        {
            fadeMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Color(fadeMaterial.color);
            GL.Begin(GL.QUADS);
            GL.Vertex3(0f, 0f, -12f);
            GL.Vertex3(0f, 1f, -12f);
            GL.Vertex3(1f, 1f, -12f);
            GL.Vertex3(1f, 0f, -12f);
            GL.End();
            GL.PopMatrix();
        }
    }
    
    IEnumerator  ScreenFade()
    {
        float elapsedTime = 0.0f;
        fadeMaterial.color = fadeColor;
        Color color = fadeColor;
        isFading = true;
        while (elapsedTime < fadeTime)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            color.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeTime);
            fadeMaterial.color = color;
        }
        isFading = false;   
    }
    void vignette()
    {
        //GL.Begin(GL.LINES);//can not to set line width
        GL.Begin(GL.QUADS);
        GL.Color(Color.black);
        //top
        GL.Vertex3(0.0f, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.995f, 0.0f);
        GL.Vertex3(0.0f, 0.995f, 0.0f);
        //bottom
        GL.Vertex3(0.0f, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.005f, 0.0f);
        GL.Vertex3(1.0f, 0.005f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 0.0f);
        //left
        GL.Vertex(new Vector3(0.0f, 1.0f, 0.0f));
        GL.Vertex(new Vector3(0.005f, 1.0f, 0.0f));
        GL.Vertex(new Vector3(0.005f, 0.0f, 0.0f));
        GL.Vertex(new Vector3(0.0f, 0.0f, 0.0f));
        //right
        GL.Vertex(new Vector3(0.995f, 1.0f, 0.0f));
        GL.Vertex(new Vector3(1.0f, 1.0f, 0.0f));
        GL.Vertex(new Vector3(1.0f, 0.0f, 0.0f));
        GL.Vertex(new Vector3(0.995f, 0.0f, 0.0f));
        GL.End();
    }

    #endregion
      
    #endregion

    /*************************************  Unity API ****************************************/
    #region Unity API
    void Awake()
    {
        eyecamera = this.GetComponent<Camera>();
        if (Pvr_UnitySDKManager.SDK.HeadDofNum == HeadDofNum.SixDof)
        {
            initialBlackTime = 0.0f;
            fadeMaterial = new Material(Shader.Find("Pvr_UnitySDK/Fade"));
        }
    }

    void Start()
    {
        Setup(eye);
        //realProj = eyecamera.cameraToWorldMatrix;
        eyecamera.enabled = true;    
    }

    void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            if (Pvr_UnitySDKManager.SDK.HeadDofNum == HeadDofNum.SixDof)
            {
                initialBlackTime = 0.0f;
            }
        }
    }
    void Update()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        if (Pvr_UnitySDKManager.SDK.HeadDofNum == HeadDofNum.SixDof)
        {
            if (Pvr_UnitySDKManager.SDK.DefaultRange)
            {
                if (Mathf.Sqrt(Mathf.Pow(Pvr_UnitySDKManager.SDK.HeadPose.Position.x,2.0f) + Mathf.Pow(Pvr_UnitySDKManager.SDK.HeadPose.Position.z, 2.0f)) >= 0.8f)
                {
                    isFading = true;
                    fadeMaterial.color = new Color(0f, 0f, 0f,
                        Mathf.Clamp((Mathf.Max(Mathf.Abs(Pvr_UnitySDKManager.SDK.HeadPose.Position.x),
                                         Mathf.Abs(Pvr_UnitySDKManager.SDK.HeadPose.Position.z)) - 0.8f) /
                                    0.16f, 0f, 0.3f));
                }
                else
                {
                    fadeMaterial.color = new Color(0f, 0f, 0f, 0f);
                    isFading = false;
                }
            }
            else
            {
                if (Mathf.Sqrt(Mathf.Pow(Pvr_UnitySDKManager.SDK.HeadPose.Position.x, 2.0f) + Mathf.Pow(Pvr_UnitySDKManager.SDK.HeadPose.Position.z, 2.0f)) >= Pvr_UnitySDKManager.SDK.CustomRange)
                {
                    isFading = true;
                    fadeMaterial.color = new Color(0f, 0f, 0f,
                        Mathf.Clamp((Mathf.Max(Mathf.Abs(Pvr_UnitySDKManager.SDK.HeadPose.Position.x),
                                         Mathf.Abs(Pvr_UnitySDKManager.SDK.HeadPose.Position.z)) - Pvr_UnitySDKManager.SDK.CustomRange) /
                                    (Pvr_UnitySDKManager.SDK.CustomRange / 5f), 0f, 0.3f));
                }
                else
                {
                    fadeMaterial.color = new Color(0f, 0f, 0f, 0f);
                    isFading = false;
                }
            }
        }
#endif
    }
    void OnEnable()
    {
        isFadeUSing = Pvr_UnitySDKManager.SDK.ScreenFade;  
        if (isFadeUSing)
        {
            fadeMaterial = new Material(Shader.Find("Pvr_UnitySDK/Fade"));
            if (fadeMaterial != null)
            {
                Debug.Log("Get fade material success");
            }
            else
            {
                Debug.Log("Get fade material Error");
                isFadeUSing = false;
            }
            StartCoroutine(ScreenFade());
        }
    }
   /*
    void OnPreCull()
    {
        if (!Pvr_UnitySDKManager.SDK.VRModeEnabled)
        {
            return;
        }
        //}
        //SetupUpdate();
        //if (Pvr_UnitySDKManager.SDK.eyeTextures[IDIndex] != null)
        //{
        //    Pvr_UnitySDKManager.SDK.eyeTextures[IDIndex].DiscardContents();
        //    eyecamera.targetTexture = Pvr_UnitySDKManager.SDK.eyeTextures[IDIndex];
        //}
    }
    */
	public static bool setLevel = false;
    void OnPostRender()
    {
        //DrawVignetteLine();
        screenFade();
        int eyeTextureId = Pvr_UnitySDKManager.SDK.eyeTextureIds[IDIndex];
       // SaveImage(Pvr_UnitySDKManager.SDK.eyeTextures[IDIndex], eye.ToString()); 
        Pvr_UnitySDKPluginEvent.IssueWithData(eventType, eyeTextureId);
#if !UNITY_EDITOR && UNITY_ANDROID
		if (eye == Eye.LeftEye && !setLevel && Pvr_UnitySDKManager.SDK.IsViewerLogicFlow)
        {   
            AndroidJavaClass AvrAPI = new UnityEngine.AndroidJavaClass("com.unity3d.player.AvrAPI");
            Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(AvrAPI, "setVrThread"); 
            setLevel = true;
            Debug.Log("Viewer setVrThread");
        }
        else
            return;
#endif
    }

#if UNITY_EDITOR
    void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        ModifyShadePara();
        Graphics.Blit(source, dest, Pvr_UnitySDKManager.SDK.Eyematerial);
    }

    void ModifyShadePara()
    {
        Matrix4x4 proj = Matrix4x4.identity;
        float near = GetComponent<Camera>().nearClipPlane;
        float far = GetComponent<Camera>().farClipPlane;
        float aspectFix = GetComponent<Camera>().rect.height / GetComponent<Camera>().rect.width / 2;

        proj[0, 0] *= aspectFix;
        Vector2 dir = transform.localPosition; // ignore Z
        dir = dir.normalized * 1.0f;
        proj[0, 2] *= Mathf.Abs(dir.x);
        proj[1, 2] *= Mathf.Abs(dir.y); proj[2, 2] = (near + far) / (near - far);
        proj[2, 3] = 2 * near * far / (near - far);

        Vector4 projvec = new Vector4(proj[0, 0], proj[1, 1],
                                    proj[0, 2] - 1, proj[1, 2] - 1) / 2;

        Vector4 unprojvec = new Vector4(realProj[0, 0], realProj[1, 1],
                                        realProj[0, 2] - 1, realProj[1, 2] - 1) / 2;

        float distortionFactor = 0.0241425f;
        Shader.SetGlobalVector("_Projection", projvec);
        Shader.SetGlobalVector("_Unprojection", unprojvec);
        Shader.SetGlobalVector("_Distortion1",
                                new Vector4(Pvr_UnitySDKManager.SDK.pvr_UnitySDKConfig.device.devDistortion.k1, Pvr_UnitySDKManager.SDK.pvr_UnitySDKConfig.device.devDistortion.k2, Pvr_UnitySDKManager.SDK.pvr_UnitySDKConfig.device.devDistortion.k3, distortionFactor));
        Shader.SetGlobalVector("_Distortion2",
                               new Vector4(Pvr_UnitySDKManager.SDK.pvr_UnitySDKConfig.device.devDistortion.k4, Pvr_UnitySDKManager.SDK.pvr_UnitySDKConfig.device.devDistortion.k5, Pvr_UnitySDKManager.SDK.pvr_UnitySDKConfig.device.devDistortion.k6));

    }
#endif
    #endregion

}