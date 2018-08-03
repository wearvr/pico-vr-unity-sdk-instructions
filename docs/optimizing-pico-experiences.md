# Optimizing Pico experiences

The following instructions are optional and should normally only be followed if your VR experience is exhibiting performance issues that:

* Degrade gameplay
* Induce nausea or motion sickness in players
* Create rendering distance issues
* Introduce delays, blurring or lag

These steps can be completed even if you do not have a development kit to test on.

### Check project settings

Check your **Project Settings* match the [recommended values](/docs/pico-vr-unity-sdk-installation.md).

## Performance Profiling

Unity provides the ability to run performance profiling tools on a Pico device that can help you diagnose the root cause of any performance issues your VR experience may have.

### Enabling the internal profiler

Open the **Player Settings** using the **Edit > Project Settings > Player** menu item.

Open the **Other Settings** section and find the **Optimization** section. Check the **Enable Internal Profiler box.

<p align="center">
  <img alt="Turn on USB Debugging" width="500px" src="assets/EnableInternalProfiler.png">
</p>

Open your **Build settings** using the **File > Build Settings**. Check the **Development Build** box.

<p align="center">
  <img alt="Check the Development Build box" width="500px" src="assets/DevelopmentBuildBox.png">
</p>

## Creating the development build

If you have a Pico development kit, connect the device via USB and build and run your application.

If you do not have a development kit, build the APK and [submit it to WEARVR as a test build](https://users.wearvr.com/developers/devices/pico-goblin/test-builds), adding in the testing notes that you would like the profile logs for the build. WEARVR will run it on a Pico headset for you and send you the results so you can use them to diagnose any performance problems.

## Capturing the profile logs

To view the profiling logs of the device, you will need to [enable developer mode](/docs/pico-goblin-developer-mode-usb-debugging.md) and have logcat installed your development machine.

If you have already installed the **Android SDK** or **Android Studio**, it should already be installed on your system. If not, you can get it by downloading the [Android SDK Platform Tools](https://developer.android.com/studio/releases/platform-tools.html).

You can start logcat with the following command:

```
adb logcat
```

The profiling logs will then appear in **logcat** when you run the development build of your VR experience on the Pico device. You will need to ensure the device is still connected to your development machine via USB to receive the logs.

If you do not see any logs, disconnect and reconnect the USB cable. Wait five seconds and then restart **logcat**.

When working correctly, you should see logging like the following appear at regular intervals:

```
10-20 02:44:08.122 4422-4441/? D/Unity: Android Unity internal profiler stats:
10-20 02:44:08.122 4422-4441/? D/Unity: cpu-player>    min: 16.1   max: 16.5   avg: 16.3
10-20 02:44:08.122 4422-4441/? D/Unity: cpu-ogles-drv> min:  0.0   max:  0.0   avg:  0.0
10-20 02:44:08.122 4422-4441/? D/Unity: gpu>           min:  0.0   max:  0.0   avg:  0.0
10-20 02:44:08.122 4422-4441/? D/Unity: cpu-present>   min:  0.3   max:  0.6   avg:  0.4
10-20 02:44:08.122 4422-4441/? D/Unity: frametime>     min: 16.5   max: 16.9   avg: 16.7
10-20 02:44:08.122 4422-4441/? D/Unity: batches>       min:  37    max:  37    avg:  37
10-20 02:44:08.122 4422-4441/? D/Unity: draw calls>    min:  37    max:  37    avg:  37
10-20 02:44:08.122 4422-4441/? D/Unity: tris>          min:  9546  max:  9546  avg:  9546
10-20 02:44:08.122 4422-4441/? D/Unity: verts>         min:  6780  max:  6780  avg:  6780
10-20 02:44:08.122 4422-4441/? D/Unity: dynamic batching> batched draw calls:   0 batches:   0 tris:     0 verts:     0
10-20 02:44:08.122 4422-4441/? D/Unity: static batching>  batched draw calls:   0 batches:   0 tris:     0 verts:     0
10-20 02:44:08.122 4422-4441/? D/Unity: player-detail> physx:  0.0 animation:  0.0 culling  0.0 skinning:  0.0 batching:  0.0 render:  0.0 fixed-update-count: 0 .. 0
10-20 02:44:08.122 4422-4441/? D/Unity: managed-scripts>  update:  0.0   fixedUpdate:  0.0 coroutines:  0.0
10-20 02:44:08.122 4422-4441/? D/Unity: managed-memory>   used heap: 577536 allocated heap: 688128, max number of collections: 0 collection total duration:  0.0
```

## Understanding the logs

Unity has an [excellent guide](https://docs.unity3d.com/Manual/iphone-InternalProfiler.html) that will help you interpret the performance profile logs. This should be read in conjunction with Unity’s explanation of [CPU bound and GPU bound performance issues](https://docs.unity3d.com/Manual/OptimizingGraphicsPerformance.html) to guide your approach to addressing performance problems.
