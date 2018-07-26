# Pico Payment & User Management

The Pico SDK provides the ability to integrate with Pico’s payment and user management infrastructure.

This is an optional step that is only required if your app has in-app purchases or needs user information to function.

## Update AndroidManifest.xml

Before enabling in-app purchases and user account features, you must first ensure the necessary values are present in the AndroidManifest.xml file in your project:

##### Permissions

```
<!-- Access to the device's network interface -->
<uses-permission android:name="android.permission.INTERNET"/>
<uses-permission android:name="android.permission.ACCESS_WIFI_STATE"/>
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE"/>

<!-- Access to configuration files on the device -->
<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE"/>
```

##### Activities

```
<activity android:name="com.pico.loginpaysdk.UnityAuthInterface">
    <intent-filter>
        <action android:name="android.intent.action.MAIN"/>
        <category android:name="android.intent.category.LAUNCHER"/>
    </intent-filter>
</activity>

<activity android:name="com.pico.loginpaysdk.component.PicoSDKBrowser"
    android:configChanges="keyboardHidden|orientation"
    android:windowSoftInputMode="adjustResize"
    android:exported="false">
</activity>
```

## Managing user sessions

### Logging the user in

Before making any in-app purchases, you must first sign the user into their account.

To display the Pico login interface:

```
void PicoPaymentSDK.Login();
```

To receive the result of the login, define a GameObject in your scene called `PicoPayment` (the name is important) and attach a script that defines a `LoginCallback` method.

```
void LoginCallback(string result);
```

Where `result` is a string containing a serialized JSON object representing the result of the sign in request.

#### Successful sign in request

When the login has succeeded, the JSON object has only the following attributes:

* `"access_token"` -  The user’s access token
* `"open_id"` - The user’s OpenID token
* `"refresh_token"` - Token for refreshing the user’s session
* `"expires_in"` - Token describing when the user’s session will expire


Example:

```
{
  "access_token": "25ba00fb73343ff1ec32e1c152fff291",
  "open_id": "2890d4a291108e73ef0e87340affe7a4",
  "refresh_token": "5a189befeb3b33f7df101fbecffe4f98",
  "expires_in": "1d6ef7f25a7b0ec3bbd5b6bf247adf71"
}
```

> The access token and open_id values must be stored in CommonDic for the subsequent user details request to work (see example implementation below).

#### Example implementation

```cs
using LitJson;

public class Callback : MonoBehaviour{
    public void LoginCallback(string result) {
        JsonData response = JsonMapper.ToObject(result);

        if (response["cancel"] != null ) {
            // User closed sign in UI
        } else (response["exception"] != null ) {
            // Sign in error occurred
        } else {
            // Access token and openID must be saved in CommonDic for user details
            // request to work later
            CommonDic.getInstance().access_token = response["access_token"];
            CommonDic.getInstance().open_id = response["open_id"];
        }
    }
}
```

#### Logging in error handling

The following responses should be handled by your app:

| Response (after being parsed as JSON) | Description | Suggested app behaviour |
| :---: | :--- | :--- |
| `{ "cancel": "cancel" }` | The user has closed the sign in UI without signing in.<br/><br/>No purchases can take place until the user has successfully signed in. | Honor the user’s wish to cancel and return them to before they selected the item to purchase. |
| `{ "exception": "..." }` | The sign in attempt has failed. The user may be able to re-attempt the request immediately, or the problem may be more persistent.<br/><br/>No purchases can take place until the user has successfully signed in.<br/><br/>A non-exhaustive list of error codes:<br/>`00061000`: OAuth user token was not found<br/>`00061001`: OAuth user token was invalid<br/>`00061002`: Unknown token error<br/>`00060000`: User could not be found<br/>`00060001`: Password was incorrect<br/>`00060002`: Username was incorrect | Provide the user the option to either re-attempt to sign in, or cancel and return to a suitable place in your app. |

#### Logging in troubleshooting

The following error conditions should not normally occur. If they do, they may indicate that the SDK has not been correctly integrated into your project.

| Response (after being parsed as JSON) | Description | Suggested app behaviour |
| :---: | :--- | :--- |
| `{ "exception": "Please install Pico Client" }` | The SDK cannot access the resources it needs to function. | Make sure you are running your app on the device and not in Unity. |

### Getting the current user session

Once the user has logged in, you can retrieve the session details at any time using PicoPaymentSDK.GetUserAPI().

```cs
void PicoPaymentSDK.GetUserAPI ();
```

To receive the result of the request for the current user’s details, define a `UserInfoCallback` method on the `PicoPayment` GameObject script you added earlier.

```cs
void UserInfoCallback(string result);
```

Where `result` is a string containing a serialized JSON object representing the result of the request for the current user’s details.

The JSON object has the following attributes:
* `"ret_code"` - A code indicating the status of the request. See Current User Request Statuses below for all available codes.
* `"ret_msg"` - A description of the response status. Use `"ret_code"` to check the status of the request rather than this attribute.
* `"data"` - An object with the current user’s details. This attribute is only present when the request succeeds. See User Attributes below for further details.
* `"exception"` - Present when a client-side exception occurs and provides a description of the error.

#### Example implementation

```cs
using LitJson;

public class Callback : MonoBehaviour{
    public void UserInfoCallback(string result) {
        JsonData response = JsonMapper.ToObject(result);

        if (response["ret_code"] == "0000" ) {
            // Current user request successful

            JsonData user = response["data"];
        } else if (response["exception"] == "user is not login") {

            PicoPaymentSDK.Login()

        } else {
            // Current user request failed
        }
    }
}
```

#### Successful user details request

| Response (after being parsed as JSON) | Description |
| :---: | :--- |
| `{ "ret_code": "0000", "data": { ... } }` | The request for the current user’s details has succeeded. You can now store and refer to the user attributes in `"data"`. |

#### User attributes

Users are required to have an email or a phone number. All other fields are optional.

| results\["data"\]\[*\] | Type | Description |
| :--- | :--- | :--- |
| `"username"` | String | User’s account name |
| `"email"` | String | User’s email address |
| `"avatar"` | String | URL to the user’s (.jpg or .png) avatar image |
| `"gender"` | String | User’s gender: `"male"` or `"female"` |
| `"birthday"` | Integer or String | Number of milliseconds between Epoch and 12:00 AM on the user’s date of birth, or an empty string if not set |
| `"firstname"` | String | User’s first name |
| `"lastname"` | String | User’s last name |
| `"aboutme"` | String | User’s About Me bio information |
| `"phone"` | String | User’s phone number |
| `"country"` | String | User’s country name ("China" for all users of the Chinese store) |
| `"city"` | String | User’s city name |


#### User details error handling

The following responses should be handled by your app:

| Response (after being parsed as JSON) | Description | Suggested app behaviour |
| :---: | :--- | :--- |
| `"ret_code": "00080001"`<br/>`"ret_code": "00090001"`<br/>`"ret_code": "00100001"` | The server refused one of the OAuth parameters.<br/><br/>`00080001`: Invalid OAUTH_CODE<br/>`00090001`: Invalid REFRESH_TOKEN<br/>`00100001`: Invalid ACCESS_TOKEN | Explain the request to receive the current user’s details has failed and provide the user the option to sign in again or cancel and return to a suitable place in your app. |
| `"ret_code": "00020000"` | The server found the user data, but it failed some validation checks. | Explain to the user that their user details were invalid and ask them to check their account on picovr.com before attempting again. |
| `"ret_code": "00003000"`<br/>`"ret_code": "00003001"`<br/>`"ret_code": "9999"` | The server refused one of the OAuth parameters.<br/><br/>`00003000`: The response signature could not be verified.<br/>`00003001`: The times on the device and the server did not match.<br/>`9999`: A system error occurred on the server. | Retry the request before explaining there was an error retrieving their user details and to try again later. Return them to a suitable place in your app in the meantime. |
| `"exception": "user is not login"` | The user is not logged in. | Call `PicoPaymentSDK.Login()` to display the login workflow and try again. |
| `"exception": "..."` | A client-side exception has occurred somewhere in creating, sending or receive the request for the user details. | Retry the request before explaining there was an error retrieving their user details and to try again later. Return them to a suitable place in your app in the meantime. |

#### User details troubleshooting

The following error conditions should not normally occur. If they do, they may indicate that the SDK has not been correctly integrated into your project.

| Response (after being parsed as JSON) | Description | Suggested app behaviour |
| :---: | :--- | :--- |
| `"ret_code": "00001000"` | The `access_key` or `open_id` parameters stored in `CommonDic` are invalid. | Check that you are setting access_key or open_id correctly in LoginCallback() before calling PicoPaymentSDK.GetUserAPI(). |
| `"ret_code": "00070001"` | The App credentials failed validation. | Check the `pico_app_id` in your AndroidManifest. |
| `"ret_code": "00071001"` | Invalid app secret key. | Check the `pico_app_key` in your AndroidManifest. |
| `"ret_code": "00110001"` | Invalid Scope.	 | Check the `pico_scope` in your AndroidManifest. |

## Next: In-app purchases

See [adding in-app purchases](/docs/pico-payment-sdk-in-app-purchases.md).
