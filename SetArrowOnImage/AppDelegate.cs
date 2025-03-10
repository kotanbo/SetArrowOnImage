﻿namespace SetArrowOnImage;

[Register ("AppDelegate")]
public class AppDelegate : UIApplicationDelegate {
	public override UIWindow? Window {
		get;
		set;
	}

	public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
	{
		// create a new window instance based on the screen size
		Window = new UIWindow (UIScreen.MainScreen.Bounds);

        var vc = new MainViewController();
        Window.RootViewController = vc;

        // make the window visible
        Window.MakeKeyAndVisible ();

		return true;
	}
}

