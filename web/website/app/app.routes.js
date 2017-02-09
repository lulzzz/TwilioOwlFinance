/* app.routes.js */

(function (angular, app) {
    app.config(routes);

    function routes($routeProvider) {
        $routeProvider.when('/', {
            controller: 'casesController',
            templateUrl: '/app/views/cases.html',
            name: 'home',
            isHome: true,
            requiresLogin: true
        });

        $routeProvider.when('/cases', {
            controller: 'casesController',
            templateUrl: '/app/views/cases.html',
            name: 'home',
            isHome: true,
            requiresLogin: true
        });

        $routeProvider.when('/cases/:id', {
            controller: 'caseDetailsController',
            templateUrl: '/app/views/case-details.html',
            name: 'case',
            requiresLogin: true
        });

        $routeProvider.when('/accounts/:id/cases', {
            controller: 'accountCasesController',
            templateUrl: '/app/views/account-cases.html',
            name: 'account-cases',
            requiresLogin: true
        });

        $routeProvider.when('/accounts/:id/history', {
            controller: 'accountHistoryController',
            templateUrl: '/app/views/account-history.html',
            name: 'account-history',
            requiresLogin: true
        });

        $routeProvider.when('/docusign', {
            controller: 'docusignController',
            templateUrl: '/app/views/docusigndone.html',
            name: 'docusign',
            requiresLogin: false
        });

        $routeProvider.when('/admin', {
            controller: 'usersAdminController',
            templateUrl: '/app/views/admin/users.html',
            name: 'admin'
        });

        $routeProvider.when('/admin/users', {
            controller: 'usersAdminController',
            templateUrl: '/app/views/admin/users.html',
            name: 'admin'
        });

        $routeProvider.when('/admin/users/add/:type', {
            controller: 'userFormAdminController',
            templateUrl: '/app/views/admin/user-form.html',
            name: 'admin'
        });

        $routeProvider.when('/admin/users/:id', {
            controller: 'userFormAdminController',
            templateUrl: '/app/views/admin/user-form.html',
            name: 'admin'
        });

        $routeProvider.when('/login', {
            controller: 'loginController',
            templateUrl: '/app/views/login.html',
            name: 'login'
        });

        $routeProvider.otherwise({ redirectTo: '/' });
    }

    routes.$inject = ['$routeProvider'];
})(angular, app);
