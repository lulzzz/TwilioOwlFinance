/* app.config.js */

(function (document, angular, app) {
    app.config(configure).run(runBlock);

    //#region module configuration
    configure.$inject = ['$httpProvider', '$locationProvider', 'storeProvider', 'authProvider', 'jwtInterceptorProvider', 'notifierProvider'];
    function configure($httpProvider, $locationProvider, storeProvider, authProvider, jwtInterceptorProvider, notifierProvider) {
        configureHtml5Mode();
        configureNotifier();
        configureStorage();
        configureAuth0();
        configureJwtInterceptor();

        function configureHtml5Mode() {
            //$locationProvider.html5Mode(true);
        }

        function configureNotifier() {
            //add notifier configuration here
            notifierProvider.configure({});
        }

        function configureStorage() {
            storeProvider.setCaching(false);
        }

        function configureAuth0() {
            authProvider.init({
                domain: AUTH0_DOMAIN,
                clientID: AUTH0_CLIENT_ID,
                loginUrl: '/login'
            });

            authProvider.on('authenticated', onAuthenticated);
            authProvider.on('loginSuccess', onLoginSuccess);
            authProvider.on('loginFailure', onLoginFailure);
            authProvider.on('logout', onLogout);

            function onAuthenticated(twilioIntegration) {
                twilioIntegration.start();
            }

            function onLoginSuccess($location, profilePromise, idToken, store, twilioIntegration) {
                console.info('Login Success');
                profilePromise.then(function (profile) {
                    //set oauth token cookie
                    document.cookie = `OAuthToken=${idToken}; path=/`;

                    store.set('profile', profile);
                    store.set('token', idToken);

                    twilioIntegration.start();

                    $location.path('/');
                });
            };

            function onLoginFailure() {
                console.warn('Login failure!');
            }

            function onLogout($location, store, twilioIntegration) {
                document.cookie = 'OAuthToken=;expires=Thu, 01 Jan 1970 00:00:01 GMT;'

                store.remove('profile');
                store.remove('token');

                twilioIntegration.stop();

                $location.path('/login');
            }

            onAuthenticated.$inject = ['twilioIntegration'];
            onLoginSuccess.$inject = ['$location', 'profilePromise', 'idToken', 'store', 'twilioIntegration'];
            onLogout.$inject = ['$location', 'store', 'twilioIntegration'];
        }

        function configureJwtInterceptor() {
            jwtInterceptorProvider.tokenGetter = function (store) {
                return store.get('token');
            };

            jwtInterceptorProvider.tokenGetter.$inject = ['store'];

            $httpProvider.interceptors.push('jwtInterceptor');
        }
    }
    //#endregion

    //#region module run block
    runBlock.$inject = ['$rootScope', '$location', 'auth', 'store', 'jwtHelper'];
    function runBlock($root, $location, auth, store, jwtHelper) {
        auth.hookEvents();

        // verify user is authenticated on location change request
        $root.$on('$locationChangeStart', function () {
            var profile, token = store.get('token');
            if (token && !jwtHelper.isTokenExpired(token)) {
                if (!auth.isAuthenticated) {
                    profile = store.get('profile');
                    auth.authenticate(profile, token);
                }
            } else if (window.location.hash === '#/docusign') {
                $location.path('/docusign');
            }
            else {
                $location.path('/login');
            }
        });

        // set navigation tab properties after route change
        $root.$on("$routeChangeSuccess", function (event, next, current) {
            $root.tab = {
                name: next.name,
                isHome: !!next.isHome
            };
        });
    }
    //#endregion
})(document, angular, app);
