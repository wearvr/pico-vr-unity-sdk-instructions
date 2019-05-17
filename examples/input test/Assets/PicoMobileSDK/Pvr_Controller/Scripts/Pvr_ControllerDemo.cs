using System;
using UnityEngine;
using System.Collections;
using Pvr_UnitySDKAPI;

public class Pvr_ControllerDemo : MonoBehaviour
{
    public GameObject HeadSetController;
    public GameObject controller0;
    public GameObject controller1;

    public GameObject cube;

    private Ray ray;
    private GameObject currentController;

    private Transform lastHit;
    private Transform currentHit;

    [SerializeField]
    private Material normat;
    [SerializeField]
    private Material gazemat;
    [SerializeField]
    private Material clickmat;
    private bool noClick;
    // Use this for initialization
    void Start()
    {
        ray = new Ray();
        Pvr_ControllerManager.PvrServiceStartSuccessEvent += ServiceStartSuccess;
        Pvr_ControllerManager.SetControllerStateChangedEvent += ControllerStateListener;
        Pvr_ControllerManager.ControllerStatusChangeEvent += CheckControllerStateForGoblin;
#if UNITY_EDITOR
        currentController = controller0;
#endif
        if (Pvr_UnitySDKManager.SDK.HeadDofNum == HeadDofNum.SixDof || Pvr_UnitySDKManager.SDK.HandDofNum == HandDofNum.SixDof)
        {
            if (Pvr_UnitySDKManager.SDK.trackingmode == 0 || Pvr_UnitySDKManager.SDK.trackingmode == 1)
            {
                if(cube != null)
                    cube.GetComponent<BoxCollider>().enabled = false;
            }
        }
    }

    void OnDestroy()
    {
        Pvr_ControllerManager.PvrServiceStartSuccessEvent -= ServiceStartSuccess;
        Pvr_ControllerManager.SetControllerStateChangedEvent -= ControllerStateListener;
        Pvr_ControllerManager.ControllerStatusChangeEvent -= CheckControllerStateForGoblin;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        if (HeadSetController.activeSelf)
        {
            HeadSetController.transform.parent.localRotation = Quaternion.Euler(Pvr_UnitySDKManager.SDK.HeadPose.Orientation.eulerAngles.x, Pvr_UnitySDKManager.SDK.HeadPose.Orientation.eulerAngles.y, 0);

            ray.direction = HeadSetController.transform.position - HeadSetController.transform.parent.parent.Find("Head").position;
            ray.origin = HeadSetController.transform.parent.parent.Find("Head").position;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                currentHit = hit.transform;

                if (currentHit != null && lastHit != null && currentHit != lastHit)
                {
                    if (lastHit.GetComponent<Pvr_UIGraphicRaycaster>() && lastHit.transform.gameObject.activeInHierarchy && lastHit.GetComponent<Pvr_UIGraphicRaycaster>().enabled)
                    {
                        lastHit.GetComponent<Pvr_UIGraphicRaycaster>().enabled = false;
                    }
                }
                if (currentHit != null && lastHit != null && currentHit == lastHit)
                {
                    if (currentHit.GetComponent<Pvr_UIGraphicRaycaster>() && !currentHit.GetComponent<Pvr_UIGraphicRaycaster>().enabled)
                    {
                        currentHit.GetComponent<Pvr_UIGraphicRaycaster>().enabled = true;
                    }
                }

                if (1 << hit.transform.gameObject.layer == LayerMask.GetMask("Water"))
                {
                    if(!noClick)
                        hit.transform.GetComponent<Renderer>().material = gazemat;
                }
                lastHit = hit.transform;
                Debug.DrawLine(ray.origin, hit.point, Color.red);
            }
            else
            {
                if (lastHit != null && 1 << lastHit.transform.gameObject.layer == LayerMask.GetMask("Water"))
                {
                    lastHit.transform.GetComponent<Renderer>().material = normat;
                }
                currentHit = null;
                noClick = false;
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetMouseButtonDown(0))
            {
                if (lastHit != null && 1 << lastHit.transform.gameObject.layer == LayerMask.GetMask("Water") && currentHit != null)
                {
                    lastHit.transform.GetComponent<Renderer>().material = clickmat;
                    noClick = true;
                }
            }
        }
        else
        {
            if (currentController != null)
            {
                ray.direction = currentController.transform.Find("dot").position - currentController.transform.Find("start").position;
                ray.origin = currentController.transform.Find("start").position;
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    currentHit = hit.transform;

                    if (currentHit != null && lastHit != null && currentHit != lastHit)
                    {
                        if (lastHit.GetComponent<Pvr_UIGraphicRaycaster>() && lastHit.transform.gameObject.activeInHierarchy && lastHit.GetComponent<Pvr_UIGraphicRaycaster>().enabled)
                        {
                            lastHit.GetComponent<Pvr_UIGraphicRaycaster>().enabled = false;
                        }
                    }
                    if (currentHit != null && lastHit != null && currentHit == lastHit)
                    {
                        if (currentHit.GetComponent<Pvr_UIGraphicRaycaster>() && !currentHit.GetComponent<Pvr_UIGraphicRaycaster>().enabled)
                        {
                            currentHit.GetComponent<Pvr_UIGraphicRaycaster>().enabled = true;

                        }
                    }
                    if (1 << hit.transform.gameObject.layer == LayerMask.GetMask("Water"))
                    {
                        if (!noClick)
                            hit.transform.GetComponent<Renderer>().material = gazemat;
                    }
                    lastHit = hit.transform;
                    Debug.DrawLine(ray.origin, hit.point, Color.red);
                    currentController.transform.Find("dot").position = hit.point;
                }
                else
                {
                    if (lastHit != null && 1 << lastHit.transform.gameObject.layer == LayerMask.GetMask("Water"))
                    {
                        lastHit.transform.GetComponent<Renderer>().material = normat;
                    }
                    currentHit = null;
                    noClick = false;
                }
            }

            if (Controller.UPvr_GetKeyDown(0, Pvr_KeyCode.TOUCHPAD) ||
                Controller.UPvr_GetKeyDown(1, Pvr_KeyCode.TOUCHPAD) || Input.GetMouseButtonDown(0))
            {
                if (lastHit != null && 1 << lastHit.transform.gameObject.layer == LayerMask.GetMask("Water") && currentHit != null)
                {
                    lastHit.transform.GetComponent<Renderer>().material = clickmat;
                    noClick = true;
                }
            }
        }
    }
    
   
    private void ServiceStartSuccess()
    {
        if (Controller.UPvr_GetControllerState(0) == ControllerState.Connected ||
            Controller.UPvr_GetControllerState(1) == ControllerState.Connected)
        {
            HeadSetController.SetActive(false);
        }
        else
        {
            HeadSetController.SetActive(true);
        }
        if (Controller.UPvr_GetMainHandNess() == 0)
        {
            currentController = controller0;
        }
        if (Controller.UPvr_GetMainHandNess() == 1)
        {
            currentController = controller1;
        }
    }

    private void ControllerStateListener(string data)
    {
        
        if (Controller.UPvr_GetControllerState(0) == ControllerState.Connected ||
            Controller.UPvr_GetControllerState(1) == ControllerState.Connected)
        {
            HeadSetController.SetActive(false);
        }
        else
        {
            HeadSetController.SetActive(true);
        }

        if (Controller.UPvr_GetMainHandNess() == 0)
        {
            currentController = controller0;
        }
        if (Controller.UPvr_GetMainHandNess() == 1)
        {
            currentController = controller1;
        }
    }

    private void CheckControllerStateForGoblin(string state)
    {
        HeadSetController.SetActive(!Convert.ToBoolean(Convert.ToInt16(state)));
    }
    
}
