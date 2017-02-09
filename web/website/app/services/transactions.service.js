/* transactions.service.js */

(function (angular, app) {
    function factory($resource, Api) {
        return $resource(Api.transactions.url);
    }

    factory.$inject = ['$resource', 'Api'];
    app.factory('Transactions', factory);
})(angular, app);