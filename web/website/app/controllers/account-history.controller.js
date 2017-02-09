/* account-history.controller.js */

(function (angular, app) {
    function controller($scope, $routeParams, $http, $location, auth, store, CaseService) {
        $scope.accountId = $routeParams.id;
        $scope.auth = auth;

        CaseService.getAccountHistory({ id: $scope.accountId }, function (events) {
            $scope.accountHistory = events;
        });
    }

    controller.$inject = ['$scope', '$routeParams', '$http', '$location', 'auth', 'store', 'CaseService'];
    app.controller('accountHistoryController', controller);
})(angular, app);
