# Working with the Unity splash screen

## The problem with splash screens

Unity provides a system to display a splash screen as the app is launched and loaded for the first time. On Pico devices it is recommended that users of the professional version of Unity disable this functionality, and create their own implementation of a splash screen if necessary. However, this method does not help users of the personal version of Unity or those who are unable to create their own splash screen implementation for their own reasons.

As stated in the [installation guide](/docs/pico-vr-unity-sdk-installation.md) the Pico SDK requires it's apps to run with XR/VR settings disabled, as it provides it's own VR implementation. This has the unfortunate effect of telling Unity to use the screenspace splashscreen, which causes the splash image to be stretched out across both the users eye displays as the app is launched. This effect is unpleasant and confusing on the eyes, and should be resolved using the method beneath.

## Using Unity's VR Splash screen with the Pico SDK.

> Code and technique courtesy of developer cmdr2.

To enable Unity's XR splash screen, while still using a non-xr setting during the apps runtime apply these steps to your project.

### For Unity 5.6.6:
1.  In the Unity project's Player Settings, set "Virtual Reality Supported" checkbox to true. And pick "Split Stereo Display" in the headset option.
2.  Set the VR Splash Screen image in Player Settings to anything. I continue to use "Made with Unity", but use a vertically inverted version since Unity locks orientation to "Landscape Left" in the Stereoscopic headset option. You could use any other image you want, but vertically inverted.
3.  In the Start() method of any class, call StartCoroutine(SetEmptyVRDevice()) as early as possible in the game.

        IEnumerator SetEmptyVRDevice() {
            VRSettings.LoadDeviceByName("");
            yield return new WaitForEndOfFrame();
            VRSettings.enabled = false;
        }

### For Unity 2017+:
1.  In Player Settings, set "XR Supported" checkbox to true, and pick "Mock HMD" in the headset option.
2.  You can use any VR Splash Screen image in Player Settings, but don't need to invert the image.
3.  Same code needs to be called in the Start() method as above, replacing the reference of 'VRSettings' to 'XRSettings'.
