/* toast.directive.js */

(function (angular, app) {
    'use strict';

    /**
    * @example <toast></toast>
    */
    app.directive('toast', directive);

    directive.$inject = ['notifier'];
    function directive(notifier) {
        var options = {
            restrict: 'E',
            templateUrl: '/app/views/templates/toast.html',
            controller: function ($scope) {
                $scope.types = notifier.settings.types;
                $scope.messages = notifier.messages;
                $scope.close = close;
                $scope.accept = accept;
                $scope.decline = decline;

                $scope.notifier = notifier;

                function close(message) {
                    notifier.remove(message);
                }

                function accept(message) {
                    if (angular.isFunction(message.onAccept)) {
                        message.onAccept();
                    }

                    close(message);
                }

                function decline(message) {
                    if (angular.isFunction(message.onDecline)) {
                        message.onDecline();
                    }

                    close(message);
                }
            }
        };

        options.controller.$inject = ['$scope'];

        return options;
    }
})(angular, app);
