using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Newtonsoft.Json;
using Twilio.Auth;
using Twilio.OwlFinance.Domain.Interfaces.Repositories;
using Twilio.OwlFinance.Domain.Interfaces.Services;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Domain.Model;
using Twilio.OwlFinance.Domain.Model.Data;
using Twilio.TaskRouter;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.BankingService.Api
{
    public class TwilioController : BaseAuthenticatedApiController
    {
        private readonly IRepository<Agent> repository;
        private readonly IAgentService agentService;
        private readonly ITwilioApiSettingsProvider settings;

        public TwilioController(IRepository<Agent> repository, ITwilioApiSettingsProvider settings, IAgentService agentService)
        {
            this.repository = repository;
            this.settings = settings;
            this.agentService = agentService;
        }

        [ResponseType(typeof(ApiResponse<TwilioToken>))]
        [Route("api/twilio/ip-messaging/token")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetIpMessagingToken(string device, string identityId, string name, string picture)
        {
            if (string.IsNullOrWhiteSpace(identityId))
            {
                identityId = UserIdentityID;
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                name = device == "browser" 
                    ? Identity.Name.Split('@')[0] 
                    : Identity.Name;
            }

            if (string.IsNullOrWhiteSpace(picture))
            {
                picture = Identity.Claims
                    .Where(c => c.Type == ClaimTypes.Uri)
                    .Select(c => c.Value)
                    .SingleOrDefault() ?? string.Empty;
            }

            var identity = new Dictionary<string, string>
            {
                { "identityId", identityId },
                { "name", name },
                { "picture", picture }
            };

            var identityJson = JsonConvert.SerializeObject(identity);

            // create an access token generator w/ specific permission grants
            var accessToken = new AccessToken(settings.Account.Sid, settings.ApiKey, settings.ApiSecret)
            {
                Identity = identityJson
            };

            var ipMessagingGrant = new IpMessagingGrant {
                EndpointId = $"TwilioChatDemo:{identityJson}:{device}",
                ServiceSid = settings.IpMessaging.Service.Sid
            };

            accessToken.AddGrant(ipMessagingGrant);

            var response = new ApiResponse<TwilioToken>(new TwilioToken {
                Identity = identityJson,
                Token = accessToken.ToJWT()
            });

            return SendHttpResponse(response);
        }

        [ResponseType(typeof(ApiResponse<TwilioToken>))]
        [Route("api/twilio/conversations/token")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetConversationsToken(string name)
        {
            // create an access token generator w/ specific permission grants
            var token = new AccessToken(
                settings.Account.Sid,
                settings.ApiKey,
                settings.ApiSecret);

            token.Identity = name == "browser" ? Identity.Name.Split('@')[0] : name;

            var convoGrant = new ConversationsGrant
            {
                ConfigurationProfileSid = settings.Conversation.Sid
            };

            var voiceGrant = new VoiceGrant();

            token.AddGrant(convoGrant);
            token.AddGrant(voiceGrant);

            var response = new ApiResponse<TwilioToken>(new TwilioToken
            {
                Identity = name,
                Token = token.ToJWT()
            });

            return SendHttpResponse(response);
        }

        [ResponseType(typeof(ApiResponse<TwilioToken>))]
        [Route("api/twilio/task-router/worker-token")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTaskRouterWorkerToken()
        {
            var agent = repository
                .Query(a => a.IdentityID == UserIdentityID)
                .SingleOrDefault();

            var capability = new TaskRouterWorkerCapability(settings.Account.Sid, settings.AuthToken, settings.TaskRouter.Workspace.Sid, agent.SID);
            capability.AllowActivityUpdates();
            capability.AllowReservationUpdates();

            var response = new ApiResponse<TwilioToken>(new TwilioToken {
                Identity = UserIdentityID,
                Token = capability.GenerateToken()
            });

            return SendHttpResponse(response);
        }

        [ResponseType(typeof(ApiResponse<TwilioToken>))]
        [Route("api/twilio/task-router/workspace-token")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetTaskRouterWorkspaceToken()
        {
            var capability = new TaskRouterWorkspaceCapability(settings.Account.Sid, settings.AuthToken, settings.TaskRouter.Workspace.Sid);
            capability.AllowFetchSubresources();
            capability.AllowUpdatesSubresources();
            capability.AllowDeleteSubresources();

            var response = new ApiResponse<TwilioToken>(new TwilioToken {
                Identity = UserIdentityID,
                Token = capability.GenerateToken()
            });

            return SendHttpResponse(response);
        }

        [ResponseType(typeof(ApiResponse<TwilioVoiceToken>))]
        [Route("api/twilio/voice-call/token")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetVoiceCallToken()
        {
            var customerPhoneNumber = await agentService.GetAssociatedCustomerPhoneNumber(UserIdentityID);

            var capability = new TwilioCapability(settings.Account.Sid, settings.AuthToken);
            capability.AllowClientOutgoing(settings.AppSid);
           
            var response = new ApiResponse<TwilioVoiceToken>(new TwilioVoiceToken
            {
                Identity = UserIdentityID,
                Token = capability.GenerateToken(),
                PhoneNumber = customerPhoneNumber
            });

            return SendHttpResponse(response);
        }
    }
}
