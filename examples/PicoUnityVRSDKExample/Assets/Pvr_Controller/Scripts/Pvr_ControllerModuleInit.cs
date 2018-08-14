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
    private int controllerDof = -1;
    private int mainHand = 0;
    void Awake()
    {
        Pvr_ControllerManager.PvrServiceStartSuccessEvent += ServiceStartSuccess;
        Pvr_ControllerManager.SetControllerAbilityEvent += CheckControllerStateOfAbility;
        Pvr_ControllerManager.ControllerStatusChangeEvent += CheckControllerStateForGoblin;
    }
    void OnDestroy()
    {
        Pvr_ControllerManager.ControllerThreadStartedCallbackEvent -= ServiceStartSuccess;
        Pvr_ControllerManager.SetControllerAbilityEvent -= CheckControllerStateOfAbility;
        Pvr_ControllerManager.ControllerStatusChangeEvent -= CheckControllerStateForGoblin;
    }

    private void ServiceStartSuccess()
    {
        mainHand = Controller.UPvr_GetMainHandNess();
        if (Variety == ControllerVariety.Controller0)
        {
            ShowAndHideRay(mainHand == 0 && Pvr_ControllerManager.controllerlink.controller0Connected);
        }
        if (Variety == ControllerVariety.Controller1)
        {
            ShowAndHideRay(mainHand == 1 && Pvr_ControllerManager.controllerlink.controller1Connected);
        }
    }

    private void CheckControllerStateForGoblin(string state)
    {
        if (Variety == ControllerVariety.Controller0)
        {
            ShowAndHideRay(Convert.ToBoolean(Convert.ToInt16(state)));
        }
    }

    private void CheckControllerStateOfAbility(string data)
    {
        mainHand = Controller.UPvr_GetMainHandNess();
        var state = Convert.ToBoolean(Convert.ToInt16(data.Substring(4, 1)));
        var id = Convert.ToInt16(data.Substring(0, 1));
        switch (id)
        {
            case 0:
                if (Variety == ControllerVariety.Controller0)
                {
                    ShowAndHideRay(mainHand == 0&& state);
                }
                break;
            case 1:
                if (Variety == ControllerVariety.Controller1)
                {
                    ShowAndHideRay(mainHand == 1 && state);
                }
                break;
        }
    }
    
    private void ShowAndHideRay(bool state)
    {
        dot.SetActive(state);
        rayLine.SetActive(state);
    }
}
