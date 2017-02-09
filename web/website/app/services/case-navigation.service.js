/* case-navigation.service.js */

(function (angular, app) {
    'use strict';

    function factory(store, key, $location) {
        store.remove(key);

        var handlers = [];

        function close(_case) {            
            var cases = (store.get(key) || []).filter(function (value) {
                return value.id !== _case.id;
            })

            update(cases);

            $location.path('/');
        }

        function getCases() {
            return store.get(key);
        }

        function select(_case) {
            var cases = store.get(key) || [];

            var exists = cases.some(function (value) {
                return value.id === _case.id;
            });

            if (!exists) {
                cases.push({
                    id: _case.id,
                    owner: _case.accountOwner,
                    imageUrl: _case.accountOwnerImgUrl
                });

                update(cases);
            }

            $location.path('/cases/' + _case.id);
        }

        function onChange(fn) {
            handlers.push(fn);
        }

        function update(cases) {
            store.set(key, cases);
            handlers.forEach(function (fn) {
                fn();
            });
        }

        return {
            close: close,
            getCases: getCases,
            select: select,
            onChange: onChange
        };
    }

    factory.$inject = ['store', 'CaseNavigationStorageKey', '$location'];
    app.factory('CaseNavigationService', factory);
})(angular, app);
