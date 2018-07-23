//
//  PVRSDKUnityPlugin.h
//  Unity-iPhone
//
//  Created by Nick on 16/7/12.
//
//

#ifndef PVRSDKUnityPlugin_h
#define PVRSDKUnityPlugin_h

/*
//eventID：
typedef NS_ENUM(NSInteger, RenderEventType)
{
    INITRENDERTHREAD = 0,
    PAUSE = 1,
    RESUME = 2,
    LEFTEYEENDFRAME = 3,
    RIGHTEYEENDFRAME = 4,
    TIMEWARP = 5,
    RESETVRMODEPARMS = 6,
    SHUTDOWNRENDERTHREAD = 7
};

typedef NS_ENUM(NSInteger, ReturnValue)
{
    SUCCESS = 0,
    FAILED = 1
}; */

extern "C"{
    void PVR_SDKVersion (int &high,int &mid, int& low);
    
    void PVR_Init_Native ( );
    
    float PVR_FOV_Native();
    
    float PVR_Separation_Native();
    
    void PVR_RenderTexturenSize_Native (int &width,int &height);
    
    void PVR_UpdateRenderParams_Native(float* renderParams,float zNear, float zFar);
    
    int PVR_HeadWearType_Native ();
    
    void PVR_ChangeHeadWearType_Native (int type);
    
    void PVR_SetRenderTextureID_Native (int eye, int texID);
    
    void PVR_StartHeadTrack_Native ();
    
    void PVR_ResetHeadTrack_Native ();
    
    void PVR_StopHeadTrack_Native ();
    
    void UnitySetGraphicsDevice(void* device, int deviceType, int eventType);
    
    void UnityRenderEvent(int marker);
    
    int PVR_OpenBLECentral();
    
    int PVR_StopBLECentral();
    
    int PVR_ConnectBLEDevice(const char* mac);
    
    int PVR_ScanBLEDevice();
    
    int PVR_ScanBLEPeripheral(int bleType);
    
    #ifdef UNITY_PROJECT
    void SpatializerUnlock();
    #endif
    
    char *PVR_GetLark2HandleMessage();
    
    char *PVR_GetLark2HandleSensorMessage();
    
    char *PVR_GetLark2HandleKeyValueMessage();
    
    void PVR_GetLark2SensorMessage(float &w, float &x, float &y, float &z);
    
    void PVR_GetLark2KeyValueMessage(int &touchPadx, int &touchPady, int &home, int &app, int &click, int &volup, int &voldown, int &powerLevel);
    
    /** 20170308重构新接口 */
    void UnityRenderEventIOS(int eventID, int textID); //GL事件
    
    /** 20170308重构新接口    aili*/
//    void UnityRenderEvent(int eventID, int textID); //GL事件
    void Pvr_SetCurrentHMDType(char *hmd);
    char * Pvr_GetSupportHMDTypes();

    //Sensor
    int Pvr_Init(int mainsensorindex = -1); // init sensor
    int Pvr_StartSensor(int index);// start sensor
    int Pvr_StopSensor(int index); //Stop sensor
    int Pvr_ResetSensor(int index); //Reset sensor

    // 获取sensor数据
    //    int Pvr_GetSensorState(int index, float &x, float &y, float &z, float &w);
    //    int Pvr_GetMainSensorState(float &x, float &y, float &z, float &w, float &fov, int &viewNumber);
    
    int Pvr_GetSensorState(int index, float &x, float &y, float &z, float &w, float &px, float &py, float &pz);
    int Pvr_GetMainSensorState(float &x, float &y, float &z, float &w, float &px, float &py, float &pz, float &fov, int &viewNumber);
    
    int Pvr_GetPsensorState(int &psensorStatus); // 获取Psensor 0 接近 1 远离状态
    int Pvr_GetSensorAcceleration(int index, float &x, float &y, float &z); // 预留
    int Pvr_GetSensorGyroscope(int index, float &x, float &y, float &z); // 预留
    int Pvr_GetSensorMagnet(int index, float &x, float &y, float &z); // 预留

    //Render
    int Pvr_SetPupillaryPoint(bool enable); // 预留
    int Pvr_ChangeHMDType(int HMDType); // Jay Pico1 /Pico1s /Pico2 /Pico NEO
    int Pvr_SetRatio(float midH, float midV); //动态调整lark屏幕
    int PVR_SetRatio(float midH, float midV); //动态调整lark屏幕
    int Pvr_SetRatioIOS(float midH, float midV); //动态调整lark屏幕

    //获取配置时返回值 // 0 读取配置文件成功，1 读取配置文件失败，2 没有此项目配置
    int Pvr_GetIntConfig(int configsenum, int &res);   //获取int配置数据
    int Pvr_GetFloatConfig(int configsenum, float &res); //获取float配置数据
   
    //System
    char * Pvr_GetSDKVersion();// 底层SDK vesion
    
    //-------For Arm Model------//
    //hand: 0-right, 1-left; gazeType:0-Never, 1-DuringMotion, 2-Always;
    void Pvr_SetArmModelParameters(int hand, int gazeType, float elbowHeight, float elbowDepth, float pointerTiltAngle);
    void Pvr_CalcArmModelParameters(float* headOrientation, float* controllerOrientation, float* gyro);
    void Pvr_GetPointerPose(float* rotation, float* position);
    void Pvr_GetElbowPose(float* rotation, float* position);
    void Pvr_GetWristPose(float* rotation, float* position);
    void Pvr_GetShoulderPose(float* rotation, float* position);
    
    void getHbAngularVelocity(float* gyro);
    void getHbAcceleration(float* acc);
}

#endif /* PVRSDKUnityPlugin_h */
