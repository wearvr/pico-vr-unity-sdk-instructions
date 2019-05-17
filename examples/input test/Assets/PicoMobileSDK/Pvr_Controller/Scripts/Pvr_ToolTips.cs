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

    [SerializeField]
    private GameObject trigger;
    [SerializeField]
    private GameObject home;
    [SerializeField]
    private GameObject app;

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
        SystemLanguage localanguage = Application.systemLanguage;
        currentDevice = transform.GetComponentInParent<Pvr_ControllerVisual>().currentDevice;
        if (localanguage == SystemLanguage.Chinese || localanguage == SystemLanguage.ChineseSimplified || localanguage == SystemLanguage.ChineseTraditional)
        {
            transform.Find("apptip/btn/Text").GetComponent<Text>().text = "返回键";
            transform.Find("touchtip/btn/Text").GetComponent<Text>().text = "确认键";
            transform.Find("hometip/btn/Text").GetComponent<Text>().text = "Home键";
            transform.Find("volup/btn/Text").GetComponent<Text>().text = "音量+";
            transform.Find("voldown/btn/Text").GetComponent<Text>().text = "音量-";
            transform.Find("triggertip/btn/Text").GetComponent<Text>().text = "扳机键";
        }
        else
        {
            transform.Find("apptip/btn/Text").GetComponent<Text>().text = "Back";
            transform.Find("touchtip/btn/Text").GetComponent<Text>().text = "Confirm";
            transform.Find("hometip/btn/Text").GetComponent<Text>().text = "Home";
            transform.Find("volup/btn/Text").GetComponent<Text>().text = "Volume+";
            transform.Find("voldown/btn/Text").GetComponent<Text>().text = "Volume-";
            transform.Find("triggertip/btn/Text").GetComponent<Text>().text = "Trigger";
        }

        if (currentDevice == ControllerDevice.Goblin1)
        {
            trigger.SetActive(false);
            app.transform.localPosition = new Vector3(185,460,0);
            home.transform.localPosition = new Vector3(185,-80,0);
        }
        if (currentDevice == ControllerDevice.Goblin2)
        {
            trigger.SetActive(true);
            app.transform.localPosition = new Vector3(185, -79, 0);
            home.transform.localPosition = new Vector3(185, -238, 0);
        }
    }
    void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            if (currentDevice == ControllerDevice.Goblin1)
            {
                trigger.SetActive(false);
                app.transform.localPosition = new Vector3(185, 460, 0);
                home.transform.localPosition = new Vector3(185, -80, 0);
            }
            if (currentDevice == ControllerDevice.Goblin2)
            {
                trigger.SetActive(true);
                app.transform.localPosition = new Vector3(185, -79, 0);
                home.transform.localPosition = new Vector3(185, -238, 0);
            }
        }
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


