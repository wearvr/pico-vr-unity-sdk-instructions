//
//  PVRRender.h
//  IPVRSDK
//
//  Created by Peiwen.Liu on 16/7/12.
//  Copyright © 2016年 PivoVR. All rights reserved.
//

#import "PVREye.h"
#import <GLKit/GLKit.h>

@interface PVRRender : NSObject

/** 
 渲染的基类，定义了渲染的生命周期方法，BaseRender与NativeRender均继承此类
 */

/** 开始渲染 */
- (void)setupRender;

/** 更新渲染 */
- (void)updateRender;

/** 渲染矩阵区域
 @param headViewMatrix 待渲染的4x4矩阵区域
 */
- (void)renderWithHeadViewMatrix:(GLKMatrix4)headViewMatrix;

/** 关闭渲染*/
- (void)shutdownRender;

@end
