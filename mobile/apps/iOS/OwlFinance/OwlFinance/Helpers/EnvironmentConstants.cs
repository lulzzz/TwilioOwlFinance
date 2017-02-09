namespace OwlFinance.Helpers
{
	public class EnvironmentConstants
	{
		public static readonly string Auth0Login = "YOUR-SUB-DOMAIN.auth0.com";
		public static readonly string Auth0Password = "AUTH0-GENERATED-PASSWORD";
		public static readonly string Auth0Secret = "AUTH0-GENERATED-SECRET";
		public static readonly string AzureNotificationHub = "AZURE-HUB-NAME";
		public static readonly string AzureNamespace = "AZURE-NAMESPACE";
		public static readonly string AzurePushAccess = "AZURE-ACCSES-KEY";
		public static readonly string AzureListenAccessEndPoint = "sb://" + EnvironmentConstants.AzureNotificationHub + ".servicebus.windows.net/";
	}
}
