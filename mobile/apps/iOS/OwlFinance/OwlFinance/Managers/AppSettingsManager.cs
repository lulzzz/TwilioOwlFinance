using OwlFinance.Helpers;

namespace OwlFinance.Managers
{
	public class AppSettingsManager
	{
		private static string GetSetting(string key)
		{
			return Settings.AppSettings.GetValueOrDefault(key, string.Empty);
		}

		private static void SetSetting(string key, string value)
		{
			Settings.AppSettings.AddOrUpdateValue(key, value);
		}

		public static string AuthToken
		{
			get { return GetSetting("AuthToken"); }
			set{ SetSetting("AuthToken", value); }
		}

		public static string AccountId
		{
			get { return GetSetting("AccountId"); }
			set { SetSetting("AccountId", value); }
		}

		public static string AccountNumber
		{
			get { return GetSetting("AccountNumber"); }
			set { SetSetting("AccountNumber", value); }
		}

		public static string ClientId
		{
			get { return GetSetting("ClientId"); }
			set { SetSetting("ClientId", value); }
		}

		public static string LoggedInUserId 
		{
			get { return GetSetting("LoggedInUserId"); }
			set { SetSetting("LoggedInUserId", value); }
		}

		public static string LoggedInUserEmail 
		{
			get { return GetSetting("LoggedInUserEmail"); }
			set { SetSetting("LoggedInUserEmail", value); }
		}

		public static string UserNickName
		{
			get { return GetSetting("UserNickName"); }
			set { SetSetting("UserNickName", value); }
		}

		public static string UserPictureUrl
		{
			get { return GetSetting("UserPictureUrl"); }
			set { SetSetting("UserPictureUrl", value); }
		}

		public static string CallToken
		{
			get { return GetSetting("CallToken"); }
			set { SetSetting("CallToken", value); }
		}
	}
}