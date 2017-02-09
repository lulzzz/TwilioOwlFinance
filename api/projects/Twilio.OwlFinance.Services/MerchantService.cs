using System;
using System.Linq;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Repositories;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;
using Twilio.OwlFinance.Domain.Model.Data;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.Services
{
    public class MerchantService : BaseService, IMerchantService, IDisposable
    {
        private readonly IAppSettingsProvider appSettings;
        private readonly IRepository<Merchant> repository;

        public MerchantService(IRepository<Merchant> repository, IAppSettingsProvider appSettings, ILogger logger)
            : base(logger)
        {
            this.appSettings = appSettings;
            this.repository = repository;
        }

        public async Task<EnumerableApiResponse<MerchantModel>> GetMerchants()
        {
            try
            {
                var baseStorageUrl = this.appSettings.Get("storage:BaseUrl");
                var merchants = this.repository
                    .Query()
                    .Select(merchant => new MerchantModel {
                        Name = merchant.Name,
                        Description = merchant.Description,
                        ImageUrl = baseStorageUrl + "/images/" + merchant.ImagePath
                    })
                    .ToList();
                var response = new EnumerableApiResponse<MerchantModel>(merchants);
                return response;
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<EnumerableApiResponse<MerchantModel>>(e);
            }
        }

        #region IDisposable Impl
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.repository.Dispose();
                }

                this.disposed = true;
            }
        }
        #endregion
    }
}
