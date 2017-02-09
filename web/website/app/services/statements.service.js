/* statements.service.js */

(function (angular, app) {
    function factory($resource, Api) {
        return $resource(Api.statements.url);
    }

    factory.$inject = ['$resource', 'Api'];
    app.factory('Statements', factory);
})(angular, app);
