//
//  PVREye.h
//  IPVRSDK
//
//  Created by Peiwen.Liu on 16/6/27.
//  Copyright © 2016年 PivoVR. All rights reserved.
//

#import "PVRSDKEnum.h"
#import <GLKit/GLKit.h>

@interface PVREye : NSObject

//视锥
@property (nonatomic, assign) float fov;

//texture
@property (nonatomic, assign) GLuint texture;

//左右眼
@property (nonatomic, assign) PVREyeType eyeType;

//变换矩阵
@property (nonatomic, assign) GLKMatrix4 translation;

//获取眼睛透视变换4x4矩阵
- (GLKMatrix4)perspectiveWithNear:(float)near andFar:(float)far;

@end
