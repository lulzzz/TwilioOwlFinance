/* app.js */

var app;

(function (angular) {
    app = angular.module('owlfinance', [
        'auth0',
        'ngRoute',
        'ngResource',
        'angular-storage',
        'angular-jwt',
        'timer'
    ]);
})(angular);
