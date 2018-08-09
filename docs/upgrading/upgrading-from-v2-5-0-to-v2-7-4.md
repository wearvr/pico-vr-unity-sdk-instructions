# Upgrading from v2.5.0 to v2.7.4

## Install the new unitypackage

<a href="https://users.wearvr.com/developers/devices/pico-goblin/resources/vr-unity-package/versions/v2-7-4" target="_blank">Download v2.7.4 of the Pico VR Unity SDK</a> from WEARVR and import it into your Unity project

<p align="center">
  <img alt="Import the .unitypackage as custom package" width="500px" src="/docs/assets/ImportUnityPackageImage.png">
</p>

### Remove old files

Delete `Assets/Plugins/Android/Pico_PaymentSDK_Unity_V1.0.16.jar` if it appears in your project - this is replaced by `Pico_PaymentSDK_Unity_V1.0.19.jar`.

Delete `Assets/Plugins/Android/hummingbirdservicecontroller.jar` if it appears in your project - this is replaced by `hbcserviceclient.jar`.

### Fix broken reference

Unfortunately, there is a bug in the latest version of the SDK that requires fixing. Replace line 944 of `Assets/Pvr_UnitySDK/Pvr_UnitySDKManager.cs`:

```cs
ViewerToast = transform.Find("Head").Find("Viewertoast").gameObject;
```

with the following:

```cs
ViewerToast = transform.Find("Height").Find("Head").Find("Viewertoast").gameObject;
```

## API changes

### Add controller indexes

Update all references to the following methods by adding an extra first parameter value of `0` (see [Controller indexes](/docs/pico-goblin-and-neo-controllers.md#controller-indexes) for more information):

* `UPvr_GetKeyDown`
* `UPvr_GetKeyUp`
* `UPvr_GetKey`
* `UPvr_GetKeyLongPressed`
* `UPvr_IsTouching`
* `UPvr_GetControllerQUA`

For example:

```cs
using Pvr_UnitySDKAPI;

public class MyClass : MonoBehaviour {

    private void Update()
    {
        if (Controller.UPvr_GetKeyUp(Pvr_KeyCode.TOUCHPAD))
        {
            // Touchpad was just released
        }
    }
}
```

Becomes:

```cs
using Pvr_UnitySDKAPI;

public class MyClass : MonoBehaviour {

    private void Update()
    {
        if (Controller.UPvr_GetKeyUp(0, Pvr_KeyCode.TOUCHPAD))
        {
            // Touchpad was just released
        }
    }
}
```

### Update UPvr_GetSlipDirection references

Update all references to `UPvr_GetSlipDirection`:

* Rename the method call to `UPvr_GetSwipeDirection`
* Add a controller index parameter of `0`
* Remove the `Pvr_SlipDirection` parameter
* Branch on the response of the method, using the corresponding `SwipeDirection` (not the old `Pvr_SlipDirection`)

For example:

```cs
using Pvr_UnitySDKAPI;

public class MyClass : MonoBehaviour {

    private void Update()
    {
        if (Controller.UPvr_GetSlipDirection(Pvr_SlipDirection.SlideDown))
        {
            // Touchpad received a touch gesture downwards since the last frame
        }
    }
}
```

becomes:

```cs
using Pvr_UnitySDKAPI;

public class MyClass : MonoBehaviour {

    private void Update()
    {
        SwipeDirection swipeDirection = Controller.UPvr_GetSwipeDirection(0);

        if (swipeDirection == SwipeDirection.SwipeDown)
        {
            // Touchpad received swipe down gesture since the last frame
        }
    }
}
```


### Update UPvr_GetTouchPadPosition references

Update all references to `UPvr_GetTouchPadPosition`:

* Remove the `axis` parameter and replace it a controller index of `0`
* Change the return type to a `Vector2`

For example:

```cs
using Pvr_UnitySDKAPI;

public class MyClass : MonoBehaviour {

    private void Update()
    {
	    float yCoordinate = Controller.UPvr_GetTouchPadPosition(0);
	    float xCoordinate = Controller.UPvr_GetTouchPadPosition(1);
    }
}
```

becomes:

```cs
using Pvr_UnitySDKAPI;

public class MyClass : MonoBehaviour {

    private void Update()
    {
	    Vector2 touchPosition = Controller.UPvr_GetTouchPadPosition(0);

	    float yCoordinate = touchPosition.y;
	    float xCoordinate = touchPosition.x;
    }
}
```

## Use new prefabs

Delete the existing `Pvr_UnitySDK` prefab from your scene and drag in the new one from `Pvr_UnitySDK/Prefabs/Pvr_UnitySDK.prefab`.

Delete the old `Pvr_Controller` prefab from your scene and [set up the new one](/docs/pico-goblin-and-neo-controllers.md#integrating-with-headset-and-controller-input).
