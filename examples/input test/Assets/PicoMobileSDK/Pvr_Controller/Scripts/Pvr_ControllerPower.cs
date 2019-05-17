using UnityEngine;
using System.Collections;
using Pvr_UnitySDKAPI;

public class Pvr_ControllerPower : MonoBehaviour
{

    [SerializeField]
    private Material power1;
    [SerializeField]
    private Material power1Red;
    [SerializeField]
    private Material power2;
    [SerializeField]
    private Material power3;
    [SerializeField]
    private Material power4;
    [SerializeField]
    private Material power5;
    
    [HideInInspector]
    public ControllerVariety variety;
    [HideInInspector]
    public ControllerDevice currentDevice;

    private MeshRenderer powerRenderMat;
    private int powerValue;
    
    void Start()
    {
        powerRenderMat = GetComponent<MeshRenderer>();
        powerValue = -1;
        variety = transform.GetComponentInParent<Pvr_ControllerModuleInit>().Variety;
        currentDevice = transform.GetComponentInParent<Pvr_ControllerVisual>().currentDevice;
    }
    // Update is called once per frame
    void Update()
    {
        RefreshPower(variety == ControllerVariety.Controller0
            ? 0
            : 1);
    }

    private void RefreshPower(int hand)
    {
        switch (currentDevice)
        {
            case ControllerDevice.Neo:
                {
                    if (powerValue == 1)
                    {
                        powerRenderMat.enabled = true;
                    }
                    else
                    {
                        powerRenderMat.enabled = Vector3.Distance(Controller.UPvr_GetControllerPOS(hand), Pvr_UnitySDKManager.SDK.HeadPose.Position) <= 0.35f;
                    }
                    if (powerRenderMat.enabled && powerValue != Controller.UPvr_GetControllerPower(hand))
                    {
                        switch (Controller.UPvr_GetControllerPower(hand))
                        {
                            case 1:
                                powerRenderMat.material = power1Red;
                                break;
                            case 2:
                                powerRenderMat.material = power1;
                                break;
                            case 3:
                                powerRenderMat.material = power1;
                                break;
                            case 4:
                                powerRenderMat.material = power2;
                                break;
                            case 5:
                                powerRenderMat.material = power2;
                                break;
                            case 6:
                                powerRenderMat.material = power3;
                                break;
                            case 7:
                                powerRenderMat.material = power3;
                                break;
                            case 8:
                                powerRenderMat.material = power4;
                                break;
                            case 9:
                                powerRenderMat.material = power4;
                                break;
                            case 10:
                                powerRenderMat.material = power5;
                                break;
                            default:
                                powerRenderMat.material = power1;
                                break;
                        }
                        powerValue = Controller.UPvr_GetControllerPower(hand);
                    }
                }
                break;
            case ControllerDevice.Goblin2:
                {
                    if (Pvr_ControllerManager.controllerlink.controller0Connected)
                    {
                        if (powerValue == 0)
                        {
                            powerRenderMat.enabled = true;
                        }
                        else
                        {
                            powerRenderMat.enabled = Vector3.Distance(transform.parent.parent.parent.localPosition,
                                                  Pvr_UnitySDKManager.SDK.HeadPose.Position) <= 0.35f;
                        }
                        if (powerRenderMat.enabled && powerValue != Controller.UPvr_GetControllerPower(0))
                        {
                            switch (Controller.UPvr_GetControllerPower(0))
                            {
                                case 0:
                                    powerRenderMat.material = power1Red;
                                    break;
                                case 1:
                                    powerRenderMat.material = power2;
                                    break;
                                case 2:
                                    powerRenderMat.material = power3;
                                    break;
                                case 3:
                                    powerRenderMat.material = power4;
                                    break;
                                case 4:
                                    powerRenderMat.material = power5;
                                    break;
                                default:
                                    powerRenderMat.material = power1;
                                    break;
                            }
                            powerValue = Controller.UPvr_GetControllerPower(0);
                        }
                    }
                    else
                    {
                        powerRenderMat.enabled = false;
                    }
                }
                break;
        }
    }
}
