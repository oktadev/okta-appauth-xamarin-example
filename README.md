# Xamarin.Forms authentication with OpenID Connect + Okta
 
These examples show how to build a Xamarin.Forms project (targeting iOS and Android) that uses Okta for easy login. Under the hood, OpenID Connect and the AppAuth pattern is used.

Please read [the blog post][blog-post] to see how this app was created.

**Prerequisites:** Visual Studio or Visual Studio for Mac with Xamarin installed.

> [Okta](https://developer.okta.com/) has Authentication and User Management APIs that reduce development time with instant-on, scalable user infrastructure. Okta's intuitive API and expert support make it easy for developers to authenticate, manage, and secure users and roles in any application.

* [Getting Started](#getting-started)
* [Links](#links)
* [Help](#help)
* [License](#license)

## Getting Started

To install this example application, run the following commands:

```bash
git clone https://github.com/oktadeveloper/okta-appauth-xamarin-example
cd okta-appauth-xamarin-example
```

Open `OktaDemo.SF.sln` in Visual Studio and compile the project.

### Setup Okta

You will need to create an application in Okta to perform authentication. 

Log in to your Okta Developer account (or [sign up](https://developer.okta.com/signup/) if you don’t have an account) and navigate to **Applications** > **Add Application**. Click **Native**, click **Next**, and give the application a name you’ll remember. The default settings are fine! Click **Done** to create the application.

Scroll to the bottom of the General tab to find the client ID. You'll need this value in the next step.

### Project configuration

Open the `OktaDemo.XF/Helpers/Constants.cs` file and update these values:

* `{clientId}` - The client ID of the Okta application
* `{redirectUri}` - The native redirect URI, like `com.oktapreview.nate-example:/callback`
* `{yourOktaDomain}` - Replace with your Okta domain, found at the top-right of the Dashboard page

**Note:** The value of `{yourOktaDomain}` should be something like `dev-123456.oktapreview.com`. Make sure you don't include `-admin` in the value!

#### Android-specific configuration

Open the `OktaDemo.XF.Android/Properties/AndroidManifest.xml` file and update the `android:scheme` property:

```xml
<data android:scheme="com.oktapreview.nate-example"/>
```

> :warning: Note: There is currently a known issue with native redirects on Android. For more information, see [this issue](https://github.com/okta/okta-sdk-appauth-android/issues/8).

#### iOS-specific configuration

Open the `OktaDemo.XF/OktaDemo.XF.iOS/Info.plist` file and update the `CFBundleURLSchemes` property:

```xml
<key>CFBundleURLSchemes</key>
<array>
  <string>com.oktapreview.nate-example</string>
</array>
```

## Links

If you aren't familiar with Xamarin and want to learn the basics, start with this blog post: [Build an App for iOS and Android with Xamarin](https://developer.okta.com/blog/2018/01/10/build-app-for-ios-android-with-xamarin)

If you're building native (non-Xamarin) apps, you can use Okta's AppAuth SDKs:

* [Okta iOS AppAuth SDK](https://github.com/okta/okta-sdk-appauth-ios)
* [Okta Android AppAuth SDK](https://github.com/okta/okta-sdk-appauth-android)

## Help

Please post any questions as comments on the [blog post][blog-post], or visit our [Okta Developer Forums](https://devforum.okta.com/). You can also email developers@okta.com if would like to create a support ticket.

## License

Apache 2.0, see [LICENSE](LICENSE).

[blog-post]: TODO
