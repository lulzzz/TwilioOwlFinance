/* collapsible.directive.js */

(function (angular, app) {
    'use strict';

    /**
    * @example <collapsible>
    *            <header>...</header>
    *            <content>...</content>
    *          </collapsible>
    */
    app.directive('collapsible', function () {
        var options = {
            restrict: 'E',
            controller: ['$scope', function ($scope) {
                this.toggle = function () {
                    $scope.$apply(function () {
                        $scope.expanded = !$scope.expanded;
                    });
                }
            }]
        };

        return options;
    });

    /**
    * @example <span toggle>Click me to expand panel</span>
    * 
    */
    app.directive('toggle', function () {
        var options = {
            restrict: 'A',
            require: '^collapsible',
            link: function (scope, element, attrs, ctrl) {
                element.bind('click', function (event) {
                    ctrl.toggle();
                });

                scope.$on('$destroy', function () {
                    element.unbind('click');
                });
            }
        };

        return options;
    });
})(angular, app);
