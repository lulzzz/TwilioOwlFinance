/* twilio-integration.service.js */

(function (angular, app) {
    'use strict';

    app.factory('twilioIntegration', factory);

    factory.$inject = ['notifier', 'AgentService', 'CaseService', 'Api', 'OwlFinanceServiceBase', '$q', '$location'];
    function factory(notifier, agentService, caseService, api, OwlFinanceServiceBase, $q, $location) {
        var defaults = {
            logging: true
        };

        function TwilioIntegrationService(config) {
            this.settings = angular.extend(defaults, config || {});
            OwlFinanceServiceBase.call(this);
        }

        TwilioIntegrationService.prototype = Object.create(OwlFinanceServiceBase.prototype);
        TwilioIntegrationService.prototype.constructor = TwilioIntegrationService;

        TwilioIntegrationService.prototype.getMessagingToken = function (params) {
            return this.get(this.api.access.getMessagingToken.url, { params: params });
        }

        TwilioIntegrationService.prototype.getConversationsToken = function (params) {
            return this.get(this.api.access.getConversationsToken.url, { params: params });
        }

        TwilioIntegrationService.prototype.getVoiceCallToken = function (params) {
            return this.get(this.api.access.getVoiceCallToken.url, { params: params });
        }

        TwilioIntegrationService.prototype.getTaskRouterWorkerToken = function () {
            return this.get(this.api.access.getTaskRouterWorkerToken.url);
        }

        TwilioIntegrationService.prototype.getTaskRouterWorkspaceToken = function () {
            return this.get(this.api.access.getTaskRouterWorkspaceToken.url);
        }

        TwilioIntegrationService.prototype.start = function () {
            var onWorkerTokenExpired = (function () {
                var worker = this.worker;
                this.getTaskRouterWorkerToken().then(function (response) {
                    worker.updateToken(response.token);
                });
            }).bind(this);

            var onWorkspaceTokenExpired = (function () {
                var workspace = this.workspace;
                this.getTaskRouterWorkspaceToken().then(function (response) {
                    workspace.updateToken(response.token);
                });
            }).bind(this);

            var onActivitiesRetrieved = (function (error, activities) {
                this.activities = activities.data;
            }).bind(this);

            var onReservationCreated = (function (reservation) {
                var caseId = reservation.task.attributes.case_id;
                caseService.get({ id: reservation.task.attributes.case_id }, function (caseInfo) {
                    var notification = {
                        content: `New case #${caseId}. ${caseInfo.accountOwner} wants to dispute a charge for ${caseInfo.txnAmount} (${caseInfo.txnDescription})`,
                        caseId: reservation.task.attributes.case_id,
                        sid: reservation.task.sid,
                        reservationSid: reservation.sid,
                        imageUrl: caseInfo.accountOwnerImgUrl,
                        onAccept: function () {
                            agentService.acceptTaskReservation(reservation.task.sid, reservation.sid)
                                .then(function (data) {
                                    $location.path(`/cases/${data.caseId}`);
                                });
                        },
                        onDecline: function () {
                            agentService.declineTaskReservation(this.sid, this.reservationSid)
                                .then(function () {
                                    agentService.getAgent();
                                });
                        },
                        type: notifier.settings.types.TASK
                    };

                    notifier.add(notification);
                });
            }).bind(this);

            var requests = {
                worker: this.getTaskRouterWorkerToken(),
                workspace: this.getTaskRouterWorkspaceToken()
            };

            $q.all(requests).then((function (response) {
                this.worker = new Twilio.TaskRouter.Worker(response.worker.token, this.settings.logging)
                    .on('token.expired', onWorkerTokenExpired)
                    .on('reservation.created', onReservationCreated);

                this.worker.activities.fetch(onActivitiesRetrieved);

                this.workspace = new Twilio.TaskRouter.Workspace(response.workspace.token, this.settings.logging)
                    .on('token.expired', onWorkspaceTokenExpired);
            }).bind(this));
        }

        TwilioIntegrationService.prototype.stop = function () {
            this.worker.removeAllListeners();
            this.workspace.removeAllListeners();
            delete this.worker;
            delete this.workspace;
        }

        return new TwilioIntegrationService();
    }
})(angular, app);
