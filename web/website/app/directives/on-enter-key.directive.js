/* on-enter-key.directive.js */

(function (angular, app) {
    'use strict';

    /**
    * @example <input type="test" on-enter-key="callScopeFn($event)" />
    */
    app.directive('onEnterKey', directive);

    directive.$inject = ['$parse'];
    function directive($parse) {
        var options = {
            restrict: 'A',
            link: function (scope, element, attrs, ctrl) {
                element.bind('keydown keypress', function (event) {
                    if (event.key === 'Enter') {
                        var handler = $parse(attrs.onEnterKey);
                        if (angular.isFunction(handler)) {
                            handler(scope, { $event: event });
                        }

                        event.preventDefault();
                    }
                });

                scope.$on('$destroy', function () {
                    element.unbind('keydown keypress');
                });
            }
        };

        return options;
    }
})(angular, app);
