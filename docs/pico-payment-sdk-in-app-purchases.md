# Pico payment SDK and in-app purchases

This is an optional step that is only required if your app has in-app purchases.

Currently, only apps sold in China support in-app purchases.

> If you wish to release your app in China with in-app purchases, you will need to submit different release builds: one with in-app purchases to be released in China and one without, for the global market.

All purchases through the Pico ecosystem are done in Pico’s own virtual currency as an integer value of P-Coins.

Buyers in the Pico ecosystems add P-Coins to their account through Pico’s website, so it does not need to be handled by your app.

| P-Coins | Yuan | USD |
| :---: | :---: | :---: |
| 10 | ¥ 1 | $0.15 (approx - check the current exchange rate) |

## Creating in-app purchases

> Before your app can create any in-app purchases, you must first have [requested credentials for your app](https://users.wearvr.com/developers/devices/pico-goblin/store-listings/) and [signed the current user in](/docs/pico-payment-sdk-user-management.md).

To make an in-app purchase, you need to use the `PicoPaymentSDK.Pay()` method. You use it in one of two ways, depending on how you wish to register purchases.

You must choose only one of the two methods and use it for all of your in-app purchases. The Pico platform does not support using a combination of the two.

### Option 1: Creating purchases for pre-registered items

Use this option when you have pre-registered a list of items that users can purchase from within your app, that have fixed prices.

```cs
void PicoPaymentSDK.Pay(string purchaseDetails);
```

Where `purchaseDetails` is a string containing a serialized JSON object with the following attributes:

| Attribute | Type | Description |
| :---: | :--- | :--- |
| `"subject"` | String | A short description of the purchase |
| `"body"` | String | A longer description of the purchase |
| `"order_id"` | String | An identifier unique to the current user and purchase. It's up to you how you generate this, but it must be less than 32 characters. |
| `"pay_code"` | String | Product code - this must match the one you have provided when registering your purchase item. |
| `"goods_tag"` | String | An optional tag to attach to the purchase. |

> It's important the purchaseDetails *not* have a "total" attribute at all (with any value), if you use this option.

### Option 2: Creating purchases with arbitrary amounts

Use this option when you have in-app purchases that have not been pre-registered and do not have fixed prices.

```cs
void PicoPaymentSDK.Pay(string purchaseDetails);
```

Where `purchaseDetails` is a string containing a serialized JSON object with the following attributes:

| Attribute | Type | Description |
| :---: | :--- | :--- |
| `"subject"` | String | A short description of the purchase |
| `"body"` | String | A longer description of the purchase |
| `"order_id"` | String | An identifier unique to the current user and purchase. It's up to you how you generate this, but it must be less than 32 characters. |
| `"total"` | Integer | The total price of the purchase in P-coins. |
| `"goods_tag"` | String | An optional tag to attach to the purchase. |

> It's important that the purchaseDetails not have a "pay_code" attribute at all (with any value), if you use this option.

## Processing the purchase response

To receive the purchase response, define a `QueryOrPayCallback()` method on the PicoPayment GameObject script you added earlier.

```cs
void QueryOrPayCallback(string result);
```

Where `result` is a string containing a serialized JSON object representing the result of the purchase request, with the following attributes:

| Attribute | Type | Description |
| :---: | :--- | :--- |
| `"code"` | String | A code indicating the status of the request. |
| `"msg"` | String | A description of the response status. Use"code" to check the status of the request rather than this attribute. |

> The response does not reference what was purchased, so your app will need to record this information before making the request to the Pico servers. When the purchase response arrives, if the purchase was successful, your app can then determine which item to unlock.

#### Successful purchase response

| Response (after being parsed as JSON) | Description |
| :---: | :--- |
| `"code": "12000", "msg": "支付成功"` | Payment succeeded. Your app can now give access to the purchased item. |

#### App purchases error handling

The following responses should be handled by your app (use the `"code"` attribute to do so - the `"msg"` value has only been included to make this page easier to search for error messages):

| Response (after being parsed as JSON) | Description | Suggested app behaviour |
| :---: | :--- | :--- |
| `"code": "ORDER_EXIST", "msg": "订单已存在"` | A purchase with this `"order_id"` already exists. | Retry the purchase with a different purchase order ID. |
| `"code": "PAY_CODE_EXIST", "msg": "用户已对商品代码消费"` | The user has already purchased this non-consumable item. | Give the user access to the purchased content as if they had just completed the purchase. |
| `"code": "12003", "msg": "P币不足"` | The user did not have enough P coins. | Explain to the user that their account did not have enough P coins and that they will need to top up their balance at picovr.com before re-attempting the purchase. |
| `"code": "11002", "msg": "用户参数错误或请求过期"` | Either the user was invalid or the request has expired. | Automatically re-attempt the purchase. If it fails again, explain to the user that there was a problem processing the purchase and ask them to check their account before trying again. |
| `"code": "12001", "msg": "支付失败"` | The purchase was valid, but it failed. | Re-attempt the purchase. If it also fails, explain to the user that there was an error trying to create the purchase and ask them to try again soon. Return them to an appropriate place in your app. |
| `"code": "13001", "msg": "获取数据失败"` | The server failed to retrieve some of the data necessary to process the purchase. | Same as above. |
| `"code": "15001", "msg": "未输入预付ID"` | The server failed to complete the purchase process. | Same as above. |
| `"code": "13002", "msg": "生成订单失败"` | An unknown server error occurred. | Same as above. |
| `"code": "SYSTEMERROR", "msg": "系统错误"` | An system error occurred on the server. | Same as above. |
| `"code": "00000", "msg": "网络异常"` | A network error occurred. | Re-attempt the purchase. If it also fails, ask the user to check their wireless connection before trying the purchase again. |

#### Troubleshooting in-app purchases

The following error conditions should not normally occur. If they do, they may indicate that the SDK has not been correctly integrated into your project.

| Response (after being parsed as JSON) | Description | Suggested app behaviour |
| :---: | :--- | :--- |
| `"code": "10002", "msg": "请输入正确金额或商品码"` | A purchase was attempted with a non-positive price. | Check the value of the `total` attribute you are using to create the purchase. |
| `"code": "10001", "msg": "用户未登录"` | The request to check the current user’s available balance failed because the user was not signed in. | Make sure you sign in your user before attempting to make a purchase. |
| `"code": "11004", "msg": "商户验证失败"` | The purchase failed because `pico_merchant_id`, `pico_app_id` or `pico_pay_key` is missing or invalid from the AndroidManifest file. | Check your AndroidManifest. |
| `"code": "PAY_CODE_NOT_EXIST", "msg": "商品代码不存在"` | The `pay_code` parameter did not match any pre-registered items. | Check that you are using the correct value of this parameter and you have already registered the item.<br/>If you are not purchasing a pre-registered item, check that you are not submitting any value for pay_code. |
| `"code": "15000", "msg": "未输入商品信息"` | `PicoPaymentSDK.Pay()` was called without a serialised JSON object | Check the serialised JSON object you are passing to `PicoPaymentSDK.Pay()`. |
| `"code": "15003", "msg": "未生成订单号/订单号超过32位"` | The `"order_id"` value passed to `PicoPaymentSDK.Pay()` was longer than 32 characters. | Check that your app is not generating order ids longer than 32 characters. |
| `"code": "APP_ID_NOT_EXIST", "msg": "APP_ID不存在"` | An app with the specified ID was not found on the server. | Check the pico_app_id in your AndroidManifest. |
| `"code": "MCHID_NOT_EXIST", "msg": "MCHID不存在"` | WEARVR’s merchant ID could not be validated on the server. | Check the `pico_merchant_id` in your AndroidManifest. |
| `"code": "NOAUTH", "msg": "商户无此接口权限"` | WEARVR’s merchant ID could not be validated on the server. | Check the `pico_merchant_id` in your AndroidManifest. |

## Recording a purchase

Once the Pico servers has confirmed the purchase was successful, it’s up to your application to record that a user has paid for a particular in-app purchase (this information is not easily queried at a later time, or on a different device).

Saving a record of the purchase will typically involve a request to your own game servers to create a record of the item the user has just purchased, or to increase a value associated with the users account (e.g. gold coins, extra lives, etc).

## Testing

Unfortunately, it's not currently possible to fully test your IAP integration without a Chinese version of the headset connected to a Chinese user account (which requires a bank account in China).

However, WEARVR is able to test this for you. You can [submit a test build](https://users.wearvr.com/developers/devices/pico-goblin/test-builds) and include in the testing instructions that you would like the IAP integration tested. Or, you can get in touch with us on `devs@wearvr.com` and we'll be happy to work closely with you to ensure everything has been set up correctly.

WEARVR is currently working with Pico Interactive on improving this process in the future to allow developers to test their IAPs directly.

## Next: Submitting your app

See [submitting your app](/Readme.md).
