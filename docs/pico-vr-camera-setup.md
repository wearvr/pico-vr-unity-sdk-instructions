# Pico VR Unity SDK camera setup

Delete the existing MainCamera from your scene and drag the prefab `Pvr_UnitySDK/Prefabs/Pvr_UnitySDK.prefab` in to replace it. If necessary, reposition the new camera prefab to where the old one was.

<p align="center">
  <img alt="Drag the Pvr_UnitySDK.prefab into your scene" width="500px" src="assets/DragPrefabIntoScene.png">
</p>

## Running in the Unity editor

Once you have dragged the `Pvr_UnitySDK` prefab into your scene, the Pico VR SDK supports running your VR app in the Unity editor. You can use this to test your progress as you complete the remaining instructions.

While running in the editor, the following controls are available to you to simulate head movement:

* Holding **alt + moving the mouse** simulates the user turning their head
* Holding **ctrl + moving the mouse** simulates the user tilting their head
* Holding **alt + left clicking the mouse** allows selecting between VR Mode and Mono mode

There is currently no way to simulate button presses from the Hummingbird controller in the Unity editor.

## Raycaster setup

### PhysicsRaycaster

Add a `PhysicsRaycaster` component to the `Head` camera.

<p align="center">
  <img alt="Add PhysicsRaycaster to Head camera" width="500px" src="assets/AddPhysicsRaycasterImage.png">
</p>

## GraphicRaycaster setup

Add a `Canvas` and `Layer` to your scene if you do not already have one.

Configure the `Canvas` to render in **World Space**.

Set the **Event Camera** to the `Head` inside the `Pvr_UnitySDK` prefab you added to your scene.

<p align="center">
  <img alt="Add a worldspace canvas with Head as the event camera" width="500px" src="assets/SelectWorldSpaceImage.png">
</p>

## Pointer input

The Pico VR SDK adapts the motion of the headset or Hummingbird controller to Unity’s `Pointer*` events. This means you can attach handlers for these events in one of the usual ways:

* Attach an `EventTrigger` with one or more `Pointer*` events
* Attach a script that implements one or more of the [Unity pointer handler interfaces](https://docs.unity3d.com/ScriptReference/EventSystems.IPointerClickHandler.html).

### Selecting an input module

The Pico SDK provides a choice of two input modules:

`Pvr_UnitySDKSightInputModule`: Requires the user to look at an object or UGUI element and press theMenu button to trigger a click action. This is the default input module used in the `Pvr_UnitySDK` prefab and to use it, you do not need to make any changes.

`Pvr_GazeInputModule`: Displays a time-based cursor that “completes” when a user gazes at a GameObject or UGUI element for long enough to automatically trigger a click action.

To use this input module, remove all other input modules from `Pvr_UnitySDK/Event` in your scene and add `Pvr_UnitySDK/System/Event/Pvr_GazeInputModule`. Drag `Pvr_UnitySDK/System/Event/Pvr_GazeInputModuleCrosshair` into the **Crosshair** field.

### Using an EventTrigger

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

### Using a pointer handler interface

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

### Next: Binding to buttons

See [Pico goblin headset and hummingbird controller buttons](/docs/pico-goblin-buttons-hummingbird-controller.md).
