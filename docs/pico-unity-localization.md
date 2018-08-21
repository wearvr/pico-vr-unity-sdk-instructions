# Pico localization

## What languages do I need?

Localization is an *optional step* for distributing your experience through the Pico store.

However, if you wish to maximise engagement with all of your users, the current list of countries serviced by the Pico store is as follows:

* Australia
* Brazil
* Canada
* China
* France
* Germany
* India
* Italy
* Japan
* Korea
* New Zealand
* Portugal
* Spain
* United Kingdom
* United States

## Implementation

When localizing Unity VR experiences for the Pico Goblin or Pico Neo headsets, you can use [Unity's `Application.systemLanguage`](https://docs.unity3d.com/ScriptReference/SystemLanguage.html) to establish the device's language.

For example:

```cs
if (Application.systemLanguage == SystemLanguage.Chinese)
{

} else {

}
```
