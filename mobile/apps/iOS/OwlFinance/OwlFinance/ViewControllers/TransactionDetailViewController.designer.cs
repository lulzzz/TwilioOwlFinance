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
    [Register ("TransactionDetailViewController")]
    partial class TransactionDetailViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView AmountDetailsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel AmountLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CardDetailsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CardExpirationLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CardHolderNameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CardNumberLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ContactSupportButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DateLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView DescriptionTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MerchantNameLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AmountDetailsView != null) {
                AmountDetailsView.Dispose ();
                AmountDetailsView = null;
            }

            if (AmountLabel != null) {
                AmountLabel.Dispose ();
                AmountLabel = null;
            }

            if (CardDetailsView != null) {
                CardDetailsView.Dispose ();
                CardDetailsView = null;
            }

            if (CardExpirationLabel != null) {
                CardExpirationLabel.Dispose ();
                CardExpirationLabel = null;
            }

            if (CardHolderNameLabel != null) {
                CardHolderNameLabel.Dispose ();
                CardHolderNameLabel = null;
            }

            if (CardNumberLabel != null) {
                CardNumberLabel.Dispose ();
                CardNumberLabel = null;
            }

            if (ContactSupportButton != null) {
                ContactSupportButton.Dispose ();
                ContactSupportButton = null;
            }

            if (DateLabel != null) {
                DateLabel.Dispose ();
                DateLabel = null;
            }

            if (DescriptionTextView != null) {
                DescriptionTextView.Dispose ();
                DescriptionTextView = null;
            }

            if (MerchantNameLabel != null) {
                MerchantNameLabel.Dispose ();
                MerchantNameLabel = null;
            }
        }
    }
}