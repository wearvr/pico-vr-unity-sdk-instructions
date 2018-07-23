# Pico VR Unity SDK Installation

## Importing the unitypackage

The Pico Unity VR SDK comes as a .unitypackage that you can import into your project via the **Assets › Import Package › Custom Package...** menu option in Unity.

<p align="center">
  <img alt="Import the .unitypackage as custom package"  width="500px" src="assets/ImportUnityPackageImage.png">
</p>

This will add a number of directories to your project:

<p align="center">
  <img alt="Files included in the unity package"  width="500px" src="assets/VRSDKAssetsImage.png">
</p>

## Project settings

### Disable multi-threaded rendering

Open **Edit › Project Settings › Player**. In the Android tab, uncheck **Multithreaded Rendering**

<p align="center">
  <img alt="Uncheck multi-threaded rendering"  width="500px" src="assets/UncheckMultithreadedRenderingImage.png">
</p>

### Disable the splash screen

As the VR cameras only initialise after the splash screen has been displayed, the splash image does not display correctly in the headset.

If you are using the premium version of Unity, it is recommended to disable the splash screen and set the static splash image to a solid black image in **Project Settings**.

<p align="center">
  <img alt="Disable the splash screen" width="500px" src="assets/DisableSplashImage.png">
</p>

### Disable bundled Unity VR SDKs

Depending on the version of Unity you are using, the **Virtual Reality Supported** option can be found in **Other Settings** or **XR Settings**. Make sure it is **NOT** checked to avoid conflicts with the Pico VR SDK.

<p align="center">
  <img alt="Uncheck Virtual Reality Supported" width="500px" src="assets/DisabledVirtualRealitySupportImage.png">
</p>

## Quality Settings

Open **Edit › Project Settings › Quality** and change the following settings:

### Turn off vertical sync

When using Unity 5.4 or higher in the **Other** section, change **V sync Count** to **Don’t Sync**

<p align="center">
  <img alt="Turn off vertical sync" width="500px" src="assets/DisableVerticalSyncImage.png">
</p>

## AndroidManifest.xml file

If your project does not already have a `Assets/Plugins/Android/AndroidManifest.xml` file, you can use the one installed by the Pico SDK unity package.

If you already have an `AndroidManifest.xml` file in your project, you will need to manually merge in the values found in the unity package’s `AndroidManifest.xml`.

## Build settings

Building directly to a Pico headset is the recommended method of running your app.

As the Pico operating system is an Android derivative, the build platform for your project must be set to **Android**.

This can be done via the **File › Build Settings** menu option.

Select **Android** and then click the **Switch Platform** button.

Change the **Texture Compression** option to **ETC2 (GLES 3.0)**.

<p align="center">
  <img alt="Switch platforms to Android" width="500px" src="assets/ChangeTextureCompressionImage.png">
</p>

### Camera setup

Delete the existing MainCamera from your scene and drag the prefab `Pvr_UnitySDK/Prefabs/Pvr_UnitySDK.prefab` in to replace it. If necessary, reposition the new camera prefab to where the old one was.

<p align="center">
  <img alt="Drag the Pvr_UnitySDK.prefab into your scene" width="500px" src="assets/DragPrefabIntoScene.png">
</p>

### Running in the Unity editor

Once you have dragged the `Pvr_UnitySDK` prefab into your scene, the Pico VR SDK supports running your VR app in the Unity editor. You can use this to test your progress as you complete the remaining instructions.

While running in the editor, the following controls are available to you to simulate head movement:

* Holding **alt + moving the mouse** simulates the user turning their head
* Holding **ctrl + moving the mouse** simulates the user tilting their head
* Holding **alt + left clicking the mouse** allows selecting between VR Mode and Mono mode

There is currently no way to simulate button presses from the Hummingbird controller in the Unity editor.

### Raycaster setup

#### PhysicsRaycaster

Add a `PhysicsRaycaster` component to the `Head` camera.

<p align="center">
  <img alt="Add PhysicsRaycaster to Head camera" width="500px" src="assets/AddPhysicsRaycasterImage.png">
</p>

### GraphicRaycaster setup

Add a `Canvas` and `Layer` to your scene if you do not already have one.

Configure the `Canvas` to render in **World Space**.

Set the **Event Camera** to the `Head` inside the `Pvr_UnitySDK` prefab you added to your scene.

<p align="center">
  <img alt="Add a worldspace canvas with Head as the event camera" width="500px" src="assets/SelectWorldSpaceImage.png">
</p>

### Pointer input

The Pico VR SDK adapts the motion of the headset or Hummingbird controller to Unity’s `Pointer*` events. This means you can attach handlers for these events in one of the usual ways:

* Attach an `EventTrigger` with one or more `Pointer*` events
* Attach a script that implements one or more of the [Unity pointer handler interfaces](https://docs.unity3d.com/ScriptReference/EventSystems.IPointerClickHandler.html).

#### Selecting an input module

The Pico SDK provides a choice of two input modules:

`Pvr_UnitySDKSightInputModule`: Requires the user to look at an object or UGUI element and press theMenu button to trigger a click action. This is the default input module used in the `Pvr_UnitySDK` prefab and to use it, you do not need to make any changes.

`Pvr_GazeInputModule`: Displays a time-based cursor that “completes” when a user gazes at a GameObject or UGUI element for long enough to automatically trigger a click action.

To use this input module, remove all other input modules from `Pvr_UnitySDK/Event` in your scene and add `Pvr_UnitySDK/System/Event/Pvr_GazeInputModule`. Drag `Pvr_UnitySDK/System/Event/Pvr_GazeInputModuleCrosshair` into the **Crosshair** field.

#### Using an EventTrigger

Using an `EventTrigger` on your UI component or `GameObject` is the simplest way of responding to gaze-based events and show normally be the preferred method.

Add an `EventTrigger` to your UI component or GameObject.

<p align="center">
  <img alt="Add an event trigger" width="500px" src="assets/AddEventTrigger.png">
</p>

Add an event type and select the pointer, drag or scroll event you want to trigger a method.

<p align="center">
  <img alt="Set an event type" width="500px" src="assets/AddEventTypeImage.png">
</p>

If you want to call a method on a script on the current object, add it by dragging it onto your UI component or `GameObject` or by using the **Add Component** button.

Select the object and method you want to call when the event is triggered.

<p align="center">
  <img alt="Select the method to call" width="500px" src="assets/AddEventFunctionImage.png">
</p>

#### Using a pointer handler interface

For greater flexibility, you can use a script that implements one or more of the [Unity pointer handler interfaces](https://docs.unity3d.com/ScriptReference/EventSystems.IPointerClickHandler.html) and attach it to your UI component or GameObject.

For example, a script that responds to when the center button is pressed on the headset has the following structure:

```cs
public class CanvasComponent : MonoBehaviour, IPointerClickHandler
  {
      void OnPointerClick(PointerEventData eventData)
      {

      }
  }
```

Either drag your script onto your UI component or GameObject to add it as a component, or use the **Add Component** button and search for it.

### Binding to buttons

See [Pico goblin headset and hummingbird controller buttons](/docs/pico-goblin-buttons-hummingbird-controller.md).
