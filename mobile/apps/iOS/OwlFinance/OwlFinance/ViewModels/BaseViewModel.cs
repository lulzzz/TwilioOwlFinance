using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace OwlFinance.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged Implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion


		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}