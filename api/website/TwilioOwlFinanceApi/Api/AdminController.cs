using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api.Admin;

namespace Twilio.OwlFinance.BankingService.Api
{
    public class AdminController : BaseAuthenticatedApiController
    {
        private readonly IAdminService service;

        public AdminController(IAdminService service)
        {
            this.service = service;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<UserModel>))]
        [Route("api/admin/users/{id:int}")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetUser(int id)
        {
            var response = await service.GetUser(id);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<UserModel>))]
        [Route("api/admin/users")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetUsers()
        {
            var response = await service.GetUsers();
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(EnumerableApiResponse<UserModel>))]
        [Route("api/admin/users/customers")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetNonPairedCustomers()
        {
            var response = await service.GetCustomers();
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<UserModel>))]
        [Route("api/admin/users")]
        [HttpPost]
        public async Task<HttpResponseMessage> SaveUser(UserModel model)
        {
            var response = await service.SaveUser(model);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<UserModel>))]
        [Route("api/admin/users/{id:int}")]
        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteUser(int id)
        {
            var response = await service.DeleteUser(id);
            return SendHttpResponse(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <response code="500">Server error, check Error Message property</response>
        /// <response code="400">User is not authorized</response>
        /// <response code="200">Everything is OK</response>
        [ResponseType(typeof(ApiResponse<CreateAgentResponse>))]
        [Route("api/admin/agent")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<HttpResponseMessage> CreateAgent(CreateAgentRequest request)
        {
            var response = await service.CreateAgentAsync(request);
            return SendHttpResponse(response);
        }
    }
}
