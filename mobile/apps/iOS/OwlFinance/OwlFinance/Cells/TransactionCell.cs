using System;
using OwlFinance.Managers;
using Twilio.OwlFinance.Domain.Model.ServiceModel;
using UIKit;

namespace OwlFinance.Cells
{
    public partial class TransactionCell : UITableViewCell
    {
        public TransactionCell (IntPtr handle) 
			: base (handle)
        {
        }

		public void UpdateData(TransactionServiceModel model)
		{
			DateLabel.Text = model.DisplayDate;
			MerchantNameLabel.Text = model.Merchant;
			SummaryLabel.Text = model.Summary;
			AmountLabel.Text = model.DisplayAmount;

			InvokeOnMainThread(() =>
			{
				SetMerchantImage(model.MerchantImageUrl);
			});
		}

		private void SetMerchantImage(string imageUrl)
		{
			try
			{
				MerchantImage.Image = ImageManager.FromUrl(imageUrl);
			}
			catch (Exception)
			{
				// ignored
			}
		}
    }
}