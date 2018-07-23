//
//  PVREnum.h
//  IPVRSDK
//
//  Created by Nick on 16/6/22.
//  Copyright © 2016年 PivoVR. All rights reserved.
//

#ifndef PVRSDKEnum_h
#define PVRSDKEnum_h
#import <Foundation/Foundation.h>

/**
 *  PVRSDKManager启动来源
 *  keep
 */
typedef NS_ENUM(NSInteger, PVRSDKFromType) {
    /**
     * 来自unity 默认
     */
    PVR_SDKFROM_UNITY = 0,
    /**
     * 来自iwing
     */
    PVR_SDKFROM_IWING = 1,
};

/**
 *  手机设备类型
 *
 *  peiwen
 */
typedef NS_ENUM(NSInteger, PVRDeviceType) {
    /**
     *  未知设备
     */
    PVR_IPHONE_UNKNOWN = 0,
    /**
     *  IPHONE ES
     */
    PVR_IPHONE_ES = 1,
    /**
     *  iphone 6
     */
    PVR_IPHONE_6 = 2,
    /**
     *  iphone 6s
     */
    PVR_IPHONE_6S = 3,
    /**
     *  iphone 6p
     */
    PVR_IPHONE_6P = 4,
    /**
     *  iphone 6sp
     */
    PVR_IPHONE_6SP = 5,
    /**
     *  iphone 7
     */
    PVR_IPHONE_7 = 6,
    /**
     *  iphone 7p
     */
    PVR_IPHONE_7P = 7
};

/**
 *  头盔设备类型
 *
 *  peiwen
 */
typedef NS_ENUM(NSInteger, PVRLensType) {
    /**
     *  无畸变的一组参数，未适配特定头盔
     */
    PVR_LENS_UNKNOWN = 0,
    /**
     *  三星GearVR2
     */
    PVR_LENS_SAMSUNG_GEARVR2 = 1,
    /**
     *  小鸟看看Pico 1
     */
    PVR_LENS_PICOVR_I = 2,
    /**
     *  三星GearVR1
     */
    PVR_LENS_SAMSUNG_GEARVR1 = 3,
    /**
     *  机饕1
     */
    PVR_LENS_ANTVR_JT1 = 4,
    /**
     *  暴风魔镜2
     */
    PVR_LENS_MOJING_2 = 5,
    /**
     *  暴风魔镜3
     */
    PVR_LENS_MOJING_3 = 6,
    /**
     *  FALCON
     */
    PVR_LENS_FALCON = 7,
    /**
     *   PICO 1S
     */
    PVR_LENS_PICO_1S = 8,
    /**
     *   PICO 2
     */
    PVR_LENS_PICO_2 = 9
};


/**
 *  眼类型
 *
 *  peiwen
 */
typedef NS_ENUM(NSInteger, PVREyeType) {
    /**
     *  左眼
     */
    PVR_EYE_LEFT = 0,
    /**
     *  右眼
     */
    PVR_EYE_RIGHT = 1,
    /**
     *  眼睛数量
     */
    PVR_EYE_COUNT = 2
};

/*
 * 蓝牙连接状态
 *
 * keep
 */
typedef NS_ENUM(NSInteger, PVRBLEState) {
    /** 未连接 */
    PVR_BLE_DISCONNECT,
    /** 连接 */
    PVR_BLE_CONNECT,
};

/*
 * 蓝牙开关状态
 *
 * keep
 */
typedef NS_ENUM(NSInteger, PVRBLESwitchState) {
    /** 未知 */
    PVR_BLE_SWITCH_UNKNOW = 0,
    PVR_BLE_SWITCH_RESETTING,
    PVR_BLE_SWITCH_UNSUPPORTED,
    PVR_BLE_SWITCH_UNAUTHORIZED,
    PVR_BLE_SWITCH_POWERED_OFF,
    PVR_BLE_SWITCH_POWERED_ON,
};

/**
 *  OTA升级状态
 *
 *  keep
 */
typedef NS_ENUM(NSInteger, PVROTAUpgradeState) {
    /** 没有在进行OTA升级 */
    PVR_BLE_OTAUPGRADE_NOT,
    /** OTA升级正常 */
    PVR_BLE_OTAUPGRADE_NORMAL,
};

/*
 * 应用前台后台
 *
 */
typedef NS_ENUM(NSInteger, PVRAppState) {
    /** 前台 */
    PVR_APP_FORNT,
    /** 后台 */
    PVR_APP_BACKGROUND,
};

/**
 * 蓝牙外设分类
 * 
 * keep
 */
typedef NS_ENUM(NSInteger, PVRBLEPeripheralType) {
    /** unkown */
    PVR_BLE_UNKOWN,
    /** Pico 1s */
    PVR_BLE_PICO1S,
    /** Pico 2 */
    PVR_BLE_PICO2,
    /** All */
    PVR_BLE_ALL
};

/*
 * lark1S 返回键值,指令类型
 *
 * keep
 */
typedef NS_ENUM(NSInteger, PVRBLEAcceptValueType) {
    /** 选中 */
    PVR_BLE_VALUE_SELECT = 0,
    /** 向上滑动 */
    PVR_BLE_VALUE_UPSLIDE,
    /** 向下滑动 */
    PVR_BLE_VALUE_DOWNSLIDE,
    /** 向左滑动 */
    PVR_BLE_VALUE_LEFTSLIDE,
    /** 向右滑动 */
    PVR_BLE_VALUE_RIGHTSLIDE,
    /** 短按返回 */
    PVR_BLE_VALUE_SHORTBACK,
    /** 长按返回 */
    PVR_BLE_VALUE_LONGBACK,
    /** Audio 蓝牙音量 */
    PVR_BLE_VALUE_AUDIO_VOLUME,
    /** 打开手机摄像头 */
    PVR_BLE_VALUE_OPEN_CAMERA,
    /** Audio jack 耳机 插入/靠近 */
    PVR_BLE_VALUE_AUDIO_JACK_NEAR,
    /** Audio jack 耳机 拔出/远离 */
    PVR_BLE_VALUE_AUDIO_JACK_AWAY,
    /** P-sensor 插入/靠近 */
    PVR_BLE_VALUE_PSENSOR_NEAR,
    /** P-sensor 拔出/远离 */
    PVR_BLE_VALUE_PSENSOR_AWAY,
    /** 蓝牙软件版本查询 */
    PVR_BLE_VALUE_BLE_EDITION,
    /** 进入OTA */
    PVR_BLE_VALUE_MCU_OTA,
    /** 电量过低 */
    PVR_BLE_VALUE_MCU_LOWELCTRICITY,
    /** OTA升级成功 */
    PVR_BLE_VALUE_OTA_SUCCESS,
    /** OTA升级失败 */
    PVR_BLE_VALUE_OTA_FAIL,
    /** OTA升级版本格式错误 */
    PVR_BLE_VALUE_OTA_EDITIONERROR,
    /** Lark重启失败 */
    PVR_BLE_VALUE_OTA_LARKERROR,
    /** OTA发送bin数据的回复 */
    PVR_BLE_VALUE_OTA_BIN,
    /** p-sensor 出错 */
    PVR_BLE_VALUE_PSENSOR_ERROR,
    /** 错误 */
    PVR_BLE_VALUE_ERROR,
    /** 蓝牙Mac地址查询 */
    PVR_BLE_VALUE_BLE_MAC,
};

#endif /* PVREnum_h */
