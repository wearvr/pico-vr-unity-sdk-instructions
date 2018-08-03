# Pico Unity SDK Instructions

Instructions for how to create new Unity virtual reality experiences for the Pico Goblin headset (or port existing ones).

<p align="center">
  <img alt="Pico Goblin" width="500px" src="/docs/assets/Pico.svg">
</p>

## The Pico Goblin

#### Support & Revenue share

WEARVR.com, the world's largest independent VR app store, has partnered with Pico Interactive to provide development kits and assistance with promotion, technical support and advice to help get your content into Pico's global marketplace (including China) - at no cost to you. You get the same high revenue share of 70%.

| Region | Developer Revenue Share |
| :---: | :----: |
| Worldwide | 70% |

#### Specifications

View the [full headset specifications](https://www.wearvr.com/developer-center/devices/pico).

#### Requesting a development kit

You can [request a Pico Goblin](/docs/pico-development-kit.md) to help get your VR experiences Pico-compatible.

## Prerequisites

You will require the following in order to develop a Pico app:

* A version of Unity 5.2 or greater (tested up to 2017.2.0) - there are known issues with Unity5.3.5f1.
* Either an Android 4.4 (or later) phone running Android API Level 19 or above, or a Pico Goblin headset
* JDK v1.7.0_01 or later

### User accounts and adding in-app purchases

To access user information on the Pico Goblin headset, or add in-app purchases to your VR content, your app will normally need to already be [registered through WEARVR](https://users.wearvr.com/apps) to generate the necessary access credentials.

If this is a problem, you can get in touch via `devs@wearvr.com` to discuss your needs.

## Overview

You can easily create a new Unity VR app, but the fastest way to get up and running on Pico Goblin is to convert an existing Google Cardboard, Google Daydream or Samsung Gear VR experience.

* [Installing and configuring the Pico VR Unity SDK](/docs/pico-vr-unity-sdk-installation.md)
* [Camera & input module setup](/docs/pico-vr-camera-setup.md)
* [Controller and headset inputs](/docs/pico-goblin-buttons-hummingbird-controller.md)
* [Enabling USB debugging](/docs/pico-goblin-developer-mode-usb-debugging.md)
* [Building to the device](/docs/building-to-pico-goblin.md)

Optional:

* [Working with the current user](/docs/pico-iap-payment-sdk-user-management.md)
* [Adding in-app purchases](/docs/pico-payment-sdk-in-app-purchases.md)
* [Troubleshooting](/docs/troubleshooting.md)
* [Optimizing Pico experiences](/docs/optimizing-pico-experiences.md)

There is an [example project](examples/PicoUnityVRSDKExample/Readme.md) to use as a reference as you follow this guide.

## Uploading and selling your experiences

When you are ready, it's time to release your Pico VR experiences to the global and Chinese Pico stores [through WEARVR.com](https://users.wearvr.com/apps).

## Copyright & Trademarks

These instructions and example project are maintained by WEARVR LLC, the largest independent virtual reality app store. WEARVR is interested in connecting VR content creators and consumers, globally. We love working with the VR community and would be delighted to hear from you at `devs@wearvr.com`.

You can find more information about WEARVR at www.wearvr.com

The Pico Interactive Inc trademark, Pico virtual reality headsets and Pico VR Unity SDK are all owned by [Pico Interactive Inc](https://www.pico-interactive.com/).

