using Newtonsoft.Json;

namespace Twilio.OwlFinance.Services.Auth0
{
    public class CreateUserResponse : Auth0Response
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone_number")]
        public string PhoneNumber { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }
    }
}