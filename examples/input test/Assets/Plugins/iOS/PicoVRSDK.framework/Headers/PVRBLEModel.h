//
//  PVRBLEModel.h
//  PicoVRSDK
//
//  Created by Periwen on 2016/10/9.
//  Copyright © 2016年 PivoVR. All rights reserved.
//

#import "PVRSDKEnum.h"
#import <Foundation/Foundation.h>
#import <CoreBluetooth/CoreBluetooth.h>

@interface PVRBLEModel : NSObject

/** 用户是否存在绑定设备 */
@property (nonatomic, assign) BOOL isBindPeripheral;

/** 用于标识psener是否接近，用以判断用户是否戴上设备 */
@property (nonatomic, assign) BOOL isPsensorNear;

/** 蓝牙当前连接状态 */
@property (nonatomic, assign) PVRBLEState bleState;

/** LARK 2当前工作面 */
@property (nonatomic, copy) NSString *otaType;

/** 当前软件版本*/
@property (nonatomic, copy) NSString *versionCode;

/** 蓝牙mac地址，作为识别外部蓝牙设备的唯一标识  */
@property (nonatomic, copy) NSString *bleMacAdress;

/** 当前绑定蓝牙类型 */
@property (nonatomic, assign) PVRBLEPeripheralType currentBindBleType;

/** 当前链接蓝牙类型 */
@property (nonatomic, assign) PVRBLEPeripheralType currentConnectBleType;

/** OTA升级状态 */
@property (nonatomic, assign) PVROTAUpgradeState otaUpgradeState;

/** 蓝牙连接状态 */
@property (nonatomic, assign) CBCentralManagerState centralManagerState;

@end
