using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using JWT;
using Newtonsoft.Json.Linq;

namespace Twilio.OwlFinance.BankingService.Http
{
    public static class JsonWebTokenValidator
    {
        private const string DefaultIssuer = "LOCAL AUTHORITY";

        private static IEnumerable<string> claimTypesForUserName = new[] { "name", "email", "user_id", "sub" };
        private static ISet<string> claimsToExclude = new HashSet<string>(new[] { "iss", "sub", "aud", "exp", "iat", "identities" });
        private static DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static ClaimsPrincipal ValidateToken(
            string token, string secretKey, string audience = null, bool checkExpiration = false, string issuer = null)
        {
            var payloadJson = JsonWebToken.Decode(token, Convert.FromBase64String(secretKey), verify: true);
            var payloadData = JObject.Parse(payloadJson).ToObject<Dictionary<string, object>>();

            // audience check
            object aud;
            if (!string.IsNullOrEmpty(audience) && payloadData.TryGetValue("aud", out aud))
            {
                if (!aud.ToString().Equals(audience, StringComparison.Ordinal))
                {
                    throw new TokenValidationException($"Audience mismatch. Expected: '{audience}' and got: '{aud}'");
                }
            }

            // expiration check
            object exp;
            if (checkExpiration && payloadData.TryGetValue("exp", out exp))
            {
                var validTo = FromUnixTime(long.Parse(exp.ToString()));
                if (DateTime.Compare(validTo, DateTime.UtcNow) <= 0)
                {
                    throw new TokenValidationException($"Token is expired. Expiration: '{validTo}'. Current: '{DateTime.UtcNow}'");
                }
            }

            // issuer check
            object iss;
            if (payloadData.TryGetValue("iss", out iss))
            {
                if (!string.IsNullOrEmpty(issuer) && !iss.ToString().Equals(issuer, StringComparison.Ordinal))
                {
                    throw new TokenValidationException($"Token issuer mismatch. Expected: '{issuer}' and got: '{iss}'");
                }

                // if issuer is not specified, set issuer with jwt[iss]
                issuer = iss.ToString();
            }

            var identity = CreateClaimsIdentityFromJwtData(payloadData, issuer);
            var principal = new ClaimsPrincipal(identity);

            return principal;
        }

        private static ClaimsIdentity CreateClaimsIdentityFromJwtData(IDictionary<string, object> jwtData, string issuer)
        {
            var claims = CreateClaimsFromJwtData(jwtData, issuer);

            var duplicateClaim = claims
                .Where(claim => claim.Type == ClaimTypes.Actor)
                .ElementAtOrDefault(1);
            if (duplicateClaim != null)
            {
                throw new InvalidOperationException(
                    $"Jwt10401: Only a single 'Actor' is supported. Found second claim of type: 'actor', value: '{duplicateClaim.Value}'");
            }

            var identity = new ClaimsIdentity(claims, "Federation", ClaimTypes.Name, ClaimTypes.Role);

            return identity;
        }

        private static ICollection<Claim> CreateClaimsFromJwtData(IDictionary<string, object> jwtData, string issuer)
        {
            var list = jwtData
                .Where(pair => !claimsToExclude.Contains(pair.Key))
                .ToDictionary(k => k.Key, pair => new JArray(pair.Value))
                .SelectMany(
                    pair => pair.Value,
                    (pair, value) => new Claim(pair.Key, value.ToString(), ClaimValueTypes.String, issuer, issuer))
                .ToList();

            // set claim for user name
            // use original jwtData because claimsToExclude filter has sub and otherwise it wouldn't be used
            var claimType = claimTypesForUserName.FirstOrDefault(ct => jwtData.ContainsKey(ct));
            if (claimType != null)
            {
                var value = jwtData[claimType].ToString();
                list.Add(new Claim(ClaimTypes.Name, value, ClaimValueTypes.String, issuer, issuer));
            }

            if (jwtData.Keys.Contains("user_id"))
            {
                var value = jwtData["user_id"].ToString();
                list.Add(new Claim(ClaimTypes.NameIdentifier, value, ClaimValueTypes.String, issuer, issuer));
            }

            if (jwtData.Keys.Contains("picture"))
            {
                var value = jwtData["picture"].ToString();
                list.Add(new Claim(ClaimTypes.Uri, value, ClaimValueTypes.String, issuer, issuer));
            }

            // set claims for roles array
            list.Where(c => c.Type == "roles")
                .ToList()
                .ForEach(r => {
                    list.Add(new Claim(ClaimTypes.Role, r.Value, ClaimValueTypes.String, issuer, issuer));
                });

            list.RemoveAll(c => c.Type == "roles");

            return list;
        }

        private static DateTime FromUnixTime(long unixTime)
        {
            return unixEpoch.AddSeconds(unixTime);
        }

        public class TokenValidationException : Exception
        {
            public TokenValidationException(string message)
                : base(message)
            { }
        }
    }
}
