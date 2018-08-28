# Pico localization

## Implementation

When localizing Unity VR experiences for the Pico Goblin or Pico Neo headsets, you can use [Unity's `Application.systemLanguage`](https://docs.unity3d.com/ScriptReference/SystemLanguage.html) to establish the device's language.

For example:

```cs
if (Application.systemLanguage == SystemLanguage.Chinese)
{

} else {

}
```

## What languages do I need?

Localization is an *optional step* for distributing your experience through the Pico store.

However, if you wish to maximise engagement with all of your users, the current list of languages Pico devices support are as follows:

| Language | Language (in English) | `SystemLanguage.*` |
| :---: | :---: | :---: |
| 中文 | Chinese | `SystemLanguage.Chinese` |
| English | English | `SystemLanguage.English` |
| Français | French | `SystemLanguage.French` |
| Deutsche | German | `SystemLanguage.German` |
| Italiano | Italian | `SystemLanguage.Italian` |
| 日本語 | Japanese | `SystemLanguage.Japanese` |
| Español | Spanish | `SystemLanguage.Spanish` |

## Testing localization

See [Changing a Pico Goblin's language setting](/docs/changing-pico-goblins-language-setting.md).
