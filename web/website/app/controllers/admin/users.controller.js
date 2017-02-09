/* users.controller.js */

(function (angular, app) {
    'use strict';

    app.controller('usersAdminController', controller);

    controller.$inject = ['$scope', 'auth', 'userAdminService'];
    function controller($scope, auth, userService) {
        $scope.auth = auth;

        userService.getUsers().then(function (users) {
            $scope.users = users;
        });
    }
})(angular, app);
