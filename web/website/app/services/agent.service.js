/* agent.service.js */

(function (angular, app) {
    'use strict';

    app.factory('AgentService', factory);

    factory.$inject = ['$rootScope', 'OwlFinanceServiceBase'];
    function factory($rootScope, OwlFinanceServiceBase) {
        function AgentService() {
            OwlFinanceServiceBase.call(this);
        }

        AgentService.prototype = Object.create(OwlFinanceServiceBase.prototype);
        AgentService.prototype.constructor = AgentService;

        AgentService.prototype.getAgent = function () {
            return this.get(this.api.agent.url)
                .then(function (agent) {
                    $rootScope.agent = agent;
                });
        }

        AgentService.prototype.getCases = function () {
            return this.get(this.api.agent.cases.url);
        }

        AgentService.prototype.closeCase = function (caseId) {
            var postData = { caseId: caseId };
            return this.post(this.api.agent.closeCase.url, postData);
        }

        AgentService.prototype.setAvailability = function (available) {
            return this.post(available ? this.api.agent.goOnline.url : this.api.agent.goOffline.url);
        }

        AgentService.prototype.acceptTaskReservation = function (taskSid, reservationSid) {
            var postData = { taskSid: taskSid, reservationSid: reservationSid };
            return this.post(this.api.agent.acceptTaskReservation.url, postData);
        }

        AgentService.prototype.declineTaskReservation = function (taskSid, reservationSid) {
            var postData = { taskSid: taskSid, reservationSid: reservationSid };
            return this.post(this.api.agent.declineTaskReservation.url, postData);
        }

        AgentService.prototype.sendDocuSignDocument = function (transactionId) {
            var postData = { caseID: transactionId, sendTo: 'Al Cook', sendToEmail: 'noreply@test.com'};
            return this.post(this.api.agent.sendDocuSign.url, postData);
        }

        AgentService.prototype.getDocument = function (caseId) {
            return this.get(this.api.agent.getDocuSign.url + "/" + caseId);
        }

        AgentService.prototype.getPairedCustomer = function () {
            return this.get(this.api.agent.getPairedCustomer.url);
        }

        AgentService.prototype.triggerActivityScanningForMyCustomers = function () {
            return this.post(this.api.agent.triggerSuspiciousActivity.url, null);
        }

        return new AgentService();
    }
})(angular, app);
