using UnityEngine;
using System.Collections;
using Pvr_UnitySDKAPI;
using UnityEngine.UI;

public class Pvr_ToolTips : MonoBehaviour
{


    public enum TipBtn
    {
        app,
        touchpad,
        home,
        volup,
        voldown,
        trigger
    }
    private ControllerDevice currentDevice;
    private float tipsAlpha;

    public void ChangeTipsText(TipBtn tip, string key)
    {
        switch (tip)
        {
            case TipBtn.app:
                {
                    transform.Find("apptip/btn/Text").GetComponent<Text>().text = key;
                }
                break;
            case TipBtn.touchpad:
                {
                    transform.Find("touchtip/btn/Text").GetComponent<Text>().text = key;
                }
                break;
            case TipBtn.home:
                {
                    transform.Find("hometip/btn/Text").GetComponent<Text>().text = key;
                }
                break;
            case TipBtn.volup:
                {
                    transform.Find("volup/btn/Text").GetComponent<Text>().text = key;
                }
                break;
            case TipBtn.voldown:
                {
                    transform.Find("voldown/btn/Text").GetComponent<Text>().text = key;
                }
                break;
            case TipBtn.trigger:
                {
                    transform.Find("triggertip/btn/Text").GetComponent<Text>().text = key;
                }
                break;
        }
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        switch (currentDevice)
        {
            case Pvr_UnitySDKAPI.ControllerDevice.Goblin1:
            case Pvr_UnitySDKAPI.ControllerDevice.Goblin2:
                {
                    tipsAlpha = (330 - transform.parent.parent.parent.localRotation.eulerAngles.x) / 45.0f;
                    if (transform.parent.parent.parent.localRotation.eulerAngles.x >= 270 &&
                        transform.parent.parent.parent.localRotation.eulerAngles.x <= 330)
                    {
                        tipsAlpha = Mathf.Max(0.0f, tipsAlpha);
                        tipsAlpha = tipsAlpha > 1.0f ? 1.0f : tipsAlpha;
                    }
                    else
                    {
                        tipsAlpha = 0.0f;
                    }
                    GetComponent<CanvasGroup>().alpha = tipsAlpha;

                }
                break;
        }

    }
}


