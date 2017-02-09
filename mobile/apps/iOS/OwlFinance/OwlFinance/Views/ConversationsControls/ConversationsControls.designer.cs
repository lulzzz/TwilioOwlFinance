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

namespace OwlFinance.Views.ConversationsControls
{
    [Register ("ConversationsControls")]
    partial class ConversationsControls
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ElapsedTimeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton HangUpButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton MuteButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SpeakerButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TalkingToLabel { get; set; }

        [Action ("HangUpButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void HangUpButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("MuteButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void MuteButton_TouchUpInside (UIKit.UIButton sender);

        [Action ("SpeakerButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SpeakerButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (DescriptionLabel != null) {
                DescriptionLabel.Dispose ();
                DescriptionLabel = null;
            }

            if (ElapsedTimeLabel != null) {
                ElapsedTimeLabel.Dispose ();
                ElapsedTimeLabel = null;
            }

            if (HangUpButton != null) {
                HangUpButton.Dispose ();
                HangUpButton = null;
            }

            if (MuteButton != null) {
                MuteButton.Dispose ();
                MuteButton = null;
            }

            if (SpeakerButton != null) {
                SpeakerButton.Dispose ();
                SpeakerButton = null;
            }

            if (TalkingToLabel != null) {
                TalkingToLabel.Dispose ();
                TalkingToLabel = null;
            }
        }
    }
}