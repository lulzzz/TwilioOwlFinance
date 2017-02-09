/* site-header.directive.js */

(function (angular, app) {
    /**
    * @example <site-header></site-header>
    */
    app.directive('siteHeader', directive);

    function directive() {
        var options = {
            restrict: 'E',
            replace: 'true',
            templateUrl: '/app/views/shared/header.html'
        };

        return options;
    }
})(angular, app);
