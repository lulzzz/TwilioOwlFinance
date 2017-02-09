using System;
using Autofac;
using OwlFinance.Helpers;
using OwlFinance.Managers;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using UIKit;
using Xamarin.SWRevealViewController;

namespace OwlFinance.ViewControllers
{
    partial class LoginViewController : BaseUIViewController
    {
        private readonly Auth0Helper client = new Auth0Helper(EnvironmentConstants.Auth0Login, EnvironmentConstants.Auth0Password);

        public LoginViewController(IntPtr handle) 
			: base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			NavigationController?.SetNavigationBarHidden(true, false);
            SignInButton.Layer.CornerRadius = 15;
            SignInButton.TouchUpInside += SignInButton_TouchUpInside;
        }

        async void SignInButton_TouchUpInside(object sender, EventArgs e)
        {
            var user = await client.LoginAsync(this, withRefreshToken: true);

            if (user != null)
            {
                AppSettingsManager.AuthToken = user.IdToken;

                var userProfile = user.Profile;
                var userId = user.Profile.GetValue("user_id");
                var userEmail = user.Profile.GetValue("email");
                var clientIdJToken = user.Profile.GetValue("clientID");
                var nickName = user.Profile.GetValue("nickname");
                var picture = user.Profile.GetValue("picture");

                if (userId != null) AppSettingsManager.LoggedInUserId = userId.ToString();
                if (userEmail != null) AppSettingsManager.LoggedInUserEmail = userEmail.ToString();
                if (clientIdJToken != null) AppSettingsManager.ClientId = clientIdJToken.ToString();
                if (nickName != null) AppSettingsManager.UserNickName = nickName.ToString();
                if (picture != null) AppSettingsManager.UserPictureUrl = picture.ToString();

                var accountInformation = await Application.Container.Resolve<IAccountService>().GetAccountInformation(AppSettingsManager.LoggedInUserId);
                if (accountInformation != null)
                {
                    AppSettingsManager.AccountId = accountInformation.AccountId.ToString();
                    AppSettingsManager.AccountNumber = accountInformation.AccountNumber;
                }

                var loginController = GetViewController(MainStoryboard, "DashboardController") as DashboardViewController;
                var revealViewController = this.RevealViewController();
                var navctrl = new UINavigationController(loginController);

                if (revealViewController != null)
                {
                    revealViewController.PushFrontViewController(navctrl, false);
                }
                else
                {
                    NavigationController.PresentViewController(navctrl, true, null);
                }
            }
        }
    }
}