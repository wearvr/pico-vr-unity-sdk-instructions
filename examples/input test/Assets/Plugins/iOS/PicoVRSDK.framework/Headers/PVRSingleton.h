//
//  IWSingleton.h
//
//
//  Created by Peiwen.liu on 15/7/3.
//  Copyright (c) 2015年 PicoVR. All rights reserved.
//

#ifndef PVRSingleton_h
#define PVRSingleton_h

// @interface
#define singleton_interface(className) \
+ (className *)shared;


// @implementation
#define singleton_implementation(className) \
static className *_instance; \
+ (id)allocWithZone:(NSZone *)zone \
{ \
static dispatch_once_t onceToken; \
dispatch_once(&onceToken, ^{ \
_instance = [super allocWithZone:zone]; \
}); \
return _instance; \
} \
+ (className *)shared \
{ \
static dispatch_once_t onceToken; \
dispatch_once(&onceToken, ^{ \
_instance = [[self alloc] init]; \
}); \
return _instance; \
}

#endif
