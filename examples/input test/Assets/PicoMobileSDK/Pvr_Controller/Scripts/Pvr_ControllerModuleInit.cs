using System;
using UnityEngine;
using System.Collections;
using Pvr_UnitySDKAPI;


namespace Pvr_UnitySDKAPI
{
    public enum ControllerVariety
    {
        Controller0,
        Controller1,
    }
}   

public class Pvr_ControllerModuleInit : MonoBehaviour
{
    
    public ControllerVariety Variety;
    public bool IsCustomModel = false;
    [SerializeField]
    private GameObject dot;
    [SerializeField]
    private GameObject rayLine;
    [SerializeField]
    private GameObject controller;
    private int controllerDof = -1;
    private int mainHand = 0;
    private bool moduleState = true;

    void Awake()
    {
        Pvr_ControllerManager.PvrServiceStartSuccessEvent += ServiceStartSuccess;
        Pvr_ControllerManager.SetControllerAbilityEvent += CheckControllerStateOfAbility;
        Pvr_ControllerManager.ControllerStatusChangeEvent += CheckControllerStateForGoblin;
    }
    void OnDestroy()
    {
        Pvr_ControllerManager.PvrServiceStartSuccessEvent -= ServiceStartSuccess;
        Pvr_ControllerManager.SetControllerAbilityEvent -= CheckControllerStateOfAbility;
        Pvr_ControllerManager.ControllerStatusChangeEvent -= CheckControllerStateForGoblin;
    }

    private void ServiceStartSuccess()
    {
        mainHand = Controller.UPvr_GetMainHandNess();
        if (Variety == ControllerVariety.Controller0)
        {
            StartCoroutine(ShowAndHideRay(mainHand == 0 && Pvr_ControllerManager.controllerlink.controller0Connected));
            
        }
        if (Variety == ControllerVariety.Controller1)
        {
            StartCoroutine(ShowAndHideRay(mainHand == 1 && Pvr_ControllerManager.controllerlink.controller1Connected));
        }
    }

    private void CheckControllerStateForGoblin(string state)
    {
        if (Pvr_ControllerManager.controllerlink.controller0Connected)
        {
            moduleState = true;
            controller.transform.localScale = Vector3.one;
        }
        if (Variety == ControllerVariety.Controller0)
        {
            StartCoroutine(ShowAndHideRay(Convert.ToBoolean(Convert.ToInt16(state))));
        }
    }

    private void CheckControllerStateOfAbility(string data)
    {
        mainHand = Controller.UPvr_GetMainHandNess();
        if (Pvr_ControllerManager.controllerlink.controller0Connected ||
            Pvr_ControllerManager.controllerlink.controller1Connected)
        {
            moduleState = true;
            controller.transform.localScale = Vector3.one;
        }
        if (Variety == ControllerVariety.Controller0)
        {
            StartCoroutine(ShowAndHideRay(mainHand == 0 && Pvr_ControllerManager.controllerlink.controller0Connected));

        }
        if (Variety == ControllerVariety.Controller1)
        {
            StartCoroutine(ShowAndHideRay(mainHand == 1 && Pvr_ControllerManager.controllerlink.controller1Connected));
        }
    }
    
    private IEnumerator ShowAndHideRay(bool state)
    {
        yield return null;
        yield return null;
        if (moduleState)
        {
            dot.SetActive(state);
            rayLine.SetActive(state);
        }
    }

    public void ForceHideOrShow(bool state)
    {
        dot.SetActive(state);
        rayLine.SetActive(state);
        controller.transform.localScale = state ? Vector3.one : Vector3.zero;
        moduleState = state;
    }
}
