/* app.constants.js */

(function (angular, app, apiBaseUrl, twilioSignalrUrl) {
    'use strict';
  
    app.constant('Api', {
        baseUrl: apiBaseUrl,
        admin: {
            users: {
                url: apiBaseUrl + '/admin/users',
                customers: { url: apiBaseUrl + '/admin/users/customers' },
            },
        },
        accounts: {
            url: apiBaseUrl + '/accounts/:id',
            cases: { url: apiBaseUrl + '/accounts/:id/cases' },
            events: { url: apiBaseUrl + '/accounts/:id/events' },
            statements: { url: apiBaseUrl + '/accounts/:id/statements' },
            transactions: { url: apiBaseUrl + '/accounts/:id/transactions' }
        },
        agent: {
            url: apiBaseUrl + '/agent',
            cases: { url: apiBaseUrl + '/agent/cases' },
            goOffline: { url: apiBaseUrl + '/agent/activities/go-offline' },
            goOnline: { url: apiBaseUrl + '/agent/activities/go-online' },
            acceptTaskReservation: { url: apiBaseUrl + '/agent/activities/accept-task-reservation' },
            declineTaskReservation: { url: apiBaseUrl + '/agent/activities/decline-task-reservation' },
            closeCase: { url: apiBaseUrl + '/closecase' },
            sendDocuSign: { url: apiBaseUrl + '/docusign/send' },
            getDocuSign: { url: apiBaseUrl + '/docusign/document' },
            getPairedCustomer: { url: apiBaseUrl + '/agent/customer' },
            triggerSuspiciousActivity: {url: apiBaseUrl + '/activity/scan'}
        },
        cases: {
            url: apiBaseUrl + '/cases/:id',
            createCase: apiBaseUrl + '/cases/createcase'
        },
        access: {
            getMessagingToken: { url: apiBaseUrl + '/twilio/ip-messaging/token' },
            getConversationsToken: { url: apiBaseUrl + '/twilio/conversations/token' },
            getVoiceCallToken: { url: apiBaseUrl + '/twilio/voice-call/token' },
            getTaskRouterWorkerToken: { url: apiBaseUrl + '/twilio/task-router/worker-token' },
            getTaskRouterWorkspaceToken: { url: apiBaseUrl + '/twilio/task-router/workspace-token' }
        },
        settings: {
            closeCases: { url: apiBaseUrl + '/settings/close-cases' },
            deleteChannels: { url: apiBaseUrl + '/settings/delete-channels' }
        },
        statements: { url: apiBaseUrl + '/statements/:id' },
        transactions: { url: apiBaseUrl + '/transactions/:id' },
        weather: { url: apiBaseUrl + '/weather' },
        defaultResponseTransform: function (data) {
            return (angular.fromJson(data) || {}).data;
        }
    });

    app.constant('CaseNavigationStorageKey', 'nav:cases');
    app.constant('signalEvents', $.connection.eventsHub);

    app.constant('Enums', {
        NotificationTypes: {
            MESSAGE: 1,
            CALL: 2,
            TASK: 3
        }
    });
})(angular, app, OWL_FINANCE_API_BASE_URL);
