
/*!
 * owl-finance.service.js
 * 
 * Provides base class functionality for other Angular services
 * that rely on Owl Finanace web APIs for their functionality.
 */

(function (angular, app) {
    'use strict';

    app.factory('OwlFinanceServiceBase', factory);

    factory.$inject = ['$http', '$q', 'Api'];
    function factory($http, $q, api) {
        function OwlFinanceServiceBase() {
            this.api = api;
        }

        OwlFinanceServiceBase.prototype.get = function (url, config) {
            var deferred = $q.defer();

            $http.get(url, config).then(
                function (response) {
                    deferred.resolve(response.data.data)
                },
                function (response) {
                    deferred.reject(response.data);
                });

            return deferred.promise;
        }

        OwlFinanceServiceBase.prototype.post = function (url, postData, config) {
            var deferred = $q.defer();

            $http.post(url, postData, config).then(
                function (response) {
                    deferred.resolve(response.data.data)
                },
                function (response) {
                    deferred.reject(response.data);
                });

            return deferred.promise;
        }

        OwlFinanceServiceBase.prototype.delete = function (url, config) {
            var deferred = $q.defer();

            $http.delete(url, config).then(
                function (response) {
                    deferred.resolve(response.data.data)
                },
                function (response) {
                    deferred.reject(response.data);
                });

            return deferred.promise;
        }

        return OwlFinanceServiceBase;
    }
})(angular, app);
