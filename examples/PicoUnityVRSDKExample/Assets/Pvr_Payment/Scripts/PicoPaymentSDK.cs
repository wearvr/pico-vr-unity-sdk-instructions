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
#if UNITY_ANDROID
public class PicoPaymentSDK {
    private static AndroidJavaObject _jo = new AndroidJavaObject("com.pico.loginpaysdk.UnityInterface");

    public static AndroidJavaObject jo
    {
        get { return _jo; }
        set { _jo = value; }
    }
    //登陆
    public static void Login(string appID, string appKey, string merchantID, string payKey) {
        
        //储存开发者提供的数据
        CommonDic.getInstance().app_ID = appID;
        CommonDic.getInstance().app_Key = appKey;
        CommonDic.getInstance().merchant_ID = merchantID;
        CommonDic.getInstance().paykey = payKey;
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");     //获取当前Activity的对象

        jo.Call("initPay", appID, merchantID, payKey);


        jo.Call("authSSO", appID, "www.picovr.com", "get_user_info", appKey);

        
    }

    public static void Login(){
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");     //获取当前Activity的对象
        jo.Call("init", mJo);
        jo.Call("initPay");

        jo.Call("authSSO");


    }


    //支付
    public static void Pay(string payOrderJson) {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");     
        jo.Call("pay", CommonDic.getInstance().access_token, CommonDic.getInstance().open_id, payOrderJson);
        
    }

    //查询订单
    public static void QueryOrder(string orderId)  {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("queryOrder", orderId);

    }

    //用户信息
    public static void GetUserAPI() {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string userToken = "{\"access_token\":\"" + CommonDic.getInstance().access_token + "\",\"open_id\":\"" + CommonDic.getInstance().open_id + "\"}";
        Debug.Log("Unity json:" + userToken);
        jo.Call("getUserAPI", userToken);
    }
   




}
#endif
