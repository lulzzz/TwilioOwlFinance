using System;
using Autofac;
using Foundation;
using HockeyApp.iOS;
using OwlFinance.Helpers;
using OwlFinance.ViewControllers;
using Twilio.OwlFinance.Domain.Interfaces;
using UIKit;
using WindowsAzure.Messaging;
using Xamarin.SWRevealViewController;

namespace OwlFinance
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register("AppDelegate")]
	public class AppDelegate : UIApplicationDelegate
	{
		public override UIWindow Window { get; set; }
		public ISettings Settings => Application.Container.Resolve<ISettings>();
		public UINavigationController NavController { get; private set; }
		public UIStoryboard MainStoryboard
		{
			get { return UIStoryboard.FromName("Main", NSBundle.MainBundle); }
		}

		// Creates an instance of viewControllerName from storyboard
		public UIViewController GetViewController(UIStoryboard storyboard, string viewControllerName)
		{
			return storyboard.InstantiateViewController(viewControllerName);
		}
				
		// Sets the RootViewController of the Apps main window with an option for animation.
		public void SetRootViewController(UIViewController rootViewController, bool animate)
		{
			if (animate)
			{
				var transitionType = UIViewAnimationOptions.TransitionFlipFromRight;
				NavController = new UINavigationController(rootViewController);
				NavController.NavigationBar.BarStyle = UIBarStyle.Default;
				Window.RootViewController = NavController;

				UIView.Transition(
					Window,
					0.5,
					transitionType,
					() => Window.RootViewController = rootViewController,
					null);
			}
			else
			{
				NavController = new UINavigationController(rootViewController);
				Window.RootViewController = NavController;
			}
		}

		public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
		{
			UIApplication.SharedApplication.IdleTimerDisabled = true;
			ProcessNotification(launchOptions);

			//var manager = BITHockeyManager.SharedHockeyManager;
			// Configure it to use our APP_ID
			// Register at http://www.hockeyapp.net/
			//manager.Configure("APP-ID-HERE");

			// Start the manager
			//manager.StartManager();

			if (!Settings.IsAuthenticated)
			{
				// User needs to log in, so show the Login View Controlller
				var loginController = GetViewController(MainStoryboard, "LoginController") as LoginViewController;
				SetRootViewController(loginController, false);
			}

			SetupGlobalAppearances();

			// Register Push Notifications to accept Alert (Message), Sound, and Badge number.
			var settings = UIUserNotificationSettings.GetSettingsForTypes(
				UIUserNotificationType.Sound |
				UIUserNotificationType.Alert |
				UIUserNotificationType.Badge,
				new NSSet());
			UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
			UIApplication.SharedApplication.RegisterForRemoteNotifications();

			return true;
		}

		static void SetupGlobalAppearances()
		{
			// NavigationBar
			UIApplication.SharedApplication.StatusBarStyle = UIStatusBarStyle.LightContent;
			UINavigationBar.Appearance.BackgroundColor = Colors.PureBlueColor;
			UINavigationBar.Appearance.BarTintColor = Colors.PureBlueColor;
			UINavigationBar.Appearance.TintColor = Colors.WhiteColor;
			UINavigationBar.Appearance.SetTitleTextAttributes(
				new UITextAttributes 
				{ 
					Font = UIFont.FromName("Lato-Regular", 18f), 
					TextColor = Colors.WhiteColor 
				});
			// NavigationBar Buttons 
			UIBarButtonItem.Appearance.SetTitleTextAttributes(
				new UITextAttributes 
				{ 
					Font = UIFont.FromName("Lato-Bold", 18f), 
					TextColor = Colors.WhiteColor 
				}, UIControlState.Normal);

			// TabBar
			UITabBarItem.Appearance.SetTitleTextAttributes(
				new UITextAttributes 
				{ 
					Font = UIFont.FromName("Lato-Bold", 18f) 
				}, UIControlState.Normal);
		}

		void AuthController_OnLoginSuccess(object sender, EventArgs e)
		{
			var dashboardController = GetViewController(MainStoryboard, "DashboardController");
			SetRootViewController(dashboardController, true);
		}

		public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
		{
			var rurl = new Rivets.AppLinkUrl(url.ToString());
			var id = string.Empty;

			if (rurl.InputQueryParameters.ContainsKey("id"))
			{
				id = rurl.InputQueryParameters["id"];
			}

			if (rurl.InputUrl.Host.Equals("transactions") && !string.IsNullOrEmpty(id))
			{
				var transactionDetailViewController = GetViewController(MainStoryboard, "DashboardController") as DashboardViewController;
				transactionDetailViewController.SelectedTransactionId = Convert.ToInt32(id);
				var frontNavigationController = new UINavigationController(transactionDetailViewController);

				var rearViewController = GetViewController(MainStoryboard, "MenuController") as MenuViewController;
				var mainRevealController = new SWRevealViewController();

				mainRevealController.RearViewController = rearViewController;
				mainRevealController.FrontViewController = frontNavigationController;
				Window.RootViewController = mainRevealController;
				Window.MakeKeyAndVisible();

				return true;
			}

			NavController.PopToRootViewController(true);
			return true;
		}

		private void ProcessNotification(NSDictionary userInfo)
		{
			// Check to see if the dictionary has the aps key. This is the notification payload you would have sent.
			if (userInfo == null) return;

			var apsString = new NSString("aps");

			if (userInfo != null && userInfo.ContainsKey(apsString))
			{
				var alertKey = new NSString("alert");
				var aps = (NSDictionary)userInfo.ObjectForKey(apsString);

				if (aps.ContainsKey(alertKey))
				{
					var alertMessage = (NSString)aps.ObjectForKey(alertKey);
					var alertView = new UIAlertView("Push Received", alertMessage, null, "OK", null);

					alertView.Show();
				}
			}
		}

		public override void RegisteredForRemoteNotifications(UIApplication app, NSData deviceToken)
		{
			// Connection string from your azure dashboard
			var cs = SBConnectionString.CreateListenAccess(
				new NSUrl(EnvironmentConstants.AzureListenAccessEndPoint),
				EnvironmentConstants.AzurePushAccess);

			// Register our information with Azure
			var hub = new SBNotificationHub(cs, EnvironmentConstants.AzureNamespace);

			hub.RegisterNativeAsync(deviceToken, null, err =>
			{
				if (err != null)
				{
					UIAlertViewHelpers.ShowAlert(
						"Uh Oh!", 
						"There was a problem registering with push notifications. Email jdavis@twilio.com", 
						"Got it!");
				}
				else
				{
					Console.WriteLine("Success");
				}
			});
		}

		public override void ReceivedRemoteNotification(UIApplication app, NSDictionary userInfo)
		{
			// Process a notification received while the app was already open
			ProcessNotification(userInfo);
		}

		public override void OnResignActivation(UIApplication application)
		{
			// Invoked when the application is about to move from active to inactive state.
			// This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
			// or when the user quits the application and it begins the transition to the background state.
			// Games should use this method to pause the game.
		}

		public override void DidEnterBackground(UIApplication application)
		{
			// Use this method to release shared resources, save user data, invalidate timers and store the application state.
			// If your application supports background exection this method is called instead of WillTerminate when the user quits.
		}

		public override void WillEnterForeground(UIApplication application)
		{
			// Called as part of the transiton from background to active state.
			// Here you can undo many of the changes made on entering the background.
		}

		public override void OnActivated(UIApplication application)
		{
			// Restart any tasks that were paused (or not yet started) while the application was inactive. 
			// If the application was previously in the background, optionally refresh the user interface.
		}

		public override void WillTerminate(UIApplication application)
		{
			// Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
		}
	}
}
