/* login.controller.js */

(function (angular, app) {
    function controller($scope, auth) {
        $scope.auth = auth;
    };

    controller.$inject = ['$scope', 'auth'];
    app.controller('loginController', controller);
})(angular, app);
