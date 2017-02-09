using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Twilio.IpMessaging;
using Twilio.OwlFinance.Domain.Extensions;
using Twilio.OwlFinance.Domain.Interfaces;
using Twilio.OwlFinance.Domain.Interfaces.Settings;
using Twilio.OwlFinance.Domain.Interfaces.TaskRouter;
using Twilio.OwlFinance.Domain.Model.TaskRouter;
using Twilio.TaskRouter;
using TwilioTask = Twilio.TaskRouter.Task;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace Twilio.OwlFinance.Infrastructure.TaskRouter
{
    public class TaskRouterManager : ITaskRouterManager
    {
        private readonly TaskRouterClient client;
        private readonly ILogger logger;
        private readonly ITwilioApiSettingsProvider settings;

        protected ILogger Logger => logger;

        public TaskRouterManager(TaskRouterClient client, ITwilioApiSettingsProvider settings, ILogger logger)
        {
            this.client = client;
            this.settings = settings;
            this.logger = logger;
        }

        public async Task<dynamic> GetWorkerInfo(string workerSid)
        {
            var worker = client.GetWorker(settings.TaskRouter.Workspace.Sid, workerSid);
            var model = CreateWorkerModel(worker);
            return model;
        }

        public async Task<dynamic> SetWorkerOffline(string workerSid)
        {
            var result = client.ListReservationsForWorker(settings.TaskRouter.Workspace.Sid, workerSid);
            result.Reservations.ForEach(r => {
                client.UpdateReservation(
                    settings.TaskRouter.Workspace.Sid, r.TaskSid, r.Sid, "rejected", settings.TaskRouter.Activities.Offline.Sid);
            });

            var worker = client.UpdateWorkerActivity(
                settings.TaskRouter.Workspace.Sid, workerSid, settings.TaskRouter.Activities.Offline.Sid);
            var model = CreateWorkerModel(worker);
            return model;
        }

        public async Task<dynamic> SetWorkerOnline(string workerSid)
        {
            var worker = client.UpdateWorkerActivity(
                settings.TaskRouter.Workspace.Sid, workerSid, settings.TaskRouter.Activities.Idle.Sid);
            var model = CreateWorkerModel(worker);
            return model;
        }

        public async Task<dynamic> AcceptTaskReservation(string workerSid, string taskSid, string reservationSid)
        {
            var reservationStatus = "accepted";
            var model = UpdateReservation(workerSid, taskSid, reservationSid, reservationStatus);
            return model;
        }

        public async Task<dynamic> RejectTaskReservation(string workerSid, string taskSid, string reservationSid)
        {
            var reservationStatus = "rejected";
            var model = UpdateReservation(workerSid, taskSid, reservationSid, reservationStatus);
            client.UpdateTask(settings.TaskRouter.Workspace.Sid, taskSid, null, null, "canceled", "Task canceled.");
            await SetWorkerOnline(workerSid);
            return model;
        }

        public async Task<dynamic> CreateTask(int caseId, string agentSid, Skill requiredSkill)
        {
            int? timeout = null;
            int? priority = 1;
            var skill = requiredSkill.ToLowerCaseString();
            var attributes = new Dictionary<string, string> {
                { "selected_skill", skill },
                { "case_id", $"{caseId}" },
                { "agent_sid", agentSid }
            };
            var attributesJson = JsonConvert.SerializeObject(attributes);
            var task = client.AddTask(
                settings.TaskRouter.Workspace.Sid, attributesJson, settings.TaskRouter.Workflow.Sid, timeout, priority);
            var model = CreateTaskModel(task);
            return model;
        }

        public async Task<dynamic> GetOrCreateCaseChannel(int caseId)
        {
            var client = new IpMessagingClient(settings.Account.Sid, settings.AuthToken);

            var friendlyName = $"Messaging channel to discuss case ID = {caseId}";
            var uniqueName = $"case{caseId}";
            var channel = client.GetChannel(settings.IpMessaging.Service.Sid, uniqueName);
            if (channel.RestException?.Status == "404")
            {
                // if channel doesn't exist, create it
                channel = client.CreateChannel(settings.IpMessaging.Service.Sid, "public", friendlyName, uniqueName, (string) null);
            }

            var model = CreateModel(channel);
            return model;
        }

        private dynamic UpdateReservation(string workerSid, string taskSid, string reservationSid, string status)
        {
            var reservation = client.UpdateReservation(
                settings.TaskRouter.Workspace.Sid, taskSid, reservationSid, status, settings.TaskRouter.Activities.Offline.Sid);

            string taskWorkspaceSid = reservation.WorkspaceSid;
            string taskTaskSid = reservation.TaskSid;

            if (string.IsNullOrWhiteSpace(taskWorkspaceSid))
            {
                taskWorkspaceSid = settings.TaskRouter.Workspace.Sid;
            }

            if (string.IsNullOrWhiteSpace(taskTaskSid))
            {
                taskTaskSid = taskSid;
            }
            var task = client.GetTask(taskWorkspaceSid, taskTaskSid);
            var model = CreateTaskModel(task);
            return model;
        }

        private dynamic CreateModel<T>(T obj) where T : TwilioBase
        {
            dynamic model = obj.ToDynamic();

            var property = typeof(T).GetProperty("Attributes");
            if (property != null)
            {
                var attributesJson = (string)property.GetValue(obj);
                var attributes = JObject.Parse(attributesJson);
                model.Attributes = attributes;
            }

            return model;
        }

        private dynamic CreateTaskModel(TwilioTask task)
        {
            dynamic model = task.ToDynamic();
            var attributes = JObject.Parse(task.Attributes);
            model.Attributes = attributes;
            model.CaseId = attributes["case_id"].ToObject<int>();
            model.SelectedSkill = attributes["selected_skill"].ToString();
            return model;
        }

        private dynamic CreateWorkerModel(Worker worker)
        {
            dynamic model = worker.ToDynamic();
            var attributes = JObject.Parse(worker.Attributes);
            model.Attributes = attributes;
            model.IsAvailable = bool.Parse(worker.Available);
            model.IsOffline = settings.TaskRouter.Activities.Offline.Sid.Equals(worker.ActivitySid, StringComparison.OrdinalIgnoreCase);
            model.Skills = attributes["skills"].ToObject<IEnumerable<Skill>>();
            return model;
        }
    }
}
