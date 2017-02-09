/* case-navigation.directive.js */

(function (angular, app) {
    /**
    * @example <case-navigation></case-navigation>
    */
    app.directive('caseNavigation', directive);

    directive.$inject = ['CaseNavigationService', '$routeParams'];
    function directive(nav, $routeParams) {
        var options = {
            restrict: 'E',
            replace: 'true',
            scope: {},
            templateUrl: '/app/views/shared/case-navigation.html',
            link: function (scope, element, attrs, ctrl) {
                scope.id = $routeParams.id;
                scope.nav = nav;
                scope.cases = nav.getCases();
                nav.onChange(function () {
                    scope.cases = nav.getCases();
                })
            }
        };

        return options;
    }
})(angular, app);
