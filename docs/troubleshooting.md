# Troubleshooting Pico VR

## Error: This device does not support this applications

If you get the following error when you attempt to run your experience on a Pico Goblin:

<p align="center">
  <img alt="Check the display buffer option" width="500px" src="assets/PicoWarning.png">
</p>


Check that you have the [camera set to three degrees of freedom](/docs/pico-vr-camera-setup.md#pico-goblin-3-degrees-of-freedom).

## Compilation problems

If your application behaves unusually when compiled, try checking the **Use 32-bit Display** buffer option in Player Settings.

<p align="center">
  <img alt="Check the display buffer option" width="500px" src="assets/DisplayBufferImage.png">
</p>

## UGUI problems

If you are having trouble with **UGUI** and you’re using a version of Unity below Unity 5.3.5 P5, try updating to a compatible version or higher as outlined by your current Pico SDK documentation.

## Application freezes when minimised and reloaded

If you’re using a version of Unity below Unity v2017.1.1, and are using a Pico SDK v2.7.4 or above, some technical issues will be present when minimising your app. It is strongly reccomended that you update your app to run on a newer version of Unity, as specified by your current Pico SDK doumentation.

## Frame rate or performance issues

Try reducing the quality settings in **Edit › Project Settings › Quality**.

Check the Simple level in the Android column.

<p align="center">
  <img alt="Check the Simple quality option" width="500px" src="assets/ReduceQualitySettingsImage.png">
</p>
