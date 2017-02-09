using System;
using System.Threading.Tasks;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

namespace Twilio.OwlFinance.Domain.Interfaces.Services
{
    public interface IDocuSignService : IDisposable
    {
        Task<ApiResponse<DocuSignResponseModel>> SendDocument(DocumentSignModel model, string serverPath);
        ApiResponse<DocuSignResponseModel> GetDocument(int caseID, string serverPath, string host);
    }
}
