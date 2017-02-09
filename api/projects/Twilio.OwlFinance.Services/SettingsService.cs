using System;
using System.Threading.Tasks;
using Twilio.IpMessaging;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Infrastructure.DataAccess;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.Services
{
    public class SettingsService : BaseService, ISettingsService
    {
        private readonly OwlFinanceDbContext context;
        private readonly ITwilioApiSettingsProvider settings;

        public SettingsService(OwlFinanceDbContext context, ITwilioApiSettingsProvider settings, ILogger logger)
            : base(logger)
        {
            this.context = context;
            this.settings = settings;
        }

        public async Task<EmptyApiResponse> CloseCases()
        {
            await context.Database.ExecuteSqlCommandAsync("UPDATE [Cases] SET Status = 3");
            return new EmptyApiResponse();
        }

        public async Task<EmptyApiResponse> DeleteChannels()
        {
            var client = new IpMessagingClient(settings.Account.Sid, settings.AuthToken);
            var channels = client.ListChannels(settings.IpMessaging.Service.Sid).Channels;
            channels.ForEach(channel => client.DeleteChannel(settings.IpMessaging.Service.Sid, channel.Sid));
            return new EmptyApiResponse();
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
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }

                disposed = true;
            }
        }
        #endregion
    }
}
