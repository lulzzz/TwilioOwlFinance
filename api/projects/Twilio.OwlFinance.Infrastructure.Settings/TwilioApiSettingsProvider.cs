using System.Collections;
using System.Collections.Generic;
using Twilio.OwlFinance.Domain.Interfaces.Settings;

namespace Twilio.OwlFinance.Infrastructure.Settings
{
    public class TwilioApiSettingsProvider : ITwilioApiSettingsProvider
    {
        public TwilioApiSettingsProvider(IAppSettingsProvider appSettings)
        {
            Initialize(appSettings);
        }

        public string AuthToken { get; private set; }

        public string ApiKey { get; private set; }

        public string ApiSecret { get; private set; }

        public string FromPhoneNumber { get; private set; }
        public string AppSid { get; private set; }

        public ITwilioApiSetting Account { get; private set; }

        public IIpMessagingSettings IpMessaging { get; private set; }

        public ITaskRouterSettings TaskRouter { get; private set; }
        public IConversationsSetting Conversation { get; private set; }
        public IDocusignSetting Docusign { get; private set; }

        private void Initialize(IAppSettingsProvider appSettings)
        {
            AuthToken = appSettings.Get("twilio:AuthToken");
            ApiKey = appSettings.Get("twilio:ApiKey");
            ApiSecret = appSettings.Get("twilio:ApiSecret");
            FromPhoneNumber = appSettings.Get("twilio:FromPhoneNumber");
            AppSid = appSettings.Get("twilio:AppSID");

            Account = new TwilioApiSetting(appSettings.Get("twilio:AccountSID"));

            IpMessaging = new IpMessagingSettings {
                Service = new TwilioApiSetting(appSettings.Get("twilio:IpMessagingSID"))
            };

            Conversation = new TwilioConversationsSetting(appSettings.Get("twilio:ConversationsSID"));

            Docusign = new DocusignSetting(appSettings.Get("docusign:UserName"), appSettings.Get("docusign:Password"), appSettings.Get("docusign:IntegratorKey"));

            TaskRouter = new TaskRouterSettings {
                Workspace = new TwilioApiSetting(appSettings.Get("twilio:WorkspaceSID")),
                Workflow = new TwilioApiSetting(appSettings.Get("twilio:WorkflowSID")),
                Activities = new ActivitySettingsCollection {
                    Offline = new ActivitySetting("Offline", appSettings.Get("twilio:OfflineActivitySID"), false),
                    Idle = new ActivitySetting("Idle", appSettings.Get("twilio:IdleActivitySID"), false),
                    Busy = new ActivitySetting("Busy", appSettings.Get("twilio:BusyActivitySID"), false),
                    Reserved = new ActivitySetting("Reserved", appSettings.Get("twilio:ReservedActivitySID"), false)
                }
            };
        }
    }

    internal class IpMessagingSettings : IIpMessagingSettings
    {
        public ITwilioApiSetting Service { get; set; }
    }

    internal class TaskRouterSettings : ITaskRouterSettings
    {
        public TaskRouterSettings()
        {
            Activities = new ActivitySettingsCollection();
        }

        public IActivitySettingsCollection Activities { get; set; }
        public ITwilioApiSetting Workspace { get; set; }
        public ITwilioApiSetting Workflow { get; set; }
    }

    internal class ActivitySettingsCollection : IActivitySettingsCollection, IEnumerable<IActivitySetting>
    {
        public IActivitySetting Offline { get; set; }
        public IActivitySetting Idle { get; set; }
        public IActivitySetting Busy { get; set; }
        public IActivitySetting Reserved { get; set; }

        #region IEnumerable<T> Impl
        public IEnumerator<IActivitySetting> GetEnumerator()
        {
            yield return Offline;
            yield return Idle;
            yield return Busy;
            yield return Reserved;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        #endregion
    }

    internal class ActivitySetting : TwilioApiSetting, IActivitySetting
    {
        public ActivitySetting(string name, string sid, bool isAvailable)
            : base(sid)
        {
            Name = name;
            IsAvailable = isAvailable;
        }

        public bool IsAvailable { get; private set; }
        public string Name { get; private set; }
    }

    internal class TwilioApiSetting : ITwilioApiSetting
    {
        public TwilioApiSetting(string sid)
        {
            Sid = sid;
        }

        public string Sid { get; private set; }
    }

    internal class TwilioConversationsSetting : IConversationsSetting
    {
        public TwilioConversationsSetting(string sid)
        {
            Sid = sid;
        }

        public string Sid { get; private set; }
    }
    internal class DocusignSetting : IDocusignSetting
    {
        public DocusignSetting(string userName, string password, string integratorKey)
        {
            UserName = userName;
            Password = password;
            IntegratorKey = integratorKey;
        }

        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string IntegratorKey { get; private set; }
    }
}
