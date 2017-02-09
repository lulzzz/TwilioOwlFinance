namespace Twilio.OwlFinance.Domain
{
    public static class OwlFinanceUris
    {
		public static readonly string ApiBaseUri = "https://YOUR-API-URL.com/api/";
		public static readonly string SignalRUri = "https://YOUR-WEB-URL.com/";
		public static readonly string GetTransactions = "accounts/{0}/transactions";
		public static readonly string CreateACase = "cases/create";
		public static readonly string GetIpMessagingToken = "twilio/ip-messaging/token?device={0}&identityId={1}&name={2}&picture={3}";
		public static readonly string GetAccountInformation = "accounts/{0}/info";
		public static readonly string GetAccountBalance = "accounts/{0}/balance";
		public static readonly string GetTransactionDetail = "transaction/{0}/detail";
        public static readonly string DoesCaseExist = "cases/{0}/doesexist";
        public static readonly string CreateCase = "cases";
		public static readonly string CaseMessages = "messages/{0}";
		public static readonly string GetConversationsToken = "twilio/conversations/token?name={0}";
	}
}
