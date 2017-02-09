/* agent-menu.directive.js */

(function (angular, app) {
    'use strict';

    /**
    * @example <agent-menu></agent-menu>
    */
    app.directive('agentMenu', directive);

    directive.$inject = ['$document', 'notifier', 'AgentService', 'Api'];
    function directive($document, notifier, agentService, Api) {
        var options = {
            restrict: 'E',
            replace: 'true',
            templateUrl: '/app/views/shared/agent-menu.html',
            controller: function ($scope, $http, $q) {
                $scope.logoff = onLogoff;
                $scope.reset = onReset;
                $scope.toggleAvailability = onToggleAvailability;

                agentService.getAgent();

                function onLogoff() {
                    $scope.auth.signout();
                }

                function onReset() {
                    $q.all([
                        $http.post(Api.settings.closeCases.url),
                        $http.post(Api.settings.deleteChannels.url)
                    ]).then(function () {
                        notifier.add('Reset complete');
                    });
                }

                function onToggleAvailability() {
                    var isOnline = !$scope.agent.isOffline;
                    agentService.setAvailability(!isOnline)
                        .then(function () {
                            agentService.getAgent();
                        });
                }
            },
            link: function (scope, element, attrs, ctrl) {
                var wrapper, button;

                wrapper = element.find('.user-dropdown-wrapper');
                button = element.find('.dropdown-button');

                button.bind('click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    wrapper.toggleClass('show');
                });

                $document.bind('click', function (event) {
                    wrapper.removeClass('show');
                });

                scope.$on('$destroy', function () {
                    button.unbind('click');
                    $document.unbind('click');
                });
            }
        };

        options.controller.$inject = ['$scope', '$http', '$q'];

        return options;
    }
})(angular, app);
