//
//  PVRViewController.h
//  IPVRSDK
//
//  Created by Peiwen.Liu on 16/6/30.
//  Copyright © 2016年 PivoVR. All rights reserved.
//

#import "PVREye.h"
#import <GLKit/GLKit.h>

@interface PVRViewController : GLKViewController

/** 建立渲染
 设置是否需要色差
 设置头戴类型，目前支持型号为Pico 1S
 调用nativeRender，加载需要显示的物体
 */
- (void)setupRenderer;

/** 更新渲染
 根据需要更新渲染物体的状态
 */
- (void)updateRender;

/** 触发事件
 处理用户点击事件，
 包括屏幕触摸和Lark1S头盔按键以及触摸板操作 
 */
- (void)triggerPressed;

@end
