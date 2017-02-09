/* notifier.service.js */

(function (angular, app) {
    'use strict';

    app.provider('notifier', provider);

    provider.$inject = ['Enums'];
    function provider(Enums) {
        var defaults = {
            types: Enums.NotificationTypes,
            message: {
                type: Enums.NotificationTypes.MESSAGE
            }
        };

        function Notifier(config) {
            this.settings = angular.extend(defaults, config);
            this.messages = [];
        }

        Notifier.prototype.add = function (message) {
            // if message is a string, convert to json object
            message = angular.isObject(message) ? message : { content: message };

            // ensure message defaults for missing settings
            message = angular.extend({}, this.settings.message, message);

            // give message a unique id
            message.id = guid();

            this.messages.push(message);
        }

        Notifier.prototype.remove = function (message) {
            var index = this.messages.findIndex(function (item) {
                return item.id === message.id;
            });

            if (index !== -1) {
                this.messages.splice(index, 1);
            }
        }

        function configure(config) {
            angular.extend(defaults, config);
        }

        return ({
            configure: configure,
            $get: create
        });

        //#region private functions
        function create() {
            return new Notifier(defaults);
        }

        // function for creating random guids
        function guid() {
            var id = '', i, random;

            for (i = 0; i < 32; i++) {
                random = Math.random() * 16 | 0;

                if (i == 8 || i == 12 || i == 16 || i == 20) {
                    id += "-";
                }
                id += (i == 12 ? 4 : (i == 16 ? (random & 3 | 8) : random)).toString(16);
            }

            return id;
        }
        //#endregion
    }
})(angular, app);
