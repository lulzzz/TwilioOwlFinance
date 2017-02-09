using Acr.UserDialogs;
using OwlFinance.Views.ConversationsControls;

namespace OwlFinance.Managers.EventActions
{
	public class RespondingToCallEvent : BaseEventAction
	{
		private static bool _isOpen = false;

		public override void Fire()
		{
			if (_isOpen) return;

			var cfg = new ConfirmConfig
			{
				OkText = "Answer",
				CancelText = "Ignore",
				Message = "Incoming call from agent.",
				OnAction = HandleAction
			};

			_isOpen = true;
			UserDialogs.Instance.Confirm(cfg);
		}

		private void HandleAction(bool ok)
		{
			if (ok)
			{
				AppDelegate.InvokeOnMainThread(async () =>
				{
					await ConversationsControls.Create().ShowAsync();
				});
			}

			_isOpen = false;
		}
	}
}