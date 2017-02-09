// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace OwlFinance.ViewControllers
{
    [Register ("SettingsViewController")]
    partial class SettingsViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AccountNumberLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LoggedInEmailLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem RevealButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel VersionNumberLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AccountNumberLabel != null) {
                AccountNumberLabel.Dispose ();
                AccountNumberLabel = null;
            }

            if (LoggedInEmailLabel != null) {
                LoggedInEmailLabel.Dispose ();
                LoggedInEmailLabel = null;
            }

            if (RevealButton != null) {
                RevealButton.Dispose ();
                RevealButton = null;
            }

            if (VersionNumberLabel != null) {
                VersionNumberLabel.Dispose ();
                VersionNumberLabel = null;
            }
        }
    }
}