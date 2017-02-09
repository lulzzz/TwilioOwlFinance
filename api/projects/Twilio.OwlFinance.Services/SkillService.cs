using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.Services
{
    public class SkillService : BaseService, ISkillService
    {
        public SkillService(ILogger logger)
            : base(logger)
        { }

        public async Task<EnumerableApiResponse<SkillModel>> GetSkills()
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<SkillModel>>(e);
            }
        }
    }
}
