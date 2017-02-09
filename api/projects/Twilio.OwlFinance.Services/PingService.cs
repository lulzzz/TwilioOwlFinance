using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.Services
{
    public class PingService : BaseService, IPingService
    {
        public PingService(ILogger logger)
            : base(logger)
        { }

        public async Task<ApiResponse<PingModel>> VerifyPing()
        {
            try
            {
                var data = new PingModel { Message = "All is good" };
                var response = new ApiResponse<PingModel>(data);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<PingModel>>(e);
            }
        }
    }
}
