//
//  PVRSDKNotification.h
//  PicoVRSDK
//
//  Created by Periwen on 2016/10/19.
//  Copyright © 2016年 PivoVR. All rights reserved.
//

#import <Foundation/Foundation.h>

// 扫描到蓝牙通知
extern NSString * const PVRBLESDKScanPeripheralNotification;

// 蓝牙开关状态变化通知
extern NSString * const PVRBLESDKPeripheralStateNotification;

// 蓝牙连接状态变化通知
extern NSString * const PVRBLESDKPerpheralStateChangedNotification;
extern NSString * const PVRBLESDKPerpheralStateKey;
extern NSString * const PVRBLESDKPerpheralStateON;
extern NSString * const PVRBLESDKPerpheralStateOFF;

// OTA升级进度通知
extern NSString * const PVRBLESDKOTASendBinProgressNotification;
extern NSString * const PVRBLESDKOTACurrentKey;
extern NSString * const PVRBLESDKOTALengthKey;

// OTA升级完成后5秒未收到反馈信息通知
extern NSString * const PVRBLESDKOTANotAcceptResponseNotification;

// LARK 2 OTA 升级完成通知
extern NSString * const PVRBLESDKLARK2OTACompleteNotification;

// 蓝牙接收键值通知
extern NSString * const PVRBLESDKAcceptedKeyValueNotification;
extern NSString * const PVRBLESDKAcceptedKey;
extern NSString * const PVRBLESDKAcceptedValue;

//蓝牙连接失败
extern NSString * const PVRBLESDKPerpheralConnectFail;
