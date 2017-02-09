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

namespace OwlFinance.Cells
{
    [Register ("MenuCell")]
    partial class MenuCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView MenuItemImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MenuItemLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (MenuItemImageView != null) {
                MenuItemImageView.Dispose ();
                MenuItemImageView = null;
            }

            if (MenuItemLabel != null) {
                MenuItemLabel.Dispose ();
                MenuItemLabel = null;
            }
        }
    }
}