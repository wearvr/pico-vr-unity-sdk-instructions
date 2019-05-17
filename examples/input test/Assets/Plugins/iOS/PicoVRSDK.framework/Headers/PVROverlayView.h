//
//  PVROverlayView.h
//  IPVRSDK
//
//  Created by Peiwen.Liu on 16/7/1.
//  Copyright © 2016年 PivoVR. All rights reserved.
//

#include "PVREye.h"

@interface PVROverlayView : UIView

/** 帧率，每秒的刷新率，用来显示当前场景的刷新速率 */
@property (nonatomic, assign) NSInteger FPS;

/** 凝视进度，当凝视功能开启的时候，聚焦物体会有一个凝视的圆圈，走loading的进度 */
@property (nonatomic, assign) float progressValue;

/** 是否开启凝视功能 */
@property (nonatomic, assign, getter=isShowProgress) BOOL showProgress;

/** 是否显示帧率 */
@property (nonatomic, assign, getter=isShowFPS) BOOL showFPS;

/** 是否显示凝视光标 */
@property (nonatomic, assign, getter=isShowGazeCursor) BOOL showGazeCursor;

/** 开始渲染 */
- (void)setupRender;

/** 更新渲染 */
- (void)updateRender;

/** 绘制眼睛矩阵区域 
 @param eye 眼睛，包含左右眼区分、视锥、texture、变换矩阵等属性参数
 @param headViewMatrix 变换矩阵
 */
- (void)drawEye:(PVREye *)eye headViewMatrix:(GLKMatrix4)headViewMatrix;

/** 关闭渲染 */
- (void)shutdownRender;

@end
