using System.Net;
using System.Net.Http;
using System.Web.Http;
using Twilio.OwlFinance.Domain.Model;

namespace Twilio.OwlFinance.BankingService.Api
{
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Returns HttpResponseMessage based on <see cref="ICanHaveError"/> interface
        /// </summary>
        /// <param name="response">Response to be returned</param>
        /// <typeparam name="T">Of Type <see cref="ICanHaveError"/></typeparam>
        /// <returns>HttpResponseMessage based on <see cref="ICanHaveError"/> interface</returns>
        protected virtual HttpResponseMessage SendHttpResponse<T>(T response) where T : ICanHaveError
        {
            HttpResponseMessage responseMessage;

            if (!response.HasError)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
            }
            else if (response.StatusCode == StatusCodes.ServerError)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.InternalServerError, response);
            }
            else if (response.StatusCode == StatusCodes.UnprocessableEntity)
            {
                responseMessage = Request.CreateResponse((HttpStatusCode)422, response);
            }
            else if (response.StatusCode == StatusCodes.Conflict)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.Conflict, response);
            }
            else if (response.StatusCode == StatusCodes.ItemNotFound)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.NotFound, response);
            }
            else if (response.StatusCode == StatusCodes.NotAuthenticated)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.Unauthorized, response);
            }
            else if (response.StatusCode == StatusCodes.NotAuthorized)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.Forbidden, response);
            }
            else if (response.StatusCode == StatusCodes.BadRequest)
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.BadRequest, response);
            }
            else
            {
                responseMessage = Request.CreateResponse(HttpStatusCode.NoContent);
            }

            return responseMessage;
        }
    }
}
