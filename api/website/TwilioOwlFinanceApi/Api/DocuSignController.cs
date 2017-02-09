using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Api;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.BankingService.Api
{
    public class DocuSignController : BaseAuthenticatedApiController
    {

        private readonly IDocuSignService service;

        public DocuSignController(IDocuSignService service)
        {
            this.service = service;
        }

        [ResponseType(typeof(ApiResponse<DocuSignResponseModel>))]
        [Route("api/docusign/send")]
        [HttpPost]

        public async Task<HttpResponseMessage> SendDocument(DocumentSignModel model)
        {
            string serverPath = HttpContext.Current.Server.MapPath("~");

            var response = await service.SendDocument(model, serverPath);
            return SendHttpResponse(response);
        }

        [ResponseType(typeof(ApiResponse<DocuSignResponseModel>))]
        [Route("api/docusign/document/{id}")]
        [HttpGet]

        public async Task<HttpResponseMessage> GetDocument(int id)
        {
            string serverPath = HttpContext.Current.Server.MapPath("~");
            var host = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + ":" + HttpContext.Current.Request.Url.Port;
            var response = service.GetDocument(id, serverPath, host);
            return SendHttpResponse(response);
        }
    }
}