/* cases.service.js */

(function (angular, app) {
    function factory($resource, Api) {

        return $resource(Api.cases.url, null,
            {
                get: {
                    method: 'GET',
                    transformResponse: Api.defaultResponseTransform
                },
                getAccountCases: {
                    url: Api.accounts.cases.url,
                    method: 'GET',
                    params: { includeClosed: false },
                    isArray: true,
                    transformResponse: Api.defaultResponseTransform
                },
                getAccountHistory: {
                    url: Api.accounts.events.url,
                    method: 'GET',
                    isArray: true,
                    transformResponse: Api.defaultResponseTransform
                },
                getAccountStatements: {
                    url: Api.accounts.statements.url,
                    method: 'GET',
                    isArray: true,
                    transformResponse: Api.defaultResponseTransform
                },
                getAccountTransactions: {
                    url: Api.accounts.transactions.url,
                    method: 'GET',
                    isArray: true,
                    transformResponse: Api.defaultResponseTransform
                }
            }
        );
    }

    factory.$inject = ['$resource', 'Api'];
    app.factory('CaseService', factory);
})(angular, app);
