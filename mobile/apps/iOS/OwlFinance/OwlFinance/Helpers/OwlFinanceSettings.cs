using System;
using JosePCL;
using Newtonsoft.Json;
using OwlFinance.Managers;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Security;

namespace OwlFinance.Helpers
{
	public class OwlFinanceSettings : ISettings
	{
		public OwlFinanceSettings() { }

		public bool IsAuthenticated
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(AppSettingsManager.AuthToken))
				{
					try
					{
						var secret = Convert.FromBase64String(EnvironmentConstants.Auth0Secret);
						DecodedToken decoded = JsonConvert.DeserializeObject<DecodedToken>(Jwt.Decode(AppSettingsManager.AuthToken, secret));
						var expiration = DateConstants.UnixEpoch.AddSeconds(decoded.Exp);
						if (DateTime.UtcNow > (expiration - DateConstants.TimSpanPadding))
						{
							return false;
						}
						return true;
					}
					catch (Exception)
					{
						return false;
					}

				}
				return false;
			}
		}

		public string ClientId => AppSettingsManager.AccountId;

	    public string AuthToken => AppSettingsManager.AuthToken;
	}
}