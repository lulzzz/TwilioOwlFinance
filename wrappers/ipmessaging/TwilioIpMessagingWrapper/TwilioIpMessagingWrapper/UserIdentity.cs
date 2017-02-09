using Newtonsoft.Json;
// ReSharper disable InconsistentNaming

namespace TwilioIpMessagingWrapper
{
    public class UserIdentity
    {
        public string identityId { get; set; }
        public string name { get; set; }
        public string picture { get; set; }
    }

    public class UserIdentityHelpers
    {
        public static string GetAlCook()
        {
            var userIdentity = new UserIdentity
            {
                identityId = "lxUAp89YjFaJq7oYuQq3GKayrks1tP57",
                name = "acook",
                picture = "https://s.gravatar.com/avatar/7cc06bde78dcabc7caa76f98a76f57f3?s=480"
            };

            return JsonConvert.SerializeObject(userIdentity);
        }

        public static string GetJonDavis()
        {
            var userIdentity = new UserIdentity
            {
                identityId = "lxUAp89YjFaJq7oYuQq3GKayrks1tP57",
                name = "jdavis",
                picture = "https://s.gravatar.com/avatar/3e1d704ba2856ad281d3e687457e8ebd?s=480"
            };

            return JsonConvert.SerializeObject(userIdentity);
        }


    }
}
