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
    [Register ("DocuSignViewController")]
    partial class DocuSignViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem DoneSigningButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIWebView SignatureWebView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DoneSigningButton != null) {
                DoneSigningButton.Dispose ();
                DoneSigningButton = null;
            }

            if (SignatureWebView != null) {
                SignatureWebView.Dispose ();
                SignatureWebView = null;
            }
        }
    }
}