//
//  PVRShaderHelper.h
//  IPVRSDK
//
//  Created by Peiwen.Liu on 16/6/23.
//  Copyright © 2016年 PivoVR. All rights reserved.
//

#import <GLKit/GLKit.h>

@interface PVRShaderHelper : NSObject

/** 创建shaderProgram */
+ (GLint)createProgram;

/** 链接shaderProgram 
 @param program 着色器程序
*/
+ (void)linkProgram:(GLint)program;

/** 创建shaders 
 @param name 着色器程序名称，此shader以在SDK中定义好
 @param type Shaders类型
 @return shader标识
*/
+ (GLuint)shaderWithSource:(NSString*)name type:(GLenum)type;

/** 创建shaders 
 @param name 着色器程序名称，此shader通过bundle加载
 @param type Shaders类型
 @return shader标识
*/
+ (GLuint)shaderWithFileName:(NSString*)name type:(GLenum)type;

/** 把创建的shaders，attach到这个shaderProgram对象上
 @param program 着色器程序
 @param vertexShader 顶点着色器
 @param fragmentShader 片段着色器
*/
+ (void)attachShader:(GLint)program vertexShader:(GLint)vertexShader fragmentShader:(GLint)fragmentShader;

/** 将shaders与其对象分离 
 @param program 着色器程序
 @param vertexShader 顶点着色器
 @param fragmentShader 片段着色器
*/
+ (void)detachShader:(GLint)program vertexShader:(GLint)vertexShader fragmentShader:(GLint)fragmentShader;

@end
