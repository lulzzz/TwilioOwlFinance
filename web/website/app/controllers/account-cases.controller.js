/* account-cases.controller.js */

(function (angular, app) {
    function controller($scope, $routeParams, $http, $location, auth, store, CaseService) {
        $scope.accountId = $routeParams.id;
        $scope.auth = auth;

        CaseService.getAccountCases({ id: $scope.accountId, includeClosed: true }, function (cases) {
            $scope.accountCases = cases;
        });
    }

    controller.$inject = ['$scope', '$routeParams', '$http', '$location', 'auth', 'store', 'CaseService'];
    app.controller('accountCasesController', controller);
})(angular, app);