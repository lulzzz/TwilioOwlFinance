/* user-form.controller.js */

(function (angular, app) {
    'use strict';

    app.controller('userFormAdminController', controller);

    controller.$inject = ['$scope', '$routeParams', '$location', '$q', 'auth', 'userAdminService'];
    function controller($scope, $routeParams, $location, $q, auth, userService) {
        $scope.auth = auth;
        $scope.id = parseInt($routeParams.id) || null;
        $scope.editMode = angular.isNumber($scope.id);
        $scope.type = $routeParams.type || 'agent';
        $scope.save = onSave;
        $scope.delete = onDelete;

        (function () {
            var requests = {
                customers: userService.getCustomers()
            };

            if ($scope.editMode) {
                requests.user = userService.getUser($scope.id);
            }

            $q.all(requests).then(function (response) {
                $scope.user = response.user ? response.user : null;
                $scope.customers = response.customers;
                if ($scope.user) {
                    $scope.type = $scope.user.type.toLowerCase();
                }
            });
        })();

        function onSave() {
            if (!$scope.userForm.$invalid) {
                $scope.user.type = $scope.type;
                userService.saveUser($scope.user).then(function (user) {
                    $scope.user = user;
                    $location.path('/admin/users');
                });
            }
        }

        function onDelete() {
            userService.deleteUser($scope.user).then(function () {
                $location.path('/admin/users');
            });
        }
    }
})(angular, app);
