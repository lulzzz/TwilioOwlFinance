/* system-report.directive.js */

(function (angular, app) {
    /**
    * @example <system-report></system-report>
    */
    app.directive('systemReport', directive);

    function directive() {
        var options = {
            restrict: 'E',
            replace: 'true',
            templateUrl: '/app/views/shared/system-report.html'
        };

        return options;
    }
})(angular, app);
