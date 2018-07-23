using UnityEngine;
using System.Collections;
using Pvr_UnitySDKAPI;

public class Pvr_ControllerVisual : MonoBehaviour {

    private Renderer controllerRenderer;
    private Renderer touchRenderer;
    private float tipsAlpha = 0;

    public GameObject touchpoint;
    public Transform tips;
    public Material m_idle;
    public Material m_app;
    public Material m_home;
    public Material m_touchpad;
    public Material m_volUp;
    public Material m_volDn;
    
    void Awake()
    {
        controllerRenderer = GetComponent<Renderer>();
        touchRenderer = touchpoint.GetComponent<Renderer>();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
        if(Controller.UPvr_IsTouching())
        {
            touchpoint.SetActive(true);
            touchpoint.transform.localPosition = new Vector3(1.4f - TouchPadPosition.y * 0.01098f, 0.9f, -0.8f - TouchPadPosition.x * 0.01098f);
        }
        else
        {
            touchpoint.SetActive(false);
        }
        if(Controller.UPvr_GetKey(Pvr_KeyCode.TOUCHPAD))
        {
            controllerRenderer.material = m_touchpad;
        }
        else if (Controller.UPvr_GetKey(Pvr_KeyCode.APP))
        {
            controllerRenderer.material = m_app;
        }
        else if (Controller.UPvr_GetKey(Pvr_KeyCode.HOME))
        {
            controllerRenderer.material = m_home;
        }
        else if (Controller.UPvr_GetKey(Pvr_KeyCode.VOLUMEUP))
        {
            controllerRenderer.material = m_volUp;
        }
        else if (Controller.UPvr_GetKey(Pvr_KeyCode.VOLUMEDOWN))
        {
            controllerRenderer.material = m_volDn;
        }
        else
        {
            controllerRenderer.material = m_idle;
        }
        //touchpoint.transform.localPosition = new Vector3(1.4f- (127f + ((TouchPadPosition.x - 127) * Mathf.Cos(-20) - (TouchPadPosition.y - 127) * Mathf.Sin(-20))) * 0.01098f, 0.9f, -0.8f -  ((127f - ((TouchPadPosition.y - 127) * Mathf.Cos(-20) + (TouchPadPosition.x - 127) * Mathf.Sin(-20))))* 0.01098f);
        
	    tipsAlpha = (330 -  transform.parent.parent.localRotation.eulerAngles.x) / 45.0f;
        if (transform.parent.parent.localRotation.eulerAngles.x >= 270 &&
	        transform.parent.parent.localRotation.eulerAngles.x <= 330)
        {
            tipsAlpha = Mathf.Max(0.0f, tipsAlpha);
            tipsAlpha = tipsAlpha > 1.0f ? 1.0f : tipsAlpha;
        }
	    else
        {
            tipsAlpha = 0.0f;
        }
        tips.GetComponent<CanvasGroup>().alpha = tipsAlpha;
	}
}
