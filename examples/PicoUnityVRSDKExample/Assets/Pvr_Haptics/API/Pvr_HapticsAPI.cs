///////////////////////////////////////////////////////////////////////////////
// Copyright 2015-2017  Pico Technology Co., Ltd. All Rights 
// File: Pvr_HapticsAPI
// Author: AiLi.Shang
// Date:  2017/03/22
// Discription: The Haptics API 
///////////////////////////////////////////////////////////////////////////////
#if !UNITY_EDITOR
#if UNITY_ANDROID
#define ANDROID_DEVICE
#elif UNITY_IPHONE
#define IOS_DEVICE
#elif UNITY_STANDALONE_WIN
#define WIN_DEVICE
#endif
#endif

using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

namespace Pvr_UnitySDKAPI
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Haptics
    {
      
        #region Android
#if ANDROID_DEVICE
        //---------------------------------------so------------------------------------------------
        public const string LibFileName = "Pvr_UnitySDK";      
              
        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool Pvr_HasControllerVibrator();

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Pvr_SetControllerVibrateMode(int[] pattern, int length, int repeat);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Pvr_SetControllerVibrateTime(int milliseconds);

        [DllImport(LibFileName, CallingConvention = CallingConvention.Cdecl)]
        private static extern void Pvr_CancelControllerVibrate();

        //---------------------------------------so------------------------------------------------
#endif
        #endregion

        /**************************** Private Static Funcations *******************************************/
        #region Private Static Funcation
        public static int HAPTICS_LEFT = 0x01;
        public static int HAPTICS_RIGHT = 0x02;
        public static int HAPTICS_ALL = 0x03;
        public static int HAPTICS_HAPTICTHEME_SIP = 1;
        public static int HAPTICS_HAPTICTHEME_DIALPAD = 2;
        public static int HAPTICS_HAPTICTHEME_LAUNCHER = 3;
        public static int HAPTICS_HAPTICTHEME_LONGPRESS = 4;
        public static int HAPTICS_HAPTICTHEME_VIRTUALKEY = 5;
        public static int HAPTICS_HAPTICTHEME_ROTATE = 7;
        public static int HAPTICS_HAPTICTHEME_GALLERY = 8;
        public static int HAPTICS_HAPTICTHEME_LOCKSCREEN = 9;
        public static int HAPTICS_HAPTICTHEME_TRY_UNLOCK = 10;
        public static int HAPTICS_HAPTICTHEME_MULTITOUCH = 11;
        public static int HAPTICS_HAPTICTHEME_SCROLLING = 12;
        public static string DATA_HAPTICTHEME_VIRTUALKEY = "data_haptictheme_virtualkey";
        public static string DATA_HAPTICTHEME_LONGPRESS = "data_haptictheme_longpress";
        public static string DATA_HAPTICTHEME_LAUNCHER = "data_haptictheme_launcher";
        public static string DATA_HAPTICTHEME_DIALPAD = "data_haptictheme_dialpad";
        public static string DATA_HAPTICTHEME_SIP = "data_haptictheme_SIP";
        public static string DATA_HAPTICTHEME_ROTATE = "data_haptictheme_rotate";
        public static string DATA_HAPTICTHEME_GALLERY = "data_haptictheme_gallery";
        public static string DATA_HAPTICTHEME_SCROLL = "data_haptictheme_scroll";
        public static string DATA_HAPTICTHEME_MULTI_TOUCH = "data_haptictheme_multi_touch";
        public static string DATA_HAPTIC_VIBRATE = "haptic_vibrate_data";
        public static string DATA_HAPTIC_A2H = "haptic_A2H_data";

        private static void playeffect(int effectID, int whichHaptic)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "playeffect", effectID, whichHaptic);
#endif
        }  
        private static void playEffectSequence(string sequence, int whichHaptic)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "playEffectSequence", sequence, whichHaptic);
#endif
        }   
        private static void setAudioHapticEnabled(bool enable, int whichHaptic)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "setAudioHapticEnabled", enable, whichHaptic);
#endif
        }   
        private static void stopPlayingEffect(int whichHaptic)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "stopPlayingEffect", whichHaptic);
#endif
        }
        private static void playeffectforce(int effectID, int whichHaptic)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "playeffectforce", effectID, whichHaptic);
#endif
        }
        private static void playTimedEffect(int effectDuration, int whichHaptic)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "playTimedEffect", effectDuration, whichHaptic);
#endif
        } 
        private static void playPatternRTP(float vibrationDuration, float vibrationStrength, int whichHaptic, bool large, bool small, int repeat_times, float silienceDuration, float HapticsDuration)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "playPatternRTP", vibrationDuration, vibrationStrength, whichHaptic, large, small, repeat_times, silienceDuration, HapticsDuration);
#endif
        }    
        private static void playEffectSeqBuff(byte[] Sequence, int buffSize, int whichHaptic)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "playEffectSeqBuff", Sequence, buffSize, whichHaptic);
#endif
        }
        private static void playRTPSequence(String sequence, int whichHaptic)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "playRTPSequence", sequence, whichHaptic);
#endif
        }       
        private static void playRTPSeqBuff(byte[] Sequence, int buffSize, int whichHaptic)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "playRTPSeqBuff", Sequence, buffSize, whichHaptic);
#endif
        }  
        private static void playRingHaptics(int index, int whichHaptic)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "playRingHaptics", index, whichHaptic);
#endif
        } 
        private static void playRingSeq(int index, int whichHaptic)
        {
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod(Pvr_UnitySDKRender.javaVrActivityClass, "playRingSeq", index, whichHaptic);
#endif
        }   
        private static string getRingHapticsName()
        {
            string value = null;
#if ANDROID_DEVICE

             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref value, Pvr_UnitySDKRender.javaVrActivityClass, "getRingHapticsName");
#endif
            return value;
        }
        private static string getRingHapticsValues()
        {        
            string value = null;
#if ANDROID_DEVICE

             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref value, Pvr_UnitySDKRender.javaVrActivityClass, "getRingHapticsValues");                   
#endif
            return value;
        }    
        private string getRingHapticsValue(int index)
        {

            string value = null;
#if ANDROID_DEVICE
             Pvr_UnitySDKAPI.System.UPvr_CallStaticMethod<string>(ref value, Pvr_UnitySDKRender.javaVrActivityClass, "getRingHapticsValue", index);
#endif
            return value;
        }



        #endregion

        /**************************** Public Static Funcations *******************************************/
        #region Public Static Funcation  

        public static void UPvr_Playeffect(int effectID, int whichHaptic)
        {
            playeffect(effectID, whichHaptic);
        }

        public static void UPvr_PlayEffectSequence(string sequence, int whichHaptic)
        {

        }

        public static void UPvr_SetAudioHapticEnabled(bool enable, int whichHaptic)
        {

        }

        public static void UPvr_StopPlayingEffect(int whichHaptic)
        {

        }

        public static void UPvr_Playeffectforce(int effectID, int whichHaptic)
        {

        }

        public static void UPvr_PlayTimedEffect(int effectDuration, int whichHaptic)
        {

        }

        public static void UPvr_PlayPatternRTP(float vibrationDuration, float vibrationStrength, int whichHaptic, bool large, bool small, int repeat_times, float silienceDuration, float HapticsDuration)
        {

        }
        public static void UPvr_PlayEffectSeqBuff(byte[] Sequence, int buffSize, int whichHaptic)
        {

        }

        public static void UPvr_PlayRTPSequence(String sequence, int whichHaptic)
        {

        }

        public static void UPvr_PlayRTPSeqBuff(byte[] Sequence, int buffSize, int whichHaptic)
        {

        }

        public static void UPvr_PlayRingHaptics(int index, int whichHaptic)
        {

        }

        public static void UPvr_PlayRingSeq(int index, int whichHaptic)
        {

        }
        public static string UPvr_GetRingHapticsName()
        {
            return null;
        }
        public static string UPvr_GetRingHapticsValues()
        {
            return null;
        }
        public static string UPvr_GetRingHapticsValue(int index)
        {
            return null;
        }

        public static bool UPvr_HasControllerVibrator()
        {
            bool value = false;
#if ANDROID_DEVICE
            return Pvr_HasControllerVibrator();
#endif
            return value;
        }

        public static void UPvr_SetControllerVibrateMode(int[] pattern, int length, int repeat)
        {
#if ANDROID_DEVICE
            Pvr_SetControllerVibrateMode(pattern, length, repeat);
#endif
        }

        public static void UPvr_SetControllerVibrateTime(int milliseconds)
        {
#if ANDROID_DEVICE
            Pvr_SetControllerVibrateTime(milliseconds);
#endif

        }

        public static void UPvr_CancelControllerVibrate()
        {
#if ANDROID_DEVICE
            Pvr_CancelControllerVibrate();
#endif

        }

        #endregion

    }

}