//
//  PVR3DObjectRender.h
//  IPVRSDK
//
//  Created by Peiwen.Liu on 16/7/12.
//  Copyright © 2016年 PivoVR. All rights reserved.
//

#import "PVREye.h"
#import <GLKit/GLKit.h>

@interface PVR3DObjectRender : NSObject

/** 是否隐藏 */
@property (nonatomic, assign, getter=isHidden)  BOOL hidden;
/** 是否准备好，可以进行渲染 */
@property (nonatomic, assign, getter=isReady)  BOOL ready;
/** 是否隐藏 */
@property (nonatomic, assign, getter=isDeleted) BOOL deleted;
/** 是否是焦点 */
@property (nonatomic, assign, getter=isFocused) BOOL focused;

/** 建立渲染
 设置shader program，设置VAO等
 */
- (void)setupRender;

/** 绘制眼睛矩阵区域
 @param eye 眼睛，包含左右眼区分、视锥、texture、变换矩阵等属性参数
 @param headViewMatrix 变换矩阵
 */
- (void)drawEye:(PVREye *)eye headViewMatrix:(GLKMatrix4)headViewMatrix;

/** 关闭渲染 */
- (void)shutdownRender;

/** 触发事件
 处理用户点击或者触摸事件
 */
- (void)triggerPressed;

@end
