using System.Collections.Generic;

namespace Twilio.OwlFinance.Domain.Interfaces.Settings
{
    public interface ITwilioApiSettingsProvider
    {
        string AuthToken { get; }
        string ApiKey { get; }
        string ApiSecret { get; }
        string FromPhoneNumber { get; }
        string AppSid { get; }
        ITwilioApiSetting Account { get; }
        IIpMessagingSettings IpMessaging { get; }
        ITaskRouterSettings TaskRouter { get; }
        IConversationsSetting Conversation { get; }
        IDocusignSetting Docusign { get; }
    }

    public interface IDocusignSetting
    {
        string UserName { get; }
        string Password { get; }
        string IntegratorKey { get; }
    }

    public interface IConversationsSetting : ITwilioApiSetting
    {
    }

    public interface IIpMessagingSettings
    {
        ITwilioApiSetting Service { get; }
    }

    public interface ITaskRouterSettings
    {
        IActivitySettingsCollection Activities { get; }
        ITwilioApiSetting Workspace { get; }
        ITwilioApiSetting Workflow { get; }
    }

    public interface IActivitySettingsCollection : IEnumerable<IActivitySetting>
    {
        IActivitySetting Offline { get; }
        IActivitySetting Idle { get; }
        IActivitySetting Busy { get; }
        IActivitySetting Reserved { get; }
    }

    public interface IActivitySetting : ITwilioApiSetting
    {
        bool IsAvailable { get; }
        string Name { get; }
    }

    public interface ITwilioApiSetting
    {
        string Sid { get; }
    }
}
