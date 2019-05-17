//
//  PVRGLNativeRender.h
//  IPVRSDK
//
//  Created by Peiwen.Liu on 16/7/12.
//  Copyright © 2016年 PivoVR. All rights reserved.
//

#import "PVRRender.h"
#import "PVR3DObjectRender.h"

@class PVROverlayView;

@interface PVRGLNativeRender : PVRRender

/** 用于显示FPS和凝视loading的view层 */
@property (nonatomic,strong,readonly) PVROverlayView *overlay;

/** 当前焦点物体 */
@property (nonatomic,weak) PVR3DObjectRender *focusedObject;

/** 触发事件，物体被点击时触发 */
- (void)triggerPressed;

/** 通过name获取object
 @param name PVR3DObjectRender的name，相当于一个key值
 */
- (PVR3DObjectRender *)findRenderObjectByName:(NSString *)name;

/** 通过name移除object
 @param name PVR3DObjectRender的name，相当于一个key值
 */
- (void)removeRenderObjectByName:(NSString *)name;

/** 移除object
 @param object PVR3DObjectRender实例
 */
- (void)removeRenderObject:(PVR3DObjectRender *)object;

/** 插入object
 @param object PVR3DObjectRender实例
 @param name PVR3DObjectRender的name，相当于一个key值
 */
- (void)insertRenderObject:(PVR3DObjectRender *)object named:(NSString *)name;

@end
