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

    public static void Login(){
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");     //获取当前Activity的对象
        jo.Call("init", mJo);
        jo.Call("authSSO");


    }


    //支付
    public static void Pay(string payOrderJson) {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("init", mJo);
        jo.Call("pay", payOrderJson);
        
    }

    //查询订单
    public static void QueryOrder(string orderId)  {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("init", mJo);
        jo.Call("queryOrder", orderId);

    }

    //用户信息
    public static void GetUserAPI() {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject mJo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("init", mJo);
        jo.Call("getUserAPI");
    }
   




}
#endif
