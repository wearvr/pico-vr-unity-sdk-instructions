using System;
using UnityEngine;
using System.Collections;
using Pvr_UnitySDKAPI;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class Pvr_ControllerInit : MonoBehaviour {

    
    private ControllerVariety Variety;
    private bool isCustomModel = false;
    [SerializeField]
    private GameObject controller1;
    [SerializeField]
    private GameObject controller2;
    [SerializeField]
    private GameObject controller3;
    [SerializeField]
    private Material generalMat;
    [SerializeField]
    private GameObject controllerpower;
    [SerializeField]
    private GameObject controllertips;
    [SerializeField]
    private GameObject pptouch;
    
    private int controllerType = -1;
    [HideInInspector]
    public bool loadModelSuccess = false;

    private string modelFilePath = "/system/media/PvrRes/controller/";
    private int systemOrUnity = 0;

    void Awake()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        int enumindex = (int)GlobalIntConfigs.iCtrlModelLoadingPri;
        Render.UPvr_GetIntConfig(enumindex, ref systemOrUnity);
#endif
        DestroyController();
        Variety = transform.GetComponentInParent<Pvr_ControllerModuleInit>().Variety;
        isCustomModel = transform.GetComponentInParent<Pvr_ControllerModuleInit>().IsCustomModel;
        if (!isCustomModel)
        {
            Pvr_ControllerManager.PvrServiceStartSuccessEvent += ServiceStartSuccess;
            Pvr_ControllerManager.SetControllerAbilityEvent += CheckControllerStateOfAbility;
            Pvr_ControllerManager.ControllerStatusChangeEvent += CheckControllerStateForGoblin;
        }

#if UNITY_EDITOR
        controller3.SetActive(true);
#endif
    }
    void OnDestroy()
    {
        Pvr_ControllerManager.PvrServiceStartSuccessEvent -= ServiceStartSuccess;
        Pvr_ControllerManager.SetControllerAbilityEvent -= CheckControllerStateOfAbility;
        Pvr_ControllerManager.ControllerStatusChangeEvent -= CheckControllerStateForGoblin;
    }
    private void ServiceStartSuccess()
    {
        var type = Controller.UPvr_GetDeviceType();
        if (controllerType != type && type != 0)
        {
            controllerType = type;
        }
        if (Pvr_ControllerManager.controllerlink.neoserviceStarted)
        {
            if (Variety == ControllerVariety.Controller0)
            {
                if (Pvr_ControllerManager.controllerlink.controller0Connected)
                {
                    if (type == 0)
                    {
                        controller1.SetActive(false);
                        controller2.SetActive(true);
                        controller3.SetActive(false);
                        loadModelSuccess = true;
                    }
                    else
                    {
                        StartCoroutine(RefreshController());
                    }
                }
                else
                {
                    HideController();
                }
            }
            if (Variety == ControllerVariety.Controller1)
            {
                if (Pvr_ControllerManager.controllerlink.controller1Connected)
                {
                    if (type == 0)
                    {
                        controller1.SetActive(false);
                        controller2.SetActive(true);
                        controller3.SetActive(false);
                        loadModelSuccess = true;
                    }
                    else
                    {
                        StartCoroutine(RefreshController());
                    }
                }
                else
                {
                    HideController();
                }
            }
        }
        if (Pvr_ControllerManager.controllerlink.goblinserviceStarted)
        {
            if (Variety == ControllerVariety.Controller0)
            {
                if (Pvr_ControllerManager.controllerlink.controller0Connected)
                {
                    if (type == 0)
                    {
                        controller1.SetActive(true);
                        controller2.SetActive(false);
                        controller3.SetActive(false);
                        loadModelSuccess = true;
                    }
                    else
                    {
                        StartCoroutine(RefreshController());
                    }
                }
                else
                {
                    HideController();
                }
            }
        }
    }

    private void CheckControllerStateForGoblin(string state)
    {
        var type = Controller.UPvr_GetDeviceType();
        if (Variety == ControllerVariety.Controller0)
        {
            if (Convert.ToInt16(state) == 1)
            {
                if (controllerType != type)
                {
                    DestroyController();
                    controllerType = type;
                    StartCoroutine(RefreshController());
                }
                else
                {
                    StartCoroutine(RefreshController());
                }
            }
            else
            {
                HideController();
            }
        }
    }

    private void CheckControllerStateOfAbility(string data)
    {
        var state = Convert.ToBoolean(Convert.ToInt16(data.Substring(4, 1)));
        var id = Convert.ToInt16(data.Substring(0, 1));
        if (state)
        {
            controllerType = 2;
            switch (id)
            {
                case 0:
                    if (Variety == ControllerVariety.Controller0)
                    {
                        StartCoroutine(RefreshController());
                    }
                    break;
                case 1:
                    if (Variety == ControllerVariety.Controller1)
                    {
                        StartCoroutine(RefreshController());
                    }
                    break;
            }
        }
        else
        {
            switch (id)
            {
                case 0:
                    if (Variety == ControllerVariety.Controller0)
                    {
                        HideController();
                    }
                    break;
                case 1:
                    if (Variety == ControllerVariety.Controller1)
                    {
                        HideController();
                    }
                    break;
            }
        }
    }

    private void HideController()
    {
        foreach (Transform t in transform)
        {
            if (t.gameObject.activeSelf)
            {
                t.gameObject.SetActive(false);
                loadModelSuccess = false;
            }
        }
    }
    private void DestroyController()
    {
        foreach(Transform t in transform)
        {
            if (t.name == "controller" + controllerType + "s")
            {
                Destroy(t.gameObject);
                loadModelSuccess = false;
            }
        }
    }
   
    private IEnumerator RefreshController()
    {
        yield return null;
        yield return null;
        var isControllerExist = false;
        foreach (Transform t in transform)
        {
            if (t.name == "controller" + controllerType + "s")
            {
                isControllerExist = true;
            }
        }
        if (!isControllerExist)
        {
            if (systemOrUnity == 0)
            {
                LoadControllerFromPrefab();
                if (!loadModelSuccess)
                {
                    LoadControllerFromSystem();
                }
            }
            else
            {
                LoadControllerFromSystem();
                if (!loadModelSuccess)
                {
                    LoadControllerFromPrefab();
                }
            }
        }
        else
        {
            var currentController = transform.Find("controller" + controllerType + "s");
            currentController.gameObject.SetActive(true);
        }
    }

    private void  LoadControllerFromSystem()
    {
        var syscontrollername = "controller" + controllerType + "s.obj";
        var fullFilePath = modelFilePath + "/" + syscontrollername ;
        if (!File.Exists(fullFilePath))
        {
            Debug.Log("Obj File does not exist");
        }
        else
        {
            GameObject go = new GameObject();
            go.name = "controller" + controllerType + "s";
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();
            meshFilter.mesh = Pvr_ObjImporter.Instance.ImportFile(fullFilePath);
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            
            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            meshRenderer.material = generalMat;
            loadModelSuccess = true;
            Pvr_ControllerVisual controllerVisual = go.AddComponent<Pvr_ControllerVisual>();
            switch (controllerType)
            {
                case 1:
                    {
                        controllerVisual.currentDevice = ControllerDevice.Goblin1;
                    }
                    break;
                case 2:
                    {
                        controllerVisual.currentDevice = ControllerDevice.Neo;
                    }
                    break;
                case 3:
                    {
                        controllerVisual.currentDevice = ControllerDevice.Goblin2;
                    }
                    break;
                default:
                    controllerVisual.currentDevice = ControllerDevice.NewController;
                    break;
            }
            controllerVisual.variety = Variety;
            LoadTextureFromSystem(controllerVisual);
            if (controllerType <= 3)
            {
                var touch = Instantiate(pptouch);
                touch.transform.SetParent(go.transform);
                touch.GetComponent<Pvr_TouchVisual>().currentDevice = controllerVisual.currentDevice;
                touch.GetComponent<Pvr_TouchVisual>().variety = Variety;
                switch (controllerType)
                {
                    case 2:
                        {
                            var power = Instantiate(controllerpower);
                            power.transform.SetParent(go.transform);
                            power.transform.localPosition = new Vector3(0, 1.07f, 3.73f);
                        }
                        break;
                    case 3:
                        {
                            var power = Instantiate(controllerpower);
                            power.transform.SetParent(go.transform);
                            power.transform.localPosition = new Vector3(0, 1.576f, 3.73f);
                            var tips = Instantiate(controllertips);
                            tips.transform.SetParent(go.transform);
                            tips.transform.localPosition = new Vector3(-0.143f, 0.44f, -0.87f);
                        }
                        break;
                }
            }
            go.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            go.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }

    private void LoadControllerFromPrefab()
    {
        switch (controllerType)
        {
            case 1:
                controller1.SetActive(true);
                controller2.SetActive(false);
                controller3.SetActive(false);
                loadModelSuccess = true;
                break;
            case 2:
                controller1.SetActive(false);
                controller2.SetActive(true);
                controller3.SetActive(false);
                loadModelSuccess = true;
                break;
            case 3:
                controller1.SetActive(false);
                controller2.SetActive(false);
                controller3.SetActive(true);
                loadModelSuccess = true;
                break;
            default:
                controller1.SetActive(false);
                controller2.SetActive(false);
                controller3.SetActive(false);
                loadModelSuccess = false;
                break;
        }
    }

    private void LoadTextureFromSystem(Pvr_ControllerVisual visual)
    {
        var texturepath = modelFilePath + "controller" + controllerType + "s_idle.png";
        visual.m_idle = LoadOneTexture(texturepath);
        texturepath = modelFilePath + "controller" + controllerType + "s_app.png";
        visual.m_app = LoadOneTexture(texturepath);
        texturepath = modelFilePath + "controller" + controllerType + "s_home.png";
        visual.m_home = LoadOneTexture(texturepath);
        texturepath = modelFilePath + "controller" + controllerType + "s_touchpad.png";
        visual.m_touchpad = LoadOneTexture(texturepath);
        texturepath = modelFilePath + "controller" + controllerType + "s_volumedown.png";
        visual.m_volDn = LoadOneTexture(texturepath);
        texturepath = modelFilePath + "controller" + controllerType + "s_volumeup.png";
        visual.m_volUp= LoadOneTexture(texturepath);
        if (Controller.UPvr_IsEnbleTrigger())
        {
            texturepath = modelFilePath + "controller" + controllerType + "s_trigger.png";
            visual.m_trigger = LoadOneTexture(texturepath);
        }
    }

    private Texture2D LoadOneTexture(string filepath)
    {
        var m_tex = new Texture2D(1024, 1024);
        m_tex.LoadImage(ReadPNG(filepath));
        return m_tex;
    }
    private byte[] ReadPNG(string path)
    {
        FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

        fileStream.Seek(0, SeekOrigin.Begin);

        byte[] binary = new byte[fileStream.Length];
        fileStream.Read(binary, 0, (int)fileStream.Length);

        fileStream.Close();

        fileStream.Dispose();

        fileStream = null;

        return binary;
    }
}
