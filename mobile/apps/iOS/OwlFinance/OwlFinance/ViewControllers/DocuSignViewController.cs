using System;
using Autofac;
using Foundation;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using UIKit;

namespace OwlFinance.ViewControllers
{
    public partial class DocuSignViewController : UIViewController
    {
		private static ISignalRService SignalRService => Application.Container.Resolve<ISignalRService>();
		
		public string DocuSignUrl { get; set; }
        
		public DocuSignViewController (IntPtr handle) 
			: base (handle)
        {
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			DoneSigningButton.Clicked += DoneSigningButton_Clicked;

			if (!string.IsNullOrWhiteSpace(DocuSignUrl))
			{
				SignatureWebView.LoadRequest(new NSUrlRequest(new NSUrl(DocuSignUrl)));
			}
		}

		public override async void ViewDidDisappear(bool animated)
		{
			await SignalRService.SendAsync("SIGNED");
			base.ViewDidDisappear(animated);
		}

		private void DoneSigningButton_Clicked(object sender, EventArgs e)
		{
			DismissModalViewController(true);
		}

		private void CloseButton_TouchUpInside(object sender, EventArgs e)
		{
			DismissModalViewController(true);
		}
	}
}