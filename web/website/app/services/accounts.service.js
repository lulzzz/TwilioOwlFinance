/* accounts.service.js */

(function (angular, app) {
    function factory($resource, Api) {
        return $resource(Api.accounts.url, null,
            {
                get: {
                    method: 'GET',
                    transformResponse: Api.defaultResponseTransform
                },
                getCases: {
                    url: Api.accounts.cases.url,
                    method: 'GET',
                    params: { id: '@accountId', includeClosed: false },
                    isArray: true,
                    transformResponse: Api.defaultResponseTransform
                },
                getHistory: {
                    url: Api.accounts.events.url,
                    method: 'GET',
                    params: { id: '@accountId' },
                    isArray: true,
                    transformResponse: Api.defaultResponseTransform
                },
                getStatements: {
                    url: Api.accounts.statements.url,
                    method: 'GET',
                    params: { id: '@accountId' },
                    isArray: true,
                    transformResponse: Api.defaultResponseTransform
                },
                getTransactions: {
                    url: Api.accounts.transactions.url,
                    method: 'GET',
                    params: { id: '@accountId' },
                    isArray: true,
                    transformResponse: Api.defaultResponseTransform
                }
            });
    }

    factory.$inject = ['$resource', 'Api'];
    app.factory('AccountService', factory);
})(angular, app);
