using UnityEngine;
using System.Collections;
using Pvr_UnitySDKAPI;
using UnityEngine.UI;


namespace Pvr_UnitySDKAPI
{
    public enum ControllerDevice
    {
        Goblin1,
        Neo,
        Goblin2,
        NewController,
    }
}

public class Pvr_ControllerVisual : MonoBehaviour
{
    
    public ControllerDevice currentDevice;
    
    public Texture2D m_idle;
    public Texture2D m_app;
    public Texture2D m_home;
    public Texture2D m_touchpad;
    public Texture2D m_volUp;
    public Texture2D m_volDn;
    public Texture2D m_trigger;
    
    private Renderer controllerRenderMat;

    [HideInInspector]
    public ControllerVariety variety;

    void Awake()
    {
        controllerRenderMat = GetComponent<Renderer>();
        
    }
    void Start()
    {
        variety = transform.GetComponentInParent<Pvr_ControllerModuleInit>().Variety;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeKeyEffects(variety == ControllerVariety.Controller0 ? 0 : 1);
    }

    
    private void ChangeKeyEffects(int hand)
    {
        if (Controller.UPvr_GetKey(hand, Pvr_KeyCode.TOUCHPAD))
        {
            controllerRenderMat.material.SetTexture("_MainTex", m_touchpad);
            controllerRenderMat.material.SetTexture("_EmissionMap", m_touchpad);
        }
        else if (Controller.UPvr_GetKey(hand, Pvr_KeyCode.APP))
        {
            controllerRenderMat.material.SetTexture("_MainTex", m_app);
            controllerRenderMat.material.SetTexture("_EmissionMap", m_app);
        }
        else if (Controller.UPvr_GetKey(hand, Pvr_KeyCode.HOME))
        {
            controllerRenderMat.material.SetTexture("_MainTex", m_home);
            controllerRenderMat.material.SetTexture("_EmissionMap", m_home);
        }
        else if (Controller.UPvr_GetKey(hand, Pvr_KeyCode.VOLUMEUP))
        {
            controllerRenderMat.material.SetTexture("_MainTex", m_volUp);
            controllerRenderMat.material.SetTexture("_EmissionMap", m_volUp);
        }
        else if (Controller.UPvr_GetKey(hand, Pvr_KeyCode.VOLUMEDOWN))
        {
            controllerRenderMat.material.SetTexture("_MainTex", m_volDn);
            controllerRenderMat.material.SetTexture("_EmissionMap", m_volDn);
        }
        else if (Controller.UPvr_GetControllerTriggerValue(hand) > 0 || Controller.UPvr_GetKey(hand,Pvr_KeyCode.TRIGGER))
        {
            if (currentDevice != ControllerDevice.Goblin1)
            {
                controllerRenderMat.material.SetTexture("_MainTex", m_trigger);
                controllerRenderMat.material.SetTexture("_EmissionMap", m_trigger);
            }
        }
        else
        {
            if (controllerRenderMat.material.GetTexture("_MainTex") != m_idle)
            {
                controllerRenderMat.material.SetTexture("_MainTex", m_idle);
                controllerRenderMat.material.SetTexture("_EmissionMap", m_idle);
            }
        }
    }
}
