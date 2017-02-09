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
    [Register ("MessagesViewController")]
    partial class MessagesViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView MessagesTableView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem RevealButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MessagesTableView != null) {
                MessagesTableView.Dispose ();
                MessagesTableView = null;
            }

            if (RevealButton != null) {
                RevealButton.Dispose ();
                RevealButton = null;
            }
        }
    }
}