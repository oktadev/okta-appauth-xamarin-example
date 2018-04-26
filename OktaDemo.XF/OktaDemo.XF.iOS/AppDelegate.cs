using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using OpenId.AppAuth;
using UIKit;

namespace OktaDemo.XF.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        // The authorization flow session which receives the return URL from SFSafariViewController.
        public IAuthorizationFlowSession CurrentAuthorizationFlow { get; set; }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(
            UIApplication application, NSUrl url,
            string sourceApplication, NSObject annotation)
        {
            // Sends the URL to the current authorization flow (if any) which will process it if it relates to
            // an authorization response.
            if (CurrentAuthorizationFlow?.ResumeAuthorizationFlow(url) == true)
            {
                return true;
            }

            // Your additional URL handling (if any) goes here.

            return false;
        }
    }
}
