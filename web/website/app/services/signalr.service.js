/* signalr.service.js  */

(function (angular, app) {
    function makeClientProxyFunction(callback) {
        return function () {
            callback.apply(this, arguments);
        };
    }

    function client(callback) {
        var clients = {};
        callback(clients);

        for (var property in clients) {
            var value = clients[property];
            if (typeof value != 'function') {
                continue;
            }
            this.on(property, makeClientProxyFunction(value));
        }
    };

    function registerHubFactory($provide, hubName) {
        $provide.factory(hubName, function () {
            var proxy = $.connection.hub.createHubProxy(hubName);
            return proxy;
        });
    }

    function __nothing() { }

    function setupAndRegisterProxies($provide) {
        for (var property in $.connection) {
            var value = $.connection[property];
            if (typeof value !== 'undefined' && value !== null) {
                if (typeof value.hubName !== 'undefined' && value !== null) {
                    var hubName = property;
                    var proxy = $.connection.hub.createHubProxy(hubName);
                    proxy.client = client;
                    proxy.client.__need_this_for_subscription__ = __nothing;
                    registerHubFactory($provide, hubName);
                }
            }
        }
    }

    app.config(["$provide",
        function ($provide) {
            setupAndRegisterProxies($provide);
            $.connection.hub.url = OWL_FINANCE_SIGNALR_SERVER_URL;
            $.connection.hub.start({jsonp: true}).done(function () {
                console.log('Hub connection up and running');
            })
            .fail(function () {
                console.log('Error setting up hub');
            });
        }
    ]);
})(angular, app);