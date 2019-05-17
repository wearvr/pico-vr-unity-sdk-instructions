#if !UNITY_EDITOR
#if UNITY_ANDROID
#define ANDROID_DEVICE
#elif UNITY_IPHONE
#define IOS_DEVICE
#elif UNITY_STANDALONE_WIN
#define WIN_DEVICE
#endif
#endif
using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;
#if UNITY_ANDROID
public class Callback : MonoBehaviour{

    private static string IS_SUCCESS = "isSuccess";
    private static string MSG = "msg";
    private static string CODE = "code";
    /// <summary>
    /// 登陆后本地缓存一份token，用于查询
    /// </summary>
    /// <param name="LoginInfo"></param>
    public void LoginCallback(string LoginInfo) {
        JsonData jsrr = JsonMapper.ToObject(LoginInfo);
        SetMassage(LoginInfo);
        DemoController.showLoading();
        
        if (jsrr[IS_SUCCESS] != null ) {
            CommonDic.getInstance().isSuccess = jsrr[IS_SUCCESS].ToString();  
        }
        if( jsrr[MSG] != null){
            CommonDic.getInstance().loginMsg = jsrr[MSG].ToString();
        }
        
        Debug.Log("调用login回调:" + LoginInfo);
    }
    /// <summary>
    /// 接收支付或者查询订单操作的返回结果，根据提示码确定当前状态及订单信息
    /// </summary>
    /// <param name="payInfo"></param>
    public void QueryOrPayCallback(string queryOrPayInfo){
        JsonData jsrr = JsonMapper.ToObject(queryOrPayInfo);
        if (jsrr[CODE] != null) {
            CommonDic.getInstance().code = jsrr["code"].ToString();
        }
        if (jsrr[MSG] != null)
        {
            CommonDic.getInstance().msg = jsrr["msg"].ToString();
        }
        if (jsrr != null) {
            CommonDic.getInstance().order_info = jsrr[1].ToString();
        }

        SetMassage(queryOrPayInfo);
        DemoController.showLoading();
        Debug.Log("调用pay回调:" + queryOrPayInfo);
    }

    public void UserInfoCallback(string userInfo) {
        
        CommonDic.getInstance().user_info = userInfo;

        SetMassage(userInfo);
        DemoController.showLoading();
        Debug.Log("调用userInfo回调:" + userInfo);
    }

    public void SetMassage(string massage) {
        if (!GetCurrentGameObject().Equals(null))
        {
            GetCurrentGameObject().GetComponent<Text>().text = massage;
        }
        else
        {
            Debug.LogError("无接收该Message的控件");
        }
    }

    public GameObject GetCurrentGameObject() {
        return GameObject.Find("MassageInfo");
    }

    public void ActivityForResultCallback(string activity)
    {
        PicoPaymentSDK.jo.Call("authCallback", activity);
    }
}
#endif