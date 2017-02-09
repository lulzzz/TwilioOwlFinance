using System.Threading.Tasks;
using UIKit;

namespace OwlFinance.Helpers
{
	// http://stackoverflow.com/questions/19120841/yes-no-confirmation-uialertview
	public static class UIAlertViewHelpers
	{
		// Displays a UIAlertView and returns the index of the button pressed.
		public static Task<int> ShowAlert(string title, string message, params string[] buttons)
		{
			var tcs = new TaskCompletionSource<int>();
			var alert = new UIAlertView
			{
				Title = title,
				Message = message
			};

			foreach (var button in buttons)
			{
				alert.AddButton(button);
			}

			alert.Clicked += (s, e) => tcs.TrySetResult((int)e.ButtonIndex);
			alert.Show();

			return tcs.Task;
		}
	}
}