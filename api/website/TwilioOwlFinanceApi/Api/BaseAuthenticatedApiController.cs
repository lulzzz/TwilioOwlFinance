using System.Linq;
using System.Security.Claims;
using System.Web.Http;

namespace Twilio.OwlFinance.BankingService.Api
{
    [Authorize]
    public class BaseAuthenticatedApiController : BaseApiController
    {
        public ClaimsIdentity Identity => User.Identity as ClaimsIdentity;

        public string UserIdentityID => Identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
    }
}
