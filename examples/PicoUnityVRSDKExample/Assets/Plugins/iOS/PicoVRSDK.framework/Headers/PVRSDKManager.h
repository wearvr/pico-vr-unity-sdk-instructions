//
//  PVRManager.h
//  IPVRSDK
//
//  Created by Peiwen.Liu on 16/6/22.
//  Copyright © 2016年 PivoVR. All rights reserved.
//

#import "PVREye.h"
#import "PVRSDKEnum.h"
#import "PVRBLEModel.h"
#import <GLKit/GLKit.h>
#import "PVRSingleton.h"
#import "PVRGLNativeRender.h"
#import <CoreMotion/CoreMotion.h>

@class UnityAppController;
@class PVRSDKSettingManager;

@protocol PVRSDKManagerDelegate <NSObject>

@optional

/** 蓝牙连接状态
 @param state 标识手机蓝牙与外设蓝牙之间的连接状态（已连接、断开连接）
 */
- (void)bleConnectState:(BOOL)state;

/** CBCentralManagerState 蓝牙外设状态
 @param state CBCentralManagerState 蓝牙外设状态
 */
- (void)peripheralConnectState:(CBCentralManagerState)state;

/** 获取蓝牙设备列表
 @param list 蓝牙设备数组，元素为字典，字典的key值是mac，value为CBPeripheral类实例
 */
- (void)obtainPeripheralList:(NSDictionary *)list;

/** lark1S触摸板以及按键触发事件
 @param type lark1S 键值指令类型
 @param value lark1S 键值指令类型中的声音大小
 */
- (void)touchEventWithType:(PVRBLEAcceptValueType)type Value:(NSString *)volum;

@end

@interface PVRSDKManager : NSObject

/** 声明单例 */
singleton_interface(PVRSDKManager)

/** 头戴类型，目前只支持Lark 1S */
@property (nonatomic, assign) PVRLensType lensType;

/** 是否开启native渲染 */
@property (nonatomic, assign, getter=isNative) BOOL native;

/** 是否开启色差 */
@property (nonatomic, assign, getter = isChromaticAberration) BOOL chromaticAberration;

/** SDK的native渲染 */
@property (nonatomic, strong) PVRGLNativeRender *nativeRender;

/** SDK设置，用于保存，连接过的蓝牙设备，头盔类型，以及读取、保存修改存储问题 */
@property (nonatomic, strong) PVRSDKSettingManager *settingManager;

/** 蓝牙model */
@property (nonatomic, strong) PVRBLEModel *bleModel;

/** unity控制器，用unity实现3D场景时会用到 */
@property (nonatomic, weak) UnityAppController *AppController;

/** PVRSDKManager启动来源，用unity实现3D场景时会用到 */
@property (nonatomic, assign) PVRSDKFromType sdkFromType;

@property (nonatomic, strong) NSMutableArray *bleDataArr;

@property (nonatomic, assign) float gMiddleVertical;

@property (nonatomic, assign) float gMiddleHorizontal;

/** 建立渲染 */
- (void)setupRender;

/** 更新渲染 */
- (void)updateRender;

/** 3D渲染
 @param headview 变换矩阵
 */
- (void)renderNative:(GLKMatrix4)headview;

/** 分屏渲染
 @param headview 变换矩阵
 */
- (void)renderBase:(GLKMatrix4)headview;

/** 关闭渲染 */
- (void)shutdownRender;
/** 获取眼睛PVREye 
 @param eyetype 眼睛类型，左眼还是右眼
 @return 眼睛，包含左右眼区分、视锥、texture、变换矩阵等属性参数
 */
- (PVREye *)eyeWithType:(PVREyeType)eyetype;

/** 视角参数 
 @return 视角值
 */
- (float)eyeFov;

/** 瞳距参数 
 @return 瞳距值
 */
- (float)sepration;

/** 启动sensor追踪
 @param orientation 头戴方向
 */
- (void)startTracking:(UIInterfaceOrientation)orientation;

/** 重置跟踪，场景矫正到眼睛正对位置 */
- (void)resetTracking;

/** 停止追踪 */
- (void)stopTracking;

/** 更新头戴位置方向 
 @param orientation 头戴方向
 */
- (void)updateDeviceOrientation:(UIInterfaceOrientation)orientation;

/** 获取头戴位置
 @return 变换矩阵
 */
- (GLKMatrix4)lastHeadView;

/** 获取手机类型 
 @return 手机类型：6, 6s, 6p, 6sp,7, 7p, 5s
 */
- (NSString *)phoneType;

/** 启动BLE 
 @return 是否开启BLE
 */
- (BOOL)openBLECenter;

/** 扫描BLE外设 
 @return 是否执行过扫描BLE外设方法
 */
- (BOOL)scanBLEDevice:(PVRBLEPeripheralType)bleType;

/** 连接BLE
 @param mac 蓝牙mac地址，作为唯一识别标识
 @return 是否执行过连接BLE方法
 */
- (BOOL)connectBLEDevice:(NSString *)mac;

/** 断开BLE
 @param mac 蓝牙mac地址，作为唯一识别标识
 @return 是否执行过连接BLE方法
 */
- (BOOL)disConnectBLEDevice:(NSString *)mac;

/** 重连设备 为LARK 2 OTA 升级专用
 @ return 是否执行过重连BLE方案
 */
- (BOOL)ReconnectCurrentPeripheral;

/** 解除当前绑定的外设
 @return 是否执行过解绑方法
 */
- (BOOL)unbindPeripheral;

/** 查询软件版本 
 @return 是否执行过查询软件版本方法
 */
- (BOOL)getSoftWare;

/** 发送MCU指令 
 @return 是否执行过发送MCU指令方法
 */
- (BOOL)sendMCU;

/** BLE升级 
 @param data 升级数据
 @param num 升级数据长度
 @return 是否执行过BLE升级方法
 */
- (BOOL)upgradeOTABinData:(NSData *)data num:(long)num;

/** 中断BLE升级
 @return 是否执行过中断BLE升级方法
 */
- (BOOL)interruptOTA;

/** 停止BLE 
 @return 是否停止BLE
 */
- (BOOL)stopBLECenter;

@property (nonatomic, weak) id <PVRSDKManagerDelegate> delegate;

/** 点击事件 */
- (void)triggerPressed;

/** 视口重新矫正
 @param midH 水平移动比例
 @param midV 垂直移动比例
 */
- (void)setRatioH:(CGFloat)midH ratioV:(CGFloat)midV;

- (NSArray *)getLark2HandleMessage;


/** 获取sensor acc 数据 */
- (CMAcceleration)accDate;

/** 获取sensor gyro 数据 */
- (CMRotationRate)gyroDate;

/** 获取sensor state 数据 */
- (NSArray *)getSensorState;
@end
