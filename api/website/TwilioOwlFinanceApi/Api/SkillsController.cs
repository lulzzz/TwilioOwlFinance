using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.BankingService.Api
{
    public class SkillsController : BaseAuthenticatedApiController
    {
        private readonly ISkillService service;

        public SkillsController(ISkillService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<SkillModel>))]
        [Route("api/skills")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetSkills()
        {
            var response = await service.GetSkills();
            return SendHttpResponse(response);
        }
    }
}
