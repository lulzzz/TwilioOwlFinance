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
using Twilio.OwlFinance.Domain.Model.Documents;
using Twilio.OwlFinance.Infrastructure.Docusign;

namespace Twilio.OwlFinance.Services
{
    public class DocuSignService : BaseService, IDocuSignService, IDisposable
    {
        private readonly IRepository<DocuSignLog> repository;
        private readonly IRepository<Case> caseRepository;
        private readonly ITwilioApiSettingsProvider settings;
        public DocuSignService(ILogger logger, IRepository<DocuSignLog> repository, ITwilioApiSettingsProvider settings, IRepository<Case> caseRepository) : base(logger)
        {
            this.repository = repository;
            this.settings = settings;
            this.caseRepository = caseRepository;
        }


        public async Task<ApiResponse<DocuSignResponseModel>> SendDocument(DocumentSignModel model, string serverPath)
        {
            
            var docusignManager = new DocuSignManager();

            var authInformation = new DocuSignAuthHeader
            {
                Username = settings.Docusign.UserName,
                Password = settings.Docusign.Password,
                IntegratorKey = settings.Docusign.IntegratorKey
            };

            var caseModel = caseRepository.Query(c => c.ID == model.CaseID).FirstOrDefault();

            var responseModel = docusignManager.SendDocument(model, caseModel, authInformation, serverPath);

            var docusignLog = new DocuSignLog
            {
                CaseID = model.CaseID,
                DocumentID = responseModel.DocumentID,
                EnvelopeID = responseModel.EnvelopeID,
                CreatedDate = DateTime.Now
            };

            repository.Add(docusignLog);
            await repository.SaveChanges();

            return new ApiResponse<DocuSignResponseModel>(responseModel);

        }

        public ApiResponse<DocuSignResponseModel> GetDocument(int caseID, string serverPath, string host)
        {
            try
            {
                var docusignLog = repository.Query(r => r.CaseID == caseID).FirstOrDefault();

                if (docusignLog != null)
                {

                    var docusignManager = new DocuSignManager();

                    var authInformation = new DocuSignAuthHeader
                    {
                        Username = settings.Docusign.UserName,
                        Password = settings.Docusign.Password,
                        IntegratorKey = settings.Docusign.IntegratorKey
                    };

                    var responseModel = docusignManager.GetDocument(docusignLog, authInformation, serverPath, host);
                    return new ApiResponse<DocuSignResponseModel>(responseModel);
                }
                throw new ArgumentException("No document found");
            }
            catch (Exception e)
            {
                return HandleErrorAndReturnStatus<ApiResponse<DocuSignResponseModel>>(e);
            }
        }

        #region IDisposable Impl
        private bool disposed;

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
                    repository.Dispose();
                }

                disposed = true;
            }
        }
        #endregion
    }
}
