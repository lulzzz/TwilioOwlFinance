/* cases.controller.js */

(function (angular, app) {
    'use strict';

    app.controller('casesController', controller);

    controller.$inject = ['$scope', 'auth', 'notifier', 'CaseNavigationService', 'AgentService'];
    function controller($scope, auth, notifier, nav, agentService) {
        $scope.auth = auth;
        $scope.history = false;
        $scope.onCaseSelected = nav.select;
        $scope.onShowHistory = onShowHistory;
        $scope.sendSms = onSendSms;

        agentService.getCases()
            .then(function (cases) {
                $scope.cases = cases;
            });

        function onShowHistory() {
            $scope.history = !$scope.history;
        }

        function onSendSms() {
            agentService.triggerActivityScanningForMyCustomers().then(function (data) {
                console.log(data);
                if (data.isSuccessful === true) {
                    notifier.add('Customers are alerted of any potential suspicious activity.');
                }
                else {
                    notifier.add('Something went wrong. ' + data.statusMessage);
                }
                
            });
        }
    }
})(angular, app);
