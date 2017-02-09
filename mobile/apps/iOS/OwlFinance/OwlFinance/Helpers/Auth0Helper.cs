using System.Threading.Tasks;
using Auth0.SDK;
using Xamarin.Auth;

namespace OwlFinance.Helpers
{
	public class Auth0Helper : Auth0Client
	{
		public Auth0Helper(string domain, string clientId) 
			: base(domain, clientId)
		{
		}

		protected override async Task<WebRedirectAuthenticator> GetAuthenticator(string connection, string scope, string title)
		{
			var authenticator = await base.GetAuthenticator(connection, scope, title);
			authenticator.Title = title;
			authenticator.AllowCancel = false;
			return authenticator;
		}
	}
}