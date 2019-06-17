# Using Pico transmission layers

## Transmission layers explained

Transmission layers, sometimes described as Compositor layers or a Synthesis layer, is a different way of rendering to VR headsets.

Normal rendering will have an applications content rendered onto an 'eye buffer', and then the 'eye buffer' will be sampled by ATW (Asynchronous TimeWarp) to find out what content needs to be re-rendered onto a screen, and then finally it will actually be rendered onto a VR screen.

Using transmission layers a different method can be used. Content rendered in this way does not need to use the 'eye buffer' at all, and instead renders directly to ATW for sampling. As a step is skipped both the speed of rendering as well as the overall sharpness of the result can be improved. The downside is that certain steps must be taken by the developer to enable this behaviour.

## Transmission layer supported textures

The transmission layer supports two transparent texture types:
1.  Standard Texture : standard 2D texture  
2.  Equirectangular Texture: panorama texture (360°) 

## Enabling transmission eye overlays

Eye overlays are added using the **'Pvr_UnitySDKEyeOverlay.cs'** script. This script should be attached to the users **'LeftEye'** and **'RightEye'** objects inside the 'Pvr_UnitySDK' prefab. 

<p align="center">
  <img alt="Splash Screen Menu" width="500px" src="/docs/assets/EyeOverlay.png">
</p>

The Pvr_UnitySDKEyeOverlay script has some configurable settings:
1.  Eye Side : Left or Right. Should be configured for whicher eye it has been placed on.
2.  Image Type: Specify standard or equirectangular texture.
3.  Image Texture: The actual texture that will be shown.
4.  Image Transform: Optional. For standard textures specify the transform (position, rotation, zoom) of the texture.

